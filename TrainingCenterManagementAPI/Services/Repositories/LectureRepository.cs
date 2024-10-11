


using AutoMapper.Features;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using TrainingCenterManagement.Domain;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;
using TrainingCenterManagementAPI.Models.Basic_HS;
using TrainingCenterManagementAPI.Models.View_HS;


namespace TrainingCenterManagementAPI.Services.Repositories
{
    public class LectureRepository : GenericRepository<Lecture>, ILectureRepository
    {

        private readonly IMapper mapper;

        public LectureRepository(TrainingCenterManagementDbContext context, IMapper mapper) : base(context)
        {
            this.mapper = mapper;
        }

        public async Task<Lecture> UpdateLectureAsync( Guid lectureId, VeiwLectureWithoutUrls veiwLecture)
        {
            var lecture = GeT((Guid) lectureId);
            if (lecture == null)
                return null;

            lecture.LectureDate = veiwLecture.LectureDate;
            lecture.Titel = veiwLecture.Titel;
            lecture.Description = veiwLecture.Description;

            SaveChanges();
            return lecture;
        }

        public async Task<VeiwLecture> PartiallyUpdateLectureAsync(Guid lectureId, JsonPatchDocument<VeiwLecture> veiwLecture)
        {
            var lecture = GeT((Guid)lectureId);
            if (lecture == null) return null;
            var lectureToPatch = new VeiwLecture()
            {
                Titel = lecture.Titel,
                Description = lecture.Description,
                LectureDate = lecture.LectureDate
            };
            return lectureToPatch;
        }

      
        public Task<List<VeiwLecture>> GetLecturesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<VeiwLectureWithoutUrls> GetLectureByIdAsync(Guid lectureId)
        {
            var lecture = GeT((Guid)lectureId);

            var convert = mapper.Map<VeiwLectureWithoutUrls>(lecture);
            return convert;
        }

        public async Task<bool> DeleteAsync(Guid lectureId)
        {
            var lecture = GeT(lectureId);
            if (lecture == null)
                return false;
            lecture.IsDeleted = true;
            SaveChanges();
            return true;
        }

        // توابع اضافية غير الاساسية
    }
}
