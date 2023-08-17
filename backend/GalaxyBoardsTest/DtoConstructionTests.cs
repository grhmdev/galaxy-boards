using GalaxyBoardsAPI.Controllers;
using GalaxyBoardsAPI.Controllers.Dtos;
using GalaxyBoardsAPI.Controllers.DTOs;
using GalaxyBoardsAPI.Data;
using GalaxyBoardsAPI.Data.Entities;
using GalaxyBoardsAPI.Data.Repository;
using GalaxyBoardsAPI.Data.Search;
using Microsoft.Extensions.Logging;
using Moq;

namespace GalaxyBoardsTests
{
    [TestFixture]
    public class DtoConstructionTests
    {
        private MockEntityFactory entityFactory;

        public DtoConstructionTests()
        {
        }

        [SetUp]
        public void Setup()
        {
            entityFactory = new MockEntityFactory();
            entityFactory.CreateEntities();
        }

        [Test]
        public void ConstructProjectBrief()
        {
            var projectEntity = entityFactory.Projects[MockEntityFactory.ProjectType.GalaxyBoardsDevelopment];

            var result = new ProjectBrief(projectEntity);

            Assert.That(result.id, Is.EqualTo(projectEntity.Id));
            Assert.That(result.name, Is.EqualTo(projectEntity.Name));
            Assert.That(result.hexColorCode, Is.EqualTo(projectEntity.HexColorCode));
        }

        [Test]
        public void ConstructProjectDetail()
        {
            var projectEntity = entityFactory.Projects[MockEntityFactory.ProjectType.GalaxyBoardsDevelopment];

            var result = new ProjectDetail(projectEntity);

            Assert.That(result.id, Is.EqualTo(projectEntity.Id));
            Assert.That(result.name, Is.EqualTo(projectEntity.Name));
            Assert.That(result.hexColorCode, Is.EqualTo(projectEntity.HexColorCode));
            Assert.That(result.tickets.Count, Is.EqualTo(projectEntity.Tickets.Count));
            Assert.That(result.boards.Count, Is.EqualTo(projectEntity.Boards.Count));
        }

        [Test]
        public void ConstructTicketBrief()
        {
            var ticketEntity = entityFactory.Tickets.First();

            var result = new TicketBrief(ticketEntity);

            Assert.That(result.id, Is.EqualTo(ticketEntity.Id));
            Assert.That(result.name, Is.EqualTo(ticketEntity.Name));
            Assert.That(result.status, Is.EqualTo(ticketEntity.Status.ToString()));
        }

        [Test]
        public void ConstructTicketDetail()
        {
            var ticketEntity = entityFactory.Tickets.First();

            var result = new TicketDetail(ticketEntity);

            Assert.That(result.id, Is.EqualTo(ticketEntity.Id));
            Assert.That(result.name, Is.EqualTo(ticketEntity.Name));
            Assert.That(result.status, Is.EqualTo(ticketEntity.Status.ToString()));
            Assert.That(result.description, Is.EqualTo(ticketEntity.Description));
        }

        [Test]
        public void ConstructBoardBrief()
        {
            var boardEntity = entityFactory.Boards[MockEntityFactory.BoardType.BackendDev];

            var result = new BoardBrief(boardEntity);

            Assert.That(result.id, Is.EqualTo(boardEntity.Id));
            Assert.That(result.name, Is.EqualTo(boardEntity.Name));
        }

        [Test]
        public void ConstructBoardDetail()
        {
            var boardEntity = entityFactory.Boards[MockEntityFactory.BoardType.BackendDev];

            var result = new BoardDetail(boardEntity);

            Assert.That(result.id, Is.EqualTo(boardEntity.Id));
            Assert.That(result.name, Is.EqualTo(boardEntity.Name));
            Assert.That(result.project.id, Is.EqualTo(boardEntity.Project.Id));
            Assert.That(result.columns.Count, Is.EqualTo(boardEntity.BoardColumns.Count));
        }

        [Test]
        public void ConstructBoardColumn()
        {
            var boardColumnEntity = entityFactory.BoardColumns[MockEntityFactory.BoardColumnType.BackendDev_Backlog];

            var result = new GalaxyBoardsAPI.Controllers.Dtos.BoardColumn(boardColumnEntity);

            Assert.That(result.id, Is.EqualTo(boardColumnEntity.Id));
            Assert.That(result.name, Is.EqualTo(boardColumnEntity.Name));
            Assert.That(result.index, Is.EqualTo(boardColumnEntity.Index));
            Assert.That(result.ticketPlacements.Count, Is.EqualTo(boardColumnEntity.TicketPlacements.Count));
        }

        [Test]
        public void ConstructTicketPlacement()
        {
            var ticketPlacementEntity = entityFactory.TicketPlacements.First();

            var result = new GalaxyBoardsAPI.Controllers.Dtos.TicketPlacement(ticketPlacementEntity);

            Assert.That(result.id, Is.EqualTo(ticketPlacementEntity.Id));
            Assert.That(result.index, Is.EqualTo(ticketPlacementEntity.Index));
            Assert.That(result.boardColumnId, Is.EqualTo(ticketPlacementEntity.BoardColumn.Id));
            Assert.That(result.ticketId, Is.EqualTo(ticketPlacementEntity.Ticket.Id));
        }
    }
}
