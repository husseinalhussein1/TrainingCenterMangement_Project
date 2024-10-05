using AutoMapper;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.ViewModels;

namespace TrainingCenterManagementAPI.Profiles
{
    public class CertificateProfile : Profile
    {
        public CertificateProfile()
        {
            // Mapping from Certificate to CertificateViewModel
            CreateMap<Certificate, CertificateViewModel>();

            // Mapping from CertificateCreateModel to Certificate
            CreateMap<CertificateCreateModel, Certificate>();

            // Mapping from CertificateUpdateModel to Certificate
            CreateMap<CertificateUpdateModel, Certificate>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Ignore null values
        }
    }
}
