using FitnessManager.Db.Entities;
using Infrastructure.Data;

namespace FitnessManager.Db.Repositories
{
    public class HallRepository : Repository<Hall>, IHallRepository
    {
        public HallRepository(FitnessDbContext context) : base(context) {}
    }

    public interface IHallRepository : IRepository<Hall>
    {
    }
}