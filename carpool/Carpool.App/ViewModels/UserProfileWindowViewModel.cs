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
        private readonly IFactory<IUserDetailViewModel> _userDetailViewModelFactory;

        public UserProfileWindowViewModel(
            ICarListViewModel carListViewModel,
            //IMediator mediator,
            //IFactory<ICarListViewModel> carListViewModelFactory,
            IFactory<IUserDetailViewModel> userDetailViewModelFactory
            )
        {
            CarListViewModel = carListViewModel;
            _userDetailViewModelFactory = userDetailViewModelFactory;
            UserDetailViewModel = _userDetailViewModelFactory.Create();

        }

        public ICollection<CarListModel> Cars { get; set; } = new List<CarListModel>()
        {
            new CarListModel(Manufacturer.Dacia, CarType.Micro),
            new CarListModel(Manufacturer.Fiat, CarType.Sedan),
            new CarListModel(Manufacturer.Volkswagen, CarType.Micro),
            new CarListModel(Manufacturer.Lamborghini, CarType.Cabriolet)
        };

        public ICarListViewModel CarListViewModel { get; }

        public IUserDetailViewModel UserDetailViewModel { get; }

        public ObservableCollection<ICarListViewModel> CaListlViewModels { get; } =
            new ObservableCollection<ICarListViewModel>();

    }
}