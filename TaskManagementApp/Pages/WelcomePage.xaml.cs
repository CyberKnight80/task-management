using TaskManagement.Infrastructure.ViewModels;

namespace TaskManagementApp.Pages;

public partial class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        InitializeComponent();
        BindingContext = MauiProgram.Services.GetRequiredService<WelcomeViewModel>();
    }
}
