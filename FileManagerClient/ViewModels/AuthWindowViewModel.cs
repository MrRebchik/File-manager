using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileManagerClient.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace FileManagerClient.ViewModels
{
    public partial class AuthWindowViewModel : ViewModelBase
    {
        private IAuthService authService;
        public AuthWindow view;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string username;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string password;
        [ObservableProperty]
        private string token;
        [ObservableProperty]
        private bool isIncorrectInput = false;
        private bool CanLogin => !(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password));
        public AuthWindowViewModel()
        {
            authService = ServiceProviderHolder.ServiceProvider.GetRequiredService<IAuthService>();
        }
        
        [RelayCommand (CanExecute = nameof(CanLogin))]
        private async void Login()
        {
            token = await authService.GetTokenAsync(username, password);

            if (!token.IsNullOrEmpty())
            {
                AuthService.Username = username;
                AuthService.Token = token;
                view.ChangeToMainWindow();
            }
            else
            {
                IsIncorrectInput = true;
            }
        }
        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async void Register()
        {
            token = await authService.GetTokenAsync(username, password, true);

            if (!token.IsNullOrEmpty())
            {
                AuthService.Username = username;
                AuthService.Token = token;
                view.ChangeToMainWindow();
            }
            else
            {
                IsIncorrectInput = true;
            }
        }
    }
}
