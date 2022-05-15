using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Carpool.App.Commands;
using Carpool.App.Factories;
using Carpool.App.Messages;
using Carpool.App.Services;
using Carpool.App.Wrappers;

namespace Carpool.App.ViewModels;

public class AppStartViewModel : ViewModelBase
{
    private readonly IFactory<ICarDetailViewModel> _carDetailViewModelFactory; //DEL
    public readonly IMediator _mediator;
    private readonly IFactory<IUserDetailViewModel> _userDetailViewModelFactory;

    public AppStartViewModel(
        ICarListViewModel carListViewModel,
        IUserListViewModel userListViewModel,
        IRideListViewModel rideListViewModel,
        IRideDetailViewModel rideDetailViewModel,
        IRideSearchViewModel rideSearchViewModel,
        IMediator mediator,
        IFactory<ICarDetailViewModel> carDetailViewModelFactory,
        IFactory<IUserDetailViewModel> userDetailViewModelFactory
    )
    {
        _mediator = mediator;
        CarListViewModel = carListViewModel;
        UserListViewModel = userListViewModel;
        RideListViewModel = rideListViewModel;
        RideDetailViewModel = rideDetailViewModel;
        RideSearchViewModel = rideSearchViewModel;

        _userDetailViewModelFactory = userDetailViewModelFactory;
        UserDetailViewModel = _userDetailViewModelFactory.Create();
        _carDetailViewModelFactory = carDetailViewModelFactory;
        CarDetailViewModel = _carDetailViewModelFactory.Create();

        CloseUserDetailTabCommand = new RelayCommand<IUserDetailViewModel>(OnCloseUserDetailTabExecute);
        CloseCarDetailTabCommand = new RelayCommand<ICarDetailViewModel>(OnCloseCarDetailTabExecute);
        Logout = new RelayCommand(OnLogout);

        mediator.Register<NewMessage<UserWrapper>>(OnUserNewMessage);
        mediator.Register<SelectedMessage<UserWrapper>>(OnUserSelected);
        mediator.Register<DeleteMessage<UserWrapper>>(OnUserDeleted);
    }

    public int SelectedIndex { get; set; }

    public IRideDetailViewModel? RideDetailViewModel { get; set; }
    public ICarListViewModel CarListViewModel { get; }
    public IUserListViewModel UserListViewModel { get; }
    public IRideListViewModel RideListViewModel { get; }
    public IUserDetailViewModel UserDetailViewModel { get; }
    public ICarDetailViewModel CarDetailViewModel { get; }
    public IRideSearchViewModel RideSearchViewModel { get; }

    public ICommand CloseUserDetailTabCommand { get; }

    public ICommand CloseCarDetailTabCommand { get; }
    public ICommand Logout { get; }

    public ObservableCollection<IUserDetailViewModel> UserDetailViewModels { get; } = new();

    public ObservableCollection<ICarDetailViewModel> CarDetailViewModels { get; } = new();

    public IUserDetailViewModel? SelectedUserDetailViewModel { get; set; }

    public ICarDetailViewModel? SelectedCarDetailViewModel { get; set; }

    private void OnLogout()
    {
        _mediator.Send(new SelectedMessage<UserWrapper> { Id = Guid.Empty });
    }


    private void OnUserNewMessage(NewMessage<UserWrapper> _)
    {
        SelectedIndex = 1;
        SelectUser(Guid.Empty);
    }

    private void OnUserSelected(SelectedMessage<UserWrapper> message)
    {
        if (message.Id == null || message.Id == Guid.Empty)
        {
            SelectUser(message.Id);
            SelectedIndex = 0;
            return;
        }

        SelectedIndex = 1;
        SelectUser(message.Id);
    }

    private void OnUserDeleted(DeleteMessage<UserWrapper> message)
    {
        var user = UserDetailViewModels.SingleOrDefault(i => i.Model?.Id == message.Id);
        if (user != null) UserDetailViewModels.Remove(user);

        SelectedIndex = 0;
    }


    private void SelectUser(Guid? id)
    {
        if (id is null)
        {
            SelectedUserDetailViewModel = null;
        }

        else
        {
            var userDetailViewModel = UserDetailViewModels.SingleOrDefault(vm => vm.Model?.Id == id);
            if (userDetailViewModel == null)
            {
                userDetailViewModel = _userDetailViewModelFactory.Create();
                UserDetailViewModels.Add(userDetailViewModel);
                userDetailViewModel.LoadAsync(id.Value);
            }

            SelectedUserDetailViewModel = userDetailViewModel;
        }
    }

    private void OnCloseUserDetailTabExecute(IUserDetailViewModel? recipeDetailViewModel)
    {
        if (recipeDetailViewModel is not null)
            // TODO: Check if the Detail has changes and ask user to cancel
            UserDetailViewModels.Remove(recipeDetailViewModel);
    }

    private void OnCloseCarDetailTabExecute(ICarDetailViewModel? recipeDetailViewModel)
    {
        if (recipeDetailViewModel is not null)
            // TODO: Check if the Detail has changes and ask user to cancel
            CarDetailViewModels.Remove(recipeDetailViewModel);
    }
}