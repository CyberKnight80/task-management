namespace TaskManagement.Server.Models;

public class Team
{
    public Team()
    {
        Users = new HashSet<User>();
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<User> Users { get; set; }
}

