using System.ComponentModel.DataAnnotations;

namespace TrainingCenterManagementAPI.Models.View_HS
{
    public class VeiwLecture
    {
        public required string Titel { get; set; }
        public string Description { get; set; }
        public DateTime LectureDate { get; set; }
        [Url]
        public string VedioUrl { get; set; }
        [Url]
        public string ThumbnailUrl { get; set; }
    }
}
