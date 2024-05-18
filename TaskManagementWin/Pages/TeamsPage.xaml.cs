using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManagement.Infrastructure.ViewModels;

namespace TaskManagementWin.Pages
{
    /// <summary>
    /// Interaction logic for TeamsPage.xaml
    /// </summary>
    public partial class TeamsPage : Page
    {
        private readonly TeamsViewModel _viewModel;

        public TeamsPage()
        {
            DataContext = _viewModel =
                ((App)App.Current).Services.GetRequiredService<TeamsViewModel>();
            InitializeComponent();
        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            try
            {
                await _viewModel.InitializeAsync();
            }
            catch
            {
                // TODO: handle this
            }
        }
    }
}
