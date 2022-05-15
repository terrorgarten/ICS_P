using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Carpool.App.Commands;
using Carpool.App.Extensions;
using Carpool.App.Messages;
using Carpool.App.Services;
using Carpool.App.Wrappers;
using Carpool.BL.Facades;
using Carpool.BL.Models;

namespace Carpool.App.ViewModels;

public class RideListViewModel : ViewModelBase, IRideListViewModel
{
    private readonly IMediator _mediator;
    private readonly RideFacade _rideFacade;

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


    public ObservableCollection<RideListModel> DriverRides { get; set; } = new();
    private static Guid? CurrentUserId { get; set; }
    public ICommand RideSelectedCommand { get; }
    public ICommand RideNewCommand { get; }

    public async Task LoadAsync()
    {
        DriverRides.Clear();
        var rides = await _rideFacade.GetDriverRides(CurrentUserId);
        DriverRides.AddRange(rides!);
        DriverRides = new ObservableCollection<RideListModel>(DriverRides.Distinct());
    }

    private async void OnUserSelected(SelectedMessage<UserWrapper> obj)
    {
        CurrentUserId = obj.Id;
        await LoadAsync();
    }


    private void RideNew()
    {
        _mediator.Send(new NewMessage<RideWrapper>());
    }

    //Toto se vola pokud dostanu command
    private void RideSelected(RideListModel? ride)
    {
        _mediator.Send(new SelectedMessage<RideWrapper> { Id = ride?.Id });
    }

    private async void RideUpdated(UpdateMessage<RideWrapper> _)
    {
        await LoadAsync();
    }

    private async void RideDeleted(DeleteMessage<RideWrapper> _)
    {
        await LoadAsync();
    }

    public override void LoadInDesignMode()
    {
        DriverRides.Add(new RideListModel("Praha", "Olomouc", DateTime.MaxValue, 4));
    }
}