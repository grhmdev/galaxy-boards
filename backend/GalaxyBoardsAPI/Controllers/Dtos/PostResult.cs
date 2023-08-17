using System.ComponentModel.DataAnnotations;

namespace GalaxyBoardsAPI.Controllers.Dtos
{
    /// <summary>
    /// Data returned in response to a successful POST request. Contains the ID of the newly created entity.
    /// </summary>
    public class PostResult
    {
        [Required]
        public Guid createdId { get; set; }
    }
}
