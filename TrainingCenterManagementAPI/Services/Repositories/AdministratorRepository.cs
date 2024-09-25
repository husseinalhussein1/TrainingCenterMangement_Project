


using TrainingCenterManagement.Domain;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;


namespace TrainingCenterManagementAPI.Services.Repositories
{
    public class AdministratorRepository : GenericRepository<Administrator>, IAdministratorRepository
    {
        public AdministratorRepository(TrainingCenterManagementDbContext context) : base(context)
        {

        }

        // توابع اضافية غير الاساسية
    }
}
