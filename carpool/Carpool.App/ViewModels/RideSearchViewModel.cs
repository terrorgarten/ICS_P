using System;
using System.Collections.ObjectModel;
using System.Linq;
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

public class RideSearchViewModel : ViewModelBase, IRideSearchViewModel
{
    private readonly IMediator _mediator;
    private readonly IMessageDialogService _messageDialogService;
    private readonly RideFacade _rideFacade;
    private readonly UserRideFacade _userRideFacade;
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

    public ObservableCollection<RideListModel> FoundRides { get; set; } = new();
    private Guid? CurrentUserId { get; set; }

    public ICommand RideSelectedCommand { get; }

    public ICommand RegisterForRideCommand { get; }

    public ICommand FilterRides { get; }

    public string? StartCity { get; set; }

    public string? EndCity { get; set; }

    public DateTime? StartFrom { get; set; } = DateTime.MinValue;
    public DateTime? StartTo { get; set; } = DateTime.MaxValue;


    public async Task LoadAsync()
    {
        FoundRides.Clear();
        var rides = await _rideFacade.GetAsync();
        FoundRides.AddRange(rides!);
        FoundRides = new ObservableCollection<RideListModel>(FoundRides.Distinct());
    }

    private void RideSelected(RideListModel? obj)
    {
        if (obj != null) CurrentRideId = obj.Id;
    }

    private async void GetFilteredRides()
    {
        if (StartCity == string.Empty || EndCity == string.Empty || StartCity == null || EndCity == null)
        {
            var _ = _messageDialogService.Show(
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

    private async void RegisterForRide()
    {
        if (CurrentRideId is null)
        {
            var _ = _messageDialogService.Show(
                "Upozorneni",
                "Musi byt vybrana jizda, do ktere se chces prihlasit",
                MessageDialogButtonConfiguration.OK,
                MessageDialogResult.OK);
        }

        try
        {
            var _ = await _userRideFacade.SaveCheckAsync(CurrentUserId, CurrentRideId);
            if (CurrentRideId is not null)
            {
                var success_join = _messageDialogService.Show(
                                "Přihlášení na jízdu", 
                                "Úspěšně jste se přihlásili na jízdu. Šťastnou cestu!",
                                MessageDialogButtonConfiguration.OK,
                                MessageDialogResult.OK);
            }
            
            _mediator.Send(new UpdatePassengerRidesMessage<RideWrapper>());
        }
        catch (Exception e)
        {
            var _ = _messageDialogService.Show(
                "Upozorneni",
                $"{e.Message}",
                MessageDialogButtonConfiguration.OK,
                MessageDialogResult.OK);
        }
    }

    private async void OnUserSelected(SelectedMessage<UserWrapper> obj)
    {
        CurrentUserId = obj.Id;
        await LoadAsync();
    }
}