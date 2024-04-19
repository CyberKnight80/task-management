using System.Windows.Input;
using TaskManagement.Infrastructure.Utils;
using System.Windows;

namespace TaskManagement.Infrastructure.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _status;

        public LoginViewModel()
        {
            SignInCommand = new RelayCommand(HandleSignIn);
            SignUpCommand = new RelayCommand(HandleSignUp);
            Status = "Enter your login and password";
        }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Status
        {
            get => _status;
            set => SetField(ref _status, value);
        }

        public ICommand SignInCommand { get; }
        public ICommand SignUpCommand { get; }

        private void HandleSignIn()
        {
            if (Login is "test" && Password is "12345")
            {
                Status = "Success: You're authorized";
            }
            else
            {
                Status = "Failure: You're not authorized";
            }
        }

        private void HandleSignUp()
        {

        }
    }
}

