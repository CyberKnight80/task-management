using Microsoft.Extensions.Logging;
using TaskManagement.Infrastructure.ViewModels;
using TaskManagementApp.Services;
using TaskManagment.Infrastructure.Services;
using TaskManagment.Infrastructure.ViewModels;

namespace TaskManagementApp;

public static class MauiProgram
{
    public static IServiceProvider Services { get; private set; }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterServices()
            .RegisterViewModels();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        Services = app.Services;

        return app;
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton<ApiClientService>()
            .AddHttpClient();

        return builder;
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services
            .AddTransient<LoginViewModel>()
            .AddTransient<RegisterViewModel>();

        return builder;
    }
}