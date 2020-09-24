using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DbContext Context;
        protected readonly DbSet<T> DbSet;

        protected Repository(DbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual void Create(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            DbSet.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public virtual async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}