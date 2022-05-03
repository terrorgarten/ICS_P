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
        var db = uowCreate.GetRepository<RideEntity>().Get();
        var ride = await db.Include(ride => ride.Car).SingleOrDefaultAsync(ride => ride.Id == id);

        return _mapper.Map<RideDetailModel>(ride);

    }

    public async Task<IEnumerable<RideListModel>> GetFilteredListAsync(Guid? id, DateTime? timeFrom, DateTime? timeTo,
        string? startCity, string? endCity)
    {
        if (id == null)
        {
            return new List<RideListModel>();
        }

        await using var _uowCreated = _uow.Create();
        var db = _uowCreated.GetRepository<RideEntity>().Get();
        /* foreach (var VARIABLE in db)
         {
             Console.WriteLine(VARIABLE);
         } */
        var rides = db.Where(x => 
                    x.Start == startCity && x.End == endCity &&
                    x.BeginTime >= timeFrom && x.BeginTime <= timeTo);

        foreach (var VARIABLE in rides)
        {
            Console.WriteLine(VARIABLE);
        }

        var rideList = await _mapper.ProjectTo<RideListModel>(rides).ToListAsync().ConfigureAwait(false);
        
        return rideList;
    }
}