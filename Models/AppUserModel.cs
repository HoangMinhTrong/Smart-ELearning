using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Smart_ELearning.Models
{
    public class AppUserModel : IdentityUser
    {
        public string FullName { get; set; }
        public int SpecificId { get; set; }

        public ICollection<StudentInClassModel> StudentInClassModels { get; set; }
        public ICollection<StudentAttendanceModel> StudentAttendanceModels { get; set; }
        public ICollection<SubmitModel> SubmitModels { get; set; }
    }
}