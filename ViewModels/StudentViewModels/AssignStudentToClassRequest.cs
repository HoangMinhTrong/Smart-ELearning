using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.ViewModels.AccountViewModels
{
    public class AssignStudentToClassRequest
    {
        public int ClassId { get; set; }
        public string FullName { get; set; }
    }
}