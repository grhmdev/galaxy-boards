using GalaxyBoardsAPI.Data.Entities;

namespace GalaxyBoardsAPI.Controllers.DTOs
{
    public class ProjectDetail : ProjectBrief
    {
        public IEnumerable<TicketBrief> tickets { get; set; }
        public IEnumerable<BoardBrief> boards { get; set; }

        public ProjectDetail(Project entity) : base(entity)
        {
            boards = entity.Boards.Select(b => new BoardBrief(b));
            tickets = entity.Tickets.Select(t => new TicketBrief(t));
        }
    }
}
