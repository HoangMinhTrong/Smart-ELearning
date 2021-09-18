using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_ELearning.Models
{
    public class TestModel
    {
        [Key]
        public int Id { get; set; }

        public int ScheduleId { get; set; }
        public ScheduleModel ScheduleModel { get; set; }
        public string Title { get; set; }
        public int NumberOfQuestion { get; set; }
        public bool Status { get; set; }
        public bool CorrectAnswerlist { get; set; }
        public DateTime LockoutEnd { get; set; }

        public ICollection<QuestionModel> QuestionModels { get; set; }
        public ICollection<SubmitModel> SubmitModels { get; set; }
    }
}