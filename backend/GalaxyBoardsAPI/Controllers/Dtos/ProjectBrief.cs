using GalaxyBoardsAPI.Data.Entities;

namespace GalaxyBoardsAPI.Controllers.DTOs
{
    public class ProjectBrief
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string hexColorCode { get; set; }
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
