using System.Windows.Input;
using TaskManagement.Infrastructure.Utils;
using TaskManagment.Infrastructure.Services;

namespace TaskManagement.Infrastructure.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ApiClientService _apiClientService;

        private string _error;

        public LoginViewModel(
            ApiClientService apiClientService,
            INavigationService navigationService)
        {
            _navigationService = navigationService;
            _apiClientService = apiClientService;

            LoginCommand = new RelayCommand(HandleLoginAsync);
            RegisterCommand = new RelayCommand(HandleRegister);
        }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Error
        {
            get => _error;
            set => SetField(ref _error, value);
        }

        public ICommand LoginCommand { get; }

        public ICommand RegisterCommand { get; }

        private async Task HandleLoginAsync()
        {
            try
            {
                var tokenResponse = await _apiClientService
                    .LoginAsync(Login, Password);

                if (tokenResponse is { AccessToken: not null, RefreshToken: not null })
                {
                    // TODO: save tokens
                    Error = string.Empty;
                    await _navigationService.GoToAsync(Route.Welcome, keepHistory: false);
                }
            }
            catch
            {
                Error = "Authentication failed.\n" +
                    "Please check your username and password and try again.";
            }
        }

        private Task HandleRegister()
        {

            Error = string.Empty;
            return _navigationService.GoToAsync(Route.Register);
        }
    }
}

