using AutoMapper;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.Models.Basic_HS;
using TrainingCenterManagementAPI.Models.View_HS;

namespace TrainingCenterManagementAPI.Models.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, VeiwPayment>();
            CreateMap<VeiwPayment, Payment>();

            CreateMap<Payment, BasicPayment>();
            CreateMap<BasicPayment, Payment>();
        }
    
    }
}
