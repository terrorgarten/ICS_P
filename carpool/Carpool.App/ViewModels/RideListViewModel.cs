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
            mediator.Register<SelectedMessage<UserWrapper>>(OnUserSelected);
        }

        private void OnUserSelected(SelectedMessage<UserWrapper> obj)
        {
            LoggedInUser = obj.Id;
            _ = LoadAsync();
        }

        public ObservableCollection<RideListModel> DriverRides { get; set; } = new();
        private static Guid? LoggedInUser { get; set; }
        public ICommand RideSelectedCommand { get; }
        public ICommand RideNewCommand { get; }

        private void RideNew() => _mediator.Send(new NewMessage<RideWrapper>());

        //Toto se vola pokud dostanu command
        private void RideSelected(RideListModel? ride) => _mediator.Send(new SelectedMessage<RideWrapper> { Id = ride?.Id });

        private async void RideUpdated(UpdateMessage<RideWrapper> _) => await LoadAsync();

        private async void RideDeleted(DeleteMessage<RideWrapper> _) => await LoadAsync();

        public async Task LoadAsync()
        {
            DriverRides.Clear();
            var rides = await _rideFacade.GetUserRides(LoggedInUser);
            DriverRides.AddRange(rides!);
        }

        public override void LoadInDesignMode()
        {
            DriverRides.Add(new RideListModel(Start: "Praha", End: "Olomouc", DateTime.MaxValue, 4));
        }
    }
}
