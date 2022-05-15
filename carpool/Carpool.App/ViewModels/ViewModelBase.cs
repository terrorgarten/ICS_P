using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Carpool.App.ViewModels;

public abstract class ViewModelBase : IViewModel, INotifyPropertyChanged
{
    protected ViewModelBase()
    {
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            // ReSharper disable once VirtualMemberCallInConstructor
            LoadInDesignMode();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public virtual void LoadInDesignMode()
    {
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}