
using Microsoft.AspNetCore.JsonPatch;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.VeiwModels;

namespace TrainingCenterManagementAPI.Interfaces
{
    public interface ILectureRepository : IRepository<Lecture>
    {
        Task<List<VeiwLecture>> GetLecturesAsync();
        Task<VeiwLectureWithoutUrls> GetLectureByIdAsync( Guid lectureId);
        Task<Lecture> UpdateLectureAsync( Guid lectureId, VeiwLectureWithoutUrls veiwLecture);
        Task<VeiwLecture> PartiallyUpdateLectureAsync( Guid lectureId, JsonPatchDocument<VeiwLecture> veiwLecture);
        Task<bool> DeleteAsync(Guid lectureId);

    }
}
