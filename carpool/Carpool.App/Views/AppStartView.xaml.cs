using System.Windows;
using System.Windows.Controls;
using Carpool.App.ViewModels;

namespace Carpool.App.Views;

/// <summary>
///     Interaction logic for UserProfileWindow.xaml
/// </summary>
public partial class AppStartView
{
    public AppStartView(AppStartViewModel mainViewModel)
    {
        InitializeComponent();
        DataContext = mainViewModel;
    }

    private void UserDetailView_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }
}