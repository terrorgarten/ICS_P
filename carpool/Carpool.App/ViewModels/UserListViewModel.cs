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
    public class UserListViewModel : ViewModelBase, IUserListViewModel
    {
        private readonly UserFacade _userFacade;
        private readonly IMediator _mediator;

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

        private void UserNew() => _mediator.Send(new NewMessage<UserWrapper>());

        //Toto se vola pokud dostanu command
        private void UserSelected(UserListModel? user)
        {
            _mediator.Send(new SelectedMessage<UserWrapper> { Id = user?.Id });
        } 

        private async void UserUpdated(UpdateMessage<UserWrapper> _) => await LoadAsync();

        private async void UserDeleted(DeleteMessage<UserWrapper> _) => await LoadAsync();

        public async Task LoadAsync()
        {
            Users.Clear();
            var users = await _userFacade.GetAsync();
            Users.AddRange(users);
        }

        public override void LoadInDesignMode()
        {
            Users.Add(new UserListModel(Name: "Arnold", Surname: "Schwarz"));
        }
    }
}
