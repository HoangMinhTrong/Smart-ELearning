using Microsoft.VisualBasic;

namespace Smart_ELearning.Models
{
    public class ScheduleModel
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int SubjectId  { get; set; }
        public DateAndTime DateTime { get; set; }
        public DateAndTime StartTime  { get; set; }
        public DateAndTime EndTime { get; set; }
    }
}