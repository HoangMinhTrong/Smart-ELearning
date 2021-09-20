using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Teacher")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IClassService _classService;

        public StudentController(IStudentService studentService, IClassService classService)
        {
            _studentService = studentService;
            _classService = classService;
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
            return RedirectToAction("StudentInClass", new { id = request.ClassId });
        }

        public async Task<IActionResult> StudentInClass(int? id)
        {
            var classId = id.Value;
            var classFromDb = await _classService.GetByIdAsync(classId);
            ViewBag.ClassId = classFromDb.Id;
            ViewBag.ClassName = classFromDb.Name;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentInClass(int id)
        {
            var listStudents = await _studentService.GetStudentInClass(id);
            return Json(new { data = listStudents });
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveStudentInClass(string studentId, int classId)
        {
            var result = await _studentService.RemoveStudentInStudent(studentId, classId);
            if (result == 0)
                return BadRequest("Cound not found");

            return Json(new { success = true, message = "Delete Successful" });
        }
    }
}