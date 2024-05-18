using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Infrastructure.DataContracts;
using TaskManagement.Server.Models;
using TaskManagement.Server.Services;

namespace TaskManagement.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TeamsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Teams
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamEntity>>> GetTeams()
    {
        var teams = await _context.Teams
            .Select(t => MapModelToContract(t))
            .ToListAsync();

        return teams;
    }

    // GET: api/Teams/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetTeamResponse>> GetTeam(int id)
    {
        var team = await _context.Teams
            .Include(t => t.Users)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (team == null)
        {
            return NotFound();
        }

        return MapModelToContract(team);
    }

    // POST: api/Teams
    [HttpPost]
    public async Task<ActionResult<GetTeamResponse>> PostTeam(Team Team)
    {
        if (_context.Teams.Any(t => t.Name == Team.Name))
        {
            return BadRequest("Team with the same name already exists");
        }

        var team = new Team { Name = Team.Name };
        _context.Teams.Add(team);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, Team);
    }

    // PUT: api/Teams/5
    [HttpPut("{id}")]
    public async Task<ActionResult<GetTeamResponse>> PutTeam(int id, Team Team)
    {
        if (id != Team.Id)
        {
            return BadRequest();
        }

        var team = await _context.Teams.FindAsync(id);
        if (team == null)
        {
            return NotFound();
        }

        team.Name = Team.Name;
        _context.Entry(team).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Teams.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return MapModelToContract(team);
    }

    // DELETE: api/Teams/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<GetTeamResponse>> DeleteTeam(int id)
    {
        var team = await _context.Teams.FindAsync(id);
        if (team == null)
        {
            return NotFound();
        }

        _context.Teams.Remove(team);
        await _context.SaveChangesAsync();

        return MapModelToContract(team);
    }

    [HttpPost("{teamId}/users/{userId}")]
    public async Task<ActionResult<GetTeamResponse>> AddUserToTeam(int teamId, int userId)
    {
        var team = await _context.Teams
            .Include(t => t.Users)
            .FirstOrDefaultAsync(t => t.Id == teamId);

        if (team == null)
        {
            return NotFound($"Team with ID {teamId} not found.");
        }

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        if (!team.Users.Contains(user))
        {
            team.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        return Ok(MapModelToContract(team));
    }

    [HttpDelete("{teamId}/users/{userId}")]
    public async Task<ActionResult<GetTeamResponse>> RemoveUserFromTeam(int teamId, int userId)
    {
        var team = await _context.Teams
            .Include(t => t.Users)
            .FirstOrDefaultAsync(t => t.Id == teamId);

        if (team == null)
        {
            return NotFound($"Team with ID {teamId} not found.");
        }

        var user = team.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return NotFound($"User with ID {userId} not found in team.");
        }

        team.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok(MapModelToContract(team));
    }

    private static GetTeamResponse MapModelToContract(Team team)
    {
        var contractData = new GetTeamResponse
        {
            Id = team.Id,
            Name = team.Name,
            Users = team.Users.Select(u => new UserEntity
            {
                Id = u.Id,
                Name = u.Username
            })
        };

        return contractData;
    }
        
}
