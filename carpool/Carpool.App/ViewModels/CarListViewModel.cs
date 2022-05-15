using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Carpool.App.Commands;
using Carpool.App.Extensions;
using Carpool.App.Factories;
using Carpool.App.Messages;
using Carpool.App.Services;
using Carpool.App.Wrappers;
using Carpool.BL.Facades;
using Carpool.BL.Models;
using Carpool.Common.Enums;

namespace Carpool.App.ViewModels;

public class CarViewModel : ViewModelBase, ICarListViewModel
{
    private readonly IFactory<ICarDetailViewModel> _carDetailViewModelFactory;
    private readonly CarFacade _carFacade;

    public CarViewModel(
        CarFacade carFacade, IMediator mediator,
        IFactory<ICarDetailViewModel> carDetailViewModelFactory
    )
    {
        _carFacade = carFacade;

        _carDetailViewModelFactory = carDetailViewModelFactory;
        CarDetailViewModel = _carDetailViewModelFactory.Create();


        CarSelectedCommand = new RelayCommand<CarListModel>(CarSelected);
        CarNewCommand = new RelayCommand(CarNew);
        CloseCarDetailTabCommand = new RelayCommand<ICarDetailViewModel>(OnCloseCarDetailTabExecute);

        mediator.Register<UpdateMessage<CarWrapper>>(CarUpdated);
        mediator.Register<DeleteMessage<CarWrapper>>(CarDeleted);
        mediator.Register<SelectedMessage<CarWrapper>>(OnCarSelected);
        mediator.Register<SelectedMessage<UserWrapper>>(UserSelected);
    }


    private Guid? LoggedInUserId { get; set; }

    //CAR DETAIL WORKAROUND
    public ICommand CloseCarDetailTabCommand { get; }
    public ICarDetailViewModel? SelectedCarDetailViewModel { get; set; }
    public ICarDetailViewModel CarDetailViewModel { get; }

    public ObservableCollection<ICarDetailViewModel> CarDetailViewModels { get; } = new();

    public ObservableCollection<CarListModel> Cars { get; set; } = new();

    public ICommand CarSelectedCommand { get; }
    public ICommand CarNewCommand { get; }

    public async Task LoadAsync()
    {
        Cars.Clear();
        var cars = await _carFacade.GetUserCars(LoggedInUserId);
        Cars.AddRange(cars!);
    }

    private async void UserSelected(SelectedMessage<UserWrapper> obj)
    {
        if (obj.Id == Guid.Empty)
        {
            CarDetailViewModels.Clear();
            SelectedCarDetailViewModel = null;
            return;
        }

        LoggedInUserId = obj.Id;
        await LoadAsync();
    }

    private void OnCarSelected(SelectedMessage<CarWrapper> message)
    {
        SelectCar(message.Id);
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

    private void OnCloseCarDetailTabExecute(ICarDetailViewModel? carDetailViewModel)
    {
        if (carDetailViewModel is not null)
            // TODO: Check if the Detail has changes and ask user to cancel
            CarDetailViewModels.Remove(carDetailViewModel);
    }

    private void CarNew()
    {
        SelectCar(Guid.Empty);
    }

    //Toto se vola pokud dostanu command z listu -> nechci a predelat
    private void CarSelected(CarListModel? car)
    {
        SelectCar(car?.Id);
    }

    private async void CarUpdated(UpdateMessage<CarWrapper> _)
    {
        await LoadAsync();
    }

    private async void CarDeleted(DeleteMessage<CarWrapper> deleteMessage)
    {
        CarDetailViewModels.Remove(CarDetailViewModels.Single(i => i.Model!.Id == deleteMessage.Id));
        await LoadAsync();
    }

    public override void LoadInDesignMode()
    {
        Cars.Add(new CarListModel(Manufacturer.Dacia, CarType.Micro)
        {
            PhotoUrl = "https://www.pngitem.com/pimgs/m/40-406527_cartoon-glass-of-water-png-glass-of-water.png"
        });
    }
}