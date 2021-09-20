using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.ViewModels.Attendance
{
    public class ClassAttendanceVm
    {
        public int ScheduleId { get; set; }
        public string ClassName { get; set; }
        public string Subject { get; set; }
        public string ScheduleTile { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
    }
}