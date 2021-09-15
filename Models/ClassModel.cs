using System.Collections.Generic;

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