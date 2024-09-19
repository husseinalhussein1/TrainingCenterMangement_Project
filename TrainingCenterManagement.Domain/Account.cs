using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenterManagement.Domain
{
    public class Account
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public bool IsRemember { get; set; }

        public Account()
        {
            Id = Guid.NewGuid();    
        }

        // One-to-One Trainee - Account
        public Guid? TraineeId { get; set; }
        public Trainee Trainee { get; set; }

        // One-to-One Trainer - Account
        public Guid? TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        // One-to-One TrainingOfficer - Account
        public Guid? TrainingOfficerId { get; set; }
        public TrainingOfficer TrainingOfficer { get; set; }

        // One-to-One Administrator - Account
        public Guid? AdministratorId { get; set; }
        public Administrator Administrator { get; set; }

        // One-to-One Receptionist - Account
        public Guid? ReceptionistId { get; set; }
        public Receptionist Receptionist { get; set; }
    }
}
