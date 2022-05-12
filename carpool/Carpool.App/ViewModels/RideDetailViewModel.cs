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
using Carpool.App.Services.MessageDialog;

namespace Carpool.App.ViewModels
{
    public class RideDetailViewModel : ViewModelBase, IRideDetailViewModel
    {
        private readonly IMediator _mediator;
        private readonly RideFacade _rideFacade;
        private readonly IMessageDialogService _messageDialogService;

        public RideDetailViewModel
            (RideFacade rideFacade,
                IMessageDialogService messageDialogService,
                IMediator mediator
                )
        {
            _rideFacade = rideFacade;
            _messageDialogService = messageDialogService;
            _mediator = mediator;

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
            DeleteCommand = new AsyncRelayCommand(DeleteAsync);

            mediator.Register<SelectedMessage<UserWrapper>>(OnUserSelected);
            mediator.Register<SelectedMessage<RideWrapper>>(OnRideSelected);
        }

        public RideWrapper? Model { get; set; }
        private static Guid? CurrentUserId { get; set; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }


        private void OnRideSelected(SelectedMessage<RideWrapper> message)
        {
            if (message.Id != null) _ = LoadAsync(message.Id.Value);
        }

        

        private static void OnUserSelected(SelectedMessage<UserWrapper> obj)
        {
            CurrentUserId = obj.Id;
        }
        

        public async Task LoadAsync(Guid id)
        {
            Model = await _rideFacade.GetAsync(id) ?? RideDetailModel.Empty;
        }

        public Task DeleteAsync()
        {
            //if (Model is null)
            //{
            //    throw new InvalidOperationException("Null model cannot be deleted");
            //}

            //if (Model.Id != Guid.Empty)
            //{
            //    var delete = _messageDialogService.Show(
            //        $"Delete",
            //        $"Do you want to delete ride to {Model?.End} scheduled for ?.",
            //        MessageDialogButtonConfiguration.YesNo,
            //        MessageDialogResult.No);

            //    if (delete == MessageDialogResult.No) return;

            //    try
            //    {
            //        await _carFacade.DeleteAsync(Model!.Id);
            //    }
            //    catch
            //    {
            //        var _ = _messageDialogService.Show(
            //            $"Deleting of {Model?.Manufacturer} failed!",
            //            "Deleting failed",
            //            MessageDialogButtonConfiguration.OK,
            //            MessageDialogResult.OK);
            //    }

            //    _mediator.Send(new DeleteMessage<CarWrapper>
            //    {
            //        Model = Model
            //    });
            //}
            throw new NotImplementedException("Delete not implemented yet");
        }

        private bool CanSave() => Model?.IsValid ?? false;
        //{
        //    if (Model == null)
        //    {
        //        return false;
        //    }
        //    //Ride has already lapsed
        //    return DateTime.Compare(Model.BeginTime, DateTime.Now) >= 0;
        //}

        //TODO - Kontroly? 
        public async Task SaveAsync()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            if (CurrentUserId == null)
            {
                throw new InvalidOperationException("No user selected");
            }
            //Model.UserId = CurrentUserId;
            Model = await _rideFacade.SaveAsync(Model.Model);
            _mediator.Send(new UpdateMessage<RideWrapper> { Model = Model });
        }
    }
}
