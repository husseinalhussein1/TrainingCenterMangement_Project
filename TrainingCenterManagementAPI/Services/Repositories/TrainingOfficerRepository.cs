


using TrainingCenterManagement.Domain;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;


namespace TrainingCenterManagementAPI.Services.Repositories
{
    public class TrainingOfficerRepository : GenericRepository<TrainingOfficer>, ITrainingOfficerRepository
    {


        public TrainingOfficerRepository(TrainingCenterManagementDbContext context) : base(context)
        {

        }


        // توابع اضافية غير الاساسية
    }
}
