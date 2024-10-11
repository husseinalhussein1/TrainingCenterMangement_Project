using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenterManagement.Domain
{
    public class Certificate
    {
        public Guid Id { get; set; }

        public required float Average { get; set; }
        [Url]
        public required string url { get; set; }



        public bool IsDelete { get; set; }


        // Trainee --> Certificate    one to many 
        public Trainee  Trainee { get; set; }
        public required Guid TraineeId { get; set; }

        // Course --> Certificate    one to many

        public Course Course { get; set; }
        public required Guid CourseId { get; set; }



        // Trainer --> Certificate    one to many
        public Trainer  Trainer { get; set; }

        public required Guid TrainerId { get; set; }



        // Exam --> Certificate    one to many

        public Exam  Exam { get; set; }
        public required Guid ExamId { get; set; }


        public Certificate()
        {
            Id = Guid.NewGuid();
            IsDelete = false;

        }








    }
}
