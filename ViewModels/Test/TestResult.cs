using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.ViewModels.Test
{
    public class TestResult
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string SpecificId { get; set; }
        public string IpAdress { get; set; }
        public string UserId { get; set; }
        public string NoOfCorrect { get; set; }
        public double TotalGrade { get; set; }
    }
}