using Smart_ELearning.Models;
using Smart_ELearning.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Smart_ELearning.Areas.User.Controllers
{
    [Area("User")]
    public class ClassController : Controller
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            ClassModel classModel = new ClassModel();
            if (id == null)
            {
                return View(classModel);
            }
            classModel = await _classService.GetById(id);
            return View(classModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ClassModel classModel)
        {
            if (!ModelState.IsValid)
            {
                return View(classModel);
            }
            var result = await _classService.Upsert(classModel);
            if (result == 0)
            {
                return View(classModel);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _classService.GetAll();
            return Ok(data);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int classId)
        {
            var result = await _classService.Delete(classId);
            if (result == 0) return BadRequest("Cound not found");
            else return Ok("Delete Success");
        }
    }
}