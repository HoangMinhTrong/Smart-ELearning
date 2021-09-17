using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Smart_ELearning.Data;
using Smart_ELearning.Models;
using Smart_ELearning.Services;
using Smart_ELearning.Services.Interfaces;
using Smart_ELearning.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Smart_ELearning.Areas.User.Controllers
{
    [Area("User")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _schedule;
        private readonly ISubjectService _subject;
        private readonly IClassService _classService;

        public ScheduleController(IScheduleService schedule, IClassService classService, ISubjectService subject)
        {
            _schedule = schedule;
            _classService = classService;
            _subject = subject;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ScheduleViewModel scheduleViewModel = new ScheduleViewModel
            {
                ScheduleModel = new ScheduleModel()
                {
                    DateTime = DateTime.Now.Date,
                    
                },
                ClassListItems = _classService.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                SubjectListItems = _subject.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                return View(scheduleViewModel);
            }
            scheduleViewModel.ScheduleModel = _schedule.GetById(id);
            return View(scheduleViewModel);
        }

        public IActionResult ScheduleToTest(int id)
        {
            var classFromDb =  _schedule.GetById(id);
            ViewBag.ScheduleId = classFromDb.Id;
            ViewBag.ScheduleName = classFromDb.Title;
            return View();
        }
        #region APICall
        [HttpGet]
        public IActionResult GetScheduleToTest(int id)
        {
            var obj = _schedule.GetScheduleToTest(id);
            return Json(new { data = obj });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ScheduleViewModel scheduleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(scheduleViewModel);
            }
            var obj = _schedule.Upsert(scheduleViewModel);
            if (obj == 0)
            {
                return View(scheduleViewModel);
            }
            return RedirectToAction(nameof(Index));
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _schedule.GetAll();
            return Json(new { data = allObj });
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult GetDisplay()
        {
            var allObj = _schedule.GetDisplay();
            return Json(new { data = allObj });
        }

        [Microsoft.AspNetCore.Mvc.HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _schedule.GetById(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _schedule.Delete(id);
            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion APICall
    }
}