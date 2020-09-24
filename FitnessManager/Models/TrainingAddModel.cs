using System;
using System.ComponentModel.DataAnnotations;
using FitnessManager.Db.Entities;

namespace FitnessManager.Models
{
    public class TrainingAddModel
    {
        [Required, MaxLength(255)]
        public string Title { get; set; }

        public TrainingType TrainingType { get; set; }
        
        public int CoachId { get; set; }

        public int HallId { get; set; }

        public DateTime StartTime { get; set; }
    }
}
