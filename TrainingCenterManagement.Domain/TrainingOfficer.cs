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
        public TrainingOfficer()
        {
            Courses = new List<Course>();
        }

        public void CourseReview(ICollection<Course> courses) { /*Implementation*/ }
        public void AttendanceRegistration(Trainee trainee) { /*Implementation*/ }
        public void AddExam(Course course) { /*Implementation*/ }
        public void RemoveExam(Course course) { /*Implementation*/ }
        public bool SendResults(Course course) { /*Implementation*/ return true; }
    }

}
