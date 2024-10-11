using AutoMapper;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.ViewModels;

namespace TrainingCenterManagementAPI.Profiles
{
    public class ReceptionistProfile : Profile
    {
        public ReceptionistProfile()
        {
            // Mapping from Receptionist to ReceptionistViewModel
            CreateMap<Receptionist, ReceptionistViewModel>();

            // Mapping from ReceptionistCreateModel to Receptionist
            CreateMap<ReceptionistCreateModel, Receptionist>();

            // Mapping from ReceptionistUpdateModel to Receptionist
            CreateMap<ReceptionistUpdateModel, Receptionist>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Ignore null values
        }
    }
}
