using Smart_ELearning.Models;
using Smart_ELearning.ViewModels.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Services.Interfaces
{
    public interface IStudentService
    {
        Task<ICollection<StudentInClassVM>> GetStudentInClass(int classId);

        Task<int> AssignStudentToClass(AssignStudentToClassRequest request);

        Task<int> RemoveStudentInStudent(string studentId, int classId);
    }
}