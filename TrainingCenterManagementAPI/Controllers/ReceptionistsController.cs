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
    public class ReceptionistsController : ControllerBase
    {
        private readonly IReceptionistRepository _receptionistRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ReceptionistsController> _logger;
        private readonly int MaxPageSize = 10;

        public ReceptionistsController(
            IReceptionistRepository receptionistRepository,
            IMapper mapper,
            ILogger<ReceptionistsController> logger)
        {
            _receptionistRepository = receptionistRepository;
            _mapper = mapper;
            _logger = logger;
        }



        [AllowAnonymous]
        // GET: api/receptionists
        [HttpGet(Name = "GetReceptionists")]
        public async Task<ActionResult<List<ReceptionistViewModel>>> GetReceptionists(int pageNumber = 1, int pageSize = 5, string? keyword = null)
        {
            if (pageSize > MaxPageSize)
            {
                _logger.LogError($"PageSize {pageSize} exceeds the maximum {MaxPageSize}");
                return BadRequest($"PageSize cannot exceed {MaxPageSize}");
            }

            var receptionists = await Task.Run(() => _receptionistRepository.All());
            var receptionistViewModels = _mapper.Map<List<ReceptionistViewModel>>(receptionists);

            var paginationData = new { PageNumber = pageNumber, PageSize = pageSize };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationData));

            return Ok(receptionistViewModels);
        }


        [AllowAnonymous]
        // GET: api/receptionists/{id}
        [HttpGet("{id}", Name = "GetReceptionistById")]
        public async Task<ActionResult<ReceptionistViewModel>> GetReceptionistById(Guid id)
        {
            var receptionist = await Task.Run(() => _receptionistRepository.GeT(id));
            if (receptionist == null)
            {
                _logger.LogError($"Receptionist with ID {id} not found.");
                return NotFound();
            }

            var receptionistViewModel = _mapper.Map<ReceptionistViewModel>(receptionist);
            return Ok(receptionistViewModel);
        }



        [AllowAnonymous]
        // POST: api/receptionists
        [HttpPost]
        public async Task<ActionResult<Receptionist>> CreateReceptionist([FromBody] ReceptionistCreateModel receptionistCreateModel)
        {
            if (receptionistCreateModel == null)
            {
                return BadRequest("Receptionist data cannot be null.");
            }

            var receptionist = _mapper.Map<Receptionist>(receptionistCreateModel);
            var newReceptionist = await Task.Run(() => _receptionistRepository.Add(receptionist));

            return CreatedAtRoute("GetReceptionistById", new { id = newReceptionist.Id }, newReceptionist);
        }



        [AllowAnonymous]
        // PUT: api/receptionists/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateReceptionist(Guid id, [FromBody] ReceptionistUpdateModel receptionistUpdateModel)
        {
            var existingReceptionist = await Task.Run(() => _receptionistRepository.GeT(id));
            if (existingReceptionist == null)
            {
                _logger.LogError($"Receptionist with ID {id} not found.");
                return NotFound();
            }

            _mapper.Map(receptionistUpdateModel, existingReceptionist);
            await Task.Run(() => _receptionistRepository.Update(existingReceptionist));

            return NoContent();
        }




        [AllowAnonymous]
        // PATCH: api/receptionists/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdateReceptionist(Guid id, [FromBody] JsonPatchDocument<ReceptionistUpdateModel> patchDocument)
        {
            var existingReceptionist = await Task.Run(() => _receptionistRepository.GeT(id));
            if (existingReceptionist == null)
            {
                _logger.LogError($"Receptionist with ID {id} not found.");
                return NotFound();
            }

            var receptionistToPatch = _mapper.Map<ReceptionistUpdateModel>(existingReceptionist);
            patchDocument.ApplyTo(receptionistToPatch, ModelState);

            if (!TryValidateModel(receptionistToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(receptionistToPatch, existingReceptionist);
            await Task.Run(() => _receptionistRepository.Update(existingReceptionist));

            return NoContent();
        }



        [AllowAnonymous]
        // DELETE: api/receptionists/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReceptionist(Guid id)
        {
            var receptionist = await Task.Run(() => _receptionistRepository.GeT(id));
            if (receptionist == null)
            {
                _logger.LogError($"Receptionist with ID {id} not found.");
                return NotFound();
            }

            await Task.Run(() => _receptionistRepository.Delete(receptionist));
            return NoContent();
        }
    }
}
