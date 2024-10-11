using Microsoft.AspNetCore.JsonPatch;
using TrainingCenterManagement.Domain;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;
using TrainingCenterManagementAPI.Models.Basic_HS;
using TrainingCenterManagementAPI.Models.View_HS;


namespace TrainingCenterManagementAPI.Services.Repositories
{
    public class ExamRepository : GenericRepository<Exam>, IExamRepository
    {


        public ExamRepository(TrainingCenterManagementDbContext context) : base(context)
        {
        }

        public async Task<VeiwExam> PartiallyUpdateExamAsync(Guid? examId, JsonPatchDocument<VeiwExam> veiwExam)
        {
            var exam = GeT((Guid)examId);
            if (exam == null)
                return null;
            var examToPutch = new BasicExam
            {
                ExamName = exam.ExamName,
                ExamDate = exam.ExamDate,
                //Mark = exam.Mark,
            };
            return examToPutch;
        }

        public async Task<Exam> UpdateExamAsync(Guid? examId, VeiwExam Exam)
        {
            var exam = GeT((Guid)examId);
            if (exam == null)
                return null;

            exam.ExamDate=Exam.ExamDate;
            exam.ExamName=Exam.ExamName;
            //exam.Mark = Exam.Mark;

            SaveChanges();
            return exam;
        }

        public async Task<bool> DeleteAsync(Guid examId)
        {
            var exam = GeT(examId);
            if (exam == null)
                return false;
            exam.IsDeleted = true;
            SaveChanges();
            return true;
        }






        // توابع اضافية غير الاساسية
    }
}
