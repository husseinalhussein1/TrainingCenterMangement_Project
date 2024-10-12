namespace TrainingCenterManagementAPI.VeiwModels
{
    public class VeiwCourse
    {
        public required string CourseName { get; set; }
        public required int BatchNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public required int NumberOfLectures { get; set; }
        public required float Price { get; set; }
        public string Description { get; set; }

    }
}
