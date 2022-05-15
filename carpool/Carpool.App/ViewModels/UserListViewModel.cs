using System.Collections.ObjectModel;
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

public class UserListViewModel : ViewModelBase, IUserListViewModel
{
    private readonly IMediator _mediator;
    private readonly UserFacade _userFacade;

    public UserListViewModel(UserFacade userFacade, IMediator mediator)
    {
        _userFacade = userFacade;
        _mediator = mediator;

        UserSelectedCommand = new RelayCommand<UserListModel>(UserSelected);
        UserNewCommand = new RelayCommand(UserNew);

        mediator.Register<UpdateMessage<UserWrapper>>(UserUpdated);
        mediator.Register<DeleteMessage<UserWrapper>>(UserDeleted);
    }

    public ObservableCollection<UserListModel> Users { get; set; } = new();

    public ICommand UserSelectedCommand { get; }
    public ICommand UserNewCommand { get; }

    public async Task LoadAsync()
    {
        Users.Clear();
        var users = await _userFacade.GetAsync();
        Users.AddRange(users);
    }

    private void UserNew()
    {
        _mediator.Send(new NewMessage<UserWrapper>());
    }

    //Toto se vola pokud dostanu command
    private void UserSelected(UserListModel? user)
    {
        _mediator.Send(new SelectedMessage<UserWrapper> { Id = user?.Id });
    }

    private async void UserUpdated(UpdateMessage<UserWrapper> _)
    {
        await LoadAsync();
    }

    private async void UserDeleted(DeleteMessage<UserWrapper> _)
    {
        await LoadAsync();
    }

    public override void LoadInDesignMode()
    {
        Users.Add(new UserListModel("Arnold", "Schwarz"));
    }
}