using AutoMapper;
using Carpool.BL.Models;
using Carpool.DAL.Entities;
using Carpool.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Carpool.BL.Facades;

public class UserFacade : CRUDFacade<UserEntity, UserListModel, UserDetailModel>
{
    private readonly IUnitOfWorkFactory _uow;
    private readonly IMapper _mapper;
    public UserFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
        _uow = unitOfWorkFactory;
        _mapper = mapper;
    }

    public override async Task<UserDetailModel?> GetAsync(Guid id)
    {
        // Je to kvuli testu
        
        if (id == Guid.Empty)
        {
            return null;
        }

        await using var uow = _uow.Create();
        var query = uow
            .GetRepository<UserEntity>()
            .Get()
            .Where(e => e.Id == id);

        var userModel = await _mapper.ProjectTo<UserDetailModel>(query).SingleOrDefaultAsync().ConfigureAwait(false);
        if (userModel is null)
            throw new InvalidDataException();

        var driverRides =
            uow.GetRepository<RideEntity>().Get().Include(x => x.Car).Where(x => x.Car!.OwnerId == id);

        userModel.DriverRides.AddRange(await _mapper.ProjectTo<RideDetailModel>(driverRides).ToArrayAsync()
            .ConfigureAwait(false));

        return userModel;
    }

    public override async Task DeleteAsync(Guid id)
    {
        // Delete rides where the user is the driver
        var uow = _uow.Create();
        var db = uow.GetRepository<RideEntity>();
        var query = db.Get()
            .Include(x => x.Car)
            .Where(x => x.Car != null && x.Car.OwnerId == id);

        foreach (var ride in query)
            db.Delete(ride.Id);

        // Delete owned cars
        var cardDb = uow.GetRepository<CarEntity>();
        var carQuery = cardDb.Get().Where(x => x.OwnerId == id);
        foreach (var car in carQuery)
            cardDb.Delete(car.Id);

        await uow.CommitAsync();
        await base.DeleteAsync(id);
    }
} 
