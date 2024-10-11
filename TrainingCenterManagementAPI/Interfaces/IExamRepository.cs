
using Microsoft.AspNetCore.JsonPatch;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.Models.Basic_HS;
using TrainingCenterManagementAPI.Models.View_HS;

namespace TrainingCenterManagementAPI.Interfaces
{
    public interface IExamRepository : IRepository<Exam>
    {
        /*Task<Course> CreateCourseAsync(BasicCourse basicCourse);7
        Task<Course> GetForNameAsync(string? name);1
        void AddTrainerAsync(Guid? courseId, Guid? trainerId);1
        //void AddLectuerAsync(Guid? courseId, Guid? lectuerId);//move to lecture
        void AddTraineeAsync(Guid? courseId, Guid? traineeId);1
        Task<VeiwCourse> GetVeiwCourse(Guid? courseId);
        Task<List<VeiwCourse>> GetVeiwCourses();
        Task<Course> UpdateCourseAsync(Guid? courseId, VeiwCourse Course);
        Task<Course> PartiallyUpdateCourseAsync(Guid? courseId, JsonPatchDocument<VeiwCourse> Course);
        Task<bool> DeletAsync(Guid? courseId);*/

        Task<VeiwExam> PartiallyUpdateExamAsync(Guid? examId, JsonPatchDocument<VeiwExam> veiwExam);
        Task<Exam> UpdateExamAsync(Guid? examId, VeiwExam Exam);
        Task<bool> DeleteAsync(Guid examId);

    }
}
