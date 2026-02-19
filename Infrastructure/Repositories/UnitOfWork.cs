using Domain.Contracts;
using Domain.Entities;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork(TM_DbContext _context) : IUnitOfWork
    {
        private ConcurrentDictionary<string, object> _repositories = new ConcurrentDictionary<string, object>();

        public IGenericRepository< TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            return (IGenericRepository<TEntity>)_repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity>(_context));
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
