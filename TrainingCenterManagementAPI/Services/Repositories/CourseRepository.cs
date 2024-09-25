


using TrainingCenterManagement.Domain;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;


namespace TrainingCenterManagementAPI.Services.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {


        public CourseRepository(TrainingCenterManagementDbContext context) : base(context)
        {
        }

        // توابع اضافية غير الاساسية
    }
}
