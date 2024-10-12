namespace TrainingCenterManagementAPI.VeiwModels
{
    public class LectuerForCreate
    {
        public required string Titel { get; set; }
        public string Description { get; set; }
        public DateTime LectureDate { get; set; }
        public IFormFile? Img { get; set; }
        public IFormFile? Video { get; set; }
    }
}
