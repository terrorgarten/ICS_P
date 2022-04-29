using System;
using Carpool.App.Extensions;
using Carpool.App.Messages;
using Carpool.App.Services;
using Carpool.App.Wrappers;
using Carpool.BL.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Carpool.App.Commands;
using Carpool.BL.Facades;
using Carpool.Common.Enums;
using Carpool.DAL.Seeds;

namespace Carpool.App.ViewModels
{
    public class CarListViewModel : ViewModelBase, ICarListViewModel
    {
        private readonly CarFacade _carFacade;
        private readonly IMediator _mediator;

        public CarListViewModel(CarFacade carFacade, IMediator mediator)
        {
            _carFacade = carFacade;
            _mediator = mediator;

            CarSelectedCommand = new RelayCommand<CarListModel>(CarSelected);
            CarNewCommand = new RelayCommand(CarNew);

            mediator.Register<UpdateMessage<CarWrapper>>(CarUpdated);
            mediator.Register<DeleteMessage<CarWrapper>>(CarDeleted);
        }

        public ObservableCollection<CarListModel> Cars { get; set; } = new();
        
        
    public ICommand CarSelectedCommand { get; }
        public ICommand CarNewCommand { get; }

        private void CarNew() => _mediator.Send(new NewMessage<CarWrapper>());
        
        //Toto se vola pokud dostanu command
        private void CarSelected(CarListModel? car) => _mediator.Send(new SelectedMessage<CarWrapper> { Id = car?.Id });

        private async void CarUpdated(UpdateMessage<CarWrapper> _) => await LoadAsync();

        private async void CarDeleted(DeleteMessage<CarWrapper> _) => await LoadAsync();

        public async Task LoadAsync()
        {
            Cars.Clear();
            var cars = await _carFacade.GetAsync();
            Cars.AddRange(cars);
        }

        public override void LoadInDesignMode()
        {
            Cars.Add(new CarListModel(Manufacturer: Manufacturer.Dacia, CarType.Micro) { PhotoUrl = "https://www.pngitem.com/pimgs/m/40-406527_cartoon-glass-of-water-png-glass-of-water.png" });
        }
    }
}
