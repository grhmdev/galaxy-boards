using GalaxyBoardsAPI.Controllers.DTOs;
using GalaxyBoardsAPI.Data.Entities;

namespace GalaxyBoardsAPI.Controllers.Dtos
{
    public class TicketDetail : TicketBrief
    {
        public string description { get; set; }

        public TicketDetail(Ticket entity) : base(entity)
        {
            description = entity.Description;
        }
    }
}
