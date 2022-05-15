using AutoMapper;
using Carpool.BL.Models;
using Carpool.DAL.Entities;
using Carpool.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Carpool.BL.Facades;

public class UserRideFacade : CRUDFacade<UserRideEntity, UserRideDetailModel, UserRideDetailModel>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWorkFactory _uow;

    public UserRideFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
        _uow = unitOfWorkFactory;
        _mapper = mapper;
    }


    public async Task<UserRideDetailModel?> SaveCheckAsync(Guid? newPassengerId, Guid? rideId)
    {
        if (newPassengerId is null || rideId is null) return null;
        var uowCreate = _uow.Create();
        var queryRides = uowCreate.GetRepository<RideEntity>().Get();
        var queryUsers = uowCreate.GetRepository<UserEntity>().Get();
        var queryUserRide = uowCreate.GetRepository<UserRideEntity>().Get();


        var rides = queryRides.Include(x => x.Car).Where(x => x.Id == rideId);
        var users = queryUsers.Where(x => x.Id == newPassengerId);
        var userRides = queryUserRide.Where(x => x.RideId == rideId);
        var ride = rides.FirstOrDefault();
        var user = users.FirstOrDefault();
        var cars = uowCreate.GetRepository<CarEntity>().Get();

        if (ride == null) return null;

        var numPassengers = userRides.Count();
        if (ride.Car != null)
        {
            if (numPassengers == ride.Car.SeatCapacity)
                throw new Exception("Neuspesne prihlaseni na jizdu, jizda je plna");

            if (ride.Car.OwnerId == newPassengerId)
                throw new Exception("Nemuzes se prihlasit na jizdu, na ktere jsi ridic");
        }

        // Check if new Passenger is not already in the ride
        foreach (var userRide in userRides)
            if (userRide.PassengerId == newPassengerId)
                throw new Exception("Nemuzes se na jizdu prihlasit 2x");

        UserRideDetailModel? model = new(user!.Name, user.Surname)
        {
            PassengerId = user.Id,
            RideId = ride.Id,
            Id = Guid.NewGuid()
        };

        return await base.SaveAsync(model);
    }

    public async Task<IEnumerable<RideListModel>?> GetUserRides(Guid? id)
    {
        if (id == null) return new List<RideListModel>();

        await using var _uowCreated = _uow.Create();
        var queryUserRides = _uowCreated.GetRepository<UserRideEntity>().Get();
        var queryRides = _uowCreated.GetRepository<RideEntity>().Get();
        var userRides = queryUserRides.Where(x => x.PassengerId == id);
        var idsToFind = userRides.Select(x => x.RideId).ToList();
        var newRideList = queryRides.Where(x => idsToFind.Any(id => id == x.Id));

        var userRideList = await _mapper.ProjectTo<RideListModel>(newRideList).ToListAsync().ConfigureAwait(false);
        return userRideList;
    }

    public async Task<IEnumerable<UserRideDetailModel>?> GetPassengers(Guid? id)
    {
        if (id == null) return new List<UserRideDetailModel>();

        await using var _uowCreated = _uow.Create();
        var queryUserRides = _uowCreated.GetRepository<UserRideEntity>().Get();
        var userRides = queryUserRides.Where(x => x.RideId == id);
        var userRideModel = await _mapper.ProjectTo<UserRideDetailModel>(userRides).ToListAsync().ConfigureAwait(false);


        return userRideModel;
    }
}