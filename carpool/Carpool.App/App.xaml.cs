using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using Carpool.App.Extensions;
using Carpool.App.Services;
using Carpool.App.Services.MessageDialog;
using Carpool.App.Settings;
using Carpool.App.ViewModels;
using Carpool.App.Views;
using Carpool.BL;
using Carpool.DAL;
using Carpool.DAL.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Carpool.App;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("cs");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("cs");

        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(ConfigureAppConfiguration)
            .ConfigureServices((context, services) => { ConfigureServices(context.Configuration, services); })
            .Build();
    }

    private static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
    {
        builder.AddJsonFile(@"AppSettings.json", false, false);
    }

    private static void ConfigureServices(IConfiguration configuration,
        IServiceCollection services)
    {
        services.AddBLServices();

        services.Configure<DALSettings>(configuration.GetSection("Carpool:DAL"));

        services.AddSingleton<IDbContextFactory<CarpoolDbContext>>(provider =>
        {
            var dalSettings = provider.GetRequiredService<IOptions<DALSettings>>().Value;
            return new SqlServerDbContextFactory(dalSettings.ConnectionString!,
                dalSettings.SkipMigrationAndSeedDemoData);
        });

        services.AddSingleton<AppStartView>();

        services.AddSingleton<IMessageDialogService, MessageDialogService>();
        services.AddSingleton<IMediator, Mediator>();

        services.AddSingleton<AppStartViewModel>();
        //services.AddSingleton<UserProfileWindowViewModel>();
        services.AddSingleton<ICarListViewModel, CarViewModel>();
        services.AddFactory<ICarDetailViewModel, CarDetailViewModel>();
        services.AddSingleton<IUserListViewModel, UserListViewModel>();
        services.AddFactory<IUserDetailViewModel, UserDetailViewModel>();
        services.AddSingleton<IRideListViewModel, RideListViewModel>();
        services.AddFactory<IRideDetailViewModel, RideDetailViewModel>();
        services.AddFactory<IRideSearchViewModel, RideSearchViewModel>();
        //services.AddFactory<IUserRideDetailViewModel, UserRideDetailViewModel>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        var dbContextFactory = _host.Services.GetRequiredService<IDbContextFactory<CarpoolDbContext>>();

        var dalSettings = _host.Services.GetRequiredService<IOptions<DALSettings>>().Value;

        await using (var dbx = await dbContextFactory.CreateDbContextAsync())
        {
            if (dalSettings.SkipMigrationAndSeedDemoData)
            {
                await dbx.Database.EnsureDeletedAsync();
                await dbx.Database.EnsureCreatedAsync();
            }
            else
            {
                await dbx.Database.MigrateAsync();
            }
        }

        var mainWindow = _host.Services.GetRequiredService<AppStartView>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync(TimeSpan.FromSeconds(5));
        }

        base.OnExit(e);
    }
}