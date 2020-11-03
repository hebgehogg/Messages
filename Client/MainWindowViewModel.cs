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
            
            
            
            SignInCommand = new DelegateCommand(SignIn);
            LogInCommand = new DelegateCommand(LogIn);
            LogOutCommand = new DelegateCommand(LogOut);
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void SignIn(object parameter)
        {
            var loginDialog = new InputLoginDialogWindow();
            loginDialog.Close += SignInClose;
            await DialogHost.Show( loginDialog);
        }

        private async void SignInClose(object sender, string login)
        {
            DialogHost.CloseDialogCommand.Execute(null,null);
            var loginDialog = (InputLoginDialogWindow) sender;
            loginDialog.Close -= SignInClose;
            await _serverApiService.RegisterAsync(login);
        }
        
        private void LogIn(object parameter)
        {

        }
        
        private void LogOut(object parameter)
        {
        
        }
        
        private async void GetLoginAsync(string login)
        {
            
        }
    }
}