using GalaxyBoardsAPI.Controllers;
using GalaxyBoardsAPI.Controllers.Dtos;
using GalaxyBoardsAPI.Data;
using GalaxyBoardsAPI.Data.Entities;
using GalaxyBoardsAPI.Data.Repository;
using GalaxyBoardsAPI.Data.Search;
using Microsoft.Extensions.Logging;
using Moq;

namespace GalaxyBoardsTests.ControllerTests
{
    [TestFixture]
    public class BoardsControllerTests
    {
        private readonly Mock<IEntityRepository<Board>> mockBoardRepo = new();
        private readonly Mock<IEntityRepository<Project>> mockProjectRepo = new();
        private readonly Mock<IEntityRepository<GalaxyBoardsAPI.Data.Entities.BoardColumn>> mockBoardColumnRepo = new();
        private readonly Mock<IEntityRepository<Ticket>> mockTicketRepo = new();
        private readonly Mock<IEntityRepository<GalaxyBoardsAPI.Data.Entities.TicketPlacement>> mockTicketPlacementRepo = new();
        private readonly Mock<ILogger<BoardsController>> mockLogger = new();

        private BoardsController uut;
        private MockEntityFactory entityFactory;

        public BoardsControllerTests()
        {
        }

        [SetUp]
        public void Setup()
        {
            uut = new BoardsController(mockLogger.Object, mockBoardRepo.Object, mockBoardColumnRepo.Object, mockProjectRepo.Object, mockTicketRepo.Object, mockTicketPlacementRepo.Object);
            mockBoardRepo.Reset();
            mockBoardColumnRepo.Reset();
            mockProjectRepo.Reset();
            mockTicketRepo.Reset();
            mockTicketPlacementRepo.Reset();
            entityFactory = new MockEntityFactory();
            entityFactory.CreateEntities();
        }

        [Test]
        public void GetBoard_ShouldReturnNotFoundForUnknownId()
        {
            Guid ticketId = Guid.NewGuid();
            mockBoardRepo.Setup(m => m.GetById(ticketId)).Returns((Board?)null);

            var result = uut.GetBoard(ticketId);

            Assert.That(result.Value, Is.Null);
        }

        [Test]
        public void GetBoard_ShouldReturnBoardDetailForExistingBoardId()
        {
            var existingBoard = entityFactory.Boards[MockEntityFactory.BoardType.BackendDev];
            mockBoardRepo.Setup(m => m.GetById(existingBoard.Id)).Returns(existingBoard);

            var result = uut.GetBoard(existingBoard.Id);

            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.id, Is.EqualTo(existingBoard.Id));
        }

