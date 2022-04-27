using Carpool.App.Factories;
using Carpool.App.Messages;
using Carpool.App.Services;
using Carpool.App.Wrappers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Carpool.App.Commands;

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

        public ICarListViewModel CarListViewModel { get; }

        public IUserDetailViewModel UserDetailViewModel { get; }

        public ObservableCollection<ICarListViewModel> RecipeDetailViewModels { get; } =
            new ObservableCollection<ICarListViewModel>();

        public ObservableCollection<IUserDetailViewModel> IngredientDetailViewModels { get; } =
            new ObservableCollection<IUserDetailViewModel>();

    }
}