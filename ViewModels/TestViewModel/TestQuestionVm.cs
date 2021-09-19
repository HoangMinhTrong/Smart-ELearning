using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Smart_ELearning.Models;

namespace Smart_ELearning.ViewModels
{
    public class TestQuestionVm
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<QuestionModel> question { get; set; }
    }
}