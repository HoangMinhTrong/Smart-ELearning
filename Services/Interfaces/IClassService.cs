using Smart_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Services.Interfaces
{
    public interface IClassService
    { 
        

        Task<ICollection<ClassModel>> GetAll();

        Task<bool> Upsert(ClassModel model);

        Task<bool> Delete(int classId);

        Task<ClassModel> GetById(int? classId);
    }
}