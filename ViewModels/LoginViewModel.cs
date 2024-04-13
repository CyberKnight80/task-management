using System.Windows.Input;
using TaskManagementApp.Utils;
using TaskManagementApp.User;
using System.Security.Cryptography;
using System.Text;

namespace TaskManagementApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            SignInCommand = new RelayCommand(HandleSignIn);
            GoToRegisterCommand = new Command(GoToRegister);
        }

        public string Login { get; set; }

        public string Password { get; set; }

        public ICommand SignInCommand { get; }

        public ICommand GoToRegisterCommand { get; }

        private async void GoToRegister()
        {
            await Shell.Current.GoToAsync("//register");
        }
        private void HandleSignIn()
        {
            var mainPage = Application.Current.MainPage;

            UserData userData = UserDataManager.LoadUserData();

            if (userData is not null)
            {
                bool isAuthenticated = VerifyPassword(Password, userData.PasswordHash, userData.Salt);

                if (isAuthenticated && Login == userData.Login)
                {
                    mainPage?.DisplayAlert("Success", "You're authorized", "Ok");
                }
                else
                {
                    mainPage?.DisplayAlert("Failure", "You're not authorized", "Ok");
                }
            }
            else
            { 
                mainPage?.DisplayAlert("Error", "User data not found", "Ok");
            }
        }

        private static bool VerifyPassword(string enteredPassword, string storedPasswordHash, string storedSalt)
        {
            string hashedEnteredPassword = HashPassword(enteredPassword, storedSalt);
            return hashedEnteredPassword == storedPasswordHash;
        }

        public static string HashPassword(string password, string salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(salt);

            // Объединение пароля и соли
            byte[] combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);

            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}

