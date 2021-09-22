using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_ELearning.Models;
using Smart_ELearning.ViewModels;

namespace Smart_ELearning.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<int> Upsert(QuestionModel model);

        Task<int> Delete(int id);

        TestQuestionVm GetTestQuestions(int testId);

        Task<ICollection<QuestionModel>> GetAll();

        Task<QuestionModel> GetById(int id);

        Task<int> AddRange(ICollection<QuestionModel> models);

        List<QuestionModel> GetByTestId(int testId);

        Task<int> UpdateRange(List<QuestionModel> models);
    }
}