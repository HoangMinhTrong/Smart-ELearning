using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.ViewModels.Attendance
{
    public class ScheduleAttendanceVm
    {
        public int ScheduleId { get; set; }
        public int AttendanceId { get; set; }
        public string StudentName { get; set; }
        public string SpecificId { get; set; }
        public string SubmitInRequire { get; set; }
        public string Status { get; set; }
        public bool IsPresent { get; set; }
    }
}