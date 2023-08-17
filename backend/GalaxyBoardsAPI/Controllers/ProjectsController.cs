using System;
using Microsoft.AspNetCore.Mvc;
using GalaxyBoardsAPI.Data.Entities;
using GalaxyBoardsAPI.Data.Repository;
using GalaxyBoardsAPI.Controllers.DTOs;
using GalaxyBoardsAPI.Controllers.Dtos;

namespace GalaxyBoardsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IEntityRepository<Project> _projectRepo;
        private readonly IEntityRepository<Ticket> _ticketRepo;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(ILogger<ProjectsController> logger, IEntityRepository<Project> projectRepo, IEntityRepository<Ticket> ticketRepo)
        {
            _projectRepo = projectRepo;
            _logger = logger;
            _ticketRepo = ticketRepo;
        }

        // GET: api/projects
        [HttpGet]
        public ActionResult<QueryResult<ProjectBrief>> QueryProjects()
        {
            return new QueryResult<ProjectBrief>
            {
                totalRecords = _projectRepo.Count(),
                items = _projectRepo.Get(orderBy: projects => projects.OrderBy(p => p.Name)).Select(p => new ProjectBrief(p))
            };
        }

        // GET: api/projects/5
        [HttpGet("{id}")]
        public ActionResult<ProjectDetail> GetProject(Guid id)
        {
            var project = _projectRepo.GetById(id);
            if (project == null)
            {
                return NotFound();
            }

            var projectDto = new ProjectDetail(project);
            return projectDto;
        }

        // PUT: api/projects/5
        [HttpPut("{id}")]
        public IActionResult PutProject(Guid id, ProjectUpdate updated)
        {
            var project = _projectRepo.GetById(id);
            if (project == null)
            {
                return NotFound();
            }
            project.Name = updated.name;
            project.HexColorCode = updated.hexColorCode;
            _projectRepo.Update(project);
            _projectRepo.Save();
            return Ok();
        }

        // POST: api/projects
        [HttpPost]
        public ActionResult<PostResult> PostProject(ProjectPostData project)
        {
            var newProject = new Project
            {
                Id = Guid.NewGuid(),
                Name = project.name,
                HexColorCode = project.hexColorCode,
                Tickets = new List<Ticket>(),
                Boards = new List<Board>()
            };
            _projectRepo.Add(newProject);
            _projectRepo.Save();

            return new PostResult
            {
                createdId = newProject.Id
            };
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProject(Guid id)
        {
            var projectTickets = _ticketRepo.Get(filter: t => t.Project.Id == id).ToList();
            projectTickets.ForEach(t =>
            {
                _ticketRepo.Delete(t.Id);
            });
            _projectRepo.Delete(id);
            _ticketRepo.Save();
            _projectRepo.Save();
            return Ok();
        }
    }
}
