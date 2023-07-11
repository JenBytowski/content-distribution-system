using System;
using System.Linq;

namespace DistributionSystemApi.Data
{
    public interface IDataContext<TEntity>
    {
        IQueryable<TEntity> Get(Guid Id);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void SaveChanges();
    }
}
