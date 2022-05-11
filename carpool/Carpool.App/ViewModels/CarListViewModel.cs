using System;
using Carpool.App.Extensions;
using Carpool.App.Messages;
using Carpool.App.Services;
using Carpool.App.Wrappers;
using Carpool.BL.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Carpool.App.Commands;
using Carpool.BL.Facades;
using Carpool.Common.Enums;
using Carpool.DAL.Seeds;
using Carpool.App.Factories;


namespace Carpool.App.ViewModels
{
    public class CarViewModel : ViewModelBase, ICarListViewModel
    {
        private readonly CarFacade _carFacade;
        private readonly IMediator _mediator;

        private readonly IFactory<ICarDetailViewModel> _carDetailViewModelFactory;

        public CarViewModel(
            CarFacade carFacade, IMediator mediator,
            IFactory<ICarDetailViewModel> carDetailViewModelFactory
            )
        {
            _carFacade = carFacade;
            _mediator = mediator;

            _carDetailViewModelFactory = carDetailViewModelFactory;
            CarDetailViewModel = _carDetailViewModelFactory.Create();

            
            CarSelectedCommand = new RelayCommand<CarListModel>(CarSelected);
            CarNewCommand = new RelayCommand(CarNew);
            CloseCarDetailTabCommand = new RelayCommand<ICarDetailViewModel>(OnCloseCarDetailTabExecute);

            mediator.Register<UpdateMessage<CarWrapper>>(CarUpdated);
            mediator.Register<DeleteMessage<CarWrapper>>(CarDeleted);
            mediator.Register<SelectedMessage<CarWrapper>>(OnCarSelected);
            mediator.Register<SelectedMessage<UserWrapper>>(UserSelected);

            //mediator.Register<NewMessage<CarWrapper>>(OnCarNewMessage);

        }
        private Guid? LoggedInUserId { get; set; }
        private void UserSelected(SelectedMessage<UserWrapper> obj)
        {
            LoggedInUserId = obj.Id;
            _ = LoadAsync();
        }

        private void OnCarSelected(SelectedMessage<CarWrapper> message)
        {
            SelectCar(message.Id);
        }

        //NEW CAR
        private void OnCarNewMessage(NewMessage<CarWrapper> _)
        {
            SelectCar(Guid.Empty);
        }

        //CAR DETAIL WORKAROUND
        public ICommand CloseCarDetailTabCommand { get; }
        public ICarDetailViewModel? SelectedCarDetailViewModel { get; set; }
        public ICarDetailViewModel CarDetailViewModel { get; }
        public ObservableCollection<ICarDetailViewModel> CarDetailViewModels { get; } =
            new ObservableCollection<ICarDetailViewModel>();

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

        private void OnCloseCarDetailTabExecute(ICarDetailViewModel? recipeDetailViewModel)
        {
            if (recipeDetailViewModel is not null)
            {
                // TODO: Check if the Detail has changes and ask user to cancel
                CarDetailViewModels.Remove(recipeDetailViewModel);
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










        public ObservableCollection<CarListModel> Cars { get; set; } = new();
        
        public ICommand CarSelectedCommand { get; }
        public ICommand CarNewCommand { get; }

      

        private void CarNew() => SelectCar(Guid.Empty);
        
        //Toto se vola pokud dostanu command z listu -> nechci a predelat
        private void CarSelected(CarListModel? car)
        {
            SelectCar(car?.Id);
        }
        private async void CarUpdated(UpdateMessage<CarWrapper> _) => await LoadAsync();

        private async void CarDeleted(DeleteMessage<CarWrapper> _) => await LoadAsync();

        

        public async Task LoadAsync()
        {
            Cars.Clear();
            var cars = await _carFacade.GetUserCars(LoggedInUserId);
            Cars.AddRange(cars!);
        }

        public override void LoadInDesignMode()
        {
            Cars.Add(new CarListModel(Manufacturer: Manufacturer.Dacia, CarType.Micro) { PhotoUrl = "https://www.pngitem.com/pimgs/m/40-406527_cartoon-glass-of-water-png-glass-of-water.png" });
        }
    }
}
