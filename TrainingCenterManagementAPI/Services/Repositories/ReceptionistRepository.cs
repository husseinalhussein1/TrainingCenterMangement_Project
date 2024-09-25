


using TrainingCenterManagement.Domain;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;


namespace TrainingCenterManagementAPI.Services.Repositories
{
    public class ReceptionistRepository : GenericRepository<Receptionist>, IReceptionistRepository
    {


        public ReceptionistRepository(TrainingCenterManagementDbContext context) : base(context)
        {
        }


        // توابع اضافية غير الاساسية
    }
}
