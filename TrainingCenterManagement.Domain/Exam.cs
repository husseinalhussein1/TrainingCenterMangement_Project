using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenterManagement.Domain
{
    public class Exam
    {
        public Guid ExamId { get; set; }
       
        public required string ExamName { get; set; }
        public DateTime ExamDate { get; set; }
        public bool IsDeleted { get; set; }

        // One-to-One: Exam -> Course
        public required Guid CourseId { get; set; }
        public Course Course { get; set; }

        public Exam()
        {
            ExamId = Guid.NewGuid();
            ExamDate=DateTime.UtcNow;
            IsDeleted =false;
        }
    }
}
