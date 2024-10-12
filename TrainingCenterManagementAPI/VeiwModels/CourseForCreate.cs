namespace TrainingCenterManagementAPI.VeiwModels
{
    public class CourseForCreate : VeiwCourse
    {
        public required Guid TrainingOfficerId { get; set; }
        public IFormFile? Img { get; set; }
        public IFormFile? Video { get; set; }
    }
}
