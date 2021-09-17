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
    public class ScheduleViewModel
    {
        public ScheduleModel ScheduleModel{ get; set; }
        public IEnumerable<SelectListItem> ClassListItems { get; set; }
        public IEnumerable<SelectListItem> SubjectListItems { get; set; }
    }
}