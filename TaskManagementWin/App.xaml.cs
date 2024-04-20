using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using System.Windows.Navigation;
using TaskManagement.Infrastructure.ViewModels;
using TaskManagementWin.Pages;
using TaskManagment.Infrastructure.Models;
using TaskManagment.Infrastructure.Services;
using TaskManagment.Infrastructure.ViewModels;

namespace TaskManagementWin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public IServiceProvider Services { get; private set; }

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services
                        .RegisterServices()
                        .RegisterViewModels();

                    // navigation window
                    services.AddSingleton<NavigationWindow>();
                })
                .Build();

            Services = _host.Services;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            await _host.StartAsync();

            var navigationWidnow = Services.GetRequiredService<NavigationWindow>();
            navigationWidnow.Width = 400;
            navigationWidnow.Height = 600;
            navigationWidnow.Title = "Task Management App";

            MainWindow = navigationWidnow;

            var navigationService = Services.GetRequiredService<INavigationService>();

            var isAuthenticated = false;

            if (isAuthenticated)
            {
                await navigationService.GoToAsync(Route.Welcome);
            }
            else
            {
                await navigationService.GoToAsync(Route.Login);
            }

            MainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }

            base.OnExit(e);
        }
    }

    static class AppExtensions
    {
        internal static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IAuthenticationService, AuthenticationService>()
                .AddSingleton<INavigationService, Services.NavigationService>()
                .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

            return services;
        }

        internal static IServiceCollection RegisterViewModels(this IServiceCollection services)
        {
            services
                .AddTransient<LoginViewModel>()
                .AddTransient<RegisterViewModel>();

            return services;
        }
    }
}
