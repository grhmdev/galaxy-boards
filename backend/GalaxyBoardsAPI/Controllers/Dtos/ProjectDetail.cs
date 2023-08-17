using GalaxyBoardsAPI.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace GalaxyBoardsAPI.Controllers.DTOs
{
    /// <summary>
    /// Describes a Project entity in full
    /// </summary>
    public class ProjectDetail : ProjectBrief
    {
        [Required]
        public IEnumerable<TicketBrief> tickets { get; set; }
        [Required]
        public IEnumerable<BoardBrief> boards { get; set; }

        public ProjectDetail(Project entity) : base(entity)
        {
            boards = entity.Boards.Select(b => new BoardBrief(b));
            tickets = entity.Tickets.Select(t => new TicketBrief(t));
        }
    }
}
