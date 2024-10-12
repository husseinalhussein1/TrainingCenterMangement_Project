using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingCenterManagement.Domain;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;
using TrainingCenterManagementAPI.VeiwModels;

namespace TrainingCenterManagementAPI.Services.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private readonly IMapper mapper;

        public CourseRepository(TrainingCenterManagementDbContext context, IMapper mapper) : base(context)
        {
            this.mapper = mapper;
        }

        public async void AddTraineeAsync(Guid? courseId, Guid? traineeId)
        {
            if (courseId == null || traineeId == null) { return; }
            var course =  GeT((Guid)courseId);
            if (course == null) return ;
            var trainee = await context.Trainees
                                       .Include(tr=>tr.Courses)
                                       .FirstOrDefaultAsync(tr=>tr.Id == traineeId);
            if (trainee == null) return;
            course.Trainees.Add(trainee);
            trainee.Courses.Add(course);
            SaveChanges();
        }

        public async void AddTrainerAsync(Guid? courseId, Guid? trainerId)
        {
            if (courseId == null || trainerId == null) { return; }
            var cor = GeT((Guid)courseId);
            if (cor == null) return;
            var tra = await context.Trainers
                                       .Include(tr => tr.Courses)
                                       .FirstOrDefaultAsync(tr => tr.Id == trainerId);
            if (tra == null) return;
            cor.Trainers.Add(tra);
            tra.Courses.Add(cor);
            SaveChanges();
        }

        public async Task<BasicCourse> GetForNameAsync(string? name)
        {
            if (name == null || name == string.Empty)
            {
                return null;
            }
            var Courses = All().FirstOrDefault(c => c.CourseName == name);
            var convert = mapper.Map<BasicCourse>(Courses);
            return convert;
        }

        public async Task<List<VeiwCourse>> GetVeiwCourses()
        {
            var Courses = All();
            var convert = mapper.Map<List<VeiwCourse>>(Courses);
            return convert;
        }

        public async Task<VeiwCourse> GetVeiwCourse(Guid courseId)
        {
            var course = GeT( courseId);
            if (course == null) return null;

            var convert = mapper.Map<VeiwCourse>(course);
            return convert;
        }

        public async Task<Course> CreateCourseAsync(BasicCourse basicCourse)
        {
            if (basicCourse == null) return null;
            var newCourse = new Course()
            {
                CourseName = basicCourse.CourseName,
                BatchNumber = basicCourse.BatchNumber,
                ReleaseDate = basicCourse.ReleaseDate,
                NumberOfLectures = basicCourse.NumberOfLectures,
                Price = basicCourse.Price,
                Description = basicCourse.Description,
                VedioUrl = basicCourse.VedioUrl,
                ThumbnailUrl = basicCourse.ThumbnailUrl,
                TrainingOfficerId = basicCourse.TrainingOfficerId,
            };

            Add(newCourse);
            SaveChanges();
            return newCourse;
        }

        public async Task<Course> UpdateCourseAsync(Guid? courseId, VeiwCourse Course)
        {
            var course = GeT((Guid)courseId);
            if (course == null) return null;

            course.CourseName = Course.CourseName;
            course.BatchNumber = Course.BatchNumber;
            course.ReleaseDate = Course.ReleaseDate;
            course.NumberOfLectures = Course.NumberOfLectures;
            course.Price = Course.Price;
            course.Description = Course.Description;
            /*course.VedioUrl = Course.VedioUrl;
            course.ThumbnailUrl = Course.ThumbnailUrl;*/

            SaveChanges();
            return course;
        }

        public async Task<VeiwCourse> PartiallyUpdateCourseAsync(Guid? courseId, JsonPatchDocument<VeiwCourse> Course)
        {
            var course = GeT((Guid)courseId);
            if (course == null) return null;
            var courseToPatch = new VeiwCourse
            {
                CourseName = course.CourseName,
                BatchNumber = course.BatchNumber,
                ReleaseDate = course.ReleaseDate,
                NumberOfLectures = course.NumberOfLectures,
                Price = course.Price,
                Description = course.Description,
                /*VedioUrl = course.VedioUrl,
                ThumbnailUrl = course.ThumbnailUrl*/
            };
            return courseToPatch;
        }

        public async Task<VeiwExam> GetExamAsync(Guid? courseId)
        {
            var newExam = GeT((Guid)courseId,e=>e.Exam).Exam;

            if (newExam == null) return null;
            var convert = mapper.Map<VeiwExam>(newExam);
            return convert;
        }

        public async Task<Exam> CreateExamAsync(Guid courseId, VeiwExam veiwExam)
        {
            var course = GeT((Guid)courseId,e=>e.Exam);
            if (course == null) return null;
            var exam=new Exam() 
            { 
                CourseId = courseId ,
                ExamName= veiwExam.ExamName,
                ExamDate= veiwExam.ExamDate,
                //Mark=veiwExam.Mark
            };
            course.Exam = exam;
            return exam;
        }

        ///moved to exam
        public async Task<BasicExam> PartiallyUpdateExamByCourseIdAsync(Guid? courseId, JsonPatchDocument<VeiwExam> Exam)
        {
            var exam = GeT((Guid)courseId).Exam;
            if (exam == null)
                return null;
            var examToPutch = new BasicExam
            {
                ExamName = exam.ExamName,
                ExamDate = exam.ExamDate,
                //Mark = exam.Mark
            };
            
            return examToPutch;
        }

        public async Task<Exam> UpdateExamByCourseIdAsync(Guid? courseId, VeiwExam Exam)
        {
            var exam = GeT((Guid)courseId, e => e.Exam).Exam;
            if (exam == null)
                return null;

            exam.ExamDate = Exam.ExamDate;
            exam.ExamName = Exam.ExamName;
            //exam.Mark = Exam.Mark;
            
            return exam;
        }

        public async Task<List<BasicLecture>> GetLecturesAsync(Guid? courseId)
        {
            var lectures = GeT((Guid)courseId, e => e.Lectures).Lectures;

            var convert = mapper.Map<List<BasicLecture>>(lectures);
            return convert;
        }

        public async Task<Lecture> CreateLectureAsync(Guid courseId, VeiwLecture basicLecture)
        {
            var course = GeT((Guid)courseId, e => e.Lectures);

            var newLecture = new Lecture()
            {
                CourseId = courseId,
                Titel = basicLecture.Titel,
                Description = basicLecture.Description,
                LectureDate = basicLecture.LectureDate,
                ThumbnailUrl = basicLecture.ThumbnailUrl,
                VedioUrl = basicLecture.VedioUrl

            };

            course.Lectures.Add(newLecture);
            return newLecture;

        }

        public async Task<Lecture> UpdateLectureAsync(Guid courseId,Guid lectureId, BasicLecture basicLecture)
        {
            var lecture = GeT((Guid)courseId, e => e.Lectures).Lectures.FirstOrDefault(le=>le.LectureId==lectureId);
            if (lecture == null)
                return null;

            lecture.LectureDate = basicLecture.LectureDate;
            lecture.Titel = basicLecture.Titel;
            lecture.Description = basicLecture.Description;
            lecture.ThumbnailUrl = basicLecture.ThumbnailUrl;
            lecture.VedioUrl = basicLecture.VedioUrl;

            SaveChanges();
            return lecture;
        }

        public async Task<Lecture> PartiallyUpdateLectureAsync(Guid courseId, Guid lectureId, JsonPatchDocument<BasicLecture> basicLecture)
        {
            /*var lecture = GeT((Guid)courseId).Lectures.FirstOrDefault(le => le.LectureId == lectureId);

            if (lecture == null)
                return null;
            var lectureToPutch = new BasicLecture
            {
                Titel = basicLecture,
                Description = basicLecture.Description,
                LectureDate = basicLecture.LectureDate,
                ThumbnailUrl = basicLecture.ThumbnailUrl,
                VedioUrl = basicLecture.VedioUrl
            };
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return lectureToPutch;*/

            return null;
        }

        public async Task<BasicLecture> GetLectureByNameAsync(Guid? courseId, string titel)
        {
            var lecture = GeT((Guid)courseId, e => e.Lectures).Lectures.FirstOrDefault(le=>le.Titel==titel);

            var convert = mapper.Map<BasicLecture>(lecture);
            return convert;
        }

        public async Task<bool> DeleteAsync(Guid courseId)
        {
            var course = GeT(courseId);
            if (course == null)
                return false;
            course.IsDeleted = true;
            SaveChanges();
            return true;
        }

       
    }
}
