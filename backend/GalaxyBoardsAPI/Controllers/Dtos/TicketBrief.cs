using GalaxyBoardsAPI.Data.Entities;

namespace GalaxyBoardsAPI.Controllers.DTOs
{
    public class TicketBrief
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
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
