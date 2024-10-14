using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.Interfaces;
using TrainingCenterManagementAPI.ViewModels;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TrainingCenterManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraineesController : ControllerBase
    {
        private readonly ITraineeRepository _traineeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TraineesController> _logger;
        private readonly int MaxPageSize = 10;

        public TraineesController(
            ITraineeRepository traineeRepository,
            IMapper mapper,
            ILogger<TraineesController> logger)
        {
            _traineeRepository = traineeRepository;
            _mapper = mapper;
            _logger = logger;
        }



        [Authorize(Roles = "TrainingOfficer,Receptionist")]
        // GET: api/trainees
        [HttpGet(Name = "GetTrainees")]
        public async Task<ActionResult<List<TraineeViewModel>>> GetTrainees(int pageNumber = 1, int pageSize = 5, string? keyword = null)
        {
            if (pageSize > MaxPageSize)
            {
                _logger.LogError($"PageSize {pageSize} exceeds the maximum {MaxPageSize}");
                return BadRequest($"PageSize cannot exceed {MaxPageSize}");
            }

            var trainees = await Task.Run(() => _traineeRepository.All());
            var traineeViewModels = _mapper.Map<List<TraineeViewModel>>(trainees);

            var paginationData = new { PageNumber = pageNumber, PageSize = pageSize };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationData));

            return Ok(traineeViewModels);
        }



        [Authorize(Roles = "TrainingOfficer,Receptionist")]
        // GET: api/trainees/{id}
        [HttpGet("{id}", Name = "GetTraineeById")]
        public async Task<ActionResult<TraineeViewModel>> GetTraineeById(Guid id)
        {
            var trainee = await Task.Run(() => _traineeRepository.GeT(id));
            if (trainee == null)
            {
                _logger.LogError($"Trainee with ID {id} not found.");
                return NotFound();
            }

            var traineeViewModel = _mapper.Map<TraineeViewModel>(trainee);
            return Ok(traineeViewModel);
        }



        [Authorize(Roles = "TrainingOfficer,Receptionist")]
        // POST: api/trainees
        [HttpPost]
        public async Task<ActionResult<Trainee>> CreateTrainee([FromBody] TraineeCreateModel traineeCreateModel)
        {
            if (traineeCreateModel == null)
            {
                return BadRequest("Trainee data cannot be null.");
            }

            var trainee = _mapper.Map<Trainee>(traineeCreateModel);
            var newTrainee = await Task.Run(() => _traineeRepository.Add(trainee));

            return CreatedAtRoute("GetTraineeById", new { id = newTrainee.Id }, newTrainee);
        }






        [Authorize(Roles = "TrainingOfficer,Receptionist")]
        // PUT: api/trainees/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTrainee(Guid id, [FromBody] TraineeUpdateModel traineeUpdateModel)
        {
            var existingTrainee = await Task.Run(() => _traineeRepository.GeT(id));
            if (existingTrainee == null)
            {
                _logger.LogError($"Trainee with ID {id} not found.");
                return NotFound();
            }

            _mapper.Map(traineeUpdateModel, existingTrainee);
            await Task.Run(() => _traineeRepository.Update(existingTrainee));

            return NoContent();
        }




        [Authorize(Roles = "TrainingOfficer,Receptionist")]
        // PATCH: api/trainees/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdateTrainee(Guid id, [FromBody] JsonPatchDocument<TraineeUpdateModel> patchDocument)
        {
            var existingTrainee = await Task.Run(() => _traineeRepository.GeT(id));
            if (existingTrainee == null)
            {
                _logger.LogError($"Trainee with ID {id} not found.");
                return NotFound();
            }

            var traineeToPatch = _mapper.Map<TraineeUpdateModel>(existingTrainee);
            patchDocument.ApplyTo(traineeToPatch, ModelState);

            if (!TryValidateModel(traineeToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(traineeToPatch, existingTrainee);
            await Task.Run(() => _traineeRepository.Update(existingTrainee));

            return NoContent();
        }




        [Authorize(Roles = "TrainingOfficer,Receptionist")]
        // DELETE: api/trainees/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTrainee(Guid id)
        {
            var trainee = await Task.Run(() => _traineeRepository.GeT(id));
            if (trainee == null)
            {
                _logger.LogError($"Trainee with ID {id} not found.");
                return NotFound();
            }

            await Task.Run(() => _traineeRepository.Delete(trainee));
            return NoContent();
        }
    }
}
