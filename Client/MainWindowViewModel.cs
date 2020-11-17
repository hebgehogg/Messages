using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using System.Windows.Input;
using Client.Annotations;
using Client.Services;
using MaterialDesignThemes.Wpf;
using Messages;

namespace Client
{
    public sealed class MainWindowViewModel : INotifyPropertyChanged
    {
        [NotNull] private readonly ServerApiService _serverApiService;
        private bool _isLogIn;
        private string _mainLogin=null;
        private string _sessionKey=null;
        private DateTime _dateTo;
        private DateTime _dateFrom;
        private IEnumerable<HardwareConfig> _dataGridDate;

        public DateTime DateFrom
        {
            get => _dateFrom;
            set
            {
                _dateFrom = value; 
                OnPropertyChanged();
            }
        }

        public DateTime DateTo
        {
            get => _dateTo;
            set
            {
                _dateTo = value; 
                OnPropertyChanged();
            }
        }
        
        public IEnumerable<HardwareConfig> DataGridDate
        {
            get => _dataGridDate;
            set
            {
                _dataGridDate = value; 
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LogInCommand { get;}
        public ICommand SignInCommand { get;}
        public ICommand LogOutCommand { get;}
        public ICommand SaveConfigCommand { get;}
        public ICommand GetConfigByPeriodCommand { get;}

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
            LogOutCommand = new DelegateCommand(LogOutAsync);
            SaveConfigCommand = new DelegateCommand(SaveConfigAsync); 
            GetConfigByPeriodCommand = new DelegateCommand(GetConfigByPeriodAsync); 
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
            
            if (string.IsNullOrWhiteSpace(_mainLogin))
                return;
            await _serverApiService.RegisterAsync(_mainLogin);
        }

        private async void LogInAsync(object parameter)
        {
            var loginDialog = new InputLoginDialogWindow();
            loginDialog.Close += LogInClose;
            await DialogHost.Show(loginDialog);
            
            loginDialog.Close -= LogInClose;
            
            if (string.IsNullOrWhiteSpace(_mainLogin))
                return;
            var result  = await _serverApiService.LogInAsync(_mainLogin);
            _sessionKey = result.key;
            
            IsLogIn = true;
        }
        
        private async void LogOutAsync(object parameter)
        {
            if (string.IsNullOrWhiteSpace(_mainLogin))
                return;
            await _serverApiService.LogOutAsync(_sessionKey, _mainLogin);
            IsLogIn = false;
        }
        
        private async void SaveConfigAsync(object parameter)
        {
            if (string.IsNullOrWhiteSpace(_mainLogin))
                return;
            await _serverApiService.SaveConfigAsync(_sessionKey);
        }
        
        private async void GetConfigByPeriodAsync(object parameter)
        {
            if (string.IsNullOrWhiteSpace(_mainLogin))
                return;
            var result = await _serverApiService.GetListOfConfigByPeriodAsync(_sessionKey, DateFrom, DateTo);
            DataGridDate = result.Item2;
        }
        
        private async void RegistrationInClose(object sender, [NotNull] string login)
        {
            DialogHost.CloseDialogCommand.Execute(null,null);
            var loginDialog = (InputLoginDialogWindow) sender;
            loginDialog.Close -= RegistrationInClose;
            _mainLogin = login;
        }
        
        private async void LogInClose(object sender, [NotNull] string login)
        {
            DialogHost.CloseDialogCommand.Execute(null,null);
            var loginDialog = (InputLoginDialogWindow) sender;
            loginDialog.Close -= LogInClose;
            _mainLogin = login;
        }
    }
}