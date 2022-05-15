using System;
using System.Windows.Input;
using Carpool.App.Commands;
using Carpool.App.Messages;
using Carpool.App.Services;
using Carpool.App.Wrappers;

namespace Carpool.App.ViewModels;

public class AppStartViewModel : ViewModelBase
{
    public readonly IMediator _mediator;

    public AppStartViewModel(
        ICarListViewModel carListViewModel,
        IUserListViewModel userListViewModel,
        IRideListViewModel rideListViewModel,
        IRideDetailViewModel rideDetailViewModel,
        IRideSearchViewModel rideSearchViewModel,
        IUserDetailViewModel userDetailViewModel,
        IMediator mediator
    )
    {
        _mediator = mediator;
        CarListViewModel = carListViewModel;
        UserListViewModel = userListViewModel;
        RideListViewModel = rideListViewModel;
        RideDetailViewModel = rideDetailViewModel;
        RideSearchViewModel = rideSearchViewModel;
        UserDetailViewModel = userDetailViewModel;


        ReloadCommand = new RelayCommand(OnReload);

        Logout = new RelayCommand(OnLogout);

        mediator.Register<NewMessage<UserWrapper>>(OnUserNewMessage);
        mediator.Register<SelectedMessage<UserWrapper>>(OnUserSelected);
    }

    public int SelectedIndex { get; set; }

    public IRideDetailViewModel? RideDetailViewModel { get; set; }
    public ICarListViewModel CarListViewModel { get; }
    public IUserListViewModel UserListViewModel { get; }
    public IRideListViewModel RideListViewModel { get; }
    public IUserDetailViewModel UserDetailViewModel { get; }
    public IRideSearchViewModel RideSearchViewModel { get; }

    public ICommand Logout { get; }

    public ICommand ReloadCommand { get; }

    private void OnReload()
    {
        var save_tab = SelectedIndex;
        _mediator.Send(new ReloadMessage<UserWrapper>());
        SelectedIndex = save_tab;
    }

    private void OnLogout()
    {
        _mediator.Send(new SelectedMessage<UserWrapper> {Id = Guid.Empty});
    }


    private void OnUserNewMessage(NewMessage<UserWrapper> _)
    {
        SelectedIndex = 1;
    }

    private void OnUserSelected(SelectedMessage<UserWrapper> message)
    {
        if (message.Id == null || message.Id == Guid.Empty)
        {
            SelectedIndex = 0;
            return;
        }

        SelectedIndex = 1;
    }
}