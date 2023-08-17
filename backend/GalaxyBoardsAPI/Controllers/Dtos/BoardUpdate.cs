namespace GalaxyBoardsAPI.Controllers.Dtos
{
    public class TicketPlacementUpdate
    {
        public required Guid? id { get; set; }
        public required uint index { get; set; }
        public required Guid ticketId { get; set; }
    }

    public class BoardColumnUpdate
    {
        public Guid? id { get; set; }
        public required string name { get; set; }
        public required uint index { get; set; }
        public required IList<TicketPlacementUpdate> ticketPlacements { get; set; }
    }

    public class BoardUpdate
    {
        public required string name { get; set; }
        public required IList<BoardColumnUpdate> columns { get; set; }
    }
}
