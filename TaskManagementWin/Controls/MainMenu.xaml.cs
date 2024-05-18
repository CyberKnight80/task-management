using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using TaskManagementWin.ViewModels;

namespace TaskManagementWin.Controls
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
            DataContext = ((App)App.Current).Services.GetRequiredService<MainMenuViewModel>();
        }
    }
}
