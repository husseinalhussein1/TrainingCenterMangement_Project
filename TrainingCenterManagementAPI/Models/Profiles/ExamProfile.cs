using AutoMapper;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.Models.Basic_HS;
using TrainingCenterManagementAPI.Models.View_HS;

namespace TrainingCenterManagementAPI.Models.Profiles
{
    public class ExamProfile : Profile
    {
        public ExamProfile()
        {
            CreateMap<Exam, VeiwExam>();
            CreateMap<VeiwExam, Exam>();

            CreateMap<Exam, BasicExam>();
            CreateMap<BasicExam, Exam>();
        }

    }
}
