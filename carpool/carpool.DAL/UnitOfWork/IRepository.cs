using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace carpool.DAL.UnitOfWork;

public interface IRepository<TEntity> where TEntity : class, IEntity
{
    IQueryable<TEntity> Get();
    void Delete(Guid entityId);
    Task<TEntity> InsertOrUpdateAsync<TModel>(
        TModel model,
        IMapper mapper,
        CancellationToken cancellationToken = default) where TModel : class;
}