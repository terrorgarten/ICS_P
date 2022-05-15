using AutoMapper;
using Carpool.BL.Models;
using Carpool.DAL.Entities;
using Carpool.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Carpool.BL.Facades;

public class CarFacade : CRUDFacade<CarEntity, CarListModel, CarDetailModel>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWorkFactory _uow;

    public CarFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
        _uow = unitOfWorkFactory;
        _mapper = mapper;
    }

    public override async Task DeleteAsync(Guid id)
    {
        await using var uow = _uow.Create();
        var db = uow.GetRepository<RideEntity>().Get().Where(ride => ride.CarId == id);
        foreach (var res in db.Select(x => x.Id)) uow.GetRepository<RideEntity>().Delete(res);
        await uow.CommitAsync();

        await base.DeleteAsync(id);
    }

    public async Task<IEnumerable<CarListModel>?> GetUserCars(Guid? id)
    {
        if (id == null) return new List<CarListModel>();

        await using var _uowCreated = _uow.Create();
        var queryUserCars = _uowCreated.GetRepository<CarEntity>().Get();
        var userCars = queryUserCars.Where(x => x.OwnerId == id);
        var carList = await _mapper.ProjectTo<CarListModel>(userCars).ToListAsync().ConfigureAwait(false);

        return carList;
    }

    public async Task<IEnumerable<CarDetailModel>?> GetUserCarsDetails(Guid? id)
    {
        if (id == null) return new List<CarDetailModel>();

        await using var _uowCreated = _uow.Create();
        var queryUserCars = _uowCreated.GetRepository<CarEntity>().Get();
        var userCars = queryUserCars.Where(x => x.OwnerId == id);
        var carDetail = await _mapper.ProjectTo<CarDetailModel>(userCars).ToListAsync().ConfigureAwait(false);

        return carDetail;
    }
}