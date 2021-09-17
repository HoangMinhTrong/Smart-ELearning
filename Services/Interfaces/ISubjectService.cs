using Smart_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<int> Upsert(SubjectModel model);

        Task<ICollection<SubjectModel>> GetAll();

        Task<int> Delete(int id);

        Task<SubjectModel> GetById(int classId);
    }
}