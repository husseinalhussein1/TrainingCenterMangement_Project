namespace TrainingCenterManagementAPI.ViewModels
{
    public class CertificateViewModel
    {
        public Guid Id { get; set; }
        public float Average { get; set; }
        public string Url { get; set; }
        public bool IsDelete { get; set; }
        public Guid TraineeId { get; set; }
        public Guid CourseId { get; set; }
        public Guid TrainerId { get; set; }
        public Guid ExamId { get; set; }
    }
}
