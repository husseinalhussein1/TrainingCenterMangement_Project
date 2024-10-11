using TrainingCenterManagementAPI.Models.View_HS;

namespace TrainingCenterManagementAPI.Models.ForCreate_HS
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
