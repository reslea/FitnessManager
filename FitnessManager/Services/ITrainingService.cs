using System.Threading.Tasks;
using FitnessManager.Models;

namespace FitnessManager.Services
{
    public interface ITrainingService
    {
        Task<bool> HasConflictingTraining(TrainingAddModel training);

        Task Add(TrainingAddModel training);
    }
}