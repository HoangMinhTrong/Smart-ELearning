using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Smart_ELearning.Data;
using Smart_ELearning.Models;
using Smart_ELearning.Services.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Smart_ELearning.Services
{

    public class ScheduleService : IScheduleService
    {
        private readonly ApplicationDbContext _context;

        public ScheduleService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<ScheduleModel> GetAll()
        {
            var ojb = _context.ScheduleModels;
            var ojbschedule = ojb.ToList();
            return ojbschedule;
        }

        public int Upsert(ScheduleModel model)
        {
            if (model.Id == 0)
            {
                _context.ScheduleModels.Add(model);
            }
            else
            {
                var scheduleFromDb =  _context.ScheduleModels.Find(model.Id);
                if (scheduleFromDb == null) throw new Exception($"Could not found class id{model.Id}");
                else
                {
                    _context.Entry<ScheduleModel>(scheduleFromDb).State = EntityState.Modified;
                    _context.Entry<ScheduleModel>(model).State = EntityState.Modified;
                }
            }

            return _context.SaveChanges();
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
        
    }
}