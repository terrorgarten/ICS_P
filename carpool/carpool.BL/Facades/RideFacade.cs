using System.Formats.Asn1;
using System.Linq;
using AutoMapper;
using Carpool.BL.Facades;
using Carpool.BL.Models;
using Carpool.DAL.Entities;
using Carpool.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Carpool.BL.Facades;

public class RideFacade : CRUDFacade<RideEntity, RideListModel, RideDetailModel>
{
    private readonly IUnitOfWorkFactory _uow;
    private readonly IMapper _mapper;
    public RideFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
        _uow = unitOfWorkFactory;
        _mapper = mapper;
    }

    public override async Task<RideDetailModel?> GetAsync(Guid id)
    {
        await using var uowCreate = _uow.Create();
        var db = uowCreate.GetRepository<RideEntity>().Get().Where(ride => ride.Id == id);

        return await _mapper.ProjectTo<RideDetailModel>(db).SingleOrDefaultAsync().ConfigureAwait(false);

    }

    public async Task<IEnumerable<RideListModel>> GetFilteredListAsync(Guid? id, DateTime? timeFrom, DateTime? timeTo,
        string? startCity, string? endCity)
    {
        if (id == null)
        {
            return new List<RideListModel>();
        }

        await using var _uowCreated = _uow.Create();
        var queryRides = _uowCreated.GetRepository<RideEntity>().Get();
        //foreach (var variable in queryRides)
        //{
        //    Console.WriteLine(variable);
        //} 
        var rides = queryRides.Where(x => 
                    x.Start == startCity && x.End == endCity &&
                    x.BeginTime >= timeFrom && x.BeginTime <= timeTo);
        //foreach (var variable in rides)
        //{
        //    Console.WriteLine("Vyfiltrovana:  ");
        //    Console.WriteLine(variable);

        //}

        var rideList = await _mapper.ProjectTo<RideListModel>(rides).ToListAsync().ConfigureAwait(false);
        
        return rideList;
    }

    public async Task<IEnumerable<RideListModel>?> GetUserRides(Guid? id)
    {
        if (id == null)
        {
            return new List<RideListModel>();
        }

        await using var _uowCreated = _uow.Create();
        var queryUserCars = _uowCreated.GetRepository<RideEntity>().Get();

        //foreach (var variable in queryUserCars)
        //{
        //    Console.WriteLine(variable);
        //}
        //Console.WriteLine("");

        var driverRides = queryUserCars.Where(x => x.UserId == id);

        foreach (var variable in driverRides)
        {
            Console.WriteLine("Vyfiltrovana:  ");
            Console.WriteLine(variable);
        }

        var driverRideList = await _mapper.ProjectTo<RideListModel>(driverRides).ToListAsync().ConfigureAwait(false);

        return driverRideList;
    }
}