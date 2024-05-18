using TaskManagement.Infrastructure.ViewModels;

namespace TaskManagementApp.Pages;

public partial class TeamsPage : ContentPage
{
    private readonly TeamsViewModel _viewModel;

    public TeamsPage()
    {
        InitializeComponent();
        BindingContext = _viewModel =
            MauiProgram.Services.GetRequiredService<TeamsViewModel>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await _viewModel.InitializeAsync();
        }
        catch
        {
            // TODO: handle error here - suggest to update page
        }
    }
}
