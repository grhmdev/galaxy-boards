using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyBoardsAPI.Data.Entities
{
    [Table("TicketPlacement")]
    public class TicketPlacement
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public virtual required Ticket Ticket { get; set; }
        [Required]
        public virtual required BoardColumn BoardColumn { get; set; }
        [Required]
        public uint Index { get; set; }
    }
}
