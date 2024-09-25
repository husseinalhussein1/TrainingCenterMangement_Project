


using TrainingCenterManagement.Domain;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;


namespace TrainingCenterManagementAPI.Services.Repositories
{
    public class PresenceRepository : GenericRepository<Presence>, IPresenceRepository
    {


        public PresenceRepository(TrainingCenterManagementDbContext context) : base(context)
        {
        }


        // توابع اضافية غير الاساسية
    }
}
