using System.ComponentModel.DataAnnotations;

namespace GalaxyBoardsAPI.Controllers.Dtos
{
    /// <summary>
    /// Represents a TicketPlacement entity. This describes a placement of a `Ticket` onto a `BoardColumn`.
    /// </summary>
    public class TicketPlacement
    {
        [Required]
        public Guid id { get; set; }
        [Required]
        public Guid ticketId { get; set; }
        [Required]
        public Guid boardColumnId { get; set; }
        [Required]
        public uint index { get; set; }

        public TicketPlacement(Data.Entities.TicketPlacement entity)
        {
            id = entity.Id;
            ticketId = entity.Ticket.Id;
            boardColumnId = entity.BoardColumn.Id;
            index = entity.Index;
        }
    }
}
