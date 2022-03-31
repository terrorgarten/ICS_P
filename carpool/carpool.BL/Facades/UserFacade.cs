using AutoMapper;
using carpool.BL.Models;
using carpool.DAL.Entities;
using carpool.DAL.UnitOfWork;

namespace carpool.BL.Facades;

public class UserFacade : CRUDFacade<UserEntity, UserListModel, UserDetailModel>
{
    public UserFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
    }
}