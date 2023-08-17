using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyBoardsAPI.Data.Entities
{
    [Table("Board")]
    public class Board
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(256)]
        public required string Name { get; set; }
        [Required]
        public virtual required Project Project { get; set; }
        [Required]
        public virtual required ICollection<BoardColumn> BoardColumns { get; set; }
    }
}
