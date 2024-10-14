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
    public class LecturesController : ControllerBase
    {
        private readonly ILectureRepository lectureRepository;
        private readonly IPresenceRepository presenceRepository;
        private readonly ITraineeRepository traineeRepository;

        public LecturesController(ILectureRepository lectureRepository,
                                  IPresenceRepository presenceRepository,
                                  ITraineeRepository traineeRepository)
        {
            this.lectureRepository = lectureRepository;
            this.presenceRepository = presenceRepository;
            this.traineeRepository = traineeRepository;
        }


        [Authorize(Roles = "TrainingOfficer")]
        // GET: api/Lectures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lecture>>> GetLectures()
        {
            return Ok(lectureRepository.All());
        }



        [Authorize(Roles = "TrainingOfficer,Trainee,Trainer")]
        // GET: api/Lectures/{id}
        [HttpGet("{id}",Name = "GetLectureById")]
        public async Task<ActionResult<Lecture>> GetLecture(Guid id)
        {
            var lecture = lectureRepository.GeT(id);

            if (lecture == null)
            {
                return NotFound();
            }

            return lecture;
        }


        [Authorize(Roles = "TrainingOfficer,Trainer")]
        // PUT: api/Lectures/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLecture(Guid id, VeiwLectureWithoutUrls veiwLecture)
        {
            var lecture = lectureRepository.UpdateLectureAsync(id, veiwLecture);
            if (lecture.Result is null) NotFound();

            return NoContent();
        }


        [Authorize(Roles = "TrainingOfficer,Trainer")]
        // PATCH: api/Lectur")]
        [HttpPatch("{id}",Name = "PartiallyLectureUpdate")]
        //[Authorize]
        public async Task<ActionResult<Lecture>> PartiallyLectureUpdate(Guid id, JsonPatchDocument<VeiwLectureWithoutUrls> veiwLecture)
        {
            var lecture = lectureRepository.GetLectureByIdAsync(id);

            if (lecture.Result == null)
                return NotFound();

            veiwLecture.ApplyTo(lecture.Result, ModelState);
            if (!ModelState.IsValid)
                return BadRequest();
            lectureRepository.UpdateLectureAsync(id, lecture.Result);
            return NoContent();
        }




        [Authorize(Roles = "TrainingOfficer,Trainer")]
        // DELETE: api/Lectures/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLecture(Guid id)
        {
            var lecture = lectureRepository.DeleteAsync(id);
            if (!lecture.Result)
            {
                return NotFound();
            }

            return NoContent();
        }



        ///////////////////////
        ///    Presence
        /// 



        [Authorize(Roles = "TrainingOfficer")]
        [HttpPost("{id}/trainee/{traineeId}/presence", Name = "CreatePresenceByLucetureIdAndTraineeId")]
        public async Task<ActionResult<Presence>> CreatePresenceByLucetureIdAndTraineeId(Guid id, Guid traineeId)
        {
            var presence = presenceRepository.AddPresenceAsync(id, traineeId);

            if (presence.Result == null)
            {
                return BadRequest();
            }


            var lecture = lectureRepository.GeT(id, e => e.Presences);
            lecture.Presences.Add(presence.Result);
            lectureRepository.Update(lecture);
            

            var trainee = traineeRepository.GeT(traineeId, e => e.Presences);
            trainee.Presences.Add(presence.Result);
            traineeRepository.Update(trainee);
            traineeRepository.SaveChanges();

            return CreatedAtAction("GetPresenceByLucetureIdAndTraineeId",
                                    new { id = id, traineeId = traineeId },
                                    presence.Result);
        }



        // alter last
        //[HttpGet("{id}/trainee/{traineeId}/presence", Name = "GetPresenceByLucetureIdAndTraineeId")]
        //public async Task<ActionResult<Course>> GetPresenceByLucetureIdAndTraineeId(Guid id, Guid traineeId)
        //{
        //    var exam = presenceRepository.GetPresenceAsync(id, traineeId);
        //    return Ok(exam.Result);
        //}


    }
}
