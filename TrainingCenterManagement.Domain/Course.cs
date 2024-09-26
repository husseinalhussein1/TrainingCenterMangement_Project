using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TrainingCenterManagement.Domain
{

    public class Course
    {
        public Guid CourseId { get; set; }
        public required string CourseName { get; set; }

        
        public required int BatchNumber { get; set; }
        public DateTime CreatedData { get; set; }
        public DateTime ReleaseDate { get; set; }
      
        public bool IsDeleted { get; set; }
        public required int NumberOfLectures { get; set; }
        public required float Price { get; set; }
        public string Description { get; set; }
        [Url]
        public string VedioUrl { get; set; }
        [Url]
        public string ThumbnailUrl { get; set; }

        // Foreign Key to TrainingOfficer
        public required Guid TrainingOfficerId { get; set; } 
        public TrainingOfficer TrainingOfficer { get; set; } 

        // Many-to-Many: Course <-> Trainer
        public ICollection<Trainer>  Trainers { get; set; }

        // One-to-One: Course -> Exam
        public Exam Exam { get; set; }

        // One-to-Many: Course -> Presence
        public ICollection<Presence> Presences { get; set; }

        // Many-to-Many: Course <-> Trainee
        public ICollection<Trainee> Trainees { get; set; }

        // One-to-Many: Course -> Lecture
        public ICollection<Lecture> Lectures { get; set; }

        /////////////////////////////////////////////////////////////////////////////////
        // One-to-Many: Course -> Payment
        public ICollection<Payment> Payments { get; set; }


        public Course()
        {
            Trainees = new List<Trainee>();
            Presences = new List<Presence>();
            Trainers = new List<Trainer>();
            CourseId = Guid.NewGuid();
            CreatedData = DateTime.UtcNow;
            Lectures = new List<Lecture>();
            Payments = new List<Payment>();
            IsDeleted = false;
        }
    }


}
