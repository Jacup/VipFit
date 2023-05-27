namespace VipFit
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.UI.Xaml;
    
    using System.Configuration;
    using System.IO;
    using VipFit.Activation;
    using VipFit.Contracts.Services;
    using VipFit.Core.Contracts.Services;
    using VipFit.Core.DataAccessLayer;
    using VipFit.Core.DataAccessLayer.Interfaces;
    using VipFit.Core.Services;
    using VipFit.Models;
    using VipFit.Notifications;
    using VipFit.Services;
    using VipFit.ViewModels;
    using VipFit.Views;
    using Windows.Storage;

    /// <summary>
    /// App class.
    /// </summary>
    public partial class App : Application
    {
        private static readonly string DbName = "vipfit_sqlite.db";
        private static readonly string Dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);

        // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            InitializeComponent();

            Host = Microsoft.Extensions.Hosting.Host
                .CreateDefaultBuilder()
                .UseContentRoot(AppContext.BaseDirectory)
                .ConfigureServices((context, services) =>
                {
                    // Default Activation Handler
                    services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

                    // Other Activation Handlers
                    services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();

                    // Services
                    services.AddSingleton<IAppNotificationService, AppNotificationService>();
                    services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
                    services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
                    services.AddTransient<INavigationViewService, NavigationViewService>();

                    services.AddSingleton<IActivationService, ActivationService>();
                    services.AddSingleton<IPageService, PageService>();
                    services.AddSingleton<INavigationService, NavigationService>();

                    // Database
                    string appDirectory = VipFitContext.GetApplicationDirectory();

                    services.AddDbContext<VipFitContext>(
                        options => options.UseSqlite($@"Data Source={appDirectory}\vf_db.db;"));

                    services.AddScoped<IClientRepository, ClientRepository>();
                    services.AddScoped<IPassTemplateRepository, PassTemplateRepository>();
                    services.AddScoped<IPassRepository, PassRepository>();
                    services.AddScoped<IEntryRepository, EntryRepository>();
                    services.AddScoped<IPaymentRepository, PaymentRepository>();

                    // Core Services
                    services.AddSingleton<IFileService, FileService>();

                    // Views and ViewModels
                    services.AddTransient<SettingsViewModel>();
                    services.AddTransient<SettingsPage>();

                    services.AddSingleton<ClientListViewModel>();
                    services.AddTransient<ClientListPage>();

                    services.AddSingleton<PassTemplateListViewModel>();
                    services.AddTransient<PassTemplateListPage>();

                    services.AddSingleton<PassListViewModel>();
                    services.AddTransient<PassListPage>();

                    services.AddSingleton<EntryListViewModel>();
                    services.AddTransient<EntryListPage>();

                    services.AddSingleton<PaymentListViewModel>();
                    services.AddTransient<ClientPaymentsPage>();

                    services.AddTransient<MainViewModel>();
                    services.AddTransient<MainPage>();

                    services.AddTransient<ShellPage>();
                    services.AddTransient<ShellViewModel>();

                    // Configuration
                    services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
                })
                .Build();

            GetService<IAppNotificationService>().Initialize();
            GetService<VipFitContext>().Initialize();

            UnhandledException += App_UnhandledException;
        }

        /// <summary>
        /// Gets the MainWindow.
        /// </summary>
        public static WindowEx MainWindow { get; } = new MainWindow();

        /// <summary>
        /// Gets the host value.
        /// </summary>
        public IHost Host { get; }

        /// <summary>
        /// Gets the specified serviee.
        /// </summary>
        /// <typeparam name="T">Type of service.</typeparam>
        /// <returns>Service.</returns>
        /// <exception cref="ArgumentException">Thrown if service is not registered in App.</exception>
        public static T GetService<T>()
            where T : class
        {
            if ((Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
                throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");

            return service;
        }

        /// <summary>
        /// Invokes on application launch.
        /// </summary>
        /// <param name="args">Launch args.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            //App.GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));

            await GetService<IActivationService>().ActivateAsync(args);
        }

        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // TODO: Log and handle exceptions as appropriate.
            // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
        }
    }
}