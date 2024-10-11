using System.ComponentModel.DataAnnotations;
using TrainingCenterManagementAPI.Models.View_HS;

namespace TrainingCenterManagementAPI.Models.Basic_HS
{
    public class BasicCourse:VeiwCourse
    {
        public Guid Id { get; set; }
        public required Guid TrainingOfficerId { get; set; }
        [Url]
        public string VedioUrl { get; set; }
        [Url]
        public string ThumbnailUrl { get; set; }

    }

}
