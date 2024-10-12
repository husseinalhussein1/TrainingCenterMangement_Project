using System.ComponentModel.DataAnnotations;

namespace TrainingCenterManagementAPI.VeiwModels
{
    public class BasicCourse : VeiwCourse
    {
        public Guid Id { get; set; }
        public required Guid TrainingOfficerId { get; set; }
        [Url]
        public string VedioUrl { get; set; }
        [Url]
        public string ThumbnailUrl { get; set; }
    }
}
