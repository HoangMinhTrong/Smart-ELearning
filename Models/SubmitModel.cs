using System.Collections.Generic;

namespace Smart_ELearning.Models
{
    public class SubmitModel
    {
        public int Id { get; set; }

        //public int ScheduleId { get; set; }
        public string UserIp { get; set; }

        public int TestId { get; set; }
        public int NumberOfCorrectAnswer { get; set; }
        public double TotalGrade { get; set; }
        public TestModel TestModels { get; set; }
        public string UserId { get; set; }

        public ICollection<SubmitDetailModel> SubmitDetailModels { get; set; }
    }
}