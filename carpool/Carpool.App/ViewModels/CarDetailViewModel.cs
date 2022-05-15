using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Carpool.App.Commands;
using Carpool.App.Messages;
using Carpool.App.Services;
using Carpool.App.Services.MessageDialog;
using Carpool.App.Wrappers;
using Carpool.BL.Facades;
using Carpool.BL.Models;

namespace Carpool.App.ViewModels;

public class CarDetailViewModel : ViewModelBase, ICarDetailViewModel
{
    private readonly CarFacade _carFacade;
    private readonly IMediator _mediator;
    private readonly IMessageDialogService _messageDialogService;

    public CarDetailViewModel(
        CarFacade carFacade,
        IMessageDialogService messageDialogService,
        IMediator mediator)
    {
        _carFacade = carFacade;
        _messageDialogService = messageDialogService;
        _mediator = mediator;

        SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
        DeleteCommand = new AsyncRelayCommand(DeleteAsync);
        mediator.Register<SelectedMessage<UserWrapper>>(OnUserSelected);
    }

    //remove nullability?
    private static Guid? CurrentUserId { get; set; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public CarWrapper? Model { get; set; }

    public async Task LoadAsync(Guid id)
    {
        Model = await _carFacade.GetAsync(id) ?? CarDetailModel.Empty;
    }

    public async Task SaveAsync()
    {
        if (Model == null) throw new InvalidOperationException("Null model cannot be saved");

        Model.OwnerId = CurrentUserId;
        Model = await _carFacade.SaveAsync(Model.Model);
        _mediator.Send(new UpdateMessage<CarWrapper> {Model = Model, TargetId = Model.OwnerId});
        _mediator.Send(new UpdateComboboxMessage<CarWrapper>());
    }

    public async Task DeleteAsync()
    {
        if (Model is null) throw new InvalidOperationException("Null model cannot be deleted");

        if (Model.Id != Guid.Empty)
        {
            var delete = _messageDialogService.Show(
                "Smazat",
                $"Chcete smazat {Model?.Manufacturer}?.",
                MessageDialogButtonConfiguration.YesNo,
                MessageDialogResult.No);

            if (delete == MessageDialogResult.No) return;

            try
            {
                await _carFacade.DeleteAsync(Model!.Id);
                _mediator.Send(new UpdateComboboxMessage<CarWrapper>());
            }
            catch
            {
                var _ = _messageDialogService.Show(
                    $"Smazat {Model?.Manufacturer} se nepovedlo!",
                    "Zkontrolujte, zda jste přihlášen/a a zkuste to znovu.",
                    MessageDialogButtonConfiguration.OK,
                    MessageDialogResult.OK);
            }

            _mediator.Send(new DeleteMessage<CarWrapper>
            {
                Model = Model,
                TargetId = Model!.OwnerId
            });
        }
    }

    private void OnUserSelected(SelectedMessage<UserWrapper> obj)
    {
        CurrentUserId = obj.Id;
    }

    private bool CanSave()
    {
        return Model?.IsValid ?? false;
    }
}