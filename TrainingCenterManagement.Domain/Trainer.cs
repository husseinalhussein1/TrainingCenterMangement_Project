using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenterManagement.Domain
{
    public class Trainer : Person
    {
        public required string Specialty { get; set; }
        public required int YearsOfExperience { get; set; }
        [Url]
        public required string BusinessLink { get; set; }

        // One-to-Many: Trainer -> Course
        public ICollection<Course> Courses { get; set; }


        // Trainer --> Certificate    one to many
        public ICollection<Certificate>   Certificates { get; set; }



        // One-to-One Account
        public Account Account { get; set; }
        public required Guid AccountId { get; set; }


        public Trainer()
        {
            Courses = new List<Course>();

            Certificates = new List<Certificate>();
        }
    }
}
