using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessManager.Db.Entities;
using FitnessManager.Db.Repositories;
using Infrastructure.Data;
using Infrastructure.Web.RequirePermission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoachController : ControllerBase
    {
        private readonly ICoachRepository _repository;

        public CoachController(ICoachRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Coach
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coach>>> Get()
        {
            return Ok(await _repository.GetAsync());
        }

        // GET: api/Coach/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<Coach>> Get(int id)
        {
            return Ok(await _repository.GetAsync(id));
        }

        // POST: api/Coach
        [HttpPost]
        [PermissionRequirement(PermissionType.AddCoaches)]
        public async Task<ActionResult<Coach>> Post([FromBody] Coach coach)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _repository.Create(coach);
            await _repository.SaveChangesAsync();
            return Ok(coach);
        }

        // DELETE: api/coach/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var toDelete = await _repository.GetAsync(id);

            if (toDelete == null)
            {
                return BadRequest($"coach with id:{id} wasn't found");
            }

            _repository.Delete(toDelete);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
