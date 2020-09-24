using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessManager.Db.Entities;
using FitnessManager.Db.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FitnessManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallController : ControllerBase
    {
        private readonly IHallRepository _repository;

        public HallController(IHallRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hall>>> Get()
        {
            return Ok(await _repository.GetAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Hall>> Create([FromBody] Hall hall)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _repository.Create(hall);
            await _repository.SaveChangesAsync();
            return Ok(hall);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var toDelete = await _repository.GetAsync(id);

            if (toDelete == null)
            {
                return BadRequest($"hall with id:{id} wasn't found");
            }
            _repository.Delete(toDelete);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
    }
}