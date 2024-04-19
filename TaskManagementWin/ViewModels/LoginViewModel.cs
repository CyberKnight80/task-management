using System.Windows.Input;
using System.Windows;
using TaskManagementWin.Utils;

namespace TaskManagementWin.ViewModels
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
            var registartionWnd = new RegistrationWindow();
            registartionWnd.ShowDialog();
        }
    }
}

