using GalaxyBoardsAPI.Controllers.DTOs;
using GalaxyBoardsAPI.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace GalaxyBoardsAPI.Controllers.Dtos
{
    /// <summary>
    /// Describes a Board entity in full
    /// </summary>
    public class BoardDetail : BoardBrief
    {
        [Required]
        public ProjectBrief project { get; set; }
        [Required]
        public IEnumerable<BoardColumn> columns { get; set; }
        public BoardDetail(Board entity) : base(entity)
        {
            columns = entity.BoardColumns.OrderBy(b => b.Index).Select(c => new BoardColumn(c));
            project = new ProjectBrief(entity.Project);
        }
    }
}
