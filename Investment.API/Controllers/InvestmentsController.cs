using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EasyNetQ;
using Investment.Models.Dtos;
using Investment.Models.Entities;
using Investment.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Messages;

namespace Investment.Controllers
{
    [ApiController]
    [Route("v1")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IBus _bus;

        public ProjectController(IProjectService projectService, IBus bus)
        {
            _projectService = projectService;
            _bus = bus;
        }

        [HttpPost("project")]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDto projectDto)
        {
            if (projectDto == null)
            {
                return BadRequest("Project data is required.");
            }
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            // Read the token and extract claims
            var handler = new JwtSecurityTokenHandler();
            if (handler.ReadToken(token) is JwtSecurityToken jsonToken)
            {
                var nameIdClaim =
                    jsonToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.NameId);

                if (nameIdClaim == null)
                {
                    return Unauthorized("User ID not found in token.");
                }

                if (!int.TryParse(nameIdClaim.Value, out var userId))
                {
                    return BadRequest("Invalid user ID in token.");
                }
                //Where do I create the object body.
                var project = new Project
                {
                    Name = projectDto.Name,
                    UserId = userId,
                    TargetAmount = projectDto.TargetAmount,
                    CurrentAmount = 0, // Initialize with 0 as it's a new project
                    Description = projectDto.Description,
                };
                var createdProject = await _projectService.AddProjectAsync(project);
                return CreatedAtAction(nameof(GetProjectById), new { id = createdProject.Id }, createdProject);
            }

            return BadRequest("Invalid Token");

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

            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            // Read the token and extract claims
            var handler = new JwtSecurityTokenHandler();
            if (handler.ReadToken(token) is JwtSecurityToken jsonToken)
            {
                var nameIdClaim =
                    jsonToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.NameId);

                if (nameIdClaim == null)
                {
                    return Unauthorized("User ID not found in token.");
                }

                if (!int.TryParse(nameIdClaim.Value, out var userId))
                {
                    return BadRequest("Invalid user ID in token.");
                }
                try
                {
                    var investMessage = new InvestMessage
                    {
                        userId = userId,
                        amount = investTransactDto.Amount
                    };
                    Console.WriteLine($"Sent message: {investMessage}");
                    var success = await _bus.Rpc.RequestAsync<InvestMessage, bool>(investMessage);
                    if (success)
                    {
                        await _projectService.InvestInProjectAsync(investTransactDto.ProjectId, userId, investTransactDto.Amount);
                        return Ok("Investment successful.");
                    }
                    return StatusCode(402, "Insufficient funds for investment.");
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

            return BadRequest("Invalid Token");
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
