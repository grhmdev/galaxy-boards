using GalaxyBoardsAPI.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace GalaxyBoardsAPI.Controllers.DTOs
{
    /// <summary>
    /// Describes a Ticket entity in brief
    /// </summary>
    public class TicketBrief
    {
        [Required]
        public Guid id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string status { get; set; }
        [Required]
        public IDictionary<string, string> links { get; set; }

        public TicketBrief(Ticket entity)
        {
            id = entity.Id;
            name = entity.Name;
            status = entity.Status.ToString();
            links = new Dictionary<string, string>{
                {"detail", $"/api/tickets/{entity.Id}" }
            };
        }
    }
}
