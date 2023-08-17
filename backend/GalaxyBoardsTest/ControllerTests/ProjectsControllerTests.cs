using GalaxyBoardsAPI.Controllers;
using GalaxyBoardsAPI.Controllers.Dtos;
using GalaxyBoardsAPI.Data;
using GalaxyBoardsAPI.Data.Entities;
using GalaxyBoardsAPI.Data.Repository;
using Microsoft.Extensions.Logging;
using Moq;

namespace GalaxyBoardsTests.ControllerTests
{
    [TestFixture]
    public class ProjectsControllerTests
    {
        private readonly Mock<IEntityRepository<Project>> mockProjectRepo = new();
        private readonly Mock<IEntityRepository<Ticket>> mockTicketRepo = new();
        private readonly Mock<ILogger<ProjectsController>> mockLogger = new();

        private ProjectsController uut;
        private MockEntityFactory entityFactory;

        public ProjectsControllerTests()
        {
        }

        [SetUp]
        public void Setup()
        {
            uut = new ProjectsController(mockLogger.Object, mockProjectRepo.Object, mockTicketRepo.Object);
            mockProjectRepo.Reset();
            mockTicketRepo.Reset();
            entityFactory = new MockEntityFactory();
            entityFactory.CreateEntities();
        }

        [Test]
        public void GetProject_ShouldReturnNotFoundForUnknownProjectId()
        {
            Guid projectId = Guid.NewGuid();
            mockProjectRepo.Setup(m => m.GetById(projectId)).Returns((Project?)null);

            var result = uut.GetProject(projectId);

            Assert.That(result.Value, Is.Null);
        }

        [Test]
        public void GetProject_ShouldReturnProjectDetailForStoredProjectId()
        {
            var project = entityFactory.Projects[MockEntityFactory.ProjectType.GalaxyBoardsDevelopment];
            mockProjectRepo.Setup(m => m.GetById(project.Id)).Returns(project);

            var result = uut.GetProject(project.Id);

            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.id, Is.EqualTo(project.Id));
        }

        [Test]
        public void GetProject_ProjectDetailShouldContainBoards()
        {
            var project = entityFactory.Projects[MockEntityFactory.ProjectType.GalaxyBoardsDevelopment];
            mockProjectRepo.Setup(m => m.GetById(project.Id)).Returns(project);

            var result = uut.GetProject(project.Id);

            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.boards.Count, Is.EqualTo(project.Boards.Count));
        }

        [Test]
        public void GetProject_ProjectDetailShouldContainTickets()
        {
            var project = entityFactory.Projects[MockEntityFactory.ProjectType.GalaxyBoardsDevelopment];
            mockProjectRepo.Setup(m => m.GetById(project.Id)).Returns(project);

            var result = uut.GetProject(project.Id);

            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.tickets.Count, Is.EqualTo(project.Tickets.Count));
        }

        [Test]
        public void QueryProjects_ShouldReturnAllStoredProjects()
        {
            var projects = entityFactory.Projects.Values.ToList();
            mockProjectRepo.SetupGet(projects, projects.Count);
            mockProjectRepo.Setup(m => m.Count()).Returns(projects.Count);

            var result = uut.QueryProjects();

            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.totalRecords, Is.EqualTo(projects.Count));
            Assert.That(result.Value.items.Count, Is.EqualTo(projects.Count));
            projects.ForEach(project =>
            {
                Assert.That(result.Value.items.Where(p => p.id == project.Id).Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void PostProject_ShouldAddNewProjectToRepo()
        {
            var data = new ProjectPostData { hexColorCode = "#123456", name = "My Project" };
            Project? createdProject = null;
            mockProjectRepo.Setup(m => m.Add(It.IsAny<Project>())).Callback<Project>(p => createdProject = p);

            var result = uut.PostProject(data);

            Assert.That(createdProject, Is.Not.Null);
            Assert.That(createdProject.Name, Is.EqualTo("My Project"));
            Assert.That(createdProject.HexColorCode, Is.EqualTo("#123456"));
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.createdId, Is.EqualTo(createdProject.Id));
            mockProjectRepo.Verify(m => m.Save(), Times.Once);
        }

        [Test]
        public void PutProject_ShouldUpdateExistingProjectInRepo()
        {
            var data = new ProjectUpdate { hexColorCode = "#0A0B0C", name = "Updated Project" };
            Project? updatedProject = null;
            mockProjectRepo.Setup(m => m.Update(It.IsAny<Project>())).Callback<Project>(p => updatedProject = p);
            var existingProject = entityFactory.Projects[MockEntityFactory.ProjectType.GalaxyBoardsDevelopment];
            mockProjectRepo.Setup(m => m.GetById(existingProject.Id)).Returns(existingProject);

            uut.PutProject(existingProject.Id, data);

            Assert.That(updatedProject, Is.Not.Null);
            Assert.That(updatedProject.Id, Is.EqualTo(existingProject.Id));
            Assert.That(updatedProject.Name, Is.EqualTo("Updated Project"));
            Assert.That(updatedProject.HexColorCode, Is.EqualTo("#0A0B0C"));
            mockProjectRepo.Verify(m => m.Save(), Times.Once);
        }

        [Test]
        public void DeleteProject_ShouldAlsoDeleteProjectTickets()
        {
            var existingProject = entityFactory.Projects[MockEntityFactory.ProjectType.GalaxyBoardsDevelopment];
            mockTicketRepo.SetupGet(existingProject.Tickets, existingProject.Tickets.Count);

            uut.DeleteProject(existingProject.Id);

            existingProject.Tickets.ToList().ForEach(ticket =>
            {
                mockTicketRepo.Verify(m => m.Delete(ticket.Id), Times.Once);
            });
            mockProjectRepo.Verify(m => m.Delete(existingProject.Id), Times.Once);
            mockProjectRepo.Verify(m => m.Save(), Times.Once);
            mockTicketRepo.Verify(m => m.Save(), Times.Once);
        }
    }
}
