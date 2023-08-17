using GalaxyBoardsAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GalaxyBoardsAPI.Data
{
    /// <summary>
    /// The purpose of this class is simply to generate dummy entity data for initial dev database seeding
    /// and unit testing.
    /// </summary>
    public class MockEntityFactory
    {
        // Entities are stored in type maps, making it easier to setup relationships between
        // entities after constructing them, without relying on indexes.
        public enum ProjectType { GalaxyBoardsDevelopment, PortfolioSite };
        public enum BoardType { FrontendDev, BackendDev, Tasks };
        public enum BoardColumnType
        {
            FrontendDev_Backlog,
            FrontendDev_InProgress,
            FrontendDev_Merge,
            FrontendDev_Done,
            BackendDev_Backlog,
            BackendDev_InProgress,
            BackendDev_Merge,
            BackendDev_Done,
            Tasks_Todo,
            Tasks_Done
        };

        public List<User> Users { get; }
        public Dictionary<ProjectType, Project> Projects { get; }
        public List<Ticket> Tickets { get; }
        public List<TicketPlacement> TicketPlacements { get; }
        public Dictionary<BoardType, Board> Boards { get; }
        public Dictionary<BoardColumnType, BoardColumn> BoardColumns { get; }

        public MockEntityFactory()
        {
            Users = new List<User>();
            Projects = new Dictionary<ProjectType, Project>();
            Tickets = new List<Ticket>();
            TicketPlacements = new List<TicketPlacement>();
            Boards = new Dictionary<BoardType, Board>();
            BoardColumns = new Dictionary<BoardColumnType, BoardColumn>();
        }

        public void CreateEntities()
        {
            CreateProjects();
            CreateUsers();
            CreateBoards();
            CreateBoardColumns();
            CreateTickets();
        }

        private void CreateProjects()
        {
            CreateProject(ProjectType.GalaxyBoardsDevelopment, "Galaxy Boards Development", "#FF0000");
            CreateProject(ProjectType.PortfolioSite, "Portfolio Site", "#00FF00");
        }

        private void CreateProject(ProjectType type, string name, string hexColorCode)
        {
            Projects.Add(type, new Project
            {
                Id = Guid.NewGuid(),
                Name = name,
                HexColorCode = hexColorCode,
                Boards = new List<Board>(),
                Tickets = new List<Ticket>()
            });
        }

        private void CreateUsers()
        {
            Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Projects = Projects.Values
            });
        }

        private void CreateBoards()
        {
            CreateBoard(BoardType.FrontendDev, "Frontend Development", ProjectType.GalaxyBoardsDevelopment);
            CreateBoard(BoardType.BackendDev, "Backend Development", ProjectType.GalaxyBoardsDevelopment);
            CreateBoard(BoardType.Tasks, "Tasks", ProjectType.PortfolioSite);
        }

        private void CreateBoard(BoardType type, string name, ProjectType project)
        {
            Boards.Add(type, new Board
            {
                Id = Guid.NewGuid(),
                Name = name,
                BoardColumns = new List<BoardColumn>(),
                Project = Projects[project]
            });
        }

        private void CreateBoardColumns()
        {
            CreateBoardColumn(BoardType.FrontendDev, BoardColumnType.FrontendDev_Backlog, "Backlog", 0);
            CreateBoardColumn(BoardType.FrontendDev, BoardColumnType.FrontendDev_InProgress, "In Progress", 1);
            CreateBoardColumn(BoardType.FrontendDev, BoardColumnType.FrontendDev_Merge, "Merge", 2);
            CreateBoardColumn(BoardType.FrontendDev, BoardColumnType.FrontendDev_Done, "Done", 3);
            CreateBoardColumn(BoardType.BackendDev, BoardColumnType.BackendDev_Backlog, "Backlog", 0);
            CreateBoardColumn(BoardType.BackendDev, BoardColumnType.BackendDev_InProgress, "In Progress", 1);
            CreateBoardColumn(BoardType.BackendDev, BoardColumnType.BackendDev_Merge, "Merge", 2);
            CreateBoardColumn(BoardType.BackendDev, BoardColumnType.BackendDev_Done, "Done", 3);
            CreateBoardColumn(BoardType.Tasks, BoardColumnType.Tasks_Todo, "Todo", 0);
            CreateBoardColumn(BoardType.Tasks, BoardColumnType.Tasks_Done, "Done", 1);
        }

        private void CreateBoardColumn(BoardType board, BoardColumnType type, string name, uint index)
        {
            var column = new BoardColumn
            {
                Id = Guid.NewGuid(),
                Name = name,
                Index = index,
                TicketPlacements = new List<TicketPlacement>(),
            };
            BoardColumns.Add(type, column);
            Boards[board].BoardColumns.Add(column);
        }

        private void CreateTickets()
        {
            AddTicket("Display boards", "As a user, I want to be able to see all boards for the active project, so that I can easily see the status of all project tickets", TicketStatus.Open, ProjectType.GalaxyBoardsDevelopment, BoardColumnType.FrontendDev_Backlog);
            AddTicket("Board creation", "As a user, I want to be able to create new boards, so that I can add tickets for tracking", TicketStatus.Open, ProjectType.GalaxyBoardsDevelopment, BoardColumnType.FrontendDev_Backlog);
            AddTicket("Board editing", "As a user, I want to be able to modify existing board names and columns, so that I can update boards to align with evolving workflows", TicketStatus.Open, ProjectType.GalaxyBoardsDevelopment, BoardColumnType.FrontendDev_Backlog);
            AddTicket("Board drag-n-drop", "As a user, I want to be able to drag tickets across board columns, so that I can easily update their current state", TicketStatus.Open, ProjectType.GalaxyBoardsDevelopment, BoardColumnType.FrontendDev_Backlog);
            AddTicket("Cross-board drag-n-drop", "As a user, I want to be able to drag tickets onto boards from other boards or the ticket list, so that I can more easily organize the tickets", TicketStatus.Open, ProjectType.GalaxyBoardsDevelopment, BoardColumnType.FrontendDev_Backlog);
            AddTicket("Ticket creation", "As a user, I want to be able to add new tickets to the active project, so that I can add these to project boards for tracking", TicketStatus.Open, ProjectType.GalaxyBoardsDevelopment, BoardColumnType.FrontendDev_InProgress);
            AddTicket("Ticket editing", "As a user, I want to edit existing tickets, so that I can keep them up-to-date", TicketStatus.Open, ProjectType.GalaxyBoardsDevelopment, BoardColumnType.FrontendDev_InProgress);
            AddTicket("Project selection", "As a user, I want to select the active project, so that the view only shows tickets and boards for that project", TicketStatus.Closed, ProjectType.GalaxyBoardsDevelopment, BoardColumnType.FrontendDev_Done);
            AddTicket("Store user data", "As a user, I want the app to store my project data, so that I can access the data again from multiple devices and clients", TicketStatus.Open, ProjectType.GalaxyBoardsDevelopment, BoardColumnType.BackendDev_Backlog);
            AddTicket("Design REST API", "Design and document the REST API", TicketStatus.Open, ProjectType.GalaxyBoardsDevelopment, BoardColumnType.BackendDev_InProgress);
            AddTicket("Develop REST API", "Develop and test the API endpoints", TicketStatus.Open, ProjectType.GalaxyBoardsDevelopment, BoardColumnType.BackendDev_Backlog);
            AddTicket("Host portfolio site", "", TicketStatus.Open, ProjectType.PortfolioSite, BoardColumnType.Tasks_Todo);
            AddTicket("Host projects", "", TicketStatus.Open, ProjectType.PortfolioSite, BoardColumnType.Tasks_Todo);
            AddTicket("Write about projects", "", TicketStatus.Open, ProjectType.PortfolioSite, BoardColumnType.Tasks_Done);
        }

        private void AddTicket(string name, string description, TicketStatus status, ProjectType project, BoardColumnType boardColumn)
        {
            Ticket t = new Ticket
            {
                Id = Guid.NewGuid(),
                Name = name,
                Status = status,
                Description = description,
                Project = Projects[project]
            };
            Tickets.Add(t);
            Projects[project].Tickets.Add(t);
            TicketPlacement tp = new TicketPlacement
            {
                Id = Guid.NewGuid(),
                BoardColumn = BoardColumns[boardColumn],
                Index = (uint)BoardColumns[boardColumn].TicketPlacements.Count,
                Ticket = t
            };
            BoardColumns[boardColumn].TicketPlacements.Add(tp);
            TicketPlacements.Add(tp);
        }
    }
}
