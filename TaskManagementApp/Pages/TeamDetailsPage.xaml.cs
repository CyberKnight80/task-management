using TaskManagement.Infrastructure.ViewModels;

namespace TaskManagementApp.Pages;

[QueryProperty(nameof(TeamId), TeamDetailsViewModel.TeamIdQueryKey)]
public partial class TeamDetailsPage : ContentPage
{
    private readonly TeamDetailsViewModel _viewModel;

    public TeamDetailsPage()
    {
        InitializeComponent();
        BindingContext = _viewModel =
            MauiProgram.Services.GetRequiredService<TeamDetailsViewModel>();
    }

    public int TeamId { get; set; }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await _viewModel.InitializeAsync(TeamId);
        }
        catch
        {
            // TODO: handle this
        }
    }
}
