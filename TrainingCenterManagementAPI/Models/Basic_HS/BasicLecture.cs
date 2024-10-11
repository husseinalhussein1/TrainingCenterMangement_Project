
using System.ComponentModel.DataAnnotations;
using TrainingCenterManagementAPI.Models.View_HS;

namespace TrainingCenterManagementAPI.Models.Basic_HS
{
    public class BasicLecture : VeiwLecture
    {
        public Guid LectureId { get; set; }
        
    }
}
