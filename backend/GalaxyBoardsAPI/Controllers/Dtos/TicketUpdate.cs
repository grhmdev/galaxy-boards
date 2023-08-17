using GalaxyBoardsAPI.Controllers.DTOs;
using GalaxyBoardsAPI.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace GalaxyBoardsAPI.Controllers.Dtos
{
    /// <summary>
    /// Ticket update data
    /// </summary>
    public class TicketUpdate
    {
        [Required]
        public required string name { get; set; }
        [Required]
        public required string status { get; set; }
        [Required]
        public required string description { get; set; }
    }
}
