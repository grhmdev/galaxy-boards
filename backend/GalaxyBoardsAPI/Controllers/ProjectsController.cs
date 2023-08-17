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
    [Produces("application/json")]
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

        /// <summary>
        /// Lists stored projects
        /// </summary>
        [HttpGet]
        public ActionResult<QueryResult<ProjectBrief>> QueryProjects()
        {
            return new QueryResult<ProjectBrief>
            {
                totalRecords = _projectRepo.Count(),
                items = _projectRepo.Get(orderBy: projects => projects.OrderBy(p => p.Name)).Select(p => new ProjectBrief(p))
            };
        }

        /// <summary>
        /// Looks up a project by ID
        /// </summary>
        /// <param name="id">ID of project to find</param>
        /// <response code="200">Project found matching ID</response>
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

        /// <summary>
        /// Updates an existing project
        /// </summary>
        /// <param name="id">ID of the project to update</param>
        /// <param name="updated">Project update data</param>
        /// <response code="200">Project updated successfully</response>
        /// <response code="404">Project not found</response>
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

        /// <summary>
        /// Creates a new project
        /// </summary>
        /// <param name="project">Project data</param>
        /// <response code="200">Project successfully created</response>
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

        /// <summary>
        /// Deletes a project and all associated boards and tickets
        /// </summary>
        /// <param name="id">ID of project to delete</param>
        /// <response code="200">Project successfully deleted</response>
        /// <response code="404">Project not found</response>
        [HttpDelete("{id}")]
        public IActionResult DeleteProject(Guid id)
        {
            var project = _projectRepo.GetById(id);
            if (project == null)
            {
                return NotFound();
            }

            project.Tickets.ToList().ForEach(t =>
            {
                _ticketRepo.Delete(t.Id);
            });
            
            _projectRepo.Delete(id);
            _projectRepo.Save();
            return Ok();
        }
    }
}
