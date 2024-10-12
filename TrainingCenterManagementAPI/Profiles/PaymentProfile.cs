using AutoMapper;
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.VeiwModels;

namespace TrainingCenterManagementAPI.Profiles
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
