using System.Windows.Input;
using TaskManagement.Infrastructure.Utils;
using TaskManagement.Infrastructure.Services;

namespace TaskManagement.Infrastructure.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IAuthenticationService _authenticationService;

        private string _error;

        public LoginViewModel(
            IAuthenticationService authenticationService,
            INavigationService navigationService)
        {
            _authenticationService = authenticationService;
            _navigationService = navigationService;

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

                var isAuthenticated = await _authenticationService
                    .AuthenticateAsync(login: Login, password: Password);

                if (isAuthenticated)
                {
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

