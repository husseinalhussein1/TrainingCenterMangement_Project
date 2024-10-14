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
    public class CertificatesController : ControllerBase
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CertificatesController> _logger;
        private readonly int MaxPageSize = 10;

        public CertificatesController(
            ICertificateRepository certificateRepository,
            IMapper mapper,
            ILogger<CertificatesController> logger)
        {
            _certificateRepository = certificateRepository;
            _mapper = mapper;
            _logger = logger;
        }
        [AllowAnonymous]
        // GET: api/certificates
        [HttpGet(Name = "GetCertificates")]
        public async Task<ActionResult<List<CertificateViewModel>>> GetCertificates(int pageNumber = 1, int pageSize = 5, string? keyword = null)
        {
            if (pageSize > MaxPageSize)
            {
                _logger.LogError($"PageSize {pageSize} exceeds the maximum {MaxPageSize}");
                return BadRequest($"PageSize cannot exceed {MaxPageSize}");
            }

            var certificates = await Task.Run(() => _certificateRepository.All());
            var certificateViewModels = _mapper.Map<List<CertificateViewModel>>(certificates);

            var paginationData = new { PageNumber = pageNumber, PageSize = pageSize };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationData));

            return Ok(certificateViewModels);
        }


        [AllowAnonymous]
        // GET: api/certificates/{id}
        [HttpGet("{id}", Name = "GetCertificateById")]
        public async Task<ActionResult<CertificateViewModel>> GetCertificateById(Guid id)
        {
            var certificate = await Task.Run(() => _certificateRepository.GeT(id));
            if (certificate == null)
            {
                _logger.LogError($"Certificate with ID {id} not found.");
                return NotFound();
            }

            var certificateViewModel = _mapper.Map<CertificateViewModel>(certificate);
            return Ok(certificateViewModel);
        }




        [AllowAnonymous]
        // POST: api/certificates
        [HttpPost]
        public async Task<ActionResult<Certificate>> CreateCertificate([FromBody] CertificateCreateModel certificateCreateModel)
        {
            if (certificateCreateModel == null)
            {
                return BadRequest("Certificate data cannot be null.");
            }

            var certificate = _mapper.Map<Certificate>(certificateCreateModel);
            var newCertificate = await Task.Run(() => _certificateRepository.Add(certificate));

            return CreatedAtRoute("GetCertificateById", new { id = newCertificate.Id }, newCertificate);
        }



        [AllowAnonymous]
        // PUT: api/certificates/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCertificate(Guid id, [FromBody] CertificateUpdateModel certificateUpdateModel)
        {
            var existingCertificate = await Task.Run(() => _certificateRepository.GeT(id));
            if (existingCertificate == null)
            {
                _logger.LogError($"Certificate with ID {id} not found.");
                return NotFound();
            }

            _mapper.Map(certificateUpdateModel, existingCertificate);
            await Task.Run(() => _certificateRepository.Update(existingCertificate));

            return NoContent();
        }



        [AllowAnonymous]
        // PATCH: api/certificates/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdateCertificate(Guid id, [FromBody] JsonPatchDocument<CertificateUpdateModel> patchDocument)
        {
            var existingCertificate = await Task.Run(() => _certificateRepository.GeT(id));
            if (existingCertificate == null)
            {
                _logger.LogError($"Certificate with ID {id} not found.");
                return NotFound();
            }

            var certificateToPatch = _mapper.Map<CertificateUpdateModel>(existingCertificate);
            patchDocument.ApplyTo(certificateToPatch, ModelState);

            if (!TryValidateModel(certificateToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(certificateToPatch, existingCertificate);
            await Task.Run(() => _certificateRepository.Update(existingCertificate));

            return NoContent();
        }




        [AllowAnonymous]
        // DELETE: api/certificates/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCertificate(Guid id)
        {
            var certificate = await Task.Run(() => _certificateRepository.GeT(id));
            if (certificate == null)
            {
                _logger.LogError($"Certificate with ID {id} not found.");
                return NotFound();
            }

            await Task.Run(() => _certificateRepository.Delete(certificate));
            return NoContent();
        }
    }
}
