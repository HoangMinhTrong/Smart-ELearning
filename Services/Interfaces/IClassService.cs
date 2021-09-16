using Smart_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Services.Interfaces
{
    public interface IClassService
    { feature/Controller
        

        Task<ICollection<ClassModel>> GetAll();

        Task<int> Upsert(ClassModel model);

        Task<int> Delete(int classId);

        Task<ClassModel> GetById(int? classId);
 develop
    }
}