namespace TrainingCenterManagementAPI.ViewModels
{
    public class AdministratorCreateModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; } // Assuming password is required for creating
    }
}
