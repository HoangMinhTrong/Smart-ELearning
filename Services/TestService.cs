using Smart_ELearning.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Smart_ELearning.Models;

using Smart_ELearning.Data;
using Smart_ELearning.ViewModels;
using Smart_ELearning.ViewModels.Test;

namespace Smart_ELearning.Services
{
    public class TestService : ITestService
    {
        private readonly ApplicationDbContext _context;

        public TestService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<TestModel> GetAll()
        {
            var ojb = _context.TestModels.Include(x => x.ScheduleModel);
            var ojbtest = ojb.ToList();
            return ojbtest;
        }

        public int Upsert(TestViewModel model)
        {
            if (model.TestModel.Id == 0)
            {
                _context.TestModels.Add(model.TestModel);
            }
            else
            {
                var testFromDb = _context.TestModels.Find(model.TestModel.Id);
                if (testFromDb == null) throw new Exception($"Could not found class id{model.TestModel.Id}");
                else
                {
                    _context.Entry<TestModel>(testFromDb).State = EntityState.Detached;
                    _context.Entry<TestModel>(model.TestModel).State = EntityState.Modified;
                }
            }

            return _context.SaveChanges();
        }

        public bool Delete(int Id)
        {
            var testFromDb = _context.TestModels.Find(Id);
            if (testFromDb == null) throw new Exception($"Could not found class id {Id}");
            _context.TestModels.Remove(testFromDb);
            _context.SaveChanges();
            return true;
        }

        TestModel ITestService.GetById(int? Id)
        {
            var testFromDb = _context.TestModels.Find(Id);
            if (testFromDb == null) throw new Exception($"Not Found");
            return testFromDb;
        }

        public async Task<int> CreateTestToSchedule(TestModel model)
        {
            _context.TestModels.Add(model);
            return await _context.SaveChangesAsync();
        }

        public StudentTestVm GetTestQuestion(int testId)
        {
            var test = _context.TestModels.Find(testId);
            var questionQuery = _context.QuestionModels
                .Where(x => x.TestId == testId).AsQueryable();

            var listQuestion = questionQuery.Select(x => new StudentQuestionVm()
            {
                Id = x.Id,
                TestId = x.TestId,
                ChoiceA = x.ChoiceA,
                ChoiceB = x.ChoiceB,
                ChoiceC = x.ChoiceC,
                ChoiceD = x.ChoiceD,
                Content = x.Content,
                CorrectAnswer = x.CorrectAnswer,
            }).ToList();

            var model = new StudentTestVm();
            model.TestId = testId;
            model.TestTitle = test.Title;
            model.QuestionsResult = listQuestion;
            model.NumberOfQuestion = test.NumberOfQuestion;

            return model;
        }
    }
}