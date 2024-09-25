


using TrainingCenterManagement.Domain;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;


namespace TrainingCenterManagementAPI.Services.Repositories
{
    public class ExamRepository : GenericRepository<Exam>, IExamRepository
    {


        public ExamRepository(TrainingCenterManagementDbContext context) : base(context)
        {
        }


        // توابع اضافية غير الاساسية
    }
}
