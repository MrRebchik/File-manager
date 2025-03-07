using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using FileManagerClient.ViewModels;
using FileManagerClient.Views;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using FileManagerClient.Services;
using Avalonia.Controls;
using System;

namespace FileManagerClient
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();
            ServiceProviderHolder.ServiceProvider = serviceProvider;
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                DisableAvaloniaDataAnnotationValidation();
                desktop.MainWindow = new AuthWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
        private void ConfigureServices(IServiceCollection services)
        {

            services.AddHttpClient();

            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IStorageProviderService, StorageProviderService>();
            services.AddTransient<AuthWindow>();
            services.AddSingleton<MainWindow>();
        }
    }
    public static class ServiceProviderHolder
    {
        public static IServiceProvider ServiceProvider { get; set; }
    }
}