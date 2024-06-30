using Investment.Models.Entities;
using Investment.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Investment.Models.Dtos;

namespace Investment.API.Controllers
{
    [ApiController]
    [Route("v1")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost("project")]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDto projectDto)
        {
            if (projectDto == null)
            {
                return BadRequest("Project data is required.");
            }

            var project = new Project
            {
                Name = projectDto.Name,
                TargetAmount = projectDto.TargetAmount,
                CurrentAmount = 0, // Initialize with 0 as it's a new project
                Description = projectDto.Description,
            };

            var createdProject = await _projectService.AddProjectAsync(project);
            return CreatedAtAction(nameof(GetProjectById), new { id = createdProject.Id }, createdProject);
        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _projectService.GetProjectsAsync();
            return Ok(projects);
        }

        [HttpPost("invest")]
        [Authorize]
        public async Task<IActionResult> Invest([FromBody] InvestTransactDto investTransactDto)
        {
            if (investTransactDto == null)
            {
                return BadRequest("Investment data is required.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Invalid user ID in token.");
            }

            try
            {
                await _projectService.InvestInProjectAsync(investTransactDto.ProjectId, userId, investTransactDto.Amount);
                return Ok("Investment successful.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("project/{id}")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound("Project not found.");
            }
            return Ok(project);
        }
    }
}
