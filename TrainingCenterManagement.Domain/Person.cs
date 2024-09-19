using System.ComponentModel.DataAnnotations;

namespace TrainingCenterManagement.Domain
{
    public abstract class Person
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public required string PhoneNumber { get; set; }
        public bool IsDeleted { get; set; }
        protected Person()
        {
            Id = Guid.NewGuid();
            IsDeleted=false;

        }
    }

}
