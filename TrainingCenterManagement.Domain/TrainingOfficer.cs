using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenterManagement.Domain
{
    public class TrainingOfficer : Person
    {
        // One-to-Many: TrainingOfficer -> Course
        public ICollection<Course> Courses { get; set; }


        // One-to-One Account
        public Account Account { get; set; }
        public required Guid AccountId { get; set; }


        public TrainingOfficer()
        {
            Courses = new List<Course>();
        }

    
    }

}
