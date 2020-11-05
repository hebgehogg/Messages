using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using System.Windows.Input;
using Client.Annotations;
using Client.Services;
using MaterialDesignThemes.Wpf;

namespace Client
{
    public sealed class MainWindowViewModel : INotifyPropertyChanged
    {
        [NotNull] private readonly ServerApiService _serverApiService;
        private bool _isLogIn;
        private string mainLogin=null;
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LogInCommand { get;}
        public ICommand SignInCommand { get;}
        public ICommand LogOutCommand { get;}

        public bool IsLogIn
        {
            get => _isLogIn;
            set
            {
                _isLogIn = value; 
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel([NotNull] ServerApiService serverApiService)
        { 
            _serverApiService = serverApiService ?? throw new ArgumentNullException(nameof(serverApiService));
            
            SignInCommand = new DelegateCommand(RegistrationInAsync);
            LogInCommand = new DelegateCommand(LogInAsync);
            LogOutCommand = new DelegateCommand(LogOut);
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void RegistrationInAsync(object parameter)
        {
            var loginDialog = new InputLoginDialogWindow();
            loginDialog.Close += RegistrationInClose;
            await DialogHost.Show(loginDialog);
            
            loginDialog.Close -= RegistrationInClose;
            
            if (string.IsNullOrWhiteSpace(mainLogin))
                return;
            await _serverApiService.RegisterAsync(mainLogin);
        }

        private async void LogInAsync(object parameter)
        {
            var loginDialog = new InputLoginDialogWindow();
            loginDialog.Close += LogInClose;
            await DialogHost.Show(loginDialog);
            
            loginDialog.Close -= LogInClose;
            
            if (string.IsNullOrWhiteSpace(mainLogin))
                return;
            await _serverApiService.LogInAsync(mainLogin);
        }
        
        private void LogOut(object parameter)
        {
        
        }
        
        private async void RegistrationInClose(object sender, [NotNull] string login)
        {
            DialogHost.CloseDialogCommand.Execute(null,null);
            var loginDialog = (InputLoginDialogWindow) sender;
            loginDialog.Close -= RegistrationInClose;
            mainLogin = login;
        }
        
        private async void LogInClose(object sender, [NotNull] string login)
        {
            DialogHost.CloseDialogCommand.Execute(null,null);
            var loginDialog = (InputLoginDialogWindow) sender;
            loginDialog.Close -= LogInClose;
            mainLogin = login;
        }
    }
}