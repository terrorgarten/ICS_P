using Carpool.App.Factories;
using Carpool.App.Messages;
using Carpool.App.Services;
using Carpool.App.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Carpool.App.Commands;
using Carpool.BL.Models;
using Carpool.Common.Enums;

namespace Carpool.App.ViewModels
{
    public class AppStartViewModel : ViewModelBase
    {
        private readonly IFactory<IUserDetailViewModel> _userDetailViewModelFactory;
        public int SelectedIndex { get; set; }
        public AppStartViewModel(
            IUserListViewModel userListViewModel,
            IMediator mediator,
            IFactory<IUserDetailViewModel> userDetailViewModelFactory
        )
        {
            UserListViewModel = userListViewModel;
            _userDetailViewModelFactory = userDetailViewModelFactory;
            UserDetailViewModel = _userDetailViewModelFactory.Create();

            CloseUserDetailTabCommand = new RelayCommand<IUserDetailViewModel>(OnCloseUserDetailTabExecute);

            mediator.Register<NewMessage<UserWrapper>>(OnUserNewMessage);
            mediator.Register<SelectedMessage<UserWrapper>>(OnUserSelected);
            mediator.Register<DeleteMessage<UserWrapper>>(OnUserDeleted);
            SelectedIndex = 0;
        }

        public IUserListViewModel UserListViewModel { get; }
        public IUserDetailViewModel UserDetailViewModel { get; }

        public ICommand CloseUserDetailTabCommand { get; }

        public ObservableCollection<IUserDetailViewModel> UserDetailViewModels { get; } =
            new ObservableCollection<IUserDetailViewModel>();

        public IUserDetailViewModel? SelectedUserDetailViewModel { get; set; }

        private void OnUserNewMessage(NewMessage<UserWrapper> _)
        {
            SelectUser(Guid.Empty);
        }
        
        private void OnUserSelected(SelectedMessage<UserWrapper> message)
        {
            SelectedIndex = 1;
            SelectUser(message.Id);
        }


        private void OnUserDeleted(DeleteMessage<UserWrapper> message)
        {
            var user = UserDetailViewModels.SingleOrDefault(i => i.Model?.Id == message.Id);
            if (user != null)
            {
                UserDetailViewModels.Remove(user);
            }
        }

        private void SelectUser(Guid? id)
        {
            if (id is null)
            {
                SelectedUserDetailViewModel = null;
            }

            else
            {
                var userDetailViewModel = UserDetailViewModels.SingleOrDefault(vm => vm.Model?.Id == id);
                if (userDetailViewModel == null)
                {
                    userDetailViewModel = _userDetailViewModelFactory.Create();
                    UserDetailViewModels.Add(userDetailViewModel);
                    userDetailViewModel.LoadAsync(id.Value);
                }

                SelectedUserDetailViewModel = userDetailViewModel;
            }
        }

        private void OnCloseUserDetailTabExecute(IUserDetailViewModel? userDetailViewModel)
        {
            
            if (userDetailViewModel is not null)
            {
                // TODO: Check if the Detail has changes and ask user to cancel
                UserDetailViewModels.Remove(userDetailViewModel);
            }
            SelectedIndex = 0;
        }
    }
}