using System;
using System.IO;
using System.Text.Json;

namespace TaskManagementApp.User
{

    public class UserData
    {
        public UserData(string login, string team, string passwordHash, string salt)
        {
            Login = login;
            Team = team;
            PasswordHash = passwordHash;
            Salt = salt;
        }

        public string Login { get; set; }
        public string Team { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        // Дополнительные данные пользователя могут быть добавлены здесь
    }

    

    public class UserDataManager
    {
        private static readonly string UserDataFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "userData.json");

        // Сохранение данных пользователя в файл
        public static void SaveUserData(UserData userData)
        {
            string json = JsonSerializer.Serialize(userData);
            File.WriteAllText(UserDataFilePath, json);
        }

        // Загрузка данных пользователя из файла
        public static UserData LoadUserData()
        {
            if (File.Exists(UserDataFilePath))
            {
                string json = File.ReadAllText(UserDataFilePath);
                return JsonSerializer.Deserialize<UserData>(json);
            }
            else
            {
                return null;
            }
        }
    }
}