using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using TaskManagement.Infrastructure.ViewModels;
using TaskManagment.Infrastructure.ViewModels;

namespace TaskManagementWin.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
            DataContext = ((App)App.Current).Services.GetRequiredService<RegisterViewModel>();
        }
    }
}
