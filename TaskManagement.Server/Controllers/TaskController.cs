using Microsoft.AspNetCore.Mvc;
using TaskManagement.Server.Models;
using System.Collections.Generic;
using System.Linq;
using TaskManagement.Infrastructure.DataContracts;
using TaskManagement.Server.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TaskManagement.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        //[HttpPost("CreateTask")]
        //public IActionResult CreateTask([FromBody] TaskManagement.Server.Models.Task task)
        //{
        //    if (task == null || string.IsNullOrWhiteSpace(task.Title) || task.Owner == null)
        //    {
        //        return BadRequest("The task, task title, or task owner is null.");
        //    }

        //    task.Id = Tasks.Count > 0 ? Tasks.Max(t => t.Id) + 1 : 1;
        //    Tasks.Add(task);
        //    return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        //}

        [HttpPost("CreateTask")]
        public IActionResult CreateTask([FromBody] PostTeamResponse task)
        {
            if (task == null || string.IsNullOrWhiteSpace(task.Title))
            {
                return BadRequest("The task, task title, or task owner is null.");
            }

            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                var username = identity.FindFirst(ClaimTypes.Name)?.Value;
                var user = _context.Users.SingleOrDefault(u => u.Username == username);

                if (user is null)
                {
                    return Unauthorized();
                }
                _context.Tasks.Add(new Models.Task() { Title = task.Title, OwnerId = user.Id});
                _context.SaveChanges();

                return Ok();
            }
            return NotFound();
        }

        [HttpGet("taskId")]
        public async Task<ActionResult<Models.Task>> GetTaskById(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }


        /* [HttpGet("{id}")]
         public IActionResult GetTaskById(int id)
         {
             var task = Tasks.FirstOrDefault(t => t.Id == id);
             if (task == null)
             {
                 return NotFound();
             }
             return Ok(task);
         }

         [HttpGet("team/{teamId}")]
         public IActionResult GetTasksByTeamId(int teamId)
         {
             var tasks = Tasks.Where(t => t.TeamId == teamId).ToList();
             return Ok(tasks);
         }

         [HttpPost("{id}/assign/user/{userId}")]
         public IActionResult AssignTaskToUser(int id, int userId)
         {
             var task = Tasks.FirstOrDefault(t => t.Id == id);
             if (task == null)
             {
                 return NotFound();
             }

             var user = Users.FirstOrDefault(u => u.Id == userId);
             if (user == null)
             {
                 return NotFound("User not found.");
             }

             task.AssignedUserId = userId;
             task.AssignedUser = user;
             return NoContent();
         }

         [HttpDelete("{id}/unassign/user")]
         public IActionResult UnassignTaskFromUser(int id)
         {
             var task = Tasks.FirstOrDefault(t => t.Id == id);
             if (task == null)
             {
                 return NotFound();
             }

             task.AssignedUserId = null;
             task.AssignedUser = null;
             return NoContent();
         }

         [HttpPost("{id}/assign/team/{teamId}")]
         public IActionResult AssignTaskToTeam(int id, int teamId)
         {
             var task = Tasks.FirstOrDefault(t => t.Id == id);
             if (task == null)
             {
                 return NotFound();
             }

             var team = Teams.FirstOrDefault(t => t.Id == teamId);
             if (team == null)
             {
                 return NotFound("Team not found.");
             }

             task.TeamId = teamId;
             task.Team = team;
             return NoContent();
         }

         [HttpDelete("{id}/unassign/team")]
         public IActionResult UnassignTaskFromTeam(int id)
         {
             var task = Tasks.FirstOrDefault(t => t.Id == id);
             if (task == null)
             {
                 return NotFound();
             }

             task.TeamId = null;
             task.Team = null;
             return NoContent();
         }*/
    }
}
