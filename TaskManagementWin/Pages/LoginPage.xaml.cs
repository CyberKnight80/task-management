using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using TaskManagement.Infrastructure.ViewModels;

namespace TaskManagementWin.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            DataContext = ((App)App.Current).Services.GetRequiredService<LoginViewModel>();
        }
    }
}
