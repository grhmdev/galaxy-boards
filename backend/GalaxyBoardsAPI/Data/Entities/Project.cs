using GalaxyBoardsAPI.Data.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyBoardsAPI.Data.Entities
{
    [Table("Project")]
    public class Project
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(256)]
        public required string Name { get; set; }
        [Required]
        [StringLength(7)]
        public required string HexColorCode { get; set; }
        [Required]
        public virtual required ICollection<Ticket> Tickets { get; set; }
        [Required]
        public virtual required ICollection<Board> Boards { get; set; }
    }
}
