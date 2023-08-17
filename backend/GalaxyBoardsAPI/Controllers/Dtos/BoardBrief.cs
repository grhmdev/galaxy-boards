using GalaxyBoardsAPI.Data.Entities;

namespace GalaxyBoardsAPI.Controllers.DTOs
{
    public class BoardBrief
    {
        public Guid id { get; set; }
        public string name { get; set; }
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
