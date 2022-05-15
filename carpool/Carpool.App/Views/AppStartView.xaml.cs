using System.Windows;
using System.Windows.Controls;
using Carpool.App.ViewModels;

namespace Carpool.App.Views;

public partial class AppStartView
{
    public AppStartView(AppStartViewModel mainViewModel)
    {
        InitializeComponent();
        DataContext = mainViewModel;
    }
}