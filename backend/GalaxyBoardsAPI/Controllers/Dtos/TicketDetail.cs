using GalaxyBoardsAPI.Controllers.DTOs;
using GalaxyBoardsAPI.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace GalaxyBoardsAPI.Controllers.Dtos
{
    /// <summary>
    /// Describes a Ticket entity in full
    /// </summary>
    public class TicketDetail : TicketBrief
    {
        [Required]
        public string description { get; set; }

        public TicketDetail(Ticket entity) : base(entity)
        {
            description = entity.Description;
        }
    }
}
