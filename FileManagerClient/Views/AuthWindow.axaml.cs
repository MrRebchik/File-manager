using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using FileManagerClient.Views;
using FileManagerClient.ViewModels;
using FileManagerClient.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FileManagerClient;

public partial class AuthWindow : Window
{
    public AuthWindow()
    {
        InitializeComponent();
        DataContext = new AuthWindowViewModel() { view = this };
    }
    public void ChangeToMainWindow()
    {
        var newWindow = new MainWindow() { DataContext = new MainWindowViewModel(ServiceProviderHolder.ServiceProvider.GetRequiredService<IStorageProviderService>()) };
        var appLifetime = Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

        if (appLifetime != null)
        {
            appLifetime.MainWindow = newWindow;
            newWindow.Show();
            this.Close();
        }
    }
}