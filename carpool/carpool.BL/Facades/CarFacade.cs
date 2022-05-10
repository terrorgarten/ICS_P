using AutoMapper;
using Carpool.BL.Models;
using Carpool.DAL.Entities;
using Carpool.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Carpool.BL.Facades;

public class CarFacade : CRUDFacade<CarEntity, CarListModel, CarDetailModel>
{
    private readonly IUnitOfWorkFactory _uow;
    private readonly IMapper _mapper;
    public CarFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
        _uow = unitOfWorkFactory;
        _mapper = mapper;
    }
    public async Task<IEnumerable<CarListModel>?> GetUserCars(Guid? id)
    {
        if (id == null)
        {
            return new List<CarListModel>();
        }

        await using var _uowCreated = _uow.Create();
        var queryUserCars = _uowCreated.GetRepository<CarEntity>().Get();

        //foreach (var variable in queryUserCars)
        //{
        //    Console.WriteLine(variable);
        //}
        //Console.WriteLine("");

        var userCars = queryUserCars.Where(x => x.OwnerId == id);

        foreach (var variable in userCars)
        {
            Console.WriteLine("Vyfiltrovana:  ");
            Console.WriteLine(variable);
        }

        var userRideList = await _mapper.ProjectTo<CarListModel>(userCars).ToListAsync().ConfigureAwait(false);

        return userRideList;
    }
}