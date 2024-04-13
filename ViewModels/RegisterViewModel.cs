using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManagementApp.Utils;
using TaskManagementApp.User;
using System.Security.Cryptography;

namespace TaskManagementApp.ViewModels
{
    internal class RegisterViewModel :BaseViewModel
    {
        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(HandleRegister);
        }

        public string Login { get; set; }

        public string Team { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public ICommand RegisterCommand { get; }

        private async void GoToLogin()
        {
            await Shell.Current.GoToAsync("//login");
        }

        private void HandleRegister()
        {
            var mainPage = Application.Current.MainPage;

            string salt = GenerateSalt(Password.Length);

            if (Password.Equals(ConfirmPassword))
            {
                var userData = new TaskManagementApp.User.UserData(Login, Team, HashPassword(Password, salt), salt);
                /*{
                    Username = Login,
                    Team = Team,
                    PasswordHash = HashPassword(Password), // Хешируем пароль
                    Salt = GenerateSalt() // Генерируем соль
                };*/

                // Сохраняем данные пользователя
                UserDataManager.SaveUserData(userData);
            }
            GoToLogin();
        }

        private static string GenerateSalt(int length)
        {
            byte[] salt = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        private static string HashPassword(string password, string salt)
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
