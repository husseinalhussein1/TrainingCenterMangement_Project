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
    public class TrainingOfficersController : ControllerBase
    {
        private readonly ITrainingOfficerRepository _trainingOfficerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TrainingOfficersController> _logger;
        private readonly int MaxPageSize = 10;

        public TrainingOfficersController(
            ITrainingOfficerRepository trainingOfficerRepository,
            IMapper mapper,
            ILogger<TrainingOfficersController> logger)
        {
            _trainingOfficerRepository = trainingOfficerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/trainingofficers
        [HttpGet(Name = "GetTrainingOfficers")]
        public async Task<ActionResult<List<TrainingOfficerViewModel>>> GetTrainingOfficers(int pageNumber = 1, int pageSize = 5, string? keyword = null)
        {
            if (pageSize > MaxPageSize)
            {
                _logger.LogError($"PageSize {pageSize} exceeds the maximum {MaxPageSize}");
                return BadRequest($"PageSize cannot exceed {MaxPageSize}");
            }

            var trainingOfficers = await Task.Run(() => _trainingOfficerRepository.All());
            var trainingOfficerViewModels = _mapper.Map<List<TrainingOfficerViewModel>>(trainingOfficers);

            var paginationData = new { PageNumber = pageNumber, PageSize = pageSize };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationData));

            return Ok(trainingOfficerViewModels);
        }

        // GET: api/trainingofficers/{id}
        [HttpGet("{id}", Name = "GetTrainingOfficerById")]
        public async Task<ActionResult<TrainingOfficerViewModel>> GetTrainingOfficerById(Guid id)
        {
            var trainingOfficer = await Task.Run(() => _trainingOfficerRepository.GeT(id));
            if (trainingOfficer == null)
            {
                _logger.LogError($"TrainingOfficer with ID {id} not found.");
                return NotFound();
            }

            var trainingOfficerViewModel = _mapper.Map<TrainingOfficerViewModel>(trainingOfficer);
            return Ok(trainingOfficerViewModel);
        }

        // POST: api/trainingofficers
        [HttpPost]
        public async Task<ActionResult<TrainingOfficer>> CreateTrainingOfficer([FromBody] TrainingOfficerCreateModel trainingOfficerCreateModel)
        {
            if (trainingOfficerCreateModel == null)
            {
                return BadRequest("TrainingOfficer data cannot be null.");
            }

            var trainingOfficer = _mapper.Map<TrainingOfficer>(trainingOfficerCreateModel);
            var newTrainingOfficer = await Task.Run(() => _trainingOfficerRepository.Add(trainingOfficer));

            return CreatedAtRoute("GetTrainingOfficerById", new { id = newTrainingOfficer.Id }, newTrainingOfficer);
        }

        // PUT: api/trainingofficers/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTrainingOfficer(Guid id, [FromBody] TrainingOfficerUpdateModel trainingOfficerUpdateModel)
        {
            var existingTrainingOfficer = await Task.Run(() => _trainingOfficerRepository.GeT(id));
            if (existingTrainingOfficer == null)
            {
                _logger.LogError($"TrainingOfficer with ID {id} not found.");
                return NotFound();
            }

            _mapper.Map(trainingOfficerUpdateModel, existingTrainingOfficer);
            await Task.Run(() => _trainingOfficerRepository.Update(existingTrainingOfficer));

            return NoContent();
        }

        // PATCH: api/trainingofficers/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdateTrainingOfficer(Guid id, [FromBody] JsonPatchDocument<TrainingOfficerUpdateModel> patchDocument)
        {
            var existingTrainingOfficer = await Task.Run(() => _trainingOfficerRepository.GeT(id));
            if (existingTrainingOfficer == null)
            {
                _logger.LogError($"TrainingOfficer with ID {id} not found.");
                return NotFound();
            }

            var trainingOfficerToPatch = _mapper.Map<TrainingOfficerUpdateModel>(existingTrainingOfficer);
            patchDocument.ApplyTo(trainingOfficerToPatch, ModelState);

            if (!TryValidateModel(trainingOfficerToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(trainingOfficerToPatch, existingTrainingOfficer);
            await Task.Run(() => _trainingOfficerRepository.Update(existingTrainingOfficer));

            return NoContent();
        }

        // DELETE: api/trainingofficers/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTrainingOfficer(Guid id)
        {
            var trainingOfficer = await Task.Run(() => _trainingOfficerRepository.GeT(id));
            if (trainingOfficer == null)
            {
                _logger.LogError($"TrainingOfficer with ID {id} not found.");
                return NotFound();
            }

            await Task.Run(() => _trainingOfficerRepository.Delete(trainingOfficer));
            return NoContent();
        }
    }
}
