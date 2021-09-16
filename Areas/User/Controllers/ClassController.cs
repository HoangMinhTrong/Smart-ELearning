using Microsoft.AspNetCore.Mvc;
using Smart_ELearning.Models;
using Smart_ELearning.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Areas.User.Controllers
{
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
    }
}