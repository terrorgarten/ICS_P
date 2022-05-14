using Carpool.App.Messages;
using Carpool.App.Services;
using Carpool.App.Services.MessageDialog;
using Carpool.App.Wrappers;
using Carpool.BL.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Carpool.App.Commands;
using Carpool.App.Extensions;
using Carpool.BL.Facades;
using Carpool.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace Carpool.App.ViewModels
{
    public class UserDetailViewModel : ViewModelBase, IUserDetailViewModel
    {
        private readonly IMediator _mediator;
        private readonly UserFacade _userFacade;
        //private readonly UserRideFacade _userRideFacade;
        private readonly RideFacade _rideFacade;
        private readonly IMessageDialogService _messageDialogService;
        private UserWrapper? _model = UserDetailModel.Empty;
        private CarWrapper? _selectedCar;

        public UserDetailViewModel(
            UserFacade userFacade,
            //UserRideFacade userRideFacade,
            RideFacade rideFacade,
            IMessageDialogService messageDialogService,
            IMediator mediator,
            ICarListViewModel carListViewModel)
        {
            _userFacade = userFacade;
            //_userRideFacade = userRideFacade;
            _rideFacade = rideFacade;
            _messageDialogService = messageDialogService;
            _mediator = mediator;
            CarListViewModel = carListViewModel;

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
            DeleteCommand = new AsyncRelayCommand(DeleteAsync);

            mediator.Register<NewMessage<CarWrapper>>(NewCar);
            mediator.Register<UpdateMessage<CarWrapper>>(UpdateCar);
            mediator.Register<DeleteMessage<CarWrapper>>(DeleteCar);
            mediator.Register<DeleteMessage<RideWrapper>>(OnRideDeleted);

        }

        private void OnRideDeleted(DeleteMessage<RideWrapper> obj)
        {
            _ = LoadAsync(Model!.Id);
        }

        public ICommand DeleteCommand { get; }

        public ICommand SaveCommand { get; }

        public ICarListViewModel CarListViewModel { get; }

        public ObservableCollection<RideListModel> PassengerRides { get; set; } = new();

        public CarWrapper? SelectedCar
        {
            get => _selectedCar;
            set
            {
                _selectedCar = value;
                OnPropertyChanged();
                _mediator.Send(new SelectedMessage<CarWrapper>
                {
                    TargetId = Model?.Id ?? Guid.Empty,
                    Model = _selectedCar
                });
            }
        }

        //public UserWrapper? Model { get; set; }
        
        public UserWrapper? Model
        {
            get => _model;
            set
            {
                _model = value;
            }
        }

        public async Task LoadAsync(Guid id)
        {
            Model = await _userFacade.GetAsync(id) ?? UserDetailModel.Empty;
            PassengerRides.Clear();
            var passengerRides = await _rideFacade.GetPassengerRides(Model.Id);
            PassengerRides.AddRange(passengerRides!);
        }

        private void DeleteCar(DeleteMessage<CarWrapper> message)
        {
            if (message.TargetId != Model?.Id || message.Model is null)
            {
                return;
            }

            _ = LoadAsync(Model.Id);
        }

        private void NewCar(NewMessage<CarWrapper> message)
        {
            if (message.TargetId != Model?.Id || message.Model is null)
            {
                return;
            }

            _ = LoadAsync(Model.Id);
        }

        private void UpdateCar(UpdateMessage<CarWrapper> message)
        {
            if (message.TargetId != Model?.Id)
            {
                return;
            }

            _ = LoadAsync(Model.Id);
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
                    "Delete",
                    $"Do you want to delete {Model?.Name}?.",
                    MessageDialogButtonConfiguration.YesNo,
                    MessageDialogResult.No);

                if (delete == MessageDialogResult.No)
                {
                    return;
                }

                try
                {
                    await _userFacade.DeleteAsync(Model!.Id);
                }
                catch
                {
                    var _ = _messageDialogService.Show(
                        $"Deleting of {Model?.Name} failed!",
                        "Deleting failed",
                        MessageDialogButtonConfiguration.OK,
                        MessageDialogResult.OK);
                }

                _mediator.Send(new DeleteMessage<UserWrapper> { Model = Model! });
            }
        }

        private bool CanSave() => Model?.IsValid ?? false;

        public async Task SaveAsync()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            Model = await _userFacade.SaveAsync(Model);
            _mediator.Send(new UpdateMessage<UserWrapper> { Model = Model });
        }

        //public override void LoadInDesignMode()
        //{
        //    base.LoadInDesignMode();
        //    Model = new UserWrapper(new UserDetailModel(
        //        Name: "Spaghetti",
        //        Description: "Spaghetti description",
        //        Duration: new TimeSpan(0, 30, 0),
        //        FoodType.MainDish
        //        )
        //    {
        //        ImageUrl = "https://cleanfoodcrush.com/wp-content/uploads/2019/01/CleanFoodCrush-Super-Easy-Beef-Stir-Fry-User.jpg"
        //    });
        //}
    }
}