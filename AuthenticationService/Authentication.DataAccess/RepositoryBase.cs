using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Entities.Authentication.Entities;
using Authentication.Infrastructure.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Authentication.DataAccess.Postgres
{
  public class RepositoryBase<TEntity, TId> : IRepositoryBase<TEntity, TId> 
    where TEntity : Entity<TId>
  {
    private readonly ApplicationDbContext _dbContext;

    public RepositoryBase(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
      return await _dbContext.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public async Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
      return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public Task<IEnumerable<TEntity>> GetRangeByIdsAsync(IEnumerable<TId> ids, CancellationToken cancellationToken)
    {
      throw new System.NotImplementedException();
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
      await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public void Delete(TEntity entity)
    {
      _dbContext.Set<TEntity>().Remove(entity);
    }

    public void Update(TEntity entity)
    {
      _dbContext.Update(entity);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
      await _dbContext.SaveChangesAsync(cancellationToken);
  }
}