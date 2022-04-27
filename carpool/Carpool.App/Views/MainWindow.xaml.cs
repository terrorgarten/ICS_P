using Carpool.App.ViewModels;

namespace Carpool.App.Views
{
    public partial class MainWindow
    {
        public MainWindow(UserProfileWindowViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
        }
    }
}