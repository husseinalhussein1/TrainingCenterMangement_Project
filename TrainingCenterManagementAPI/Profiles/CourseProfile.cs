using AutoMapper;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.VeiwModels;

namespace TrainingCenterManagementAPI.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, VeiwCourse>();
            CreateMap<VeiwCourse, Course>();

            CreateMap<Course, BasicCourse>();
            CreateMap<BasicCourse, Course>();
        }
    }
}
