using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Smart_ELearning.Data;
using Smart_ELearning.Models;
using Smart_ELearning.Services;
using Smart_ELearning.Services.Interfaces;

namespace Smart_ELearning.Areas.User.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IScheduleService _schedule;

        public ScheduleController(ApplicationDbContext context, IScheduleService schedule)
        {
            _context = context;
            _schedule = schedule;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ScheduleModel scheduleModel = new ScheduleModel();
            if (id == null)
            {
                return View(scheduleModel);
            }
            scheduleModel = _schedule.GetById(id);
            if (scheduleModel == null)
            {
                return NotFound();
            }
            return View(scheduleModel);
        }

        #region APICall

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ScheduleModel scheduleModel)
        {
            if (ModelState.IsValid)
            {
                if (scheduleModel.Id == 0)
                {
                    _context.ScheduleModels.Add(scheduleModel);
                }
                else
                {
                    _context.ScheduleModels.Update(scheduleModel);
                }

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(scheduleModel);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _schedule.GetAll();
            return Json(new { data = allObj });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _schedule.GetById(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _context.ScheduleModels.Remove(objFromDb);
            _context.SaveChanges();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}