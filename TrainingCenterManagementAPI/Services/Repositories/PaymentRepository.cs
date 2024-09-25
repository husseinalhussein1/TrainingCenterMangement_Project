


using TrainingCenterManagement.Domain;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;


namespace TrainingCenterManagementAPI.Services.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {


        public PaymentRepository(TrainingCenterManagementDbContext context) : base(context)
        {
        }


        // توابع اضافية غير الاساسية
    }
}
