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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Smart_ELearning.Areas.User.Controllers
{
    [Area("User")]
    public class TestController : Controller
    {
        private readonly ITestService _testService;
        private readonly IScheduleService _scheduleService;
        private readonly ApplicationDbContext _context;
        private readonly IQuestionService _questionService;
        private readonly UserManager<AppUserModel> _userManager;
        private readonly IAttendanceService _attendanceService;
        private readonly ISubmissionService _submissionService;

        public TestController(IQuestionService questionService,
            ITestService testService,
            UserManager<AppUserModel> userManager,
            IAttendanceService attendanceService,
            ISubmissionService submissionService, IScheduleService scheduleService, ApplicationDbContext context)
        {
            _testService = testService;
            _scheduleService = scheduleService;
            _context = context;
            _questionService = questionService;
            _userManager = userManager;
            _attendanceService = attendanceService;
            _submissionService = submissionService;
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Teacher")]
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

        [Authorize(Roles = "Teacher")]
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

        [Authorize(Roles = "Teacher")]
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

        [Authorize(Roles = "Teacher")]
        public IActionResult TestQuestion(int testId)
        {
            var data = _questionService.GetTestQuestions(testId);

            return View(data);
        }

        #region APICall

        [Authorize(Roles = "Teacher")]
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

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _testService.GetAll();
            return Json(new { data = allObj });
        }

        [Authorize(Roles = "Teacher")]
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

        [Authorize(Roles = "Teacher")]
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

        [Authorize]
        public IActionResult TestForm(int testId)
        {
            var isExpired = _submissionService.IsExpired(testId);
            if (isExpired == 1)
            {
                TempData["DangerMessage"] = "Over time!!!!!!";
                return RedirectToAction("Index", "Home");
            }
            // Check IP here
            //var lockout = _context.TestModels.Find(testId);
            var ipResult = _submissionService.CheckFakeAddress();
            if (ipResult != 0)
            {
                TempData["DangerMessage"] = "Looks like you're using VPN. Turn it off to take the test!!!";
                return RedirectToAction("Index", "Home");
            }
            var isDuplicate = _submissionService.IsDuplicate(testId);
            if (isDuplicate == 1)
            {
                TempData["DangerMessage"] = "Your Have Submmited Before!!!";
                return RedirectToAction("Index", "Home");
            }

            //if (lockout.LockoutEnd > DateTime.Now)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            var studentIp = _submissionService.GetIpAddress();
            ViewBag.StudentIp = studentIp;
            var data = _testService.GetTestQuestion(testId);
            return View(data);
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult SubmitRecord(int id)
        {
            ViewBag.RecordId = id;
            var grade = _submissionService.GetById(id).TotalGrade;
            var data = _testService.GetSubmitDetail(id);
            ViewBag.TotalGrade = grade;
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> TestForm(StudentTestVm model)
        {
            var isExpired = _submissionService.IsExpired(model.TestId);
            if (isExpired == 1)
            {
                TempData["DangerMessage"] = "Over time!!!!!!";
                return RedirectToAction("Index", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var submit = await _testService.AddSubmitRecord(model);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _attendanceService.IsFulFillTest(model.ScheduleId, userId);
            //return RedirectToAction("SubmitRecord", "Test", new { id = submit.Id });
            TempData["SuccessMessage"] = "Successful Submission!!!!!!";

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public IActionResult GetTestResult(int id)
        {
            var data = _testService.GetTestResults(id);

            return Json(new { data = data });
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult TestResult(int id)
        {
            var test = _testService.GetById(id);
            ViewBag.TestId = id;
            return View(test);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public IActionResult ChangeTestStatus(int id)
        {
            var result = _testService.ChangeTestStatus(id);
            return Json(new { success = true, message = "Change Successful" });
        }
    }
}