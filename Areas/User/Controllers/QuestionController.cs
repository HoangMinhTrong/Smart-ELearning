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
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
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
        public async Task<IActionResult> GetQuestionByTestId(int testId)
        {
            var data = await _questionService.GetTestQuestions(testId);

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
            return RedirectToAction(nameof(Index));
            //return RedirectToAction("StudentInClass", new { id = request.ClassId });
        }
    }
}