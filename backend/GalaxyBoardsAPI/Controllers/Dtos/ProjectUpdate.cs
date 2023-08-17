using System.ComponentModel.DataAnnotations;

namespace GalaxyBoardsAPI.Controllers.Dtos
{
    /// <summary>
    /// Project update data
    /// </summary>
    public class ProjectUpdate
    {
        [Required]
        public required string name { get; set; }
        [Required]
        public required string hexColorCode { get; set; }
    }
}
