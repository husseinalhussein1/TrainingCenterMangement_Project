using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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


        private readonly IConfiguration configuration;

        public TrainingCenterManagementDbContext(
            DbContextOptions<TrainingCenterManagementDbContext> options,
            IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("TrainingCenterManagementDBConnectionString"));
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




            // hussien

            ////////////////////////////////////////////////// data Test //////////////////////////////////////////////
            // data seeding
            // seeding default product with its brands
            //modelBuilder.Entity<Product>().HasData(new Product()
            //{
            //    Id = 1,
            //    Name = "Learn how to create console application from scratch"
            //});

            //modelBuilder.Entity<Brand>().HasData(new Brand()
            //{
            //    Id = 1,
            //    Name = "Microsoft publish",
            //    Description = "another book created by microsoft",
            //    ProductId = 1
            //});

            //modelBuilder.Entity<Brand>().HasData(new Brand()
            //{
            //    Id = 2,
            //    Name = "Amazon publish",
            //    Description = "another book created by amazon",
            //    ProductId = 1
            //});
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////











            base.OnModelCreating(modelBuilder); 
        }


        // for Testing Database
        public static void CreatInitalTestingDatabase(TrainingCenterManagementDbContext context)
        {
            //Delete if exists
            context.Database.EnsureDeleted();

            // Migration (Create and appliy Migration)
            context.Database.Migrate();

            // add dummy data (seeding data)

            //==============================

            //(Trainee)*4
            var trainee1 = new Trainee
            {
                FirstName = "Ali",
                LastName = "Ahmad",
                PhoneNumber = "1234567890",
                Account = new Account
                {
                    UserName = "ali",
                    Password = "1234567890",
                    Email = "ali@gmail.com",
                    IsRemember = true
                }
            };

            var trainee2 = new Trainee
            {
                FirstName = "Sara",
                LastName = "Salem",
                PhoneNumber = "1234567890",
                Account = new Account
                {
                    UserName = "sara",
                    Password = "1234567890",
                    Email = "sara@gmail.com",
                    IsRemember = true
                }
            };

            var trainee3 = new Trainee
            {
                FirstName = "Omar",
                LastName = "Khaled",
                PhoneNumber = "1234567890",
                Account = new Account
                {
                    UserName = "omar",
                    Password = "1234567890",
                    Email = "omar@gmail.com",
                    IsRemember = true
                }
            };

            var trainee4 = new Trainee
            {
                FirstName = "Nada",
                LastName = "Hassan",
                PhoneNumber = "1234567890",
                Account = new Account
                {
                    UserName = "nada",
                    Password = "1234567890",
                    Email = "nada@gmail.com",
                    IsRemember = true
                }
            };
            //==============================

            //  (Trainer)
            var trainer = new Trainer
            {
                FirstName = "Mohammad",
                LastName = "Nour",
                PhoneNumber = "1234567890",
                Specialty = "Software Development",
                YearsOfExperience = 5,
                BusinessLink = "http://linkedin.com/mohammad",
                Account = new Account
                {
                    UserName = "mohammad",
                    Password = "1234567890",
                    Email = "mohammad@gmail.com",
                    IsRemember = true
                }
            };
            //==============================

            //(Training Officer)
            var trainingOfficer = new TrainingOfficer
            {
                FirstName = "Huda",
                LastName = "Ali",
                PhoneNumber = "1234567890",
                Account = new Account
                {
                    UserName = "huda",
                    Password = "1234567890",
                    Email = "huda@gmail.com",
                    IsRemember = true
                }
            };
            //==============================

            //(Administrator)
            var admin = new Administrator
            {
                FirstName = "Ahmed",
                LastName = "Hassan",
                PhoneNumber = "1234567890",
                Account = new Account
                {
                    UserName = "admin",
                    Password = "admin",
                    Email = "admin@gmail.com",
                    IsRemember = true
                }
            };
            //==============================

            // (Course 1)
            var course1 = new Course
            {
                CourseName = "ASP.NET Core",
                BatchNumber = 1,
                NumberOfLectures = 10,
                Price = 150.00f,
                ReleaseDate = DateTime.UtcNow,
                TrainingOfficerId = trainingOfficer.Id,
                TrainingOfficer = trainingOfficer,
                ThumbnailUrl= "~/StaticFiles/Courses/CoursesThumbnails/Asp.net_1_Course.png",
                VedioUrl= "~/StaticFiles/Courses/CoursesVideos/Asp.net_1_Course.mp4",
                Description = "This course provides a comprehensive introduction to ASP.NET Core, a modern web framework for building dynamic and scalable web applications. Students will learn how to create RESTful APIs, implement MVC architecture, and leverage middleware for enhanced functionality. By the end of the course, participants will be equipped with the skills to develop robust applications using best practices and design patterns in ASP.NET Core.",
            };
            //==============================

            //(Course 2)
            var course2 = new Course
            {
                CourseName = "Angular",
                BatchNumber = 2,
                NumberOfLectures = 12,
                Price = 200.00f,
                ReleaseDate = DateTime.UtcNow.AddDays(7),
                TrainingOfficerId = trainingOfficer.Id,
                TrainingOfficer = trainingOfficer,
                ThumbnailUrl= "~/StaticFiles/Courses/CoursesThumbnails/Angular_2_Course.png",
                VedioUrl = "~/StaticFiles/Courses/CoursesVideos/Angular_2_Course.mp4",
                Description = "This course offers an in-depth exploration of Angular, a powerful front-end framework for building dynamic web applications. Participants will learn about components, services, and routing, as well as how to manage state and implement responsive designs. By the end of the course, students will have the skills to create maintainable and scalable single-page applications (SPAs) using Angular best practices and tools."
            };
            //==============================

            //(Many-to-Many: Trainee <-> Course)
            course1.Trainees.Add(trainee1);
            course1.Trainees.Add(trainee2);
            course2.Trainees.Add(trainee3);
            course2.Trainees.Add(trainee4);

            trainee1.Courses.Add(course1);
            trainee2.Courses.Add(course1);
            trainee3.Courses.Add(course2);
            trainee4.Courses.Add(course2);
            //==============================

            //(Many-to-Many: Course <-> Trainer)
            course1.Trainers.Add(trainer);
            course2.Trainers.Add(trainer);
            //==============================

            //(Lectures)*2
            var lecture1 = new Lecture
            {
                Titel = "Introduction to ASP.NET Core",
                Description = "Overview of the ASP.NET Core framework",
                VedioUrl = "~/StaticFiles/Lectures/LectureVideos/ASP.NET_Core_1/ASP.NET_Core_1_Lecture_1.mp4",
                ThumbnailUrl = "~/StaticFiles/Lectures/LectureThumbnails/ASP.NET_Core_1/ASP.NET_Core_1_Lecture_1.png",
                CourseId = course1.CourseId,
                Course = course1
            };

            var lecture2 = new Lecture
            {
                Titel = "Angular Basics",
                Description = "Getting started with Angular",
                VedioUrl = "~/StaticFiles/Lectures/LectureVideos/Angular_2/Angular_2_Lecture_1.mp4",
                ThumbnailUrl = "~/StaticFiles/Lectures/LectureThumbnails/Angular_2/Angular_2_Lecture_1.png",
                Course = course2
            };

            course1.Lectures.Add(lecture1);
            course2.Lectures.Add(lecture2);
            //==============================

            //(Many-to-Many: Lecture <-> Trainee through Presence)
            var presence1 = new Presence
            {
                IsPresence = true,
                TraineeId = trainee1.Id,
                Trainee = trainee1,
                LectureId = lecture1.LectureId,
                Lecture = lecture1
            };

            var presence2 = new Presence
            {
                IsPresence = true,
                TraineeId = trainee3.Id,
                Trainee = trainee3,
                LectureId = lecture2.LectureId,
                Lecture = lecture2
            };

            lecture1.Presences.Add(presence1);
            lecture2.Presences.Add(presence2);
            //==============================

            //(Payments)
            var payment1 = new Payment
            {
                TotalAmount = 150,
                CourseId = course1.CourseId,
                TraineeId = trainee1.Id,
                Trainee = trainee1
            };

            var payment2 = new Payment
            {
                TotalAmount = 150,
                CourseId = course1.CourseId,
                TraineeId = trainee2.Id,
                Trainee = trainee3
            };

            var payment3 = new Payment
            {
                TotalAmount = 150,
                CourseId = course1.CourseId,
                TraineeId = trainee3.Id,
                Trainee = trainee1
            };

            var payment4 = new Payment
            {
                TotalAmount = 150,
                CourseId = course1.CourseId,
                TraineeId = trainee4.Id,
                Trainee = trainee1
            };

            //===============

            var payment5 = new Payment
            {
                TotalAmount = 200,
                CourseId = course2.CourseId,
                TraineeId = trainee1.Id,
                Trainee = trainee1
            };

            var payment6 = new Payment
            {
                TotalAmount = 200,
                CourseId = course2.CourseId,
                TraineeId = trainee2.Id,
                Trainee = trainee3
            };

            var payment7 = new Payment
            {
                TotalAmount = 200,
                CourseId = course2.CourseId,
                TraineeId = trainee3.Id,
                Trainee = trainee1
            };

            var payment8 = new Payment
            {
                TotalAmount = 200,
                CourseId = course2.CourseId,
                TraineeId = trainee4.Id,
                Trainee = trainee1
            };
            //Adding payments to courses
            course1.Payments.Add(payment1);
            course1.Payments.Add(payment2);
            course1.Payments.Add(payment3);
            course1.Payments.Add(payment4);

            course2.Payments.Add(payment1);
            course2.Payments.Add(payment2);
            course2.Payments.Add(payment3);
            course2.Payments.Add(payment4);

            //Adding payments to trainees
            trainee1.Payments.Add(payment1);
            trainee1.Payments.Add(payment5);

            trainee2.Payments.Add(payment2);
            trainee2.Payments.Add(payment6);

            trainee3.Payments.Add(payment3);
            trainee3.Payments.Add(payment7);

            trainee4.Payments.Add(payment4);
            trainee4.Payments.Add(payment8);

            //==============================

            // Adding to context
            context.Trainees.AddRange(trainee1, trainee2, trainee3, trainee4);
            context.Trainers.Add(trainer);
            context.TrainingOfficers.Add(trainingOfficer);
            context.Administrators.Add(admin);
            context.Courses.AddRange(course1, course2);
            context.Lectures.AddRange(lecture1, lecture2);
            context.Presences.AddRange(presence1, presence2);
            context.Payments.AddRange(payment1, payment2,payment3,payment4,payment5,payment6,payment7,payment8);

            //==============================

            //Save Data on DB
            context.SaveChanges();
        }



    }


}
