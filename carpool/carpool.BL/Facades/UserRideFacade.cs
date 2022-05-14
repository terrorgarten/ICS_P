using AutoMapper;
using Carpool.BL.Models;
using Carpool.DAL.Entities;
using Carpool.DAL.Seeds;
using Carpool.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Carpool.BL.Facades;

public class UserRideFacade : CRUDFacade<UserRideEntity, UserRideDetailModel, UserRideDetailModel>
{
    private readonly IUnitOfWorkFactory _uow;
    private readonly IMapper _mapper;

    public UserRideFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
        _uow = unitOfWorkFactory;
        _mapper = mapper;
    }
    

    public async Task<UserRideDetailModel?> SaveCheckAsync(Guid newPassengerId, Guid rideId)
    {
        var uowCreate = _uow.Create();
        var queryRides = uowCreate.GetRepository<RideEntity>().Get();
        var queryUsers = uowCreate.GetRepository<UserEntity>().Get();


        var rides = queryRides.Include(x => x.Car).Where(x => x.Id == rideId);
        var users = queryUsers.Where(x => x.Id == newPassengerId);
        var ride = rides.FirstOrDefault();
        var user = users.FirstOrDefault();

        if (ride == null)
        {
            // The ride for some reason does not exist
            
            return null;
        }

        var numPassengers = ride.PassengerRides.Count();
        if (ride.Car != null)
        {
            if (numPassengers == ride.Car.SeatCapacity)
            {
                
                return null;
            }
        }

        // Check if new Passenger is not already in the ride
        foreach (var userRide in ride.PassengerRides)
        {
            if (userRide.Id == newPassengerId)
            {
               
                return null;
            }
        }
        // Check if new Passenger is not the driver
        if (ride.Car.OwnerId == newPassengerId)
        {
            
            return null;
        }
        
        UserRideDetailModel? model = new UserRideDetailModel(user.Name, user.Surname)
        {
            PassengerId = user.Id,
            RideId = ride.Id,
            Id = Guid.NewGuid()
        };
        
        return await base.SaveAsync(model);

    }

    public async Task<IEnumerable<RideListModel>?> GetUserRides(Guid? id)
    {
        if(id == null)
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
        return userRideList;
    }

    public async Task<IEnumerable<UserRideDetailModel>?> GetPassengers(Guid? id)
    {
        if (id == null)
        {
            return new List<UserRideDetailModel>();
        }

        await using var _uowCreated = _uow.Create();
        var queryUserRides = _uowCreated.GetRepository<UserRideEntity>().Get();
        var userRides = queryUserRides.Where(x => x.RideId == id);
        var userRideModel = await _mapper.ProjectTo<UserRideDetailModel>(userRides).ToListAsync().ConfigureAwait(false);
        
            
        return userRideModel;
    }


}

