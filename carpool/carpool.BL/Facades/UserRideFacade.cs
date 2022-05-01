using AutoMapper;
using Carpool.BL.Models;
using Carpool.DAL.Entities;
using Carpool.DAL.UnitOfWork;

namespace Carpool.BL.Facades;

public class UserRideFacade : CRUDFacade<UserRideEntity, UserRideDetailModel, UserRideDetailModel>
{
    public UserRideFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
    }
    public override Task<UserRideDetailModel> SaveAsync(UserRideDetailModel model)
        {
            //TEST
            return base.SaveAsync(model);
        }
}