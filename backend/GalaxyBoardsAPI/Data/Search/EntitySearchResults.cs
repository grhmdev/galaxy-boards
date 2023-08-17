using System;
using System.Collections.Generic;
using System.Text;

namespace GalaxyBoardsAPI.Data.Search
{
    public class EntitySearchResults<TEntity> where TEntity : class
    {
        public required IList<TEntity> MatchedEntities { get; set; }
        public required int TotalMatchedEntities { get; set; }
    }
}
