using GalaxyBoardsAPI.Controllers.DTOs;
using GalaxyBoardsAPI.Data.Entities;

namespace GalaxyBoardsAPI.Controllers.Dtos
{
    public class BoardDetail : BoardBrief
    {
        public ProjectBrief project { get; set; }
        public IEnumerable<BoardColumn> columns { get; set; }
        public BoardDetail(Board entity) : base(entity)
        {
            columns = entity.BoardColumns.OrderBy(b => b.Index).Select(c => new BoardColumn(c));
            project = new ProjectBrief(entity.Project);
        }
    }
}
