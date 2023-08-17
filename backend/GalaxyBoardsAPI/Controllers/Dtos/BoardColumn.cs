using GalaxyBoardsAPI.Controllers.DTOs;

namespace GalaxyBoardsAPI.Controllers.Dtos
{
    public class BoardColumn
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public uint index { get;set; }
        public IEnumerable<TicketPlacement> ticketPlacements {  get; set; }

        public BoardColumn(GalaxyBoardsAPI.Data.Entities.BoardColumn entity)
        {
            id = entity.Id;
            name = entity.Name;
            index = entity.Index;
            ticketPlacements = entity.TicketPlacements.OrderBy(tp => tp.Index).Select(tp => new TicketPlacement(tp)).ToList();
        }
    }
}
