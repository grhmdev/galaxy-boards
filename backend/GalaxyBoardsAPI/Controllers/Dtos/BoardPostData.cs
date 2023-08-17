using System.ComponentModel.DataAnnotations;

namespace GalaxyBoardsAPI.Controllers.Dtos
{
    /// <summary>
    /// Required data for Board Column creation
    /// </summary>
    public class BoardPostDataColumn
    {
        [Required]
        public required string name { get; set; }
        [Required]
        public required uint index { get; set; }
    }

    /// <summary>
    /// Required data for Board creation
    /// </summary>
    public class BoardPostData
    {
        [Required]
        public required string name { get; set; }
        [Required]
        public required IList<BoardPostDataColumn> columns { get; set; }
        [Required]
        public required Guid projectId { get; set; }
    }
}
