using System.Collections.Generic;
using Infrastructure.Data;

namespace FitnessManager.Db.Entities
{
    public class Coach : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public TrainingType Specialty { get; set; }

        public IEnumerable<Training> Trainings { get; set; }
    }
}
