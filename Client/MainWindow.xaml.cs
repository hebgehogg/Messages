using System.Windows;
using Client.Services;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ServerApiService serverApiService = new ServerApiService();
            DataContext = new MainWindowViewModel(serverApiService);
        }
    }
}