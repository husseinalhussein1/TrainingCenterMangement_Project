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
        public bool IsDeleted { get; set; }


        public Account()
        {
            if(Id!=Guid.Empty)
            Id = Guid.NewGuid(); 
            IsDeleted = false;
        }



        // One-to-One Trainee - Account
        public Trainee Trainee { get; set; }

        // One-to-One Trainer - Account
        public Trainer Trainer { get; set; }

        // One-to-One TrainingOfficer - Account
        public TrainingOfficer TrainingOfficer { get; set; }

        // One-to-One Administrator - Account
        public Administrator Administrator { get; set; }

        // One-to-One Receptionist - Account
        public Receptionist Receptionist { get; set; }
    }
}
