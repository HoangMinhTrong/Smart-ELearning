﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Smart_ELearning.Models
{
    public class ClassModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<StudentInClassModel> StudentInClassModels { get; set; }
        public ICollection<ScheduleModel> ScheduleModels { get; set; }
    }
}