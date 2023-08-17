using System.ComponentModel.DataAnnotations;

namespace GalaxyBoardsAPI.Controllers.Dtos
{
    /// <summary>
    /// Ticket Placement update data
    /// </summary>
    public class TicketPlacementUpdate
    {
        /// <summary>
        /// Set to `null` to create a new ticket placement, or set to the ID of an existing ticket placement to update it.
        /// </summary>
        public required Guid? id { get; set; }
        [Required]
        public required uint index { get; set; }
        [Required]
        public required Guid ticketId { get; set; }
    }

    /// <summary>
    /// Board Column update data
    /// </summary>
    public class BoardColumnUpdate
    {
        /// <summary>
        /// Set to `null` to create a new column on the board, or set to the ID of an existing board column to update it.
        /// </summary>
        public Guid? id { get; set; }
        [Required]
        public required string name { get; set; }
        [Required]
        public required uint index { get; set; }
        [Required]
        public required IList<TicketPlacementUpdate> ticketPlacements { get; set; }
    }

    /// <summary>
    /// Board update data
    /// </summary>
    public class BoardUpdate
    {
        [Required]
        public required string name { get; set; }
        [Required]
        public required IList<BoardColumnUpdate> columns { get; set; }
    }
}
