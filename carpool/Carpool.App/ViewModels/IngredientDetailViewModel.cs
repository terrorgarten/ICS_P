using Carpool.App.Messages;
using Carpool.BL.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Carpool.App.Services;
using Carpool.App.Services.MessageDialog;
using Carpool.App.Wrappers;
using Carpool.App.Commands;
using Carpool.BL.Facades;

namespace Carpool.App.ViewModels
{
    public class IngredientDetailViewModel : ViewModelBase, IIngredientDetailViewModel
    {
        private readonly IMediator _mediator;
        private readonly CarFacade _carFacade;
        private readonly IMessageDialogService _messageDialogService;

        public IngredientDetailViewModel(
            CarFacade carFacade,
            IMessageDialogService messageDialogService,
            IMediator mediator)
        {
            _carFacade = carFacade;
            _messageDialogService = messageDialogService;
            _mediator = mediator;

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
            DeleteCommand = new AsyncRelayCommand(DeleteAsync);
        }

        public IngredientWrapper? Model { get; private set; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }


        public async Task LoadAsync(Guid id)
        {
            Model = await _carFacade.GetAsync(id) ??  CarDetailModel.Empty;
        }

        public async Task SaveAsync()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            Model = await _carFacade.SaveAsync(Model.Model);
            _mediator.Send(new UpdateMessage<IngredientWrapper> { Model = Model });
        }

        private bool CanSave() => Model?.IsValid ?? false;

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
                    $"Do you want to delete {Model?.Name}?.",
                    MessageDialogButtonConfiguration.YesNo,
                    MessageDialogResult.No);

                if (delete == MessageDialogResult.No) return;

                try
                {
                    await _carFacade.DeleteAsync(Model!.Id);
                }
                catch
                {
                    var _ = _messageDialogService.Show(
                        $"Deleting of {Model?.Name} failed!",
                        "Deleting failed",
                        MessageDialogButtonConfiguration.OK,
                        MessageDialogResult.OK);
                }

                _mediator.Send(new DeleteMessage<IngredientWrapper>
                {
                    Model = Model
                });
            }
        }

        public override void LoadInDesignMode()
        {
            base.LoadInDesignMode();
            Model = new IngredientWrapper(new CarDetailModel(
                Name: "Water",
                Description: "Water description")
            {
                ImageUrl = "https://www.pngitem.com/pimgs/m/40-406527_cartoon-glass-of-water-png-glass-of-water.png"
            });
        }
    }
}