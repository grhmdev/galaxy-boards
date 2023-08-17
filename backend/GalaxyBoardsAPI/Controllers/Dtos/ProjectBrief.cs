using GalaxyBoardsAPI.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace GalaxyBoardsAPI.Controllers.DTOs
{
    /// <summary>
    /// Describes a Project entity in brief
    /// </summary>
    public class ProjectBrief
    {
        [Required]
        public Guid id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string hexColorCode { get; set; }
        [Required]
        public IDictionary<string, string> links { get; set; }

        public ProjectBrief(Project entity)
        {
            id = entity.Id;
            name = entity.Name;
            hexColorCode = entity.HexColorCode;
            links = new Dictionary<string, string>{
                {"detail", $"/api/projects/{entity.Id}" }
            };
        }
    }
}
