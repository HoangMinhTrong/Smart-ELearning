using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.ViewModels.ScheduleViewModels
{
    public class ScheduleVM
    {
        public int Id { get; set; }
        public string DateTime { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string Title { get; set; }
    }
}