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
    public class AdministratorsController : ControllerBase
    {
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AdministratorsController> _logger;
        private readonly int MaxPageSize = 10;

        public AdministratorsController(
            IAdministratorRepository administratorRepository,
            IMapper mapper,
            ILogger<AdministratorsController> logger)
        {
            _administratorRepository = administratorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/administrators
        [HttpGet(Name = "GetAdministrators")]
        public async Task<ActionResult<List<AdministratorViewModel>>> GetAdministrators(int pageNumber = 1, int pageSize = 5, string? keyword = null)
        {
            if (pageSize > MaxPageSize)
            {
                _logger.LogError($"PageSize {pageSize} exceeds the maximum {MaxPageSize}");
                return BadRequest($"PageSize cannot exceed {MaxPageSize}");
            }

            var administrators = await Task.Run(() => _administratorRepository.All());
            var administratorViewModels = _mapper.Map<List<AdministratorViewModel>>(administrators);

            // Pagination logic (if needed)
            var paginationData = new { PageNumber = pageNumber, PageSize = pageSize };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationData));

            return Ok(administratorViewModels);
        }

        // GET: api/administrators/{id}
        [HttpGet("{id}", Name = "GetAdministratorById")]
        public async Task<ActionResult<AdministratorViewModel>> GetAdministratorById(Guid id)
        {
            var administrator = await Task.Run(() => _administratorRepository.GeT(id));
            if (administrator == null)
            {
                _logger.LogError($"Administrator with ID {id} not found.");
                return NotFound();
            }

            var administratorViewModel = _mapper.Map<AdministratorViewModel>(administrator);
            return Ok(administratorViewModel);
        }

        // POST: api/administrators
        [HttpPost]
        public async Task<ActionResult<Administrator>> CreateAdministrator([FromBody] AdministratorCreateModel adminCreateModel)
        {
            if (adminCreateModel == null)
            {
                return BadRequest("Administrator data cannot be null.");
            }

            var administrator = _mapper.Map<Administrator>(adminCreateModel);
            var newAdministrator = await Task.Run(() => _administratorRepository.Add(administrator));

            return CreatedAtRoute("GetAdministratorById", new { id = newAdministrator.Id }, newAdministrator);
        }

        // PUT: api/administrators/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAdministrator(Guid id, [FromBody] AdministratorUpdateModel adminUpdateModel)
        {
            var existingAdministrator = await Task.Run(() => _administratorRepository.GeT(id));
            if (existingAdministrator == null)
            {
                _logger.LogError($"Administrator with ID {id} not found.");
                return NotFound();
            }

            _mapper.Map(adminUpdateModel, existingAdministrator);
            await Task.Run(() => _administratorRepository.Update(existingAdministrator));

            return NoContent();
        }

        // PATCH: api/administrators/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdateAdministrator(Guid id, [FromBody] JsonPatchDocument<AdministratorUpdateModel> patchDocument)
        {
            var existingAdministrator = await Task.Run(() => _administratorRepository.GeT(id));
            if (existingAdministrator == null)
            {
                _logger.LogError($"Administrator with ID {id} not found.");
                return NotFound();
            }

            var adminToPatch = _mapper.Map<AdministratorUpdateModel>(existingAdministrator);
            patchDocument.ApplyTo(adminToPatch, ModelState);

            if (!TryValidateModel(adminToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(adminToPatch, existingAdministrator);
            await Task.Run(() => _administratorRepository.Update(existingAdministrator));

            return NoContent();
        }

        // DELETE: api/administrators/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAdministrator(Guid id)
        {
            var administrator = await Task.Run(() => _administratorRepository.GeT(id));
            if (administrator == null)
            {
                _logger.LogError($"Administrator with ID {id} not found.");
                return NotFound();
            }

            await Task.Run(() => _administratorRepository.Delete(administrator));
            return NoContent();
        }
    }
}
