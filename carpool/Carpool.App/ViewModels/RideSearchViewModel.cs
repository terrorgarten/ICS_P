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
using Carpool.App.Services.MessageDialog;
using Carpool.BL.Facades;
using Carpool.Common.Enums;
using Carpool.DAL.Seeds;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Carpool.App.ViewModels
{
    public class RideSearchViewModel : ViewModelBase, IRideSearchViewModel
    {
        private readonly RideFacade _rideFacade;
        private readonly UserRideFacade _userRideFacade;
        private readonly IMediator _mediator;
        private readonly IMessageDialogService _messageDialogService;
        private Guid? CurrentRideId;

        public RideSearchViewModel(
            UserRideFacade userRideFacade,
            RideFacade rideFacade,
            IMediator mediator,
            IMessageDialogService messageDialogService
            )
        {
            _rideFacade = rideFacade;
            _mediator = mediator;
            _messageDialogService = messageDialogService;
            _userRideFacade = userRideFacade;

            RideSelectedCommand = new RelayCommand<RideListModel>(RideSelected);
            RegisterForRideCommand = new RelayCommand(RegisterForRide);
            FilterRides = new RelayCommand(GetFilteredRides);

            mediator.Register<SelectedMessage<UserWrapper>>(OnUserSelected);
        }

        private void RideSelected(RideListModel? obj)
        {
            if (obj != null)
            {
                CurrentRideId = obj.Id;
            }
        }

        async private void GetFilteredRides()
        {

            if ((StartCity == String.Empty || EndCity == String.Empty) || StartCity == null || EndCity == null)
            {
                var warning = _messageDialogService.Show(
                    "Nevalidny filter",
                    "Pocatecne mesto a konecne mesto su nevyhnutne",
                    MessageDialogButtonConfiguration.OK,
                    MessageDialogResult.OK);


                await LoadAsync();
            }
            else
            {
                FoundRides.Clear();
                var rides = await _rideFacade.GetFilteredListAsync(StartFrom, StartTo, StartCity, EndCity);
                FoundRides.AddRange(rides!);
                FoundRides = new ObservableCollection<RideListModel>(FoundRides.Distinct());
            }

        }

        async private void RegisterForRide()
        {
            if (CurrentRideId is null)
            {
                var warning = _messageDialogService.Show(
                    "Upozorneni",
                    "Musi byt vybrana jizda, do ktere se chces prihlasit",
                    MessageDialogButtonConfiguration.OK,
                    MessageDialogResult.OK);
            }
            try
            {
                var _ = await _userRideFacade.SaveCheckAsync(CurrentUserId, CurrentRideId);
                _mediator.Send(new UpdatePassengerRidesMessage<RideWrapper>());

            }
            catch (Exception e)
            {
                var warning = _messageDialogService.Show(
                    "Upozorneni",
                    $"{e.Message}",
                    MessageDialogButtonConfiguration.OK,
                    MessageDialogResult.OK);
            }
        }

        public ObservableCollection<RideListModel> FoundRides { get; set; } = new();
        private Guid? CurrentUserId { get; set; }

        public ICommand RideSelectedCommand { get; }

        public ICommand RegisterForRideCommand { get; }

        public ICommand FilterRides { get; }

        public string? StartCity
        {
            get;
            set;
        }
        public string? EndCity
        {
            get;
            set;
        }

        public DateTime? StartFrom { get; set; } = DateTime.MinValue;
        public DateTime? StartTo { get; set; } = DateTime.MaxValue;

        private void OnUserSelected(SelectedMessage<UserWrapper> obj)
        {
            CurrentUserId = obj.Id;
            _ = LoadAsync();
        }


        public async Task LoadAsync()
        {
            FoundRides.Clear();
            var rides = await _rideFacade.GetAsync();
            FoundRides.AddRange(rides!);
            FoundRides = new ObservableCollection<RideListModel>(FoundRides.Distinct());
        }


    }
}
