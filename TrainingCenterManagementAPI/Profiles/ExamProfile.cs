using AutoMapper;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.VeiwModels;

namespace TrainingCenterManagementAPI.Profiles
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
