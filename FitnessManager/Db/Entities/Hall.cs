using System.Collections.Generic;
using Infrastructure.Data;

namespace FitnessManager.Db.Entities
{
    public class Hall : BaseEntity
    {
        public string Title { get; set; }

        public int Capacity { get; set; }

        public IEnumerable<Training> Trainings { get; set; }
    }
}
