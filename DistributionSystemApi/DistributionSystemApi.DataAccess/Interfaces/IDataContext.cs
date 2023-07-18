namespace DistributionSystemApi.Data.Interfaces
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DistributionSystemApi.Data.Entities;

    public interface IDataContext
    {
        IQueryable<TEntity> Get<TEntity>()
            where TEntity : BaseEntity;

        void Create<TEntity>(TEntity entity)
            where TEntity : BaseEntity;

        void Update<TEntity>(TEntity entity)
            where TEntity : BaseEntity;

        void Remove<TEntity>(TEntity entity)
            where TEntity : BaseEntity;

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
