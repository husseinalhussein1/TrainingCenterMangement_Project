using Microsoft.EntityFrameworkCore;
using TrainingCenterManagement.Domain;

namespace TrainingCenterManagement.Infrastructure
{
    

  
    public class TrainingCenterManagementDbContext : DbContext
    {
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Presence> Presences { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Receptionist> Receptionists { get; set; }
        public DbSet<TrainingOfficer> TrainingOfficers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrainingCenterManagement_DB";
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // تجاهل كلاس Person لأننا لا نريد إنشاء جدول له
            modelBuilder.Ignore<Person>();

            // تكوين العلاقة One-to-One بين Course و Exam
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Exam)
                .WithOne(e => e.Course)
                .HasForeignKey<Exam>(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict); 

            // تكوين العلاقة One-to-Many بين Course و Lecture
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Lectures)
                .WithOne(l => l.Course)
                .HasForeignKey(l => l.CourseId)
                .OnDelete(DeleteBehavior.Restrict); 

            // تكوين العلاقة One-to-Many بين Course و Presence (من خلال المحاضرات)
            modelBuilder.Entity<Lecture>()
                .HasMany(l => l.Presences)
                .WithOne(p => p.Lecture)
                .HasForeignKey(p => p.LectureId)
                .OnDelete(DeleteBehavior.Restrict);

            // تكوين العلاقة One-to-Many بين Trainee و Presence (الحضور)
            modelBuilder.Entity<Trainee>()
                .HasMany(t => t.Presences)
                .WithOne(p => p.Trainee)
                .HasForeignKey(p => p.TraineeId)
                .OnDelete(DeleteBehavior.Restrict); 

            // تكوين العلاقة One-to-Many بين TrainingOfficer و Course
            modelBuilder.Entity<TrainingOfficer>()
                .HasMany(to => to.Courses)
                .WithOne(c => c.TrainingOfficer)
                .HasForeignKey(c => c.TrainingOfficerId)
                .OnDelete(DeleteBehavior.Restrict); 

            base.OnModelCreating(modelBuilder); 
        }
    }


}
