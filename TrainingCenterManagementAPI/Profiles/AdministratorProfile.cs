using AutoMapper;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.ViewModels;

namespace TrainingCenterManagementAPI.Profiles
{
    public class AdministratorProfile : Profile
    {
        public AdministratorProfile()
        {
            // Map from Administrator to AdministratorViewModel
            CreateMap<Administrator, AdministratorViewModel>();

            // Map from AdministratorCreateModel to Administrator
            CreateMap<AdministratorCreateModel, Administrator>();

            // Map from AdministratorUpdateModel to Administrator
            CreateMap<AdministratorUpdateModel, Administrator>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // To ensure partial updates work correctly
        }
    }
}
