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

    public override  async Task DeleteAsync(Guid id)
    {
        await using var uow = _uow.Create();
        var db = uow.GetRepository<UserRideEntity>().Get().Where(ride => ride.Id == id);
        foreach (var res in db.Select(x => x.Id))
        {
            uow.GetRepository<UserRideEntity>().Delete(res);
        }
        await uow.CommitAsync();

        await base.DeleteAsync(id);
    }

    public async Task<IEnumerable<RideListModel>?> GetPassengerRides(Guid? id)
    {
        if (id == null)
        {
            return new List<RideListModel>();
        }

        await using var _uowCreated = _uow.Create();
        var queryUserRides = _uowCreated.GetRepository<UserRideEntity>().Get();
        var queryRides = _uowCreated.GetRepository<RideEntity>().Get();
        var userRides = queryUserRides.Where(x => x.PassengerId == id);
        var idsToFind = userRides.Select(x => x.RideId).ToList();
        var newRideList = queryRides.Where(x => idsToFind.Any(id => id == x.Id));

        var userRideList = await _mapper.ProjectTo<RideListModel>(newRideList).ToListAsync().ConfigureAwait(false);
        //foreach (var variable in userRideList)
        //{
        //    Console.WriteLine("Vyfiltrovana:  ");
        //    Console.WriteLine(variable);
        //}
        return userRideList;
    }

    public async Task<IEnumerable<RideListModel>?> GetDriverRides(Guid? id)
    {
        if (id == null)
        {
            return new List<RideListModel>();
        }

        await using var _uowCreated = _uow.Create();
        var queryDriverRides = _uowCreated.GetRepository<RideEntity>().Get();

        //foreach (var variable in queryUserCars)
        //{
        //    Console.WriteLine(variable);
        //}
        //Console.WriteLine("");

        var driverRides = queryDriverRides.Include(x => x.Car).Where(x => x.Car!.OwnerId == id);

        foreach (var variable in driverRides)
        {
            Console.WriteLine("Vyfiltrovana:  ");
            Console.WriteLine(variable);
        }

        var driverRideList = await _mapper.ProjectTo<RideListModel>(driverRides).ToListAsync().ConfigureAwait(false);

        return driverRideList;
    }

    public async Task<IEnumerable<UserRideDetailModel>?> GetPassengers(Guid? id)
    {
        if (id == null)
        {
            return new List<UserRideDetailModel>();
        }

        await using var _uowCreated = _uow.Create();
        var queryUserRides = _uowCreated.GetRepository<UserRideEntity>().Get();
        //foreach (var variable in queryUserRides)
        //{
        //    Console.WriteLine(variable);
        //}

        var userRides = queryUserRides.Where(x => x.RideId == id);
        //foreach (var variable in userRides)
        //{
        //    Console.WriteLine("Vyfiltrovana:  ");
        //    Console.WriteLine(variable);
        //}
        var userRideModel = await _mapper.ProjectTo<UserRideDetailModel>(userRides).ToListAsync().ConfigureAwait(false);


        return userRideModel;
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
}