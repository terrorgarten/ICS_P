using Carpool.App.Factories;
using Carpool.App.Messages;
using Carpool.App.Services;
using Carpool.App.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Carpool.App.Commands;
using Carpool.BL.Models;
using Carpool.Common.Enums;

namespace Carpool.App.ViewModels
{
    public class UserProfileWindowViewModel : ViewModelBase
    {
        //private readonly IFactory<IUserDetailViewModel> _userDetailViewModelFactory;

        public UserProfileWindowViewModel(
            ICarListViewModel carListViewModel,
            IUserListViewModel userListViewModel,
            IRideListViewModel rideListViewModel
            //IMediator mediator,
            //IFactory<ICarListViewModel> carListViewModelFactory,
            //IFactory<IUserDetailViewModel> userDetailViewModelFactory
            )
        {
            CarListViewModel = carListViewModel;
            UserListViewModel = userListViewModel;
            RideListViewModel = rideListViewModel;
            //_userDetailViewModelFactory = userDetailViewModelFactory;
            //UserDetailViewModel = _userDetailViewModelFactory.Create();

        }

        public ICarListViewModel CarListViewModel { get; }
        public IUserListViewModel UserListViewModel { get; }
        public IRideListViewModel RideListViewModel { get; }


        //public ObservableCollection<ICarListViewModel> CaListlViewModels { get; } =
        //    new ObservableCollection<ICarListViewModel>();

    }
}