using System.Windows.Input;
using TaskManagement.Infrastructure.Utils;
using TaskManagement.Infrastructure.ViewModels;
using TaskManagment.Infrastructure.Services;

namespace TaskManagment.Infrastructure.ViewModels;

public class RegisterViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authenticationManager;
    private readonly INavigationService _navigationService;

    private string _error;

    public RegisterViewModel(
        IAuthenticationService authenticationManager,
        INavigationService navigationService)
    {
        _authenticationManager = authenticationManager;
        _navigationService = navigationService;

        RegisterCommand = new RelayCommand(HandleRegister);
    }

    public string Login { get; set; }

    public string Password { get; set; }

    public string Error
    {
        get => _error;
        set => SetField(ref _error, value);
    }

    public ICommand RegisterCommand { get; }

    public ICommand LoginCommand { get; }


    private async Task HandleRegister()
    {
        try
        {
            var isRegistered = await _authenticationManager
                .RegisterAsync(Login, Password);

            if (isRegistered)
            {
                Error = string.Empty;
                await _navigationService.GoBackAsync();
            }
        }
        catch
        {
            Error = "Please use another username to register";
        }
    }
}

