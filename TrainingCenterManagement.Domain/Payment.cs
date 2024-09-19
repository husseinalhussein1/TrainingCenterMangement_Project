using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenterManagement.Domain
{
    public class Payment
    {
        public Guid PaymentId { get; set; }

        public required decimal TotalAmount { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsDelete { get; set; }

        // course -> Payment  
        // كل كورس له دفعة او اكثر وكل دفعة تخص كورس معين
        public Course Course { get; set; }

        public required Guid CourseId { get; set; }

        //Trainee -> Payment   one to Many
        // كل متدرب بكورس يمكن ان يكون له دفعة او اكثر وكل دفعة خاصة بمتدرب

        public Trainee Trainee { get; set; }
        public required Guid TraineeId { get; set; }



        public Payment()
        {
            PaymentId=new Guid();

            CreatedDate = DateTime.UtcNow;

            IsDelete = false;
        }

    }
}
