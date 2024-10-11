using AutoMapper;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.Models.Basic_HS;
using TrainingCenterManagementAPI.Models.View_HS;

namespace TrainingCenterManagementAPI.Models.Profiles
{
    public class LectureProfile : Profile
    {
        public LectureProfile()
        {
            CreateMap<Lecture, VeiwLectureWithoutUrls>();
            CreateMap<VeiwLectureWithoutUrls, Lecture>();

            CreateMap<Lecture, VeiwLecture>();
            CreateMap<VeiwLecture, Lecture>();

            CreateMap<Lecture, BasicLecture>();
            CreateMap<BasicLecture, Lecture>();
        }
    }
}
