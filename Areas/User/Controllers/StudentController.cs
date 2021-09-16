using Microsoft.AspNetCore.Mvc;
using Smart_ELearning.Services.Interfaces;
using Smart_ELearning.ViewModels.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Areas.User.Controllers
{
    [Area("User")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public IActionResult AssignStudentToClass(int classId)
        {
            var model = new AssignStudentToClassRequest()
            {
                ClassId = classId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignStudentToClass(AssignStudentToClassRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _studentService.AssignStudentToClass(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentInClass(int classId)
        {
            var listStudents = await _studentService.GetStudentInClass(classId);
            return Json(new { data = listStudents });
        }
    }
}