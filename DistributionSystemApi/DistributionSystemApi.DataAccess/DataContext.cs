﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DistributionSystemApi.Data.Interfaces;

namespace DistributionSystemApi.Data
{
    public class DataContext : IDataContext
    {
        private readonly ContentDistributionSystemContext contentDistributionSystemContext;

        public void Create<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            contentDistributionSystemContext.Set<TEntity>().Add(entity);
        }

        public IQueryable<TEntity> Get<TEntity>() where TEntity : BaseEntity
        {
            return contentDistributionSystemContext.Set<TEntity>();
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            contentDistributionSystemContext.Set<TEntity>().Remove(entity);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await contentDistributionSystemContext.SaveChangesAsync(cancellationToken);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            contentDistributionSystemContext.Set<TEntity>().Update(entity);
        }
    }
}
