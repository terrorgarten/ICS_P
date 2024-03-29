﻿using System.Windows;
using System.Windows.Controls;
using Carpool.App.ViewModels;

namespace Carpool.App.Views;

public abstract class UserControlBase : UserControl
{
    protected UserControlBase()
    {
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is IListViewModel viewModel) await viewModel.LoadAsync();
    }
}