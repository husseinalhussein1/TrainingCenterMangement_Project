using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenterManagement.Domain
{
    public class Presence
    {
        public Guid PresenceId { get; set; }
        public required bool IsPresence { get; set; }

        // Foreign Key to Trainee
        public required Guid TraineeId { get; set; }
        public Trainee Trainee { get; set; } // One-to-Many: Trainee -> Presence

        // Foreign Key to Lecture
        public required Guid LectureId { get; set; }
        public Lecture Lecture { get; set; }

        public bool IsDeleted { get; set; }


        public Presence()
        {
            IsDeleted = false;
        }


    }
}
