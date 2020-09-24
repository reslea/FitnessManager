using System;
using Infrastructure.Data;

namespace FitnessManager.Db.Entities
{
    public class Training : BaseEntity
    {
        public string Title { get; set; }

        public TrainingType TrainingType { get; set; }

        // Write
        public int CoachId { get; set; }
        
        public int HallId { get; set; }

        // Read
        public Coach Coach { get; set; }
        
        public Hall Hall { get; set; }

        public DateTime StartTime { get; set; }
    }
}
