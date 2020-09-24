using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public interface IRepository<T> where T : BaseEntity
    {

        Task<IEnumerable<T>> GetAsync();

        Task<T> GetAsync(int id);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task SaveChangesAsync();
    }
}
