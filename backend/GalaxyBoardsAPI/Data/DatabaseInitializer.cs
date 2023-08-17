using GalaxyBoardsAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalaxyBoardsAPI.Data
{
    public class DatabaseInitializer
    {
        public static void Seed(GalaxyBoardsContext context)
        {
            context.Database.EnsureCreated();

            // Prevent re-seeding existing data
            if (context.Users.Any())
                return;

            var dummyEntityFactory = new MockEntityFactory();
            dummyEntityFactory.CreateEntities();

            context.Users.AddRange(dummyEntityFactory.Users);
            context.Projects.AddRange(dummyEntityFactory.Projects.Values);
            context.Boards.AddRange(dummyEntityFactory.Boards.Values);
            context.Tickets.AddRange(dummyEntityFactory.Tickets);
            context.BoardColumns.AddRange(dummyEntityFactory.BoardColumns.Values);

            context.SaveChanges();
        }
    }
}
