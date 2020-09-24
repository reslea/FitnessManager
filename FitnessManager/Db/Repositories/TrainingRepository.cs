using System;
using System.Collections.Generic;
using System.Linq;
using FitnessManager.Db.Entities;
using Infrastructure.Data;

namespace FitnessManager.Db.Repositories
{
    public class TrainingRepository : Repository<Training>, ITrainingRepository
    {
        public TrainingRepository(FitnessDbContext context) : base(context)
        {
        }

        public IEnumerable<Training> GetForRange(DateTime dateFrom, DateTime dateTo)
        {
            return DbSet.Where(t => 
                t.StartTime >= dateFrom && t.StartTime <= dateTo);
        }
    }

    public interface ITrainingRepository : IRepository<Training>
    {
        IEnumerable<Training> GetForRange(DateTime dateFrom, DateTime dateTo);
    }
}
