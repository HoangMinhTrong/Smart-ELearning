using System.Collections.Generic;

namespace Smart_ELearning.Models
{
    public class SubjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ScheduleModel> ScheduleModels { get; set; }
    }
}