using System.Windows.Input;
using TaskManagement.Infrastructure.Utils;
using TaskManagment.Infrastructure.Services;

namespace TaskManagement.Infrastructure.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authenticationManager;
        private readonly INavigationService _navigationService;

        private string _error;

        public LoginViewModel(
            IAuthenticationService authenticationManager,
            INavigationService navigationService)
        {
            _authenticationManager = authenticationManager;
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
                var isAuthenticated =
                    await _authenticationManager.LoginAsync(Login, Password);

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

