using GalaxyBoardsAPI.Controllers;
using GalaxyBoardsAPI.Controllers.Dtos;
using GalaxyBoardsAPI.Data;
using GalaxyBoardsAPI.Data.Entities;
using GalaxyBoardsAPI.Data.Repository;
using GalaxyBoardsAPI.Data.Search;
using Microsoft.Extensions.Logging;
using Moq;

namespace GalaxyBoardsTests.ControllerTests
{
    [TestFixture]
    public class TicketsControllerTests
    {
        private readonly Mock<IEntityRepository<Ticket>> mockTicketRepo = new();
        private readonly Mock<IEntityRepository<Project>> mockProjectRepo = new();
        private readonly Mock<IEntitySearch<Ticket>> mockSearchRepo = new();
        private readonly Mock<ILogger<TicketsController>> mockLogger = new();

        private TicketsController uut;
        private MockEntityFactory entityFactory;

        public TicketsControllerTests()
        {
        }

        [SetUp]
        public void Setup()
        {
            uut = new TicketsController(mockLogger.Object, mockTicketRepo.Object, mockProjectRepo.Object, mockSearchRepo.Object);
            mockTicketRepo.Reset();
            mockProjectRepo.Reset();
            mockSearchRepo.Reset();
            entityFactory = new MockEntityFactory();
            entityFactory.CreateEntities();
        }

        [Test]
        public void GetTicket_ShouldReturnNotFoundForUnknownId()
        {
            Guid ticketId = Guid.NewGuid();
            mockTicketRepo.Setup(m => m.GetById(ticketId)).Returns((Ticket?)null);

            var result = uut.GetTicket(ticketId);

            Assert.That(result.Value, Is.Null);
        }

        [Test]
        public void GetTicket_ShouldReturnTicketDetailForExistingTicketId()
        {
            var existingTicket = entityFactory.Tickets.First();
            mockTicketRepo.Setup(m => m.GetById(existingTicket.Id)).Returns(existingTicket);

            var result = uut.GetTicket(existingTicket.Id);

            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.id, Is.EqualTo(existingTicket.Id));
        }

        [Test]
        public void QueryTickets_ShouldReturnAllStoredTicketsWithoutQueryParams()
        {
            var tickets = entityFactory.Tickets;
            mockTicketRepo.SetupGet(tickets, tickets.Count);
            mockTicketRepo.Setup(m => m.Count()).Returns(tickets.Count);

            var result = uut.QueryTickets();

            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.totalRecords, Is.EqualTo(tickets.Count));
            Assert.That(result.Value.items.Count, Is.EqualTo(tickets.Count));
            tickets.ForEach(ticket =>
            {
                Assert.That(result.Value.items.Where(t => t.id == ticket.Id).Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void PostTicket_ShouldAddNewTicketToRepo()
        {
            var existingProject = entityFactory.Projects[MockEntityFactory.ProjectType.GalaxyBoardsDevelopment];
            mockProjectRepo.Setup(m => m.GetById(existingProject.Id)).Returns(existingProject);
            var data = new TicketPostData { name = "Ticket1", description = "Description1", projectId = existingProject.Id, status = TicketStatus.Open.ToString() };
            Ticket? createdTicket = null;
            mockTicketRepo.Setup(m => m.Add(It.IsAny<Ticket>())).Callback<Ticket>(p => createdTicket = p);

            var result = uut.PostTicket(data);

            Assert.That(createdTicket, Is.Not.Null);
            Assert.That(createdTicket.Name, Is.EqualTo("Ticket1"));
            Assert.That(createdTicket.Description, Is.EqualTo("Description1"));
            Assert.That(createdTicket.Status, Is.EqualTo(TicketStatus.Open));
            Assert.That(createdTicket.Project, Is.Not.Null);
            Assert.That(createdTicket.Project.Id, Is.EqualTo(existingProject.Id));
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.createdId, Is.EqualTo(createdTicket.Id));
            mockTicketRepo.Verify(m => m.Save(), Times.Once);
        }

        [Test]
        public void PutTicket_ShouldUpdateExistingTicketInRepo()
        {
            var data = new TicketUpdate { name = "Updated Ticket", description = "Updated description", status = TicketStatus.Closed.ToString() };
            Ticket? updatedTicket = null;
            mockTicketRepo.Setup(m => m.Update(It.IsAny<Ticket>())).Callback<Ticket>(t => updatedTicket = t);
            var existingTicket = entityFactory.Tickets.First();
            mockTicketRepo.Setup(m => m.GetById(existingTicket.Id)).Returns(existingTicket);

            uut.PutTicket(existingTicket.Id, data);

            Assert.That(updatedTicket, Is.Not.Null);
            Assert.That(updatedTicket.Id, Is.EqualTo(existingTicket.Id));
            Assert.That(updatedTicket.Name, Is.EqualTo("Updated Ticket"));
            Assert.That(updatedTicket.Description, Is.EqualTo("Updated description"));
            Assert.That(updatedTicket.Status, Is.EqualTo(TicketStatus.Closed));
            mockTicketRepo.Verify(m => m.Save(), Times.Once);
        }

        [Test]
        public void DeleteTicket()
        {
            var existingTicket = entityFactory.Tickets.First();

            uut.DeleteTicket(existingTicket.Id);

            mockTicketRepo.Verify(m => m.Delete(existingTicket.Id), Times.Once);
            mockTicketRepo.Verify(m => m.Save(), Times.Once);
        }
    }
}
