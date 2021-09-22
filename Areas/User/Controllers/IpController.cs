using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smart_ELearning.Data;
using Smart_ELearning.Models;
using Smart_ELearning.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Areas.User.Controllers
{
    [Area("User")]
    public class IpController : Controller
    {
        private readonly ITestService _testService;

        private readonly ApplicationDbContext _context;

        private readonly IIpService _ipService;
        private readonly UserManager<AppUserModel> _userManager;

        private readonly IAttendanceService _attendanceService;
        private readonly ISubmissionService _submissionService;

        public IpController(IQuestionService questionService,
            ITestService testService,
            UserManager<AppUserModel> userManager,
            IAttendanceService attendanceService,
            ISubmissionService submissionService,
            IIpService ipService
            )
        {
            _testService = testService;
            _ipService = ipService;

            _userManager = userManager;
            _attendanceService = attendanceService;
            _submissionService = submissionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _ipService.GetAll();
            return Json(new { data = data });
        }

        public IActionResult WhiteList()
        {
            return View();
        }
        public IActionResult BlackList()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetWhiteList()
        {
            var data = _ipService.GetWhiteList();
            return Json(new { data = data });
        }

        [HttpGet]
        public IActionResult GetBlackList()
        {
            var data = _ipService.GetBlockList();
            return Json(new { data = data });
        }

        [HttpPost]
        public IActionResult ChangeStatus(int id)
        {
            var result = _ipService.ChangeStatus(id);
            if (result == 0) return BadRequest("Cound not found");
            else return Json(new { success = true, message = "Change Successful" }); ;
        }
    }
}