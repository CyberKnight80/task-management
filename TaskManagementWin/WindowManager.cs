using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TaskManagementWin
{
    static class WindowManager
    {
        public static void OpenRegistrationWindow()
        {
            var window = new RegistrationWindow();
            window.ShowDialog();
        }
    }
}
