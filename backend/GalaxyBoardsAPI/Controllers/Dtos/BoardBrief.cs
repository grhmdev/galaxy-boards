using GalaxyBoardsAPI.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace GalaxyBoardsAPI.Controllers.DTOs
{
    /// <summary>
    /// Describes a Board entity in brief
    /// </summary>
    public class BoardBrief
    {
        [Required]
        public Guid id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public IDictionary<string, string> links {  get; set; }

        public BoardBrief(Board entity)
        {
            id = entity.Id;
            name = entity.Name;
            links = new Dictionary<string, string>{
                {"detail", $"/api/boards/{entity.Id}" }
            };
        }
    }
}
