using AutoMapper;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.Models.Basic_HS;
using TrainingCenterManagementAPI.Models.View_HS;

namespace TrainingCenterManagementAPI.Models.Profiles
{
    public class CourseProfile:Profile
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
