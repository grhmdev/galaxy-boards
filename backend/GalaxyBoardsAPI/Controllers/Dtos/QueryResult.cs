using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GalaxyBoardsAPI.Controllers.Dtos
{
    /// <summary>
    /// Data returned in response to a GET request to queryable endpoints. Contains a list of records matching any given query parameters, and the total count.
    /// </summary>
    public class QueryResult<DTO>
    {
        [Required]
        public required IEnumerable<DTO> items { get; set; }
        [Required]
        public required int totalRecords { get; set; }
    }

}