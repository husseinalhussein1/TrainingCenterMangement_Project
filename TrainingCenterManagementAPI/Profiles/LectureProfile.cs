using AutoMapper;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.VeiwModels;

namespace TrainingCenterManagementAPI.Profiles
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
