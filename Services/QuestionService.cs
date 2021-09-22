using Microsoft.EntityFrameworkCore;
using Smart_ELearning.Data;
using Smart_ELearning.Models;
using Smart_ELearning.Services.Interfaces;
using Smart_ELearning.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Services
{
    public class QuestionSerive : IQuestionService
    {
        private readonly ApplicationDbContext _context;

        public QuestionSerive(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddRange(ICollection<QuestionModel> models)
        {
            var numberOfQuestion = models.Count();
            foreach (var item in models)
            {
                item.Score = 100 / numberOfQuestion;
            }
            await _context.QuestionModels.AddRangeAsync(models);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var question = await this.GetById(id);
            _context.QuestionModels.Remove(question);
            return await _context.SaveChangesAsync();
        }

        public async Task<ICollection<QuestionModel>> GetAll()
        {
            var data = await _context.QuestionModels.ToListAsync();

            return data;
        }

        public async Task<QuestionModel> GetById(int id)
        {
            var question = await _context.QuestionModels.FindAsync(id);
            return question;
        }

        public List<QuestionModel> GetByTestId(int testId)
        {
            var data = _context.QuestionModels.Where(x => x.TestId == testId).ToList();

            return data;
        }

        public TestQuestionVm GetTestQuestions(int testId)
        {
            var questions = _context.QuestionModels.Where(x => x.TestId == testId);
            var test = _context.TestModels.FirstOrDefault(x => x.Id == testId);
            var model = new TestQuestionVm()
            {
                Id = testId,
                Title = test.Title,
                question = questions.ToList(),
            };
            return model;
        }

        public async Task<int> UpdateRange(List<QuestionModel> models)
        {
            _context.QuestionModels.UpdateRange(models);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Upsert(QuestionModel model)
        {
            if (model.Id == 0)
                await _context.QuestionModels.AddAsync(model);
            else
            {
                var question = await _context.QuestionModels.FindAsync(model.Id);
                if (question == null) throw new Exception("not found");
                else
                {
                    _context.Entry<QuestionModel>(question).State = EntityState.Detached;
                    _context.Entry<QuestionModel>(model).State = EntityState.Modified;
                }
            }
            return await _context.SaveChangesAsync();
        }
    }
}