using Carpool.App.ViewModels;

namespace Carpool.App.Views
{
    public partial class MainWindow
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
        }
    }
}