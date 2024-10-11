
using Microsoft.AspNetCore.JsonPatch;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.Models.Basic_HS;
using TrainingCenterManagementAPI.Models.View_HS;

namespace TrainingCenterManagementAPI.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<Course> CreateCourseAsync(BasicCourse basicCourse);  //ok
        Task<BasicCourse> GetForNameAsync(string? name);  //ok
        void AddTrainerAsync(Guid? courseId, Guid? trainerId);  ///////// wait
        void AddTraineeAsync(Guid? courseId, Guid? traineeId);  ///////// wait
        Task<VeiwCourse> GetVeiwCourse(Guid courseId);  //ok
        Task<List<VeiwCourse>> GetVeiwCourses();  //ok
        Task<Course> UpdateCourseAsync(Guid? courseId, VeiwCourse Course);   //ok
        Task<VeiwCourse> PartiallyUpdateCourseAsync(Guid? courseId, JsonPatchDocument<VeiwCourse> Course);  //ok
        Task<bool> DeleteAsync(Guid courseId);  //ok

        //Exame 
        Task<VeiwExam> GetExamAsync(Guid? courseId);  //ok
        Task<Exam> CreateExamAsync(Guid courseId, VeiwExam veiwExam);  //ok
        Task<BasicExam> PartiallyUpdateExamByCourseIdAsync(Guid? courseId, JsonPatchDocument<VeiwExam> Exam);   //ok
        Task<Exam> UpdateExamByCourseIdAsync(Guid? courseId, VeiwExam Exam);  //ok

        //Lecture
        Task<List<BasicLecture>> GetLecturesAsync(Guid? courseId);  //ok
        Task<BasicLecture> GetLectureByNameAsync(Guid? courseId, string titel);  //ok
        Task<Lecture> CreateLectureAsync(Guid courseId, VeiwLecture basicLecture);  //ok




    }


}
