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
    /// Interaction logic for TeamDetailsPage.xaml
    /// </summary>
    public partial class TeamDetailsPage : Page
    {
        private readonly TeamDetailsViewModel _viewModel;
        private readonly int _teamId;

        public TeamDetailsPage(int teamId)
        {
            DataContext = _viewModel =
                ((App)App.Current).Services.GetRequiredService<TeamDetailsViewModel>();
            _teamId = teamId;
            InitializeComponent();
        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            try
            {
                await _viewModel.InitializeAsync(_teamId);
            }
            catch
            {
                // TODO: handle this
            }
        }
    }
}
