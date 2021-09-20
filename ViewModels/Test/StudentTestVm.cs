using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_ELearning.Models.Enums;

namespace Smart_ELearning.ViewModels.Test
{
    public class StudentTestVm
    {
        public int ScheduleId { get; set; }
        public int TestId { get; set; }
        public string TestTitle { get; set; }
        public int NumberOfQuestion { get; set; }
        public string StudentIp { get; set; }

        public List<StudentQuestionVm> QuestionsResult { get; set; }
    }
}