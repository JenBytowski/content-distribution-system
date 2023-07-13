using System.Linq;

namespace DistributionSystemApi.Data
{
    public interface IDataContext<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> Get();

        void Create(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
