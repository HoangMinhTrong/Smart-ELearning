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
using Smart_ELearning.ViewModels.Attendance;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Smart_ELearning.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "Teacher")]
    public class AttendanceController : Controller
    {
        private readonly ITestService _testService;
        private readonly IScheduleService _scheduleService;
        private readonly ApplicationDbContext _context;
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService, ITestService testService, IScheduleService scheduleService, ApplicationDbContext context)
        {
            _testService = testService;
            _scheduleService = scheduleService;
            _context = context;
            _attendanceService = attendanceService;
        }

        // public IActionResult Index()
        // {
        //     return View();
        // }
        //
        // public IActionResult Upsert(int? id, int? scheduleId)
        // {
        //     return View();
        // }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // public IActionResult Upsert(TestViewModel testViewModel)
        // {
        //     //if (!ModelState.IsValid)
        //     //{
        //     //    return View(testViewModel);
        //     //}
        //
        //     //var obj = _testService.Upsert(testViewModel);
        //     //if (obj == 0)
        //     //{
        //     //    return View(testViewModel);
        //     //}
        //     // return RedirectToAction(nameof(Index));
        // }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _testService.GetAll();
            return Json(new { data = allObj });
        }

        public IActionResult ScheduleAttendance(int scheduleId)
        {
            var data = _attendanceService.GetScheduleAttendance(scheduleId).ToList();
            var schedule = _scheduleService.GetById(scheduleId);
            ViewBag.ClassId = schedule.ClassId;
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleAttendance(List<ScheduleAttendanceVm> request)
        {
            await _attendanceService.ChangeAttendanceStatus(request);

            return RedirectToAction("ScheduleAttendance", "Attendance", new { scheduleId = request[0].ScheduleId });
        }

        public async Task<IActionResult> ClassAttendance(int classId)
        {
            var data = await _attendanceService.GetClassAttendace(classId);

            return View(data);
        }
    }
}