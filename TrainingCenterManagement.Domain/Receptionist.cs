using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenterManagement.Domain
{
    public class Receptionist : Person
    {


        // One-to-One Account
        public Account Account { get; set; }

    

    }
}
