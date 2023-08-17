using GalaxyBoardsAPI.Data.Entities;
using GalaxyBoardsAPI.Data.Repository;
using GalaxyBoardsAPI.Data.Search;
using Moq;

namespace GalaxyBoardsTests
{
    [TestFixture]
    public class EntitySearchTests
    {
        private readonly Mock<IEntityRepository<Ticket>> mockTicketRepo = new();

        private EntitySearch<Ticket> uut;
        private IList<Ticket> repoEntities;

        public EntitySearchTests()
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = "project1",
                HexColorCode = "#123456",
                Boards = new List<Board>(),
                Tickets = new List<Ticket>()
            };

            repoEntities = new List<Ticket>()
            {
                new Ticket()
                {
                    Id = Guid.NewGuid(),
                    Name = "ticket1",
                    Description = "foo",
                    Status = TicketStatus.Closed,
                    Project = project
                },
                new Ticket()
                {
                    Id = Guid.NewGuid(),
                    Name = "ticket2",
                    Description = "bar",
                    Status = TicketStatus.Open,
                    Project = project
                }
            };
        }

        [SetUp]
        public void Setup()
        {
            uut = new EntitySearch<Ticket>(mockTicketRepo.Object);
            mockTicketRepo.Reset();
        }

        // Non-matching search queries
        [TestCase("foob", 0, 0)]
        [TestCase("", 0, 0)]
        [TestCase("ticket3", 0, 0)]
        [TestCase(" ", 0, 0)]
        // Matching search queries
        [TestCase("ticket1", 1, 1, 0)]
        [TestCase("bar", 1, 1, 1)]
        [TestCase("1", 1, 1, 0)]
        [TestCase("ticket", 2, 2, 0, 1)]
        [TestCase("foo  bar", 2, 2, 0, 1)]
        [TestCase(" 1 2 ", 2, 2, 0, 1)]
        public void Search_QueryMatching(string searchQuery, int expectedResults, int expectedTotalResults, params int[] expectedIndexes)
        {
            int resultsToTake = repoEntities.Count;
            int resultsToSkip = 0;
            mockTicketRepo.SetupGet(repoEntities, repoEntities.Count);

            EntitySearchResults<Ticket> results = uut.Search(searchQuery, resultsToTake, resultsToSkip);

            Assert.That(results.MatchedEntities.Count, Is.EqualTo(expectedResults));
            Assert.That(results.TotalMatchedEntities, Is.EqualTo(expectedTotalResults));
            for (int i = 0; i < expectedIndexes.Length; ++i)
            {
                Assert.IsTrue(results.MatchedEntities.Contains(repoEntities[expectedIndexes[i]]));
            }
        }

        [TestCase(1, 0, 1, 2)]
        [TestCase(1, 1, 1, 2)]
        [TestCase(1, 2, 0, 2)]
        [TestCase(3, 0, 2, 2)]
        public void Search_TakeAndSkip(int resultsToTake, int resultsToSkip, int expectedResults, int expectedTotalResults)
        {
            string searchQuery = "foo bar";
            mockTicketRepo.SetupGet(repoEntities, repoEntities.Count);

            EntitySearchResults<Ticket> results = uut.Search(searchQuery, resultsToTake, resultsToSkip);

            Assert.That(results.MatchedEntities.Count, Is.EqualTo(expectedResults));
            Assert.That(results.TotalMatchedEntities, Is.EqualTo(expectedTotalResults));
        }

        [TestCase("foo bar bar", 1, 0)]
        [TestCase("bar foo foo", 0, 1)]
        [TestCase("ticket1 ticket2 bar", 1, 0)]
        [TestCase("ticket1 ticket2 foo", 0, 1)]
        [TestCase("1 1 2", 0, 1)]
        [TestCase("2 2 1", 1, 0)]
        public void Search_ResultOrdering(string searchQuery, params int[] expectedIndexes)
        {
            int resultsToTake = repoEntities.Count;
            int resultsToSkip = 0;
            int expectedResults = repoEntities.Count;
            int expectedTotalResults = repoEntities.Count;
            mockTicketRepo.SetupGet(repoEntities, repoEntities.Count);

            EntitySearchResults<Ticket> results = uut.Search(searchQuery, resultsToTake, resultsToSkip);

            Assert.That(results.MatchedEntities.Count, Is.EqualTo(expectedResults));
            Assert.That(results.TotalMatchedEntities, Is.EqualTo(expectedTotalResults));
            for (int i = 0; i < expectedIndexes.Length; ++i)
            {
                if (results.MatchedEntities.Count > i)
                {
                    Assert.That(results.MatchedEntities[i], Is.EqualTo(repoEntities[expectedIndexes[i]]));
                }
            }
        }
    }
}
