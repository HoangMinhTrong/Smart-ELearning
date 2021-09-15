using System.Collections.Generic;

namespace Smart_ELearning.Models
{
    public class TestModel
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public ScheduleModel ScheduleModel { get; set; }
        public string Title { get; set; }
        public int NumberOfQuestion { get; set; }
        public string Duration { get; set; }
        public bool Status { get; set; }
        public bool CorrectAnswerlist { get; set; }

        public ICollection<QuestionModel> QuestionModels { get; set; }
    }
}