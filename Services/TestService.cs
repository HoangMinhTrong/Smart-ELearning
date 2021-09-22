using Smart_ELearning.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Smart_ELearning.Models;

using Smart_ELearning.Data;
using Smart_ELearning.Models.Enums;
using Smart_ELearning.ViewModels;
using Smart_ELearning.ViewModels.Test;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Smart_ELearning.Services
{
    public class TestService : ITestService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ISubmissionService _submissionService;

        public TestService(ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            ISubmissionService submissionService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _submissionService = submissionService;
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

        public TestModel GetById(int? Id)
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
            var rnd = new Random();
            var listQuestion = questionQuery.Select(x => new StudentQuestionVm()
            {
                Id = x.Id,
                Score = x.Score,
                TestId = x.TestId,
                ChoiceA = x.ChoiceA,
                ChoiceB = x.ChoiceB,
                ChoiceC = x.ChoiceC,
                ChoiceD = x.ChoiceD,
                Content = x.Content,
                CorrectAnswer = x.CorrectAnswer,
                StudentAnswer = null
            }).ToList();
            var questionData = listQuestion.OrderBy(x => rnd.Next());

            var model = new StudentTestVm();
            model.ScheduleId = test.ScheduleId;
            model.TestId = testId;
            model.TestTitle = test.Title;
            model.QuestionsResult = questionData.ToList();
            model.NumberOfQuestion = test.NumberOfQuestion;

            return model;
        }

        public SubmitTestVM SubmitDeatilRecord(int submitid)
        {
            var test = _context.TestModels.Find(submitid);
            var submit = _context.submitModels.Find(submitid);
            var model = new SubmitTestVM();
            model.TestId = submit.TestId;
            model.TestTitle = test.Title;
            model.TotalGrade = submit.TotalGrade;
            model.NumberOfQuestion = test.NumberOfQuestion;
            model.CorrectAnswer = model.CorrectAnswer;
            model.StudentAnswer = model.StudentAnswer;
            return model;
        }

        public async Task<SubmitModel> AddSubmitRecord(StudentTestVm request)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var questionScore = request.QuestionsResult.First().Score;
            var noOfCorrect = 0;
            // Get User IP

            foreach (var item in request.QuestionsResult)
            {
                if (item.StudentAnswer != null)
                {
                    if (item.StudentAnswer == item.CorrectAnswer) noOfCorrect++;
                }
            }

            var objsub = new SubmitModel()
            {
                NumberOfCorrectAnswer = noOfCorrect,
                TestId = request.TestId,
                TotalGrade = noOfCorrect * questionScore,
                UserId = userId,
                UserIp = request.StudentIp
            };

            //if (objsub.NumberOfCorrectAnswer == (int)objquest.CorrectAnswer)
            //{
            //    var objsubNumberOfCorrectAnswer = objsub.NumberOfCorrectAnswer + 1;
            //}
            await _context.submitModels.AddAsync(objsub);
            await _context.SaveChangesAsync();
            foreach (var item in request.QuestionsResult)
            {
                var submitDetail = new SubmitDetailModel()
                {
                    QuestionId = item.Id,
                    StudentAnswer = item.StudentAnswer.HasValue ? item.StudentAnswer.Value : AnswerChoice.Null,
                    SubmitId = objsub.Id,
                };
                _context.SubmitDetailModels.Add(submitDetail);
            }
            await _context.SaveChangesAsync();
            var submitId = objsub.Id;
            return objsub;
        }

        public List<SubmitDetailVm> GetSubmitDetail(int submitId)
        {
            var models = new List<SubmitDetailVm>();
            var submit = _context.submitModels.Find(submitId);
            var submitDetails = _context.SubmitDetailModels.Where(x => x.SubmitId == submitId).ToList();
            var questions = _context.QuestionModels.Where(x => x.TestId == submit.TestId).ToList();

            foreach (var question in questions)
            {
                foreach (var submitDetail in submitDetails)
                {
                    if (submitDetail.QuestionId == question.Id)
                    {
                        var model = new SubmitDetailVm()
                        {
                            QuestionContent = question.Content,
                            ChoiceA = question.ChoiceA,
                            ChoiceB = question.ChoiceB,
                            ChoiceC = question.ChoiceC,
                            ChoiceD = question.ChoiceD,
                            CorrectAnswer = question.CorrectAnswer,
                            StudentAnswer = submitDetail.StudentAnswer
                        };
                        models.Add(model);
                    }
                }
            }
            return models.ToList();
        }

        public List<TestResult> GetTestResults(int testId)
        {
            var result = new List<TestResult>();
            var test = _context.TestModels.Find(testId);
            var query = _context.submitModels.Where(x => x.TestId == testId).ToList();
            foreach (var item in query)
            {
                var student = _context.AppUserModels.Find(item.UserId);
                var model = new TestResult()
                {
                    Id = item.Id,
                    SpecificId = "SL" + student.SpecificId.ToString(),
                    NoOfCorrect = item.NumberOfCorrectAnswer.ToString() + "/" + test.NumberOfQuestion.ToString(),
                    StudentName = student.FullName,
                    TotalGrade = item.TotalGrade,
                    UserId = item.UserId,
                    IpAdress = item.UserIp,
                };
                result.Add(model);
            }
            return result;
        }

        public int ChangeTestStatus(int id)
        {
            var test = this.GetById(id);
            if (test.Status == true)
                test.Status = false;
            else
                test.Status = true;
            _context.Entry(test).State = EntityState.Modified;
            return _context.SaveChanges();
        }
    }
}