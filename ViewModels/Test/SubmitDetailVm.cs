using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_ELearning.Models.Enums;

namespace Smart_ELearning.ViewModels.Test
{
    public class SubmitDetailVm
    {
        public string QuestionContent { get; set; }
        public string ChoiceA { get; set; }
        public string ChoiceB { get; set; }
        public string ChoiceC { get; set; }
        public string ChoiceD { get; set; }
        public AnswerChoice? StudentAnswer { get; set; }
        public AnswerChoice? CorrectAnswer { get; set; }
    }
}