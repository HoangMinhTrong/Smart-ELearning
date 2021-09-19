using System.Collections.Generic;
using Smart_ELearning.Models.Enums;
using Smart_ELearning.ViewModels.Test;

namespace Smart_ELearning.ViewModels
{
    public class SubmitTestVM
    {
        
        public string TestTitle { get; set; }
        public int TestId { get; set; }
        public int QuestionId { get; set; }
        public AnswerChoice? NumberOfCorrectAnswer { get; set; }
        public double TotalGrade { get; set; }
        public int NumberOfQuestion { get; set; }
        public AnswerChoice? CorrectAnswer { get; set; }
        public string StudentAnswer { get; set; }
        public List<StudentQuestionVm> QuestionsResult { get; set; }
    }
}