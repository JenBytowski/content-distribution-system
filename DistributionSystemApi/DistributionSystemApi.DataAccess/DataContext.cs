using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DistributionSystemApi.Data.Interfaces;

namespace DistributionSystemApi.Data
{
    public class DataContext : IDataContext
    {
        private readonly ContentDistributionSystemContext contentDistributionSystemContext;

        public async void Create<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : BaseEntity
        {
            entity.Id = Guid.NewGuid();

            contentDistributionSystemContext.Set<TEntity>().Add(entity);

            await SaveChangesAsync(cancellationToken);
        }

        public IQueryable<TEntity> Get<TEntity>() where TEntity : BaseEntity
        {
            return contentDistributionSystemContext.Set<TEntity>();
        }

        public async void Remove<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : BaseEntity
        {
            contentDistributionSystemContext.Set<TEntity>().Remove(entity);

            await SaveChangesAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await contentDistributionSystemContext.SaveChangesAsync(cancellationToken);
        }

        public async void Update<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : BaseEntity
        {
            contentDistributionSystemContext.Set<TEntity>().Update(entity);

            await SaveChangesAsync(cancellationToken);
        }
    }
}
