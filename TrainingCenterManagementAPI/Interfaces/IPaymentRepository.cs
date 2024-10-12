
using TrainingCenterManagement.Domain;
using TrainingCenterManagementAPI.VeiwModels;

namespace TrainingCenterManagementAPI.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<Payment> CreateAsync(Guid courseId, Guid traineeId, VeiwPayment veiwPayment);
        Task<List<Payment>> GeT(Guid courseId, Guid traineeId);//return All Payment from this trainee to this course
        Task<Decimal> GetTotalAmount(Guid courseId, Guid traineeId);//return total amount for All Payment from this trainee to this course
        Task<Payment> UpdateAsync(Guid paymentId, decimal totalAmount);


    }
}
