using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyBoardsAPI.Data.Entities
{
    [Table("BoardColumn")]
    public class BoardColumn
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(256)]
        public required string Name { get; set; }
        [Required]
        public required uint Index { get; set; }
        [Required]
        public virtual required ICollection<TicketPlacement> TicketPlacements{ get; set; }
    }
}
