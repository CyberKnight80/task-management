
namespace TaskManagement.Infrastructure.DataContracts;

public class GetTeamResponse
{
    public int Id { get; set; }

    public string Name { get; set; }

    public IEnumerable<UserEntity> Users { get; set; }
}

