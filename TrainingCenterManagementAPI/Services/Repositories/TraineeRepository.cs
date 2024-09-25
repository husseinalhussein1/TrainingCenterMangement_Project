


using TrainingCenterManagement.Domain;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;


namespace TrainingCenterManagementAPI.Services.Repositories
{
    public class TraineeRepository : GenericRepository<Trainee>, ITraineeRepository
    {


        public TraineeRepository(TrainingCenterManagementDbContext context) : base(context)
        {
        }


        // توابع اضافية غير الاساسية
    }
}
