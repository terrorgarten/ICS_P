using AutoMapper;
using carpool.BL.Models;
using carpool.DAL.Entities;
using carpool.DAL.UnitOfWork;

namespace carpool.BL.Facades;

public class CarFacade : CRUDFacade<CarEntity, CarListModel, CarDetailModel>
{
    public CarFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
    }
}