using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Infrastructure.DataContracts;
using TaskManagement.Server.Models;
using TaskManagement.Server.Services;
using TaskStatus = TaskManagement.Infrastructure.DataContracts.TaskStatus;

namespace TaskManagement.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Tasks/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskEntity>> GetTask(int id)
    {
        var task = await _context.Tasks
            .Include(t => t.AssignedUser)
            .Include(t => t.Owner)
            .Include(t => t.Team)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
        {
            return NotFound();
        }

        return MapTaskModelToContract(task);
    }

    // POST: api/Tasks
    [HttpPost]
    public async Task<ActionResult<TaskEntity>> CreateTask(CreateTaskRequest request)
    {
        if (_context.Tasks.Any(t => t.Title == request.Title))
        {
            return BadRequest("Task with the same title already exists");
        }

        User? user = null;

        if (HttpContext.User.Identity is ClaimsIdentity identity)
        {
            var username = identity.FindFirst(ClaimTypes.Name)?.Value;
            user = _context.Users.SingleOrDefault(u => u.Username == username);
        }

        if (user is null)
        {
            return Unauthorized();
        }

        var newTask = new Models.Task()
        {
            Title = request.Title,
            Description = request.Description,
            CreatedAt = DateTime.Now,
            Status = TaskStatus.New,
            OwnerId = user.Id,
        };

        _context.Tasks.Add(newTask);
        await _context.SaveChangesAsync();

        return MapTaskModelToContract(newTask);
    }

    // POST: api/Tasks/users/{userId}
    [HttpPost("{taskId}/users/{userId}")]
    public async Task<ActionResult<TaskEntity>> AssignTaskToUser(int taskId, int userId)
    {
        var task = await _context.Tasks
            .Include(t => t.Owner)
            .Include(t => t.AssignedUser)
            .Include(t => t.Team)
            .FirstOrDefaultAsync(t => t.Id == taskId);

        if (task is null)
        {
            return NotFound("Task is not found");
        }

        var user = await _context.Users.FindAsync(userId);

        if (user is null)
        {
            return NotFound("User is not found");
        }

        if (task.AssignedUserId == user.Id)
        {
            return Ok("Already assigned");
        }

        task.AssignedUserId = user.Id;
        await _context.SaveChangesAsync();

        return MapTaskModelToContract(task);
    }

    // DELETE: api/Tasks/users
    [HttpDelete("{taskId}/users")]
    public async Task<ActionResult<TaskEntity>> UnassignUserFromTask(int taskId)
    {
        var task = await _context.Tasks
            .Include(t => t.Owner)
            .Include(t => t.AssignedUser)
            .Include(t => t.Team)
            .FirstOrDefaultAsync(t => t.Id == taskId);

        if (task is null)
        {
            return NotFound("Task is not found");
        }

        if (task.AssignedUserId is null)
        {
            return Ok("Already unassigned");
        }

        task.AssignedUserId = null;
        await _context.SaveChangesAsync();

        return MapTaskModelToContract(task);
    }

    // POST: api/Tasks/teams/{teamId}
    [HttpPost("{taskId}/teams/{teamId}")]
    public async Task<ActionResult<TaskEntity>> AssignTaskToTeam(int taskId, int teamId)
    {
        var task = await _context.Tasks
            .Include(t => t.Owner)
            .Include(t => t.AssignedUser)
            .Include(t => t.Team)
            .FirstOrDefaultAsync(t => t.Id == taskId);

        if (task is null)
        {
            return NotFound("Task is not found");
        }

        var team = await _context.Teams.FindAsync(teamId);

        if (team is null)
        {
            return NotFound("Team is not found");
        }

        if (task.TeamId == team.Id)
        {
            return Ok("Already assigned");
        }

        task.TeamId = team.Id;
        await _context.SaveChangesAsync();

        return MapTaskModelToContract(task);
    }

    // DELETE: api/Tasks/users
    [HttpDelete("{taskId}/teams")]
    public async Task<ActionResult<TaskEntity>> UnassignTeamFromTask(int taskId)
    {
        var task = await _context.Tasks
            .Include(t => t.Owner)
            .Include(t => t.AssignedUser)
            .Include(t => t.Team)
            .FirstOrDefaultAsync(t => t.Id == taskId);

        if (task is null)
        {
            return NotFound("Task is not found");
        }

        if (task.TeamId is null)
        {
            return Ok("Already unassigned");
        }

        task.TeamId = null;
        await _context.SaveChangesAsync();

        return MapTaskModelToContract(task);
    }

    internal static TaskEntity MapTaskModelToContract(Models.Task task)
    {
        var assignedUser = task.AssignedUser is null ? null : new UserEntity()
        {
            Id = task.AssignedUser.Id,
            Name = task.AssignedUser.Username
        };

        var team = task.Team is null ? null : new TeamEntity()
        {
            Id = task.Team.Id,
            Name = task.Team.Name
        };

        var contractData = new TaskEntity()
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            CreatedAt = task.CreatedAt,
            DueDate = task.DueDate,
            Status = task.Status,
            Owner = new UserEntity()
            {
                Id = task.Owner.Id,
                Name = task.Owner.Username
            },
            AssignedUser = assignedUser,
            Team = team
        };

        return contractData;
    }

}
