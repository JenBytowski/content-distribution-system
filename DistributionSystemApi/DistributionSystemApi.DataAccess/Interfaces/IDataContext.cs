using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DistributionSystemApi.Data.Interfaces
{
    public interface IDataContext
    {
        IQueryable<TEntity> Get<TEntity>() where TEntity : BaseEntity;

        void Create<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : BaseEntity;

        void Update<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : BaseEntity;

        void Remove<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : BaseEntity;

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
