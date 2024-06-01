using TaskStatus = TaskManagement.Infrastructure.DataContracts.TaskStatus;

namespace TaskManagement.Server.Models;

public class Task
{
    public int Id { get; set; } 
    public required string Title { get; set; } 
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } 
    public DateTime? DueDate { get; set; }

    public TaskStatus Status { get; set; } 

    public required int OwnerId { get; set; }
    public User Owner { get; set; }

    public int? AssignedUserId { get; set; }
    public User? AssignedUser { get; set; }

    /// <summary>
    /// Can be optional - means it's a personal task of <see cref="Owner"/>
    /// </summary>
    public int? TeamId { get; set; }
    public Team? Team { get; set; }
}

