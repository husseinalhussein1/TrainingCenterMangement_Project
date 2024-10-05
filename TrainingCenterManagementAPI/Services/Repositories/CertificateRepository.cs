using TrainingCenterManagement.Domain;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;

namespace TrainingCenterManagementAPI.Services.Repositories
{
    public class CertificateRepository : GenericRepository<Certificate>, ICertificateRepository
    {
        public CertificateRepository(TrainingCenterManagementDbContext context) : base(context)
        {
        }

        // يمكنك إضافة توابع إضافية هنا إذا لزم الأمر
    }
}
