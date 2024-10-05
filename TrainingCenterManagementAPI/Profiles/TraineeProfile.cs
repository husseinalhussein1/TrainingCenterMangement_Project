using AutoMapper;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.ViewModels;

namespace TrainingCenterManagementAPI.Profiles
{
    public class TraineeProfile : Profile
    {
        public TraineeProfile()
        {
            // Mapping from Trainee to TraineeViewModel
            CreateMap<Trainee, TraineeViewModel>();

            // Mapping from TraineeCreateModel to Trainee
            CreateMap<TraineeCreateModel, Trainee>();

            // Mapping from TraineeUpdateModel to Trainee
            CreateMap<TraineeUpdateModel, Trainee>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Ignore null values
        }
    }
}
