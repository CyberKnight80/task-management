using System.Threading;
using System.Windows.Input;
using TaskManagement.Infrastructure.DataContracts;
using TaskManagement.Infrastructure.Helpers;
using TaskManagement.Infrastructure.Services;
using TaskManagement.Infrastructure.Utils;

namespace TaskManagement.Infrastructure.ViewModels;

public class TeamDetailsViewModel : BaseViewModel
{
    public const string TeamIdQueryKey = "teamId";

    public readonly ApiClientService _apiClientService;
    public readonly IAuthenticationService _authenticationService;

    public TeamDetailsViewModel(
        ApiClientService apiClientService,
        IAuthenticationService authenticationService)
    {
        _apiClientService = apiClientService;
        _authenticationService = authenticationService;

        JoinLeaveCommand = new RelayCommand(HandleJoinLeaveCommandAsync);
    }

    public GetTeamResponse Team { get; private set; }

    public IEnumerable<IndexedListItem> Members { get; private set; }

    public bool HasMembership { get; private set; }

    public ICommand JoinLeaveCommand { get; }

    public async Task InitializeAsync(int teamId,
        CancellationToken cancellationToken = default)
    {
        var teamResponse = await _apiClientService
            .GetTeamAsync(teamId, cancellationToken);

        Update(teamResponse);
    }

    private void Update(GetTeamResponse teamResponse)
    {
        Team = teamResponse;

        Members = Team.Users.Select((user, index) => new IndexedListItem()
        {
            Index = index + 1,
            Name = user.Name
        });

        HasMembership = Team.Users.Any(u => u.Id == _authenticationService.UserId);

        OnPropertyChanged(nameof(Team));
        OnPropertyChanged(nameof(Members));
        OnPropertyChanged(nameof(HasMembership));
    }

    private async Task HandleJoinLeaveCommandAsync()
    {
        var userId = _authenticationService.UserId ?? -1;

        var updatedTeamResponse = HasMembership
            ? await _apiClientService.RemoveUserFromTeamAsync(Team.Id, userId)
            : await _apiClientService.AddUserToTeamAsync(Team.Id, userId);

        Update(updatedTeamResponse);
    }
}

