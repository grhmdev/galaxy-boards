using System;
using Microsoft.AspNetCore.Mvc;
using GalaxyBoardsAPI.Data.Entities;
using GalaxyBoardsAPI.Data.Repository;
using GalaxyBoardsAPI.Controllers.DTOs;
using GalaxyBoardsAPI.Controllers.Dtos;
using System.Linq;

namespace GalaxyBoardsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardsController : ControllerBase
    {
        private readonly IEntityRepository<Board> _boardRepo;
        private readonly IEntityRepository<Data.Entities.BoardColumn> _boardColumnRepo;
        private readonly IEntityRepository<Project> _projectRepo;
        private readonly IEntityRepository<Ticket> _ticketRepo;
        private readonly IEntityRepository<Data.Entities.TicketPlacement> _ticketPlacementRepo;
        private readonly ILogger<BoardsController> _logger;

        public BoardsController(ILogger<BoardsController> logger, IEntityRepository<Board> boardRepo, IEntityRepository<Data.Entities.BoardColumn> boardColumnRepo, IEntityRepository<Project> projectRepo, IEntityRepository<Ticket> ticketRepo, IEntityRepository<Data.Entities.TicketPlacement> ticketPlacementRepo)
        {
            _boardRepo = boardRepo;
            _logger = logger;
            _boardColumnRepo = boardColumnRepo;
            _projectRepo = projectRepo;
            _ticketRepo = ticketRepo;
            _ticketPlacementRepo = ticketPlacementRepo; 
        }

        // GET: api/boards
        [HttpGet]
        public ActionResult<QueryResult<BoardBrief>> QueryBoards()
        {
            return new QueryResult<BoardBrief>{
                totalRecords = _boardRepo.Count(),
                items = _boardRepo.Get().Select(b => new BoardBrief(b))
            };
        }

        // GET: api/boards/5
        [HttpGet("{id}")]
        public ActionResult<BoardDetail> GetBoard(Guid id)
        {
            var board = _boardRepo.GetById(id);
            if (board == null)
            {
                return NotFound();
            }

            var boardDto = new BoardDetail(board);
            return boardDto;
        }

        // PUT: api/boards/5
        [HttpPut("{id}")]
        public IActionResult PutBoard(Guid id, BoardUpdate updated)
        {
            var board = _boardRepo.GetById(id);
            if (board == null)
            {
                return NotFound();
            }
            board.Name = updated.name;

            var receivedColumnIds = updated.columns.Where(tp => tp.id != null).Select(tp => tp.id).OfType<Guid>().ToHashSet();
            var existingColumnIds = board.BoardColumns.Select(tp => tp.Id).OfType<Guid>().ToHashSet();

            var deletedColumnIds = existingColumnIds.Except(receivedColumnIds).ToList();
            deletedColumnIds.ForEach(id =>
            {
                DeleteBoardColumn(id, board);
            });

            var updatedColumnIds = existingColumnIds.Intersect(receivedColumnIds).ToList();
            updatedColumnIds.ForEach(id =>
            {
                UpdateBoardColumn(board.BoardColumns.First(bc => bc.Id == id), updated.columns.First(bc => bc.id == id));
            });

            updated.columns.Where(bc => bc.id == null).ToList().ForEach(columnData =>
            {
                CreateBoardColumn(board, columnData);
            });

            _boardRepo.Update(board);
            _boardRepo.Save();
            return Ok();
        }

        // POST: api/boards
        [HttpPost]
        public ActionResult<PostResult> PostBoard(BoardPostData boardData)
        {
            var project = _projectRepo.GetById(boardData.projectId);
            if (project == null)
            {
                return BadRequest();
            }

            var newBoard = CreateBoard(project, boardData);
            _boardRepo.Add(newBoard);
            _boardRepo.Save();
            return new PostResult
            {
                createdId = newBoard.Id
            };
        }

        // DELETE: api/boards/5
        [HttpDelete("{id}")]
        public IActionResult DeleteBoard(Guid id)
        {
            var board = _boardRepo.GetById(id);
            if (board == null)
            {
                return NotFound();
            }
            // Delete associated board columns
            board.BoardColumns.ToList().ForEach(bc =>
            {
                DeleteBoardColumn(bc.Id, board);
            });
            _boardRepo.Delete(id);
            _boardRepo.Save();
            return Ok();
        }

        private void DeleteBoardColumn(Guid id, Board board)
        {
            var boardColumn = board.BoardColumns.Where(bc => bc.Id == id).First();
            boardColumn.TicketPlacements.ToList().ForEach(tp =>
            {
                DeleteTicketPlacement(boardColumn, tp.Id);
            });
            board.BoardColumns.Remove(board.BoardColumns.First(bc => bc.Id == id));
            _boardColumnRepo.Delete(id);
        }

        private Board CreateBoard(Project project, BoardPostData boardPostData)
        {
            var newBoardColumns = boardPostData.columns.Select(col => new Data.Entities.BoardColumn
            {
                Id = Guid.NewGuid(),
                Name = col.name,
                Index = col.index,
                TicketPlacements = new List<Data.Entities.TicketPlacement>()
            }).ToList();

            var newBoard = new Board
            {
                Id = Guid.NewGuid(),
                Name = boardPostData.name,
                Project = project,
                BoardColumns = newBoardColumns
            };
            return newBoard;
        }

        private void UpdateBoardColumn(Data.Entities.BoardColumn boardColumn, BoardColumnUpdate columnData)
        {
            boardColumn.Name = columnData.name;
            boardColumn.Index = columnData.index;

            var receivedIds = columnData.ticketPlacements.Where(tp => tp != null).Select(tp => tp.id).OfType<Guid>().ToHashSet();
            var existingIds = boardColumn.TicketPlacements.Where(tp => tp != null).Select(tp => tp.Id).OfType<Guid>().ToHashSet();

            var deletedIds = existingIds.Except(receivedIds).ToList();
            deletedIds.ForEach(id => DeleteTicketPlacement(boardColumn, id));

            var updatedIds = existingIds.Intersect(receivedIds).ToList();
            updatedIds.ForEach(id =>
            {
                UpdateTicketPlacement(boardColumn, columnData, id);
            });

            // Create new placements for any objects with null ids
            columnData.ticketPlacements.Where(tp => tp.id == null).ToList().ForEach(tp =>
            {
                CreateTicketPlacement(boardColumn, tp);
            });
            _boardColumnRepo.Update(boardColumn);
        }

        private void DeleteTicketPlacement(Data.Entities.BoardColumn boardColumn, Guid ticketPlacementId)
        {
            boardColumn.TicketPlacements.Remove(boardColumn.TicketPlacements.First(tp => tp.Id == ticketPlacementId));
            _ticketPlacementRepo.Delete(ticketPlacementId);
        }

        private void UpdateTicketPlacement(Data.Entities.BoardColumn boardColumn, BoardColumnUpdate columnData, Guid ticketPlacementId)
        {
            var ticketPlacement = boardColumn.TicketPlacements.First(tp => tp.Id == ticketPlacementId);
            var ticketPlacementData = columnData.ticketPlacements.First(tp => tp.id == ticketPlacementId);
            ticketPlacement.Index = ticketPlacementData.index;
            _ticketPlacementRepo.Update(ticketPlacement);
        }

        private void CreateBoardColumn(Board board, BoardColumnUpdate columnData)
        {
            var newColumn = new Data.Entities.BoardColumn
            {
                Index = columnData.index,
                Name = columnData.name,
                TicketPlacements = new List<Data.Entities.TicketPlacement>()
            };
            board.BoardColumns.Add(newColumn);
            _boardColumnRepo.Add(newColumn);
        }

        private void CreateTicketPlacement(Data.Entities.BoardColumn boardColumn, TicketPlacementUpdate ticketPlacementData)
        {
            var ticket = _ticketRepo.GetById(ticketPlacementData.ticketId);
            if (ticket == null)
            {
                throw new ArgumentException($"Ticket not found with id {ticketPlacementData.ticketId}");
            }
            var ticketPlacement = new Data.Entities.TicketPlacement
            {
                Id = Guid.NewGuid(),
                Index = ticketPlacementData.index,
                Ticket = ticket,
                BoardColumn = boardColumn,
            };
            _ticketPlacementRepo.Add(ticketPlacement);
            boardColumn.TicketPlacements.Add(ticketPlacement);
        }

    }

}
