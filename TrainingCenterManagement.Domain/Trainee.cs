using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenterManagement.Domain
{

    public class Trainee : Person
    {
        // Many-to-Many: Trainee <-> Course
        public ICollection<Course> Courses { get; set; }

        // Many-to-Many: Trainee <-> Lecture (through Presence)
        public ICollection<Presence> Presences { get; set; }

///////////////////////////////////////////////////////

        //Trainee -> Payment   one to Many
        public ICollection<Payment> Payments { get; set; }



        // One-to-One Account
        public Account Account { get; set; }



        public Trainee()
        {
            Presences = new List<Presence>();
            Courses = new List<Course>();
            Payments = new List<Payment>();
            
        }
    }
}
