using AutoMapper;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.ViewModels;

namespace TrainingCenterManagementAPI.Profiles
{
    public class TrainerProfile : Profile
    {
        public TrainerProfile()
        {
            // Mapping from Trainer to TrainerViewModel
            CreateMap<Trainer, TrainerViewModel>();

            // Mapping from TrainerCreateModel to Trainer
            CreateMap<TrainerCreateModel, Trainer>();

            // Mapping from TrainerUpdateModel to Trainer
            CreateMap<TrainerUpdateModel, Trainer>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Ignore null values
        }
    }
}
