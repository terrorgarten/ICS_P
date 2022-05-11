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
    public class AppStartViewModel : ViewModelBase
    {
        private readonly IFactory<IUserDetailViewModel> _userDetailViewModelFactory;
        private readonly IFactory<ICarDetailViewModel> _carDetailViewModelFactory; //DEL
        public int SelectedIndex { get; set; }

        public AppStartViewModel(
            ICarListViewModel carListViewModel,
            IUserListViewModel userListViewModel,
            IRideListViewModel rideListViewModel,
            IMediator mediator,
            IFactory<ICarDetailViewModel> carDetailViewModelFactory,
            IFactory<IUserDetailViewModel> userDetailViewModelFactory
        )
        {
            CarListViewModel = carListViewModel;
            UserListViewModel = userListViewModel;
            RideListViewModel = rideListViewModel;
            _userDetailViewModelFactory = userDetailViewModelFactory;
            UserDetailViewModel = _userDetailViewModelFactory.Create();
            _carDetailViewModelFactory = carDetailViewModelFactory;
            CarDetailViewModel = _carDetailViewModelFactory.Create();

            CloseUserDetailTabCommand = new RelayCommand<IUserDetailViewModel>(OnCloseUserDetailTabExecute);
            CloseCarDetailTabCommand = new RelayCommand<ICarDetailViewModel>(OnCloseCarDetailTabExecute);

            mediator.Register<NewMessage<UserWrapper>>(OnUserNewMessage);
            mediator.Register<SelectedMessage<UserWrapper>>(OnUserSelected);
            mediator.Register<DeleteMessage<UserWrapper>>(OnUserDeleted);
            SelectedIndex = 0;
        }

        public ICarListViewModel CarListViewModel { get; }
        public IUserListViewModel UserListViewModel { get; }
        public IRideListViewModel RideListViewModel { get; }
        public IUserDetailViewModel UserDetailViewModel { get; }
        public ICarDetailViewModel CarDetailViewModel { get; }

        public ICommand CloseUserDetailTabCommand { get; }

        public ICommand CloseCarDetailTabCommand { get; }


        public ObservableCollection<IUserDetailViewModel> UserDetailViewModels { get; } =
            new ObservableCollection<IUserDetailViewModel>();

        public ObservableCollection<ICarDetailViewModel> CarDetailViewModels { get; } =
            new ObservableCollection<ICarDetailViewModel>();

        public IUserDetailViewModel? SelectedUserDetailViewModel { get; set; }

        public ICarDetailViewModel? SelectedCarDetailViewModel { get; set; }

        private void OnUserNewMessage(NewMessage<UserWrapper> _)
        {
            SelectUser(Guid.Empty);
        }

        private void OnCarNewMessage(NewMessage<CarWrapper> _)
        {
            SelectCar(Guid.Empty);
        }

        private void OnUserSelected(SelectedMessage<UserWrapper> message)
        {
            SelectedIndex = 1;
            SelectUser(message.Id);
        }

        private void OnCarSelected(SelectedMessage<CarWrapper> message)
        {
            SelectedIndex = 3;
            SelectCar(message.Id);
        }

        private void OnUserDeleted(DeleteMessage<UserWrapper> message)
        {
            var user = UserDetailViewModels.SingleOrDefault(i => i.Model?.Id == message.Id);
            if (user != null)
            {
                UserDetailViewModels.Remove(user);
            }
        }

        private void OnCarDeleted(DeleteMessage<CarWrapper> message)
        {
            var car = CarDetailViewModels.SingleOrDefault(i => i.Model?.Id == message.Id);
            if (car != null)
            {
                CarDetailViewModels.Remove(car);
            }
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

        private void SelectCar(Guid? id)
        {
            if (id is null)
            {
                SelectedCarDetailViewModel = null;
            }

            else
            {
                var carDetailViewModel = CarDetailViewModels.SingleOrDefault(vm => vm.Model?.Id == id);
                if (carDetailViewModel == null)
                {
                    carDetailViewModel = _carDetailViewModelFactory.Create();
                    CarDetailViewModels.Add(carDetailViewModel);
                    carDetailViewModel.LoadAsync(id.Value);
                }

                SelectedCarDetailViewModel = carDetailViewModel;
            }
        }

        private void OnCloseUserDetailTabExecute(IUserDetailViewModel? recipeDetailViewModel)
        {
            if (recipeDetailViewModel is not null)
            {
                // TODO: Check if the Detail has changes and ask user to cancel
                UserDetailViewModels.Remove(recipeDetailViewModel);
            }
        }

        private void OnCloseCarDetailTabExecute(ICarDetailViewModel? recipeDetailViewModel)
        {
            if (recipeDetailViewModel is not null)
            {
                // TODO: Check if the Detail has changes and ask user to cancel
                CarDetailViewModels.Remove(recipeDetailViewModel);
            }
        }
    }
}