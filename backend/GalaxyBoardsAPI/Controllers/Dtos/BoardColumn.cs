using GalaxyBoardsAPI.Controllers.DTOs;
using System.ComponentModel.DataAnnotations;

namespace GalaxyBoardsAPI.Controllers.Dtos
{
    /// <summary>
    /// Represents a single column on a `Board`
    /// </summary>
    public class BoardColumn
    {
        [Required]
        public Guid id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public uint index { get;set; }
        [Required]
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
