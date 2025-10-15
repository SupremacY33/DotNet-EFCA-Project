using Microsoft.EntityFrameworkCore;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly StudentDbContext _studentDbContext;

        public Repository(StudentDbContext studentDbContext)
        {
            _studentDbContext = studentDbContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _studentDbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetbyIdAsync(int Id)
        {
            return await _studentDbContext.Set<T>().FindAsync(Id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _studentDbContext.Set<T>().AddAsync(entity);
            await _studentDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _studentDbContext.Set<T>().Update(entity);
            await _studentDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int Id)
        {
            var entity = await GetbyIdAsync(Id);
            if (entity != null)
            {
                _studentDbContext.Set<T>().Remove(entity);
                await _studentDbContext.SaveChangesAsync();
            }
        }
    }
}
