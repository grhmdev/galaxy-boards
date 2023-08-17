using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalaxyBoardsAPI.Controllers.Dtos
{
    public class QueryResult<DTO>
    {
        public required IEnumerable<DTO> items { get; set; }
        public required int totalRecords { get; set; }
    }

}