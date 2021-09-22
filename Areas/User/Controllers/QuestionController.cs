using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_ELearning.Models;
using Smart_ELearning.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "Teacher")]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IScheduleService _scheduleService;
        private readonly ITestService _testService;

        public QuestionController(IQuestionService questionService,
            IScheduleService scheduleService,
            ITestService testService)

        {
            _questionService = questionService;
            _scheduleService = scheduleService;
            _testService = testService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            QuestionModel model = new QuestionModel();
            if (id == null)
            {
                return View(model);
            }
            var questionId = id.Value;
            model = await _questionService.GetById(questionId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(QuestionModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _questionService.Upsert(model);
            if (result == 0)
            {
                return View(model);
            }
            return RedirectToAction(nameof(Index));
            //return RedirectToAction("StudentInClass", new { id = request.ClassId });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _questionService.GetAll();
            return Ok(data);
        }

        [HttpGet]
        public IActionResult GetQuestionByTestId(int testId)
        {
            var data = _questionService.GetTestQuestions(testId);

            return Ok(data);
        }

        public IActionResult AddRange(int testId, int numberOfQuestion)

        {
            var models = new List<QuestionModel>();
            for (int i = 0; i < numberOfQuestion; i++)
            {
                var model = new QuestionModel() { TestId = testId };
                models.Add(model);
            }
            //ViewBag.TestId = testId;
            ViewBag.NumberOfQuestion = numberOfQuestion;
            //ViewBag.TestTile = testTitle;
            return View(models);
        }

        [HttpPost]
        public async Task<IActionResult> AddRange(List<QuestionModel> models)
        {
            var result = await _questionService.AddRange(models.ToList());
            var scheduleId = _testService.GetById(models[0].TestId).ScheduleId;

            return RedirectToAction("ScheduleToTest", "Schedule", new { id = scheduleId });
        }

        public IActionResult EditQuestion(int testId)
        {
            var data = _questionService.GetByTestId(testId);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> EditQuestion(List<QuestionModel> models)
        {
            var result = await _questionService.UpdateRange(models);
            var scheduleId = _testService.GetById(models[0].TestId).ScheduleId;
            return RedirectToAction("ScheduleToTest", "Schedule", new { id = scheduleId });
        }
    }
}