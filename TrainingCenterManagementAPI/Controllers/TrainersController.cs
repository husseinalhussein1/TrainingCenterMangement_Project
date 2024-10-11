using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.Interfaces;
using TrainingCenterManagementAPI.ViewModels;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrainingCenterManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainersController : ControllerBase
    {
        private readonly ITrainerRepository _trainerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TrainersController> _logger;
        private readonly int MaxPageSize = 10;

        public TrainersController(
            ITrainerRepository trainerRepository,
            IMapper mapper,
            ILogger<TrainersController> logger)
        {
            _trainerRepository = trainerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/trainers
        [HttpGet(Name = "GetTrainers")]
        public async Task<ActionResult<List<TrainerViewModel>>> GetTrainers(int pageNumber = 1, int pageSize = 5, string? keyword = null)
        {
            if (pageSize > MaxPageSize)
            {
                _logger.LogError($"PageSize {pageSize} exceeds the maximum {MaxPageSize}");
                return BadRequest($"PageSize cannot exceed {MaxPageSize}");
            }

            var trainers = await Task.Run(() => _trainerRepository.All());
            var trainerViewModels = _mapper.Map<List<TrainerViewModel>>(trainers);

            var paginationData = new { PageNumber = pageNumber, PageSize = pageSize };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationData));

            return Ok(trainerViewModels);
        }

        // GET: api/trainers/{id}
        [HttpGet("{id}", Name = "GetTrainerById")]
        public async Task<ActionResult<TrainerViewModel>> GetTrainerById(Guid id)
        {
            var trainer = await Task.Run(() => _trainerRepository.GeT(id));
            if (trainer == null)
            {
                _logger.LogError($"Trainer with ID {id} not found.");
                return NotFound();
            }

            var trainerViewModel = _mapper.Map<TrainerViewModel>(trainer);
            return Ok(trainerViewModel);
        }

        // POST: api/trainers
        [HttpPost]
        public async Task<ActionResult<Trainer>> CreateTrainer([FromBody] TrainerCreateModel trainerCreateModel)
        {
            if (trainerCreateModel == null)
            {
                return BadRequest("Trainer data cannot be null.");
            }

            var trainer = _mapper.Map<Trainer>(trainerCreateModel);
            var newTrainer = await Task.Run(() => _trainerRepository.Add(trainer));

            return CreatedAtRoute("GetTrainerById", new { id = newTrainer.Id }, newTrainer);
        }

        // PUT: api/trainers/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTrainer(Guid id, [FromBody] TrainerUpdateModel trainerUpdateModel)
        {
            var existingTrainer = await Task.Run(() => _trainerRepository.GeT(id));
            if (existingTrainer == null)
            {
                _logger.LogError($"Trainer with ID {id} not found.");
                return NotFound();
            }

            _mapper.Map(trainerUpdateModel, existingTrainer);
            await Task.Run(() => _trainerRepository.Update(existingTrainer));

            return NoContent();
        }

        // PATCH: api/trainers/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdateTrainer(Guid id, [FromBody] JsonPatchDocument<TrainerUpdateModel> patchDocument)
        {
            var existingTrainer = await Task.Run(() => _trainerRepository.GeT(id));
            if (existingTrainer == null)
            {
                _logger.LogError($"Trainer with ID {id} not found.");
                return NotFound();
            }

            var trainerToPatch = _mapper.Map<TrainerUpdateModel>(existingTrainer);
            patchDocument.ApplyTo(trainerToPatch, ModelState);

            if (!TryValidateModel(trainerToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(trainerToPatch, existingTrainer);
            await Task.Run(() => _trainerRepository.Update(existingTrainer));

            return NoContent();
        }

        // DELETE: api/trainers/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTrainer(Guid id)
        {
            var trainer = await Task.Run(() => _trainerRepository.GeT(id));
            if (trainer == null)
            {
                _logger.LogError($"Trainer with ID {id} not found.");
                return NotFound();
            }

            await Task.Run(() => _trainerRepository.Delete(trainer));
            return NoContent();
        }
    }
}
