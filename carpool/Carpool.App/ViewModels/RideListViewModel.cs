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
    public class RideListViewModel : ViewModelBase, IRideListViewModel
    {
        private readonly RideFacade _rideFacade;
        private readonly IMediator _mediator;

        public RideListViewModel(RideFacade rideFacade, IMediator mediator)
        {
            _rideFacade = rideFacade;
            _mediator = mediator;

            RideSelectedCommand = new RelayCommand<RideListModel>(RideSelected);
            RideNewCommand = new RelayCommand(RideNew);

            mediator.Register<UpdateMessage<RideWrapper>>(RideUpdated);
            mediator.Register<DeleteMessage<RideWrapper>>(RideDeleted);
        }

        public ObservableCollection<RideListModel> Rides { get; set; } = new();

        public ICommand RideSelectedCommand { get; }
        public ICommand RideNewCommand { get; }

        private void RideNew() => _mediator.Send(new NewMessage<RideWrapper>());

        //Toto se vola pokud dostanu command
        private void RideSelected(RideListModel? ride) => _mediator.Send(new SelectedMessage<RideWrapper> { Id = ride?.Id });

        private async void RideUpdated(UpdateMessage<RideWrapper> _) => await LoadAsync();

        private async void RideDeleted(DeleteMessage<RideWrapper> _) => await LoadAsync();

        public async Task LoadAsync()
        {
            Rides.Clear();
            var rides = await _rideFacade.GetAsync();
            Rides.AddRange(rides);
        }

        public override void LoadInDesignMode()
        {
            Rides.Add(new RideListModel(Start: "Praha", End: "Olomouc", DateTime.MaxValue, 4));
        }
    }
}
