namespace GalaxyBoardsAPI.Controllers.Dtos
{
    public class BoardPostDataColumn
    {
        public required string name { get; set; }
        public required uint index { get; set; }
    }

    public class BoardPostData
    {
        public required string name { get; set; }
        public required IList<BoardPostDataColumn> columns { get; set; }
        public required Guid projectId { get; set; }
    }
}
