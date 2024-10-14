using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using TrainingCenterManagement.Domain;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;
using TrainingCenterManagementAPI.VeiwModels;
using TrainingCenterManagementAPI.Services.Repositories;

namespace TrainingCenterManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IExamRepository examRepository;

        public ExamsController(IExamRepository examRepository)
        {
            this.examRepository = examRepository;
        }


        [Authorize(Roles = "TrainingOfficer")]
        // GET: api/Exams
        [HttpGet(Name = "GetExams")]
        public async Task<ActionResult<IEnumerable<Exam>>> GetExams()
        {
            return Ok(examRepository.All());
        }


        [Authorize(Roles = "TrainingOfficer")]
        // GET: api/Exams/5
        [HttpGet("{id}", Name = "GetExamById")]
        public async Task<ActionResult<Exam>> GetExamById(Guid id)
        {
            var exam = examRepository.GeT(id);

            if (exam == null)
            {
                return NotFound();
            }

            return exam;
        }


        [Authorize(Roles = "TrainingOfficer")]
        // PUT: api/Exams/5
        [HttpPut("{id}",Name = "PutExam")]
        //[Authorize]
        public async Task<IActionResult> PutExam(Guid id, VeiwExam veiwExam)
        {
            var exam = examRepository.UpdateExamAsync(id, veiwExam);
            if (exam.Result is null) NotFound();
            examRepository.SaveChanges();
            return NoContent();
        }



        [Authorize(Roles = "TrainingOfficer")]
        // PATCH: api/Exams/{id}
        [HttpPatch("{id}", Name = "PartiallyUpdateExam")]
        //[Authorize]
        public async Task<ActionResult<Exam>> PartiallyUpdateExam(Guid id, JsonPatchDocument<VeiwExam> veiwExam)
        {
            var exam = examRepository.PartiallyUpdateExamAsync(id,veiwExam);

            if (exam.Result == null)
                return NotFound();

            veiwExam.ApplyTo(exam.Result, ModelState);
            if (!ModelState.IsValid)
                return BadRequest();
            examRepository.UpdateExamAsync(id, exam.Result);
            examRepository.SaveChanges();
            return NoContent();
        }



        [Authorize(Roles = "TrainingOfficer")]
        // DELETE: api/Exams/5
        [HttpDelete("{id}",Name = "DeleteExam")]
        //[Authorize]
        public async Task<IActionResult> DeleteExam(Guid id)
        {
            var exam = examRepository.DeleteAsync(id);
            if (!exam.Result) NotFound();
            examRepository.SaveChanges();
            return NoContent();
        }


    }
}
