using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using Smart_ELearning.Models.Enums;

namespace Smart_ELearning.Models
{
    public class SubmitDetailModel
    {
        public int Id { get; set; }
        public int SubmitId { get; set; }
        public SubmitModel SubmitModel { get; set; }

        public int QuestionId { get; set; }
        public AnswerChoice? StudentAnswer { get; set; }

        public ICollection<ScheduleModel> ScheduleModels { get; set; }
    }
}