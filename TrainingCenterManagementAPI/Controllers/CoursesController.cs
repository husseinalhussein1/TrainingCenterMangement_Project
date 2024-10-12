using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.Interfaces;
using TrainingCenterManagementAPI.VeiwModels;

namespace TrainingCenterManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ICourseRepository courseRepository;
        private readonly IExamRepository examRepository;
        private readonly ILectureRepository lectureRepository;
        private readonly IPaymentRepository paymentRepository;
        private readonly ITraineeRepository traineeRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CoursesController> logger;

        public CoursesController(
            IConfiguration configuration,
            ICourseRepository courseRepository,
            IExamRepository examRepository,
            ILectureRepository lectureRepository,
            IPaymentRepository paymentRepository,
            ITraineeRepository traineeRepository,
            IMapper mapper,
            ILogger<CoursesController> logger)
        {
            this.configuration = configuration;
            this.courseRepository = courseRepository;
            this.examRepository = examRepository;
            this.lectureRepository = lectureRepository;
            this.paymentRepository = paymentRepository;
            this.traineeRepository = traineeRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        
        /////////////////////////////////////////////////////////////
        /// 
        ///  لازم ما انسى اعمل :::::
        /// 
        ///  لا تنسى الغيت تبع الكورس و اللوكتشر تخليها ترجع قيم لو عاطيها جزء من الاسم 
        ///  مشان نستخدمها بالبحث
        ///  و موقع حفظ الملفات لازم تخليه يكون بالاب ستنغ كراي مو اكثر 
        ///  و تشوف الابديت تبع اللوكتشر بتشتغل هيك ؟؟؟ ولا لاء
        ///  و تشيل التعليقات علو الحماية
        ///  و طلاس البيزك تبع اللوكتشر و الفيو تزبطهن لان ما عجبني 
        /// 
        /// 
        ///  لالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالالا تنسى تتست اهم شي
        /// 

        
        // GET: api/Courses
        [HttpGet(Name = "GetCourses")]  //chick
        public async Task<ActionResult<List<Course>>> GetCourses()
        {
            return Ok(courseRepository.All(co => co.Trainees,
                                           co => co.Payments,
                                           co => co.Presences,
                                           co => co.Exam,
                                           co => co.TrainingOfficer,
                                           co => co.Trainers,
                                           co => co.Lectures));
        }

        // GET: api/Courses/veiw
        [HttpGet("veiw", Name = "GetVeiwCourses")]   //chick
        public async Task<ActionResult<List<VeiwCourse>>> GetVeiwCourses()
        {
            return Ok(courseRepository.GetVeiwCourses().Result);
        }

        // GET: api/Courses/veiw/sorting/date
        [HttpGet("veiw/sorting/date", Name = "GetVeiwCoursesWhithSortingByDate")]   //chick
        public async Task<ActionResult<List<VeiwCourse>>> GetVeiwCoursesWhithSortingByDate()
        {
            return Ok(courseRepository.GetVeiwCourses().Result
                         .OrderByDescending(course => course.ReleaseDate)   //تم وضعها عكسية لان التاريخ بالمقلوب الاحدث اكبر من الاقدم 
                         .ToList()); 
        }

        // GET: api/Courses/veiw/sorting/name
        [HttpGet("veiw/sorting/name", Name = "GetVeiwCoursesWhithSortingByName")]   //chick
        public async Task<ActionResult<List<VeiwCourse>>> GetVeiwCoursesWhithSortingByName()
        {
            return Ok(courseRepository.GetVeiwCourses().Result
                         .OrderBy(course => course.CourseName)
                         .ToList());
        }

        // GET: api/Courses/{id}
        [HttpGet("{id}", Name = "GetCourseById")]   //chick
        public async Task<ActionResult<Course>> GetCourseById(Guid id)
        {
            var course = courseRepository.GeT( id , co => co.Trainees,
                                                    co => co.Payments,
                                                    co => co.Presences,
                                                    co => co.Exam,
                                                    co => co.TrainingOfficer,
                                                    co => co.Trainers,
                                                    co => co.Lectures);
            if (course == null) NotFound();
            return Ok(course);
        }

        // GET: api/Courses/veiw/{id}
        [HttpGet("veiw/{id}", Name = "GetVeiwCourseById")]     //chick
        public async Task<ActionResult<VeiwCourse>>? GetVeiwCourseById(Guid id)
        {
            var course = courseRepository.GetVeiwCourse(id);
            if (course.Result is null) NotFound();
            return Ok(course.Result);
        }

        // GET: api/Courses/basic/{name}
        [HttpGet("basic/{name}", Name = "GetBasicCourseByName")]   //chick
        public async Task<ActionResult<BasicCourse>> GetBasicCourseByName(string? name)
        {
            var course = courseRepository.GetForNameAsync(name);
            if (course.Result == null) NotFound();
            courseRepository.SaveChanges();

            return Ok(course.Result);
        }

        // POST: api/Courses
        [HttpPost(Name = "PostCourse")]   //chick
        //[Authorize]
        public async Task<ActionResult<Course>> PostCourse(CourseForCreate courseForCreate)
        {
            var basicCourse = new BasicCourse()
            {
                BatchNumber=courseForCreate.BatchNumber,
                CourseName=courseForCreate.CourseName,
                NumberOfLectures=courseForCreate.NumberOfLectures,
                Price = courseForCreate.Price,
                TrainingOfficerId=courseForCreate.TrainingOfficerId,
                Description=courseForCreate.Description,
                ReleaseDate=courseForCreate.ReleaseDate,
            };

            //creating name files
            var fileName = $"{courseForCreate.CourseName.Replace(" ","")}_" +
                           $"{courseForCreate.BatchNumber}_Course".ToLower();

            //saveing img 
            var imgName = CreateFile(courseForCreate.Img,
                                     "StaticFiles/Courses/CoursesThumbnails",
                                     fileName).Result.ToString();
            basicCourse.ThumbnailUrl = imgName;


            //saveing vedio
            var videoName = CreateFile(courseForCreate.Video,
                                     "StaticFiles/Courses/CoursesVideos",
                                     fileName).Result.ToString();
            basicCourse.VedioUrl = videoName;


            var course = courseRepository.CreateCourseAsync(basicCourse);
            if (course.Result is null) NotFound();
            courseRepository.SaveChanges();

            return CreatedAtAction("GetCourseById", new { id = course.Result.CourseId }, course.Result);
        }

        // PUT: api/Courses/{id}
        [HttpPut("{id}", Name = "PutCourse")]   //chick
        //[Authorize]
        public async Task<IActionResult> PutCourse(Guid id, VeiwCourse viwCourse)
        {
            var course = courseRepository.UpdateCourseAsync(id, viwCourse);
            if (course.Result is null) NotFound();
            courseRepository.SaveChanges();

            return NoContent();
        }

        // PATCH: api/Courses/{id}
        [HttpPatch("{id}", Name = "PartiallyUpdateCourse")]   //chick
        //[Authorize]
        public async Task<ActionResult<Course>> PartiallyUpdateCourse(Guid id, JsonPatchDocument<VeiwCourse> veiwCourse)
        {
            var course = courseRepository.GetVeiwCourse(id);

            if (course.Result == null)
                return NotFound();

            veiwCourse.ApplyTo(course.Result, ModelState);
            if (!ModelState.IsValid)
                return BadRequest();
            courseRepository.UpdateCourseAsync(id, course.Result);
            courseRepository.SaveChanges();
            return NoContent();
        }

        // DELETE: api/Courses/{id}
        [HttpDelete("{id}", Name = "DeleteCourse")]    //chick
        //[Authorize]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var course = courseRepository.DeleteAsync(id);
            if (!course.Result) NotFound();
            courseRepository.SaveChanges();

            return NoContent();
        }


        ///////////////////////
        ///
        ///   EXAM
        ///

        // GET: api/Course/{id}/exam
        [HttpGet("{id}/exam", Name = "GetExamByCourseId")]  //chick
        public async Task<ActionResult<VeiwExam>> GetExamByCourseId(Guid id)
        {
            var exam = courseRepository.GetExamAsync(id);
            if (exam.Result == null) NotFound();
            return Ok(exam.Result);
        }

        // POST: api/Course/{id}/exam
        [HttpPost("{id}/exam", Name = "CreateExamByCourseId")]  //chick
        //[Authorize]
        public async Task<ActionResult<Course>> PostExam(Guid id, VeiwExam veiwExam)
        {
            var exam = courseRepository.CreateExamAsync(id, veiwExam);
            if (exam.Result is null) NotFound();

            examRepository.Add(exam.Result);
            courseRepository.SaveChanges();
            return CreatedAtAction("GetExamByCourseId", new { id = exam.Result.CourseId }, exam.Result);
        }

        // PUT: api/Courses/{id}/exam
        [HttpPut("{id}/exam", Name = "UpdateExamByCourseId")]   //chick
        //[Authorize]
        public async Task<IActionResult> PutExamByCourseId(Guid id, VeiwExam veiwExam)
        {
            var exam = courseRepository.UpdateExamByCourseIdAsync(id, veiwExam);
            if (exam.Result is null) NotFound();

            examRepository.Update(exam.Result);
            examRepository.SaveChanges();

            return NoContent();
        }

        // PATCH: api/Courses/{id}/exam
        [HttpPatch("{id}/exam", Name = "PartiallyUpdateExamByCourseId")]   //chick
        //[Authorize]
        public async Task<ActionResult<BasicExam>> PartiallyUpdateExamByCourseImdAsync(Guid? id, JsonPatchDocument<VeiwExam> veiwExam)
        {
            var exam = courseRepository.GetExamAsync(id);

            if (exam.Result == null)
                return NotFound();

            veiwExam.ApplyTo(exam.Result, ModelState);
            if (!ModelState.IsValid)
                return BadRequest();
            var newExm=courseRepository.UpdateExamByCourseIdAsync(id, exam.Result);

            examRepository.Update(newExm.Result);
            examRepository.SaveChanges();

            return NoContent();
        }


        ///////////////////////
        ///
        ///   LECETUERS
        ///

        // GET: api/Course/{id}/lecetuers
        [HttpGet("{id}/lecetuers", Name = "AllLecetuersByCourseId")]   //chick
        public async Task<ActionResult<List<BasicLecture>>> GetLecetuers(Guid id)
        {
            var lecetuers = courseRepository.GetLecturesAsync(id);
            if (lecetuers.Result == null) NotFound();
            return Ok(lecetuers);
        }

        // GET: api/Course/{id}/lecetuers/{name}
        [HttpGet("{id}/lectures/{name}", Name = "GetLectureByCourseIdAndName")]  //chick
        public async Task<IActionResult> GetLectureByCourseIdAndName(Guid id, string name)
        {
            var lecetuer = courseRepository.GetLectureByNameAsync(id, name);
            if (lecetuer.Result == null) NotFound();
            return Ok(lecetuer.Result);
        }

        // POST: api/Course/{id}/exam
        [HttpPost("{id}/lecetuers", Name = "CreateLecetuerByCourseId")]   //chick
        //[Authorize]
        public async Task<ActionResult<BasicLecture>> Postlecetuer(Guid id, LectuerForCreate lectuerForCreate)
        {
            var veiwLecture = new VeiwLecture()
            {
                Titel = lectuerForCreate.Titel,
                Description = lectuerForCreate.Description,
                LectureDate = lectuerForCreate.LectureDate
            };

            var course = courseRepository.GeT(id, e => e.Lectures);

            //creating name files
            var fileName = $"{lectuerForCreate.Titel.Replace(" ", "")}_lecetuer_" +
                           $"{course.CourseName.Replace(" ", "")}_" +
                           $"{course.BatchNumber}".ToLower();

            //saveing img 
            var imgName = CreateFile(lectuerForCreate.Img,
                                     "StaticFiles/Courses/CoursesThumbnails",
                                     fileName).Result;
            veiwLecture.ThumbnailUrl = imgName;


            //saveing vedio
            var videoName = CreateFile(lectuerForCreate.Video,
                                     "StaticFiles/Courses/CoursesVideos",
                                     fileName).Result.ToString();
            veiwLecture.VedioUrl = videoName;


            var lecetuer = courseRepository.CreateLectureAsync(id, veiwLecture);
            if (lecetuer.Result is null) NotFound();
            lectureRepository.Add(lecetuer.Result);
            lectureRepository.SaveChanges();

            var lecetuerForReturn= mapper.Map<BasicLecture>(lecetuer.Result);

            return CreatedAtAction("GetLectureByCourseIdAndName", 
                                    new { id = lecetuer.Result.CourseId, name = lecetuerForReturn.Titel },
                                    lecetuerForReturn);
        }

        

        ///////////////////////
        ///
        ///   PAYMENT
        ///


        // GET: api/Course/{id}/trainees/{traineeId}/payments
        [HttpGet("{id}/trainees/{traineeId}/payments", Name = "GetAllPaymentsForTraineeAndCourse")]   //chick
        public async Task<ActionResult<List<Payment>>> GetAllPaymentsForTraineeAndCourse(Guid id, Guid traineeId)
        {
            var payments = paymentRepository.GeT(id, traineeId);
            if (payments.Result == null) NotFound();
            return Ok(payments.Result);
        }


        // GET: api/Course/{id}/trainees/{traineeId}/totalAmount
        [HttpGet("{id}/trainees/{traineeId}/totalAmount", Name = "GetTotalAmountForAllPaymentsForTraineeAndCourse")]    //chick
        public async Task<ActionResult<Decimal>> GetTotalAmountForAllPaymentsForTraineeAndCourse(Guid id, Guid tId)
        {
            var payments = paymentRepository.GetTotalAmount(id, tId);

            return Ok(payments.Result);
        }


        // POST: api/Course/{id}/trainees/{traineeId}/payments
        [HttpPost("{id}/trainees/{traineeId}/payments", Name = "AddPaymentForTraineeAndCourse")]   //chick
        //[Authorize]
        public async Task<ActionResult<Course>> PostPayment(Guid id, Guid traineeId, VeiwPayment veiwPayment)
        {
            var payment = paymentRepository.CreateAsync(id,traineeId,veiwPayment);
            if (payment.Result is null) NotFound();
            courseRepository.GeT(id,
                                e=>e.Payments,
                                e=>e.Trainees).Trainees.FirstOrDefault(e=>e.Id==traineeId)
                                                       .Payments.Add(payment.Result);

            traineeRepository.GeT(traineeId,
                                  e => e.Payments).Payments.Add(payment.Result);

            paymentRepository.SaveChanges();

            return CreatedAtAction("GetAllPaymentsForTraineeAndCourse",
                                    new { id = payment.Result.CourseId, traineeId= payment.Result.TraineeId },
                                    payment.Result);
        }



        ///////////////////////////////////////////////////////////////////////////////     //chick
        ///
        ///          static files
        ///          

        // GET: api/Courses/{name}/video
        [HttpGet("{videoCoursename}/video", Name = "videoCourse")]
        public async Task<ActionResult> GetVideoCourses(string videoCoursename)
        {

            return await GetFile($"StaticFiles/Courses/CoursesVideos{videoCoursename}");
        }

        // GET: api/Courses/img
        [HttpGet("{imgCoursename}/img", Name = "imgCourse")]
        public async Task<ActionResult> GetImgCourses(string imgCoursename)
        {
            return await GetFile($"StaticFiles/Courses/CoursesThumbnails{imgCoursename}");
        }
        
        // GET: api/Courses/{name}/video
        [HttpGet("{id}/leceture/{videoCoursename}/video", Name = "videoLecetuer")]
        public async Task<ActionResult> GetVideoLeceture(string videoCoursename)
        {
            return await GetFile($"StaticFiles/Lectures/LectureVideos{videoCoursename}");
        }

        // GET: api/Courses/img
        [HttpGet("{id}/leceture/{imgCoursename}/img", Name = "imgLecetuer")]
        public async Task<ActionResult> GetImgLeceture(string imgCoursename)
        {
            return await GetFile($"StaticFiles/Lectures/LectureThumbnails{imgCoursename}");
        }

        // PUT: api/Courses/{id}/video
        [HttpPut("{id}/video")]
        //[Authorize]
        public async Task<IActionResult> UpdateVideoCourses(Guid id, IFormFile file)
        {
            var course = courseRepository.GeT(id);

            string videoName = Path.GetFileNameWithoutExtension(course.VedioUrl);
            var result = await CreateFile(file, "StaticFiles/Courses/CoursesVideos", videoName + "(new)");

            course.VedioUrl = result;
            courseRepository.Update(course);
            courseRepository.SaveChanges();

            return Ok(result);
        }

        // PUT: api/Courses/{id}/img
        [HttpPut("{id}/img")]
        //[Authorize]
        public async Task<IActionResult> UpdateimgCourses(Guid id, IFormFile file)
        {
            var course = courseRepository.GeT(id);

            string imgName = Path.GetFileNameWithoutExtension(course.ThumbnailUrl);
            var result = await CreateFile(file, "StaticFiles/Courses/CoursesThumbnails", imgName + "(new)");

            course.ThumbnailUrl = result;
            courseRepository.Update(course);
            courseRepository.SaveChanges();

            return Ok(result);
        }

        // PUT: api/Courses/video/{id}
        [HttpPut("{id}/leceture/{titel}/video")]
        //[Authorize]
        public async Task<IActionResult> UpdateVideoLeceture(Guid id,string titel, IFormFile file)
        {
            var course = courseRepository.GeT(id, e => e.Lectures);
            var leceture = course.Lectures.FirstOrDefault(l=>l.Titel==titel);

            string videoName = Path.GetFileNameWithoutExtension(leceture.VedioUrl);
            var result = await CreateFile(file, "StaticFiles/Lectures/LectureVideos", videoName + "(new)");

            leceture.VedioUrl = result;
            lectureRepository.Update(leceture);
            lectureRepository.SaveChanges();

            return Ok(result);
        }

        // PUT: api/Courses/img/{id}
        [HttpPut("{id}/leceture/{titel}/img")]
        //[Authorize]
        public async Task<IActionResult> UpdateimgLeceture(Guid id , string titel , IFormFile file)
        {
            var course = courseRepository.GeT(id, e => e.Lectures);
            var leceture = course.Lectures.FirstOrDefault(l => l.Titel == titel);

            string imgName = Path.GetFileNameWithoutExtension(leceture.ThumbnailUrl);
            var result = await CreateFile(file, "StaticFiles/Lectures/LectureThumbnails", imgName + "(new)");

            leceture.ThumbnailUrl = result  ;
            lectureRepository.Update(leceture);
            lectureRepository.SaveChanges();

            return Ok(result);
        }




        ////////////////////////////////////////////////////
        ///
        ///           privete metods 
        /// 

        private async Task<ActionResult> GetFile(string filePath)
        {
            string baseDirectory = Directory.GetCurrentDirectory();
            string folderPath = Path.Combine(baseDirectory, filePath.ToLower()); 
            if (!System.IO.File.Exists(folderPath))
            {
                return NotFound();
            }

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath.ToLower(), out var contenttype))
            {
                contenttype = "application/octet-stream";
            }

            var mytextfile = System.IO.File.ReadAllBytes(folderPath);
            return File(mytextfile, contenttype, Path.GetFileName(folderPath));
        }



        private async Task<string> CreateFile(IFormFile file,string filePath,string fileNameForSave)
        {
            string filename = "";
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = (fileNameForSave + extension).ToLower();

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), filePath , filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                
            }
            catch (Exception ex)
            {
                logger.LogError(ex+"");
                return null;
            }
            return filename;
        }

    }
}
