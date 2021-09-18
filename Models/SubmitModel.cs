using System.Collections.Generic;

namespace Smart_ELearning.Models
{
    public class SubmitModel
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public int TestId { get; set; }
        public int NumberOfCorrectAnswer { get; set; }
        public double TotalGrade { get; set; }

        public StudentAttendanceModel StudentAttendanceModel { get; set; }
    }
}