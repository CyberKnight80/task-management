using System.Windows.Input;
using TaskManagementApp.Utils;

namespace TaskManagementApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            SignInCommand = new RelayCommand(HandleSignIn);
        }

        public string Login { get; set; }

        public string Password { get; set; }

        public ICommand SignInCommand { get; }

        private void HandleSignIn()
        {
            var mainPage = Application.Current.MainPage;

            if (Login is "test" && Password is "12345")
            {
                mainPage?.DisplayAlert("Success", "You're authorized", "Ok");
            }
            else
            {
                mainPage?.DisplayAlert("Failure", "You're not authorized", "Ok");
            }
        }
    }
}

