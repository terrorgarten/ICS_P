using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Carpool.App.Commands;
using Carpool.App.Extensions;
using Carpool.App.Messages;
using Carpool.App.Services;
using Carpool.App.Services.MessageDialog;
using Carpool.App.Wrappers;
using Carpool.BL.Facades;
using Carpool.BL.Models;

namespace Carpool.App.ViewModels;

public class UserDetailViewModel : ViewModelBase, IUserDetailViewModel
{
    private readonly IMediator _mediator;
    private readonly IMessageDialogService _messageDialogService;
    private readonly RideFacade _rideFacade;
    private readonly UserFacade _userFacade;

    public UserDetailViewModel(
        UserFacade userFacade,
        RideFacade rideFacade,
        IMessageDialogService messageDialogService,
        IMediator mediator,
        ICarListViewModel carListViewModel)
    {
        _userFacade = userFacade;
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
        mediator.Register<UpdateMessage<RideWrapper>>(OnRideUpdated);
        mediator.Register<UpdatePassengerRidesMessage<RideWrapper>>(OnPassengerRidesUpdated);
        mediator.Register<SelectedMessage<UserWrapper>>(OnUserSelected);
    }


    public ICommand DeleteCommand { get; }

    public ICommand SaveCommand { get; }

    public ICarListViewModel CarListViewModel { get; }

    public ObservableCollection<RideListModel> PassengerRides { get; set; } = new();


    public UserWrapper? Model { get; set; } = UserDetailModel.Empty;

    public async Task LoadAsync(Guid id)
    {
        PassengerRides.Clear();
        if (id == Guid.Empty)
            Model = UserDetailModel.Empty;
        else
            try
            {
                Model = await _userFacade.GetAsync(id) ?? throw new InvalidOperationException();
                var passengerRides = await _rideFacade.GetPassengerRides(Model.Id);
                PassengerRides.AddRange(passengerRides!);
            }
            catch
            {
                var _ = _messageDialogService.Show(
                    "Načítání selhalo",
                    "Zkontrolujte, zda jste přihlášeni",
                    MessageDialogButtonConfiguration.OK,
                    MessageDialogResult.OK);
            }
    }

    public async Task DeleteAsync()
    {
        if (Model is null) throw new InvalidOperationException("Nelze smazat prázdného uživatele");

        if (Model.Id != Guid.Empty)
        {
            var delete = _messageDialogService.Show(
                "Smazat uživatele",
                $"Chcete smazat uživatele {Model?.Name}?.",
                MessageDialogButtonConfiguration.YesNo,
                MessageDialogResult.No);

            if (delete == MessageDialogResult.No) return;

            try
            {
                await _userFacade.DeleteAsync(Model!.Id);
                Model = UserDetailModel.Empty;
            }
            catch
            {
                var _ = _messageDialogService.Show(
                    $"Mazání {Model?.Name} selhalo!",
                    "Zkontrolujte, zda je vybraný validní uživatel",
                    MessageDialogButtonConfiguration.OK,
                    MessageDialogResult.OK);
            }

            _mediator.Send(new DeleteMessage<UserWrapper> {Model = Model!});
        }
    }

    public async Task SaveAsync()
    {
        if (Model == null) throw new InvalidOperationException("Nelze uložit prázdného uživatele");

        Model = await _userFacade.SaveAsync(Model);
        _mediator.Send(new UpdateMessage<UserWrapper> {Model = Model});
        _mediator.Send(new SelectedMessage<UserWrapper> {Id = Model?.Id});
    }

    private async void OnUserSelected(SelectedMessage<UserWrapper> obj)
    {
        if (obj.Id != null) await LoadAsync(obj.Id.Value);
    }

    private async void OnPassengerRidesUpdated(UpdatePassengerRidesMessage<RideWrapper> obj)
    {
        await LoadAsync(Model!.Id);
    }

    private async void OnRideUpdated(UpdateMessage<RideWrapper> obj)
    {
        await LoadAsync(Model!.Id);
    }

    private async void OnRideDeleted(DeleteMessage<RideWrapper> obj)
    {
        await LoadAsync(Model!.Id);
    }

    private async void DeleteCar(DeleteMessage<CarWrapper> message)
    {
        if (message.TargetId != Model?.Id || message.Model is null) return;

        await LoadAsync(Model!.Id);
    }

    private async void NewCar(NewMessage<CarWrapper> message)
    {
        if (message.TargetId != Model?.Id || message.Model is null) return;

        await LoadAsync(Model!.Id);
    }

    private async void UpdateCar(UpdateMessage<CarWrapper> message)
    {
        if (message.TargetId != Model?.Id) return;

        await LoadAsync(Model!.Id);
    }

    private bool CanSave()
    {
        return Model?.IsValid ?? false;
    }
}