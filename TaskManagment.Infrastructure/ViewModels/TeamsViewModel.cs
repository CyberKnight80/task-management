using TaskManagement.Infrastructure.DataContracts;
using TaskManagement.Infrastructure.Services;

namespace TaskManagement.Infrastructure.ViewModels;

public class TeamsViewModel : BaseViewModel
{
    private readonly ApiClientService _apiClientService;
    private readonly INavigationService _navigationService;

    private TeamEntity _selectedTeam;

    public TeamsViewModel(
        ApiClientService apiClientService,
        INavigationService navigationService)
    {
        _apiClientService = apiClientService;
        _navigationService = navigationService;
    }

    public IEnumerable<TeamEntity> Teams { get; private set; }

    public TeamEntity? SelectedTeam
    {
        get => _selectedTeam;
        set
        {
            if (value is not null && SetField(ref _selectedTeam, value))
            {
                HandleSelectedItemChanged();
            }
        }
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        Teams = await _apiClientService.GetTeamsAsync(cancellationToken);
        OnPropertyChanged(nameof(Teams));
    }

    private async void HandleSelectedItemChanged()
    {
        try
        {
            await _navigationService.GoToAsync(Route.TeamDetails,
                new Dictionary<string, object>()
                {
                    { TeamDetailsViewModel.TeamIdQueryKey, _selectedTeam.Id }
                });
        }
        catch
        {
            // TODO: Handle exception here
        }
    }
}

