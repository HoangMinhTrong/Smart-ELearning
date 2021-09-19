using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Smart_ELearning.Data;
using Smart_ELearning.Models;
using Smart_ELearning.Services;
using Smart_ELearning.Services.Interfaces;
using Smart_ELearning.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System.Threading.Tasks;
using Smart_ELearning.ViewModels.Test;

namespace Smart_ELearning.Areas.User.Controllers
{
    [Area("User")]
    public class TestController : Controller
    {
        private readonly ITestService _testService;
        private readonly IScheduleService _scheduleService;
        private readonly ApplicationDbContext _context;
        private readonly IQuestionService _questionService;

        public TestController(IQuestionService questionService, ITestService testService, IScheduleService scheduleService, ApplicationDbContext context)
        {
            _testService = testService;
            _scheduleService = scheduleService;
            _context = context;
            _questionService = questionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id, int? scheduleId)
        {
            TestViewModel testViewModel = new TestViewModel()
            {
                TestModel = new TestModel(),
                ScheduleListItems = _scheduleService.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Title,
                    Value = i.Id.ToString()
                })
            };
            if (id == null)
            {
                return View(testViewModel);
            }
            testViewModel.TestModel = _testService.GetById(id);
            return View(testViewModel);
        }

        public IActionResult CreateTestToSchedule(int scheduleId)
        {
            ViewBag.SchedulTitle = _scheduleService.GetById(scheduleId).Title;
            ViewBag.ScheduleId = scheduleId;
            var model = new TestModel()
            {
                ScheduleId = scheduleId,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTestToSchedule(TestModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _testService.CreateTestToSchedule(model);
            return RedirectToAction("AddRange", "Question", new { testId = model.Id, numberOfQuestion = model.NumberOfQuestion });
        }

        public IActionResult TestQuestion(int testId)
        {
            var data = _questionService.GetTestQuestions(testId);

            return View(data);
        }

        #region APICall

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(TestViewModel testViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(testViewModel);
            }

            var obj = _testService.Upsert(testViewModel);
            if (obj == 0)
            {
                return View(testViewModel);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _testService.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _testService.GetById(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _testService.Delete(id);
            return Json(new { success = true, message = "Delete Successful" });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] int id)
        {
            var ojbFromdb = _context.TestModels.FirstOrDefault(u => u.Id == id);
            if (ojbFromdb == null)
            {
                return Json(new { success = false, Message = "Error While Lock/Unlock" });
            }

            if (ojbFromdb.LockoutEnd != null && ojbFromdb.LockoutEnd > DateTime.Now)
            {
                ojbFromdb.LockoutEnd = DateTime.Now;
            }
            else
            {
                ojbFromdb.LockoutEnd = DateTime.Now.AddYears(1000);
            }

            _context.SaveChanges();
            return Json(new { success = true, Message = "Success" });
        }

        #endregion APICall

        public IActionResult TestForm(int testId)
        {
            var data = _testService.GetTestQuestion(testId);
            return View(data);
        }
        public IActionResult SubmitRecord(int recordid)
        {
            ViewBag.RecordId = recordid;
            var data = _testService.GetTestQuestion(recordid);
            return View(data);
        }
        
        [HttpPost]
        public async Task<IActionResult> TestForm(StudentTestVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
             await _testService.AddSubmitRecord(model);
            return RedirectToAction("SubmitRecord", "Test", new {});
        }
        [HttpPost]
        public async Task<IActionResult> SubmitRecord(StudentTestVm model)
        {
            return View();
        }
        
    }
}