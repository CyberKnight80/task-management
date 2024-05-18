using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using TaskManagement.Infrastructure.ViewModels;
using TaskManagement.Infrastructure.Services;
using TaskManagementWin.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using TaskManagementWin.ViewModels;

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
                })
                .Build();

            Services = _host.Services;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            await _host.StartAsync();

            MainWindow = new MainWindow();

            await SetStartupPageAsync();

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

        private async Task SetStartupPageAsync()
        {
            var authenticationService = Services.GetRequiredService<IAuthenticationService>();
            var navigationService = Services.GetRequiredService<INavigationService>();

            var isAuthenticated = await authenticationService.CheckIsAuthenticatedAsync();

            var startupRoute = isAuthenticated
                ? Route.Welcome
                : Route.Login;

            await navigationService.GoToAsync(startupRoute, keepHistory: false);
        }
    }

    static class AppExtensions
    {
        internal static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<ISecureStorageService, SecureStorageService>()
                .AddSingleton<IAuthenticationService, AuthenticationService>()
                .AddSingleton<RefreshTokenHandler>()
                .AddSingleton<ApiClientService>(provider =>
                {
                    var logger = provider.GetRequiredService<ILogger<ApiClientService>>();
                    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                    return new (logger, httpClientFactory, "http://localhost:7071");
                })
                .AddHttpClient();

            services.AddHttpClient(ApiClientService.AutorizedHttpClient)
                .AddHttpMessageHandler<RefreshTokenHandler>();

            return services;
        }

        internal static IServiceCollection RegisterViewModels(this IServiceCollection services)
        {
            services
                .AddTransient<MainMenuViewModel>()
                .AddTransient<LoginViewModel>()
                .AddTransient<RegisterViewModel>()
                .AddTransient<TeamsViewModel>()
                .AddTransient<TeamDetailsViewModel>();

            return services;
        }
    }
}
