using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Smart_ELearning.Models
{
    public class ScheduleModel
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public ClassModel ClassModel { get; set; }
        public int SubjectId { get; set; }
        public string Title { get; set; }
        public SubjectModel SubjectModel { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public ICollection<TestModel> TestModels { get; set; }
        public ICollection<StudentAttendanceModel> StudentAttendanceModels { get; set; }
    }
}