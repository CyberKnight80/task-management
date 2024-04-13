
using Microsoft.Extensions.Logging;

using TaskManagementApp.User;

namespace TaskManagementApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        RegisterDependencies(builder);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        LoadUserData();

        Services = app.Services;

        return app;
    }

    private static void RegisterDependencies(MauiAppBuilder builder)
    {
        // TODO: specify dependencies
    }

    public static IServiceProvider Services { get; private set; }

    private static void LoadUserData()
    {
        UserData userData = UserDataManager.LoadUserData();

        if (userData is not null)
        {

        }
    }

    
}