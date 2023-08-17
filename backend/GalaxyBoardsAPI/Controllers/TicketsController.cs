using System;
using Microsoft.AspNetCore.Mvc;
using GalaxyBoardsAPI.Data.Entities;
using GalaxyBoardsAPI.Data.Repository;
using GalaxyBoardsAPI.Controllers.DTOs;
using GalaxyBoardsAPI.Controllers.Dtos;
using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.IdentityModel.Tokens;
using GalaxyBoardsAPI.Data.Search;

namespace GalaxyBoardsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TicketsController : ControllerBase
    {
        private readonly IEntityRepository<Ticket> _ticketRepo;
        private readonly IEntitySearch<Ticket> _searchRepo;
        private readonly IEntityRepository<Project> _projectRepo;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(ILogger<TicketsController> logger, IEntityRepository<Ticket> ticketRepo, IEntityRepository<Project> projectRepo, IEntitySearch<Ticket> searchRepo)
        {
            _ticketRepo = ticketRepo;
            _projectRepo = projectRepo;
            _logger = logger;
            _searchRepo = searchRepo;
        }

        /// <summary>
        /// Queries stored tickets
        /// </summary>
        /// <param name="search">Filters tickets by those whose names or descriptions contain 1 or more of the provided search terms/words</param>
        /// <param name="orderBy">The ticket property to sort tickets by, before filtering</param>
        /// <param name="orderAscending">The sort direction to use when `orderBy` is provided. Set to `true` for ascending order. Defaults to descending order. </param>
        /// <param name="limit">The maximum tickets that will be returned. May be used for pagination when combined with `offset`.</param>
        /// <param name="offset">The tickets to skip. May be used for pagination when combined with `limit`.</param>
        /// <param name="projectId">Filters tickets by their associated project ID</param>
        /// <remarks>All parameters are optional. A request without any parameters set will return all stored tickets without any filtering or sorting applied. </remarks>
        /// <response code="200">Successful query</response>
        [HttpGet]
        public ActionResult<QueryResult<TicketBrief>> QueryTickets(string? search = null, string? orderBy = null, bool? orderAscending = false, int? limit = null, int? offset = null, string? projectId = null)
        {
            var response = new QueryResult<TicketBrief>
            {
                items = new List<TicketBrief>(),
                totalRecords = _ticketRepo.Count()
            };

            Expression<Func<Ticket, bool>>? filter = null;
            if (!projectId.IsNullOrEmpty())
            {
                filter = t => t.Project.Id.ToString().Equals(projectId);
            }

            if (search.IsNullOrEmpty())
            {
                response.items = _ticketRepo.Get(filter: filter, take: limit, skip: offset).Select(t => new TicketBrief(t));
            }
            else
            {
                var searchResults = _searchRepo.Search(search!, resultsToTake: int.MaxValue, resultsToSkip: 0, entityFilter: filter);
                response.items = searchResults.MatchedEntities.Select(t => new TicketBrief(t));
            }
            return response;
        }

        /// <summary>
        /// Looks up a ticket by ID
        /// </summary>
        /// <param name="id">ID of ticket to retrieve</param>
        /// <response code="200">Ticket found</response>
        /// <response code="404">Ticket not found</response>
        [HttpGet("{id}")]
        public ActionResult<TicketDetail> GetTicket(Guid id)
        {
            var board = _ticketRepo.GetById(id);
            if (board == null)
            {
                return NotFound();
            }

            var boardDto = new TicketDetail(board);
            return boardDto;
        }

        /// <summary>
        /// Updates an existing ticket
        /// </summary>
        /// <param name="id">ID of ticket to update</param>
        /// <param name="ticket">Ticket update data</param>
        /// <response code="200">Ticket successfully updated</response>
        /// <response code="400">Invalid payload data</response>
        /// <response code="404">Ticket not found</response>
        [HttpPut("{id}")]
        public IActionResult PutTicket(Guid id, TicketUpdate ticket)
        {
            if (!Enum.TryParse(ticket.status, out TicketStatus status))
            {
                return BadRequest();
            }

            var entity = _ticketRepo.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = ticket.name;
            entity.Status = status;
            entity.Description = ticket.description;
            _ticketRepo.Update(entity);
            _ticketRepo.Save();
            return Ok();
        }

        /// <summary>
        /// Creates a new ticket
        /// </summary>
        /// <param name="ticket">Ticket data</param>
        /// <response code="200">Successfully created ticket</response>
        [HttpPost]
        public ActionResult<PostResult> PostTicket(TicketPostData ticket)
        {
            if (!Enum.TryParse(ticket.status, out TicketStatus status))
            {
                return BadRequest();
            }

            var project = _projectRepo.GetById(ticket.projectId);
            if (project == null)
            {
                return BadRequest();
            }

            var entity = new Ticket
            {
                Id = Guid.NewGuid(),
                Name = ticket.name,
                Description = ticket.description,
                Status = status,
                Project = project,
            };
            _ticketRepo.Add(entity);
            _ticketRepo.Save();

            project.Tickets.Add(entity);
            _projectRepo.Update(project);
            _projectRepo.Save();

            return new PostResult
            {
                createdId = entity.Id
            };
        }

        /// <summary>
        /// Deletes a ticket by ID
        /// </summary>
        /// <param name="id">ID of the ticket to delete</param>
        /// <response code="200">Ticket successfully deleted</response>
        [HttpDelete("{id}")]
        public IActionResult DeleteTicket(Guid id)
        {
            _ticketRepo.Delete(id);
            _ticketRepo.Save();
            return Ok();
        }
    }
}
