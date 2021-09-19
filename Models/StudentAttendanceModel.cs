namespace Smart_ELearning.Models
{
    public class StudentAttendanceModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool IsPresent { get; set; }
        public int ScheduleId { get; set; }
        public ScheduleModel ScheduleModel { get; set; }
        public int SubmitId { get; set; }
    }
}