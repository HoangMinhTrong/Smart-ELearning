using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Smart_ELearning.Data;
using Smart_ELearning.Models;
using Smart_ELearning.Services.Interfaces;

using Smart_ELearning.ViewModels;
using Smart_ELearning.ViewModels.ScheduleViewModels;

namespace Smart_ELearning.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAttendanceService _attendanceService;

        public ScheduleService(ApplicationDbContext context,
            IAttendanceService attendanceService)
        {
            _context = context;
            _attendanceService = attendanceService;
        }

        public List<ScheduleModel> GetAll()
        {
            var query = _context.ScheduleModels.Include(x => x.ClassModel).Include(x => x.SubjectModel).AsQueryable();

            var list = query.ToList();
            return list;
        }

        public int Upsert(ScheduleViewModel model)
        {
            if (model.ScheduleModel.Id == 0)
            {
                _context.ScheduleModels.Add(model.ScheduleModel);
            }
            else
            {
                var scheduleFromDb = _context.ScheduleModels.Find(model.ScheduleModel.Id);
                if (scheduleFromDb == null) throw new Exception($"Could not found class id{model.ScheduleModel.Id}");
                else
                {
                    _context.Entry<ScheduleModel>(scheduleFromDb).State = EntityState.Detached;
                    _context.Entry<ScheduleModel>(model.ScheduleModel).State = EntityState.Modified;
                }
            }

            var result = _context.SaveChanges();
            _attendanceService.GenerateAttendanceList(model.ScheduleModel.Id, model.ScheduleModel.ClassId);
            return result;
        }

        public bool Delete(int Id)
        {
            var scheduleFromDb = _context.ScheduleModels.Find(Id);
            if (scheduleFromDb == null) throw new Exception($"Could not found class id {Id}");
            _context.ScheduleModels.Remove(scheduleFromDb);
            _context.SaveChanges();
            return true;
        }

        ScheduleModel IScheduleService.GetById(int? Id)
        {
            var scheduleFromDb = _context.ScheduleModels.Find(Id);
            if (scheduleFromDb == null) throw new Exception($"Not Found");
            return scheduleFromDb;
        }

        public List<TestToScheduleViewModel> GetScheduleToTest(int scheduleid)
        {
            var obj = _context.TestModels.Where(x => x.ScheduleId == scheduleid).AsQueryable();
            var objlist = obj.Select(x => new TestToScheduleViewModel()
            {
                Id = x.Id,
                Title = x.Title,
                NumberOfQuestion = x.NumberOfQuestion,
                lockoutEnd =  x.LockoutEnd,
                Status = x.Status ? "Open" : "Close",

            });
            return objlist.ToList();
        }

        public List<ScheduleVM> GetDisplay()
        {
            var query = _context.ScheduleModels.Include(x => x.ClassModel).Include(x => x.SubjectModel).AsQueryable();
            var list = query.Select(x => new ScheduleVM()
            {
                Id = x.Id,
                DateTime = x.DateTime.ToString("dddd, dd MMMM yyyy"),
                StartTime = x.StartTime.ToString("HH:mm"),
                EndTime = x.EndTime.ToString("HH:mm"),
                ClassName = x.ClassModel.Name,
                SubjectName = x.SubjectModel.Name,
                Title = x.Title,
            }).ToList();

            return list;
        }

        public List<ScheduleVM> GetClassSchedule(int classId)
        {
            var list = new List<ScheduleVM>();
            var query = _context.ScheduleModels.Include(x => x.ClassModel)
                .Include(x => x.SubjectModel)
                .Where(x => x.ClassId == classId)
                .OrderBy(x=>x.DateTime)
                .ThenBy(x=>x.StartTime)
                .AsQueryable();

            foreach(var x in query)
            {
                var model = new ScheduleVM()
                {
                    Id = x.Id,
                    DateTime = x.DateTime.ToString("ddd, dd MMM yyyy"),
                    StartTime = x.StartTime.ToString("HH:mm"),
                    EndTime = x.EndTime.ToString("HH:mm"),
                    ClassName = x.ClassModel.Name,
                    SubjectName = x.SubjectModel.Name,
                    Title = x.Title,
                };
                list.Add(model);
            }    
            //var list = query.Select(x => new ScheduleVM()
            //{
            //    Id = x.Id,
            //    DateTime = x.DateTime.ToString("ddd, dd MMM yyyy"),
            //    StartTime = x.StartTime.ToString("HH:mm"),
            //    EndTime = x.EndTime.ToString("HH:mm"),
            //    ClassName = x.ClassModel.Name,
            //    SubjectName = x.SubjectModel.Name,
            //    Title = x.Title,
            //}).ToList();

            return list;
        }
    }
}