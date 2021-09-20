using System;

namespace Smart_ELearning.ViewModels
{
    public class TestToScheduleViewModel
    {
        public int Id { get; set; }
        public string Title {get; set;}
        public int NumberOfQuestion { get; set; }
        public DateTime lockoutEnd { get; set; }
        public string Status { get; set; }
    }
}