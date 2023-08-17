using GalaxyBoardsAPI.Data.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyBoardsAPI.Data.Entities
{
    public enum TicketStatus
    {
        Closed,
        Open
    }

    [Table("Ticket")]
    public class Ticket : ISearchQueryableEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(256)]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public TicketStatus Status { get; set; }
        [Required]
        public virtual required Project Project { get; set; }

        /// <summary>
        /// Returns selected string data that we want the
        /// entity to be discovered by when the user enters
        /// a search query.
        /// </summary>
        public IEnumerable<string> GetSearchQueryableStrings()
        {
            var queryableStrings = new List<string>
            {
                Name
            };
            queryableStrings.AddRange(Description.Split(new char[','], StringSplitOptions.RemoveEmptyEntries));
            return queryableStrings;
        }
    }
}
