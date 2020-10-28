using System.Linq;
using System.Threading.Tasks;
using FitnessManager.Db.Entities;
using FitnessManager.Db.Repositories;
using FitnessManager.Models;
using FitnessManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingService _trainingService;
        private readonly ICoachRepository _coachRepository;
        private readonly IHallRepository _hallRepository;

        public TrainingController(
            ITrainingService trainingService,
            ICoachRepository coachRepository,
            IHallRepository hallRepository)
        {
            _trainingService = trainingService;
            _coachRepository = coachRepository;
            _hallRepository = hallRepository;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] TrainingAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var coach = await _coachRepository.GetAsync(model.CoachId);
            if (coach == null)
            {
                return BadRequest($"No coach was found with Id:{model.CoachId}");
            }

            var hall = await _hallRepository.GetAsync(model.HallId);
            if (hall == null)
            {
                return BadRequest($"No hall was found with Id:{model.HallId}");
            }

            if (await _trainingService.HasConflictingTraining(model))
            {
                return BadRequest($"Cannot add training for this time");
            }

            await _trainingService.Add(model);

            return Accepted();
        }
    }
}