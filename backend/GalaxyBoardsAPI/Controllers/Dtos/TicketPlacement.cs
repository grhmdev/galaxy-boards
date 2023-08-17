namespace GalaxyBoardsAPI.Controllers.Dtos
{
    public class TicketPlacement
    {
        public Guid id { get; set; }
        public Guid ticketId { get; set; }
        public Guid boardColumnId { get; set; }
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
