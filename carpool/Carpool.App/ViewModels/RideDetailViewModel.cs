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
        }

        public RideWrapper? Model { get; set; }
        private static Guid? CurrentUserId { get; set; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        


        private bool CanSave() => Model?.IsValid ?? false;

        private void OnUserSelected(SelectedMessage<UserWrapper> obj)
        {
            CurrentUserId = obj.Id;
        } 





        public async Task LoadAsync(Guid id)
        {
            Model = await _rideFacade.GetAsync(id) ?? RideDetailModel.Empty;
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