        [Test]
        public void GetBoard_BoardDetailShouldContainColumnData()
        {
            var existingBoard = entityFactory.Boards[MockEntityFactory.BoardType.BackendDev];
            mockBoardRepo.Setup(m => m.GetById(existingBoard.Id)).Returns(existingBoard);

            var result = uut.GetBoard(existingBoard.Id);

            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.columns.Count, Is.EqualTo(existingBoard.BoardColumns.Count));
            existingBoard.BoardColumns.ToList().ForEach(existingCol =>
            {
                Assert.That(result.Value.columns.Where(col => col.id == existingCol.Id).Count, Is.EqualTo(1));
            });
        }


        [Test]
        public void GetBoard_BoardDetailShouldContainTicketPlacements()
        {
            var existingBoard = entityFactory.Boards[MockEntityFactory.BoardType.BackendDev];
            mockBoardRepo.Setup(m => m.GetById(existingBoard.Id)).Returns(existingBoard);

            var result = uut.GetBoard(existingBoard.Id);

            Assert.That(result.Value, Is.Not.Null);
            var ticketPlacementDtos = result.Value.columns.SelectMany(col => col.ticketPlacements).ToList();
            var ticketPlacementEntities = existingBoard.BoardColumns.SelectMany(bc => bc.TicketPlacements).ToList();
            Assert.That(ticketPlacementDtos.Count, Is.EqualTo(ticketPlacementEntities.Count));
            ticketPlacementEntities.ForEach(existingTicketPlacement =>
            {
                Assert.That(ticketPlacementDtos.Where(tp => tp.id == existingTicketPlacement.Id).Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void QueryBoards_ShouldReturnAllStoredBoardsWithoutQueryParams()
        {
            var boards = entityFactory.Boards.Values.ToList();
            mockBoardRepo.SetupGet(boards, boards.Count);
            mockBoardRepo.Setup(m => m.Count()).Returns(boards.Count);

            var result = uut.QueryBoards();

            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.totalRecords, Is.EqualTo(boards.Count));
            Assert.That(result.Value.items.Count, Is.EqualTo(boards.Count));
            boards.ForEach(board =>
            {
                Assert.That(result.Value.items.Where(b => b.id == board.Id).Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void PostBoard_ShouldAddNewBoardToRepo()
        {
            var existingProject = entityFactory.Projects[MockEntityFactory.ProjectType.GalaxyBoardsDevelopment];
            mockProjectRepo.Setup(m => m.GetById(existingProject.Id)).Returns(existingProject);
            var data = new BoardPostData
            {
                name = "Board1",
                projectId = existingProject.Id,
                columns = new List<BoardPostDataColumn>
                {
                    new BoardPostDataColumn{ index = 0, name = "Todo" },
                    new BoardPostDataColumn { index = 1, name = "Done" },
                }
            };
            Board? createdBoard = null;
            mockBoardRepo.Setup(m => m.Add(It.IsAny<Board>())).Callback<Board>(p => createdBoard = p);

            var result = uut.PostBoard(data);

            Assert.That(createdBoard, Is.Not.Null);
            Assert.That(createdBoard.Name, Is.EqualTo("Board1"));
            Assert.That(createdBoard.Project, Is.Not.Null);
            Assert.That(createdBoard.Project.Id, Is.EqualTo(existingProject.Id));
            Assert.That(createdBoard.BoardColumns.Count, Is.EqualTo(2));
            Assert.That(createdBoard.BoardColumns.Where(bc => bc.Name == "Todo" && bc.Index == 0).Count, Is.EqualTo(1));
            Assert.That(createdBoard.BoardColumns.Where(bc => bc.Name == "Done" && bc.Index == 1).Count, Is.EqualTo(1));
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.createdId, Is.EqualTo(createdBoard.Id));
            mockBoardRepo.Verify(m => m.Save(), Times.Once);
        }

        [Test]
        public void PutBoard_UpdateBoardName()
        {
            var data = new BoardUpdate
            {
                name = "Updated Board",
                columns = new List<BoardColumnUpdate>()
            };
            Board? updatedBoard = null;
            mockBoardRepo.Setup(m => m.Update(It.IsAny<Board>())).Callback<Board>(t => updatedBoard = t);
            var existingBoard = entityFactory.Boards[MockEntityFactory.BoardType.BackendDev];
            mockBoardRepo.Setup(m => m.GetById(existingBoard.Id)).Returns(existingBoard);

            uut.PutBoard(existingBoard.Id, data);

            Assert.That(updatedBoard, Is.Not.Null);
            Assert.That(updatedBoard.Id, Is.EqualTo(existingBoard.Id));
            Assert.That(updatedBoard.Name, Is.EqualTo("Updated Board"));
            mockBoardRepo.Verify(m => m.Save(), Times.Once);
        }

        [Test]
        public void PutBoard_RemoveColumn()
        {
            var data = new BoardUpdate
            {
                name = "Updated Board",
                columns = new List<BoardColumnUpdate>()
            };
            Board? updatedBoard = null;
            mockBoardRepo.Setup(m => m.Update(It.IsAny<Board>())).Callback<Board>(t => updatedBoard = t);
            var existingBoard = entityFactory.Boards[MockEntityFactory.BoardType.BackendDev];
            mockBoardRepo.Setup(m => m.GetById(existingBoard.Id)).Returns(existingBoard);

            uut.PutBoard(existingBoard.Id, data);

            Assert.That(updatedBoard, Is.Not.Null);
            Assert.That(updatedBoard.BoardColumns.Count, Is.EqualTo(0));
        }

        [Test]
        public void PutBoard_AddColumn()
        {
            var data = new BoardUpdate
            {
                name = "Updated Board",
                columns = new List<BoardColumnUpdate>
                {
                    new BoardColumnUpdate()
                    {
                        index = 0,
                        name = "Todo",
                        ticketPlacements = new List<TicketPlacementUpdate>()
                    }
                }
            };
            Board? updatedBoard = null;
            mockBoardRepo.Setup(m => m.Update(It.IsAny<Board>())).Callback<Board>(t => updatedBoard = t);
            var existingBoard = entityFactory.Boards[MockEntityFactory.BoardType.BackendDev];
            mockBoardRepo.Setup(m => m.GetById(existingBoard.Id)).Returns(existingBoard);

            uut.PutBoard(existingBoard.Id, data);

            Assert.That(updatedBoard, Is.Not.Null);
            Assert.That(updatedBoard.BoardColumns.Count, Is.EqualTo(1));
            Assert.That(updatedBoard.BoardColumns.First().Name, Is.EqualTo("Todo"));
            Assert.That(updatedBoard.BoardColumns.First().Index, Is.EqualTo(0));
        }

        [Test]
        public void PutBoard_UpdateColumn()
        {
            var existingBoard = entityFactory.Boards[MockEntityFactory.BoardType.BackendDev];
            var existingColumn = existingBoard.BoardColumns.First();
            var data = new BoardUpdate
            {
                name = "Updated Board",
                columns = new List<BoardColumnUpdate>
                {
                    new BoardColumnUpdate()
                    {
                        id = existingColumn.Id,
                        index = 5,
                        name = "Updated name",
                        ticketPlacements = new List<TicketPlacementUpdate>()
                    }
                }
            };
            Board? updatedBoard = null;
            mockBoardRepo.Setup(m => m.Update(It.IsAny<Board>())).Callback<Board>(t => updatedBoard = t);
            mockBoardRepo.Setup(m => m.GetById(existingBoard.Id)).Returns(existingBoard);

            uut.PutBoard(existingBoard.Id, data);

            Assert.That(updatedBoard, Is.Not.Null);
            Assert.That(updatedBoard.BoardColumns.Count, Is.EqualTo(1));
            Assert.That(updatedBoard.BoardColumns.First(), Is.EqualTo(existingColumn));
            Assert.That(updatedBoard.BoardColumns.First().Name, Is.EqualTo("Updated name"));
            Assert.That(updatedBoard.BoardColumns.First().Index, Is.EqualTo(5));
        }

        [Test]
        public void DeleteBoard()
        {
            var existingBoard = entityFactory.Boards[MockEntityFactory.BoardType.BackendDev];
            mockBoardRepo.Setup(m => m.GetById(existingBoard.Id)).Returns(existingBoard);

            uut.DeleteBoard(existingBoard.Id);

            mockBoardRepo.Verify(m => m.Delete(existingBoard.Id), Times.Once);
            mockBoardRepo.Verify(m => m.Save(), Times.Once);
        }
    }
}
