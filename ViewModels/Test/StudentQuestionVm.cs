using Smart_ELearning.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.ViewModels.Test
{
    public class StudentQuestionVm
    {
        public int Id { get; set; }
        public double Score { get; set; }
        public int TestId { get; set; }
        public string Content { get; set; }
        public AnswerChoice? CorrectAnswer { get; set; }
        public string ChoiceA { get; set; }
        public string ChoiceB { get; set; }
        public string ChoiceC { get; set; }
        public string ChoiceD { get; set; }
        public AnswerChoice? StudentAnswer { get; set; }
    }
}