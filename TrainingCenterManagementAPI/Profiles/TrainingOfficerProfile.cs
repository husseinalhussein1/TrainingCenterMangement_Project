using AutoMapper;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.ViewModels;

namespace TrainingCenterManagementAPI.Mappings
{
    public class TrainingOfficerProfile : Profile
    {
        public TrainingOfficerProfile()
        {
            // Mapping from TrainingOfficer to TrainingOfficerViewModel
            CreateMap<TrainingOfficer, TrainingOfficerViewModel>();

            // Mapping from TrainingOfficerCreateModel to TrainingOfficer
            CreateMap<TrainingOfficerCreateModel, TrainingOfficer>();

            // Mapping from TrainingOfficerUpdateModel to TrainingOfficer
            CreateMap<TrainingOfficerUpdateModel, TrainingOfficer>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Ignore null values
        }
    }
}
