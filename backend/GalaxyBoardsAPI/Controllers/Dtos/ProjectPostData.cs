using System.ComponentModel.DataAnnotations;

namespace GalaxyBoardsAPI.Controllers.Dtos
{
    /// <summary>
    /// Required data for Project creation
    /// </summary>
    public class ProjectPostData
    {
        [Required]
        public required string name { get; set; }
        [Required]
        public required string hexColorCode { get; set; }
    }
}
