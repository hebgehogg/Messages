using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;

namespace Client
{
    public partial class InputLoginDialogWindow : UserControl
    {
        public event EventHandler<string> Close;
        public InputLoginDialogWindow()
        {
            InitializeComponent();
        }

        protected virtual void OnClose(string e)
        {
            Close?.Invoke(this, e);
        }
        void OnClickAccept(object sender, RoutedEventArgs e)
        {
            var login = textbox.Text;
            OnClose(login);
        }
        void OnClickCancel(object sender, RoutedEventArgs e)
        {
            OnClose(null);
        }
    }
}