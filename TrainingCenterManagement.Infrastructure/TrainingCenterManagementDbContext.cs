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
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Account> Accounts { get; set; }

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




            //Certifacate
          modelBuilder.Entity<Certificate>()
                        .HasOne(c => c.Trainer)
                        .WithMany(t => t.Certificates) // إذا كان هناك علاقة عديدة إلى واحدة
                        .HasForeignKey(c => c.TrainerId)
                        .OnDelete(DeleteBehavior.NoAction); // تحديد الحذف بدون تأثير متتابع

         modelBuilder.Entity<Certificate>()
                      .HasOne(c => c.Trainee)
                      .WithMany(t => t.Certificates) // إذا كان هناك علاقة عديدة إلى واحدة
                      .HasForeignKey(c => c.TraineeId)
                      .OnDelete(DeleteBehavior.NoAction); // تحديد الحذف بدون تأثير متتابع

        modelBuilder.Entity<Certificate>()
                        .HasOne(c => c.Exam)
                        .WithMany(t => t.Certificates) // إذا كان هناك علاقة عديدة إلى واحدة
                        .HasForeignKey(c => c.ExamId)
                        .OnDelete(DeleteBehavior.NoAction); // تحديد الحذف بدون تأثير متتابع

        modelBuilder.Entity<Certificate>()
                    .HasOne(c => c.Course)
                      .WithMany(t => t.Certificates) // إذا كان هناك علاقة عديدة إلى واحدة
                      .HasForeignKey(c => c.CourseId)
                      .OnDelete(DeleteBehavior.NoAction); // تحديد الحذف بدون تأثير متتابع



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

            //(Account)
            var account1 = new Account()
            {
                Id = Guid.Parse("4c14a5aa-218b-45fa-acbc-080dc4cb2c14"),
                UserName = "ali",
                Password = "1234567890",
                Email = "ali@gmail.com",
                IsRemember = true
            };
            var account2 = new Account()
            {
                Id = Guid.Parse("b8673de3-e88c-4e2d-95cb-4a963f53d048"),
                UserName = "sara",
                Password = "1234567890",
                Email = "sara@gmail.com",
                IsRemember = true
            };
            var account3 = new Account()
            {
                Id = Guid.Parse("5eec759d-d655-4753-90e9-987a60ba7f68"),
                UserName = "omar",
                Password = "1234567890",
                Email = "omar@gmail.com",
                IsRemember = true
            };
            var account4 = new Account()
            {
                Id = Guid.Parse("f56b5941-9ac4-4734-bf50-183fd689e8cd"),
                UserName = "nada",
                Password = "1234567890",
                Email = "nada@gmail.com",
                IsRemember = true
            };
            var account5 = new Account()
            {
                Id = Guid.Parse("2935753a-84fe-4a8a-8f08-a461a241a9db"),
                UserName = "mohammad",
                Password = "1234567890",
                Email = "mohammad@gmail.com",
                IsRemember = true
            };
            var account6 = new Account()
            {
                Id = Guid.Parse("fdafc22a-bd54-4e56-89f7-cacedc3bd0cd"),
                UserName = "huda",
                Password = "1234567890",
                Email = "huda@gmail.com",
                IsRemember = true
            };
            var account7 = new Account()
            {
                Id = Guid.Parse("12f5cb7d-edc3-444b-8dbc-5f7c8244cf25"),
                UserName = "admin",
                Password = "admin",
                Email = "admin@gmail.com",
                IsRemember = true
            };

            context.Accounts.Add(account1);
            context.Accounts.Add(account2);
            context.Accounts.Add(account3);
            context.Accounts.Add(account4);
            context.Accounts.Add(account5);
            context.Accounts.Add(account6);
            context.Accounts.Add(account7);

            //(Trainee)*4
            var trainee1 = new Trainee
            {
                
                FirstName = "Ali",
                LastName = "Ahmad",
                PhoneNumber = "1234567890",
                AccountId = Guid.Parse("4c14a5aa-218b-45fa-acbc-080dc4cb2c14")   
            };

            var trainee2 = new Trainee
            {
                
                FirstName = "Sara",
                LastName = "Salem",
                PhoneNumber = "1234567890",
                AccountId = Guid.Parse("b8673de3-e88c-4e2d-95cb-4a963f53d048")
                
            };

            var trainee3 = new Trainee
            {
                FirstName = "Omar",
                LastName = "Khaled",
                PhoneNumber = "1234567890",
                AccountId = Guid.Parse("5eec759d-d655-4753-90e9-987a60ba7f68")
               
            };

            var trainee4 = new Trainee
            {
                FirstName = "Nada",
                LastName = "Hassan",
                PhoneNumber = "1234567890",
                AccountId = Guid.Parse("f56b5941-9ac4-4734-bf50-183fd689e8cd")
                
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
                AccountId = Guid.Parse("2935753a-84fe-4a8a-8f08-a461a241a9db")
               
            };
            //==============================

            //(Training Officer)
            var trainingOfficer = new TrainingOfficer
            {
                FirstName = "Huda",
                LastName = "Ali",
                PhoneNumber = "1234567890",
                AccountId = Guid.Parse("fdafc22a-bd54-4e56-89f7-cacedc3bd0cd")
               
            };
            //==============================

            //(Administrator)
            var admin = new Administrator
            {
                FirstName = "Ahmed",
                LastName = "Hassan",
                PhoneNumber = "1234567890",
                AccountId = Guid.Parse("12f5cb7d-edc3-444b-8dbc-5f7c8244cf25")
              
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
                ThumbnailUrl = "asp.net_1_course.png",
                VedioUrl = "asp.net_1_course.mp4",
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
                ThumbnailUrl = "angular_2_course.png",
                VedioUrl = "angular_2_course.mp4",
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
                VedioUrl = "introduction_to_asp.net_core_lecetuer_asp.net_1.mp4",
                ThumbnailUrl = "introduction_to_asp.net_core_lecetuer_asp.net_1.png",
                CourseId = course1.CourseId,
                Course = course1
            };

            var lecture2 = new Lecture
            {
                Titel = "Angular Basics",
                Description = "Getting started with Angular",
                VedioUrl = "angularbasics_leceture_angular_2.mp4",
                ThumbnailUrl = "angularbasics_leceture_angular_2.png",
                CourseId = course2.CourseId,
                Course = course2,
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


            //Exam
            var exam1 = new Exam()
            {
                CourseId = course1.CourseId,
                ExamName = "asp",
                ExamDate = DateTime.Parse("2024-11-11")
            };

            var exam2 = new Exam()
            {
                CourseId = course2.CourseId,
                ExamName = "angular",
                ExamDate = DateTime.Parse("2024-10-11")
            };
            course1.Exam = exam1;
            course2.Exam = exam2;

            // Adding to context
            context.Trainees.AddRange(trainee1, trainee2, trainee3, trainee4);
            context.Trainers.Add(trainer);
            context.TrainingOfficers.Add(trainingOfficer);
            context.Administrators.Add(admin);
            context.Exams.AddRange(exam1, exam2);
            context.Courses.AddRange(course1, course2);
            context.Lectures.AddRange(lecture1, lecture2);
            context.Presences.AddRange(presence1, presence2);
            context.Payments.AddRange(payment1, payment2, payment3, payment4, payment5, payment6, payment7, payment8);

            //==============================

            //Save Data on DB
            context.SaveChanges();
        }



    }


}
