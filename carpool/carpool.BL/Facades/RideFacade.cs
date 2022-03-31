using AutoMapper;
using carpool.BL.Facades;
using carpool.BL.Models;
using carpool.DAL.Entities;
using carpool.DAL.UnitOfWork;

namespace carpool.BL.Facades;

public class RideFacade : CRUDFacade<RideEntity, RideListModel, RideDetailModel>
{
    public RideFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
    }
}