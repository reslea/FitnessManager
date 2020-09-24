using FitnessManager.Db.Entities;
using Infrastructure.Data;

namespace FitnessManager.Db.Repositories
{
    public class CoachRepository : Repository<Coach>, ICoachRepository
    {
        public CoachRepository(FitnessDbContext context) : base(context) {}
    }

    public interface ICoachRepository : IRepository<Coach>
    {
    }
}