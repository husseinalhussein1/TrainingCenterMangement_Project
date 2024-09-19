using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenterManagement.Domain
{
    public class Lecture
    {
        
        public Guid LectureId { get; set; }
        public required string Titel { get; set; }
        public string Description { get; set; }
        [Url]
        public string VedioUrl { get; set; }
        [Url]
        public string ThumbnailUrl { get; set; }

        // date lecture 
        public DateTime LectureDate { get; set; }
        public bool IsDeleted { get; set; }


        // Foreign Key to Course
        public Guid CourseId { get; set; }
        public Course Course { get; set; }  // One-to-Many: Course -> Lecture

        // Many-to-Many: Lecture <-> Trainee (through Presence)
        public List<Presence> Presences { get; set; }

        public Lecture()
        {
            LectureId = Guid.NewGuid();
            Presences = new List<Presence>();
            
            LectureDate = DateTime.UtcNow;
            IsDeleted=false;
        }
    }
}
