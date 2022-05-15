using System;
using Carpool.App.Extensions;
using Carpool.App.Messages;
using Carpool.App.Services;
using Carpool.App.Wrappers;
using Carpool.BL.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Carpool.App.Commands;
using Carpool.BL.Facades;
using Carpool.Common.Enums;
using Carpool.DAL.Seeds;
using Carpool.App.Factories;
using Carpool.App.Services.MessageDialog;

namespace Carpool.App.ViewModels
{
    public class RideDetailViewModel : ViewModelBase, IRideDetailViewModel
    {
        private readonly IMediator _mediator;
        private readonly RideFacade _rideFacade;
        private readonly IMessageDialogService _messageDialogService;
        private readonly CarFacade _carFacade;
        private readonly UserRideFacade _userRideFacade;

        public RideDetailViewModel
            (RideFacade rideFacade,
                IMessageDialogService messageDialogService,
                IMediator mediator,
                CarFacade carFacade,
                UserRideFacade userRideFacade
                )
        {
            _rideFacade = rideFacade;
            _messageDialogService = messageDialogService;
            _mediator = mediator;
            _carFacade = carFacade;
            _userRideFacade = userRideFacade;

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
            DeleteCommand = new AsyncRelayCommand(DeleteAsync);
            CarSelectedCommand = new RelayCommand<CarDetailModel>(OnCarSelected);
            PassengerDeleteCommand = new AsyncRelayCommand<UserRideDetailModel>(OnPassengerDelete);

            mediator.Register<SelectedMessage<UserWrapper>>(OnUserSelected);
            mediator.Register<SelectedMessage<RideWrapper>>(OnRideSelected);
            mediator.Register<NewMessage<RideWrapper>>(OnNewRide);
            mediator.Register<UpdateComboboxMessage<CarWrapper>>(OnCarUpdated);
        }

        private async Task OnPassengerDelete(UserRideDetailModel? rideDetailModel)
        {
            if(rideDetailModel == null) return;
            
            await _userRideFacade.DeleteAsync(rideDetailModel!.Id);
            Passengers.Clear();
            var passengers = await _userRideFacade.GetPassengers(Model!.Id);
            Passengers.AddRange(passengers!);
        }
            

        private void OnCarSelected(CarDetailModel? car)
        {
            if (car is null)
            {
                return;
            }

            Model!.CarId = car!.Id;
            Model!.Car = car;
            _ = SaveAsync();
        }

        private void OnNewRide(NewMessage<RideWrapper> obj)
        {
            Model = RideDetailModel.Empty;
            Model.Id = Guid.NewGuid();
            Model.UserId = CurrentUserId;
        }

        private void OnCarUpdated(UpdateComboboxMessage<CarWrapper> obj)
        {
            _ = LoadAsync((Guid) CurrentUserId!);
        }

        public RideWrapper? Model { get; set; }
        private static Guid? CurrentUserId { get; set; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ObservableCollection<CarDetailModel> UserCars { get; set; } = new();
        public ObservableCollection<UserRideDetailModel> Passengers { get; set; } = new();

        public ICommand CarSelectedCommand { get; }

        public ICommand PassengerDeleteCommand { get; }


        private void OnRideSelected(SelectedMessage<RideWrapper> message)
        {
            if (message.Id != null) _ = LoadAsync(message.Id.Value);
        }

        private async void OnUserSelected(SelectedMessage<UserWrapper> obj)
        {
            CurrentUserId = obj.Id;
            if (CurrentUserId == Guid.Empty)
            {
                Model = null;
            }

        }

        public async Task LoadAsync(Guid id)
        {
            Model = await _rideFacade.GetAsync(id) ?? RideDetailModel.Empty;
            UserCars.Clear();
            var cars = await _carFacade.GetUserCarsDetails(CurrentUserId);
            UserCars.AddRange(cars!);
            Passengers.Clear();
            var passengers = await _userRideFacade.GetPassengers(Model.Id);
            Passengers.AddRange(passengers!);
        }

        public async Task DeleteAsync()
        {
            if (Model is null)
            {
                throw new InvalidOperationException("Null model cannot be deleted");
            }

            if (Model.Id != Guid.Empty)
            {
                var delete = _messageDialogService.Show(
                    $"Delete",
                    $"Do you want to delete ride to {Model?.End} scheduled for ?.",
                    MessageDialogButtonConfiguration.YesNo,
                    MessageDialogResult.No);

                if (delete == MessageDialogResult.No) return;

                try
                {
                    await _rideFacade.DeleteAsync(Model!.Id);
                    Model = null;
                }
                catch
                {
                    var _ = _messageDialogService.Show(
                        $"Deleting of ride failed!",
                        "Deleting failed",
                        MessageDialogButtonConfiguration.OK,
                        MessageDialogResult.OK);
                }

                _mediator.Send(new DeleteMessage<RideWrapper>());
                //TODO - odebrat z rides vsude -> DELETED MESSAGE 
            }
            else
            {
                Model = null;
            }
        }

        private bool CanSave() => Model?.IsValid ?? false;
       

        

        //TODO - Kontroly? 
        public async Task SaveAsync()
        {
            if (Model is null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            if (CurrentUserId == null)
            {
                throw new InvalidOperationException("No user selected");
            }

            try
            {
                Model = await _rideFacade.SaveAsync(Model.Model);
                _mediator.Send(new UpdateMessage<RideWrapper> { Model = Model });
            }
            catch
            {
                var _ = _messageDialogService.Show(
                    $"Zkontrolujte, zda jsou všechna pole vyplněna",
                    "Ukládání jízdy selhalo",
                    MessageDialogButtonConfiguration.OK,
                    MessageDialogResult.OK);
            }
        }
    }
}
