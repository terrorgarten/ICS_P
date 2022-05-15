﻿using System;
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
    }

    public ICommand DeleteCommand { get; }

    public ICommand SaveCommand { get; }

    public ICarListViewModel CarListViewModel { get; }

    public ObservableCollection<RideListModel> PassengerRides { get; set; } = new();


    public UserWrapper? Model { get; set; } = UserDetailModel.Empty;

    public async Task LoadAsync(Guid id)
    {
        Model = await _userFacade.GetAsync(id) ?? UserDetailModel.Empty;
        PassengerRides.Clear();
        var passengerRides = await _rideFacade.GetPassengerRides(Model.Id);
        PassengerRides.AddRange(passengerRides!);
    }

    public async Task DeleteAsync()
    {
        if (Model is null) throw new InvalidOperationException("Null model cannot be deleted");

        if (Model.Id != Guid.Empty)
        {
            var delete = _messageDialogService.Show(
                "Delete",
                $"Do you want to delete {Model?.Name}?.",
                MessageDialogButtonConfiguration.YesNo,
                MessageDialogResult.No);

            if (delete == MessageDialogResult.No) return;

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

    public async Task SaveAsync()
    {
        if (Model == null) throw new InvalidOperationException("Null model cannot be saved");

        Model = await _userFacade.SaveAsync(Model);
        _mediator.Send(new UpdateMessage<UserWrapper> { Model = Model });
        _mediator.Send(new SelectedMessage<UserWrapper> { Id = Model?.Id });
    }

    private void OnPassengerRidesUpdated(UpdatePassengerRidesMessage<RideWrapper> obj)
    {
        _ = LoadAsync(Model!.Id);
    }

    private void OnRideUpdated(UpdateMessage<RideWrapper> obj)
    {
        _ = LoadAsync(Model!.Id);
    }

    private void OnRideDeleted(DeleteMessage<RideWrapper> obj)
    {
        _ = LoadAsync(Model!.Id);
    }

    private void DeleteCar(DeleteMessage<CarWrapper> message)
    {
        if (message.TargetId != Model?.Id || message.Model is null) return;

        _ = LoadAsync(Model!.Id);
    }

    private void NewCar(NewMessage<CarWrapper> message)
    {
        if (message.TargetId != Model?.Id || message.Model is null) return;

        _ = LoadAsync(Model!.Id);
    }

    private void UpdateCar(UpdateMessage<CarWrapper> message)
    {
        if (message.TargetId != Model?.Id) return;

        _ = LoadAsync(Model!.Id);
    }

    private bool CanSave()
    {
        return Model?.IsValid ?? false;
    }
}