using GalaxyBoardsAPI.Controllers.DTOs;
using GalaxyBoardsAPI.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace GalaxyBoardsAPI.Controllers.Dtos
{
    /// <summary>
    /// Required data for Ticket creation
    /// </summary>
    public class TicketPostData
    {
        [Required]
        public required string name { get; set; }
        [Required]
        public required string status { get; set; }
        [Required]
        public required string description { get; set; }
        [Required]
        public Guid projectId { get; set; }
    }
}
