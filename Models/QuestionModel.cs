﻿namespace Smart_ELearning.Models
{
    public class QuestionModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public double Score { get; set; }
        public string Content { get; set; }
        public char CorrectAnswer { get; set; }
        public string ChoiceA { get; set; }
        public string ChoiceB { get; set; }
        public string ChoiceC { get; set; }
        public string ChoiceD { get; set; }
    }
}