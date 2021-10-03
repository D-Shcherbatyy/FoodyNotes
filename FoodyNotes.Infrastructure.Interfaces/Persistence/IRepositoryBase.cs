using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodyNotes.Infrastructure.Interfaces.Persistence
{
  public interface IRepositoryBase<TEntity, in TId>
    where TEntity : class
  {
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        
    Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
        
    Task<IEnumerable<TEntity>> GetRangeByIdsAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    void Delete(TEntity entity);

    void Update(TEntity entity);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
  }
}