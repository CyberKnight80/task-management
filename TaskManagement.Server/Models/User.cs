namespace TaskManagement.Server.Models;

public class User
{
    public User()
    {
        Teams = new HashSet<Team>();
        OwnedTasks = new HashSet<Task>();
        AssignedTasks = new HashSet<Task>();
    }

    public int Id { get; set; }

    public string Username { get; set; }

    public string PasswordHash { get; set; }

    public string Salt { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public ICollection<Team> Teams { get; set; }

    public ICollection<Task> OwnedTasks { get; set; }

    public ICollection<Task> AssignedTasks { get; set; }
}

