


using TrainingCenterManagement.Domain;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;
using TrainingCenterManagementAPI.Models.View_HS;


namespace TrainingCenterManagementAPI.Services.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {


        public PaymentRepository(TrainingCenterManagementDbContext context) : base(context)
        {
        }

        public async Task<Payment> CreateAsync(Guid courseId,Guid traineeId,VeiwPayment veiwPayment)
        {
            if (veiwPayment == null || courseId == null || traineeId == null) return null;
            var newPayment = new Payment()
            {
                CourseId = courseId,
                TotalAmount = veiwPayment.TotalAmount,
                TraineeId = traineeId
            };

            Add(newPayment);

            return newPayment;
        }

        public async Task<List<Payment>> GeT(Guid courseId, Guid traineeId)
        {
            var payments= All() 
                          .Where(p => p.CourseId == courseId && p.TraineeId == traineeId)
                          .ToList();

            return payments;  
        }

        public async Task<decimal> GetTotalAmount(Guid courseId, Guid traineeId)
        {
            var totalAmount = GeT(courseId, traineeId)
                              .Result
                              .Sum(t => t.TotalAmount);

            return totalAmount;
        }

        public async Task<Payment> UpdateAsync(Guid paymentId, decimal totalAmount)
        {
            var payment = GeT(paymentId);
            if(payment == null) return null;

            payment.TotalAmount= totalAmount;

            Update(payment);
            SaveChanges();

            return payment;
        }

       

        // توابع اضافية غير الاساسية
    }
}
