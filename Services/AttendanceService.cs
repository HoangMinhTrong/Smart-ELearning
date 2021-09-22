using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Smart_ELearning.Data;
using Smart_ELearning.Models;
using Smart_ELearning.Services.Interfaces;
using Smart_ELearning.ViewModels.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Smart_ELearning.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AttendanceService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> ChangeAttendanceStatus(List<ScheduleAttendanceVm> request)
        {
            foreach (var item in request)
            {
                var studentAttendance = await _context.StudentAttendanceModels
                .FindAsync(item.AttendanceId);
                if (item.Status == "Absent")
                {
                    if (studentAttendance.IsPresent == true) studentAttendance.IsPresent = false;
                }
                if (item.Status == "Present")
                {
                    if (studentAttendance.IsPresent == false) studentAttendance.IsPresent = true;
                }
                _context.Entry(studentAttendance).State = EntityState.Modified;
            }
            return await _context.SaveChangesAsync();

            //_context.Entry<StudentAttendanceModel>(studentAttendance).State = EntityState.Detached;
            //_context.Entry<StudentAttendanceModel>().State = EntityState.Modified;
        }

        public string CheckNumberOfSubmit(int scheduleId, string userId)
        {
            var noOfTest = _context.TestModels
                .Where(x => x.ScheduleId == scheduleId)
                .Count();

            var noOfSumitted = _context.submitModels
                .Include(x => x.TestModels)
                .Where(x => x.TestModels.ScheduleId == scheduleId)
                .Where(x => x.UserId == userId)
                .Count();

            return noOfSumitted.ToString() + "/" + noOfTest.ToString();
        }

        //public int CheckNumberOfSubmit(int scheduleId, string userId)
        //{
        //}

        public void GenerateAttendanceList(int scheduleId, int classId)
        {
            var studentIds = _context.StudentInClassModels.Where(x => x.ClassId == classId)
                .Select(x => x.UserId)
                .ToList();
            var studenntAttendance = new List<StudentAttendanceModel>();
            foreach (var studentId in studentIds)
            {
                var studentAttendance = new StudentAttendanceModel()
                {
                    UserId = studentId,
                    ScheduleId = scheduleId,
                    SubmitId = 0,
                    IsPresent = false,
                };
                studenntAttendance.Add(studentAttendance);
            }
            _context.StudentAttendanceModels.AddRange(studenntAttendance);
            _context.SaveChanges();
        }

        public StudentAttendanceModel GetById(int id)
        {
            var model = _context.StudentAttendanceModels.Find(id);

            return model;
        }

        public async Task<List<ClassAttendanceVm>> GetClassAttendace(int classId)

        {
            //var query = _context.ClassModels
            //    .Include(x => x.ScheduleModels)
            //    .ThenInclude(x => x.SubjectModel)
            //    .Where(x => x.Id == classId)
            //    .AsQueryable();
            var query = _context.ScheduleModels
                .Include(x => x.ClassModel)
                .Include(x => x.SubjectModel)
                .Where(x => x.ClassId == classId)
                .OrderBy(x => x.DateTime)
                .ThenBy(x => x.StartTime)
                .AsQueryable();

            //foreach(var item in query)
            //{
            //    var model = new ClassAttendanceVm()
            //    {
            //        ClassName = item.ScheduleModels.Select(x => x.ClassModel.Name).ToString(),
            //        ScheduleTile = item.ScheduleModels.Select(x => x.Title).ToString(),
            //        Subject = item.ScheduleModels.Select(x => x.SubjectModel.Name).ToString(),
            //        Date = item.ScheduleModels.Select(x => x.DateTime.Date).ToString(),
            //        Time = item.ScheduleModels.Select(x => x.StartTime.ToString("hh:mm")) + "-" + item.ScheduleModels.Select(x => x.EndTime.ToString("hh:mm")),
            //    };

            //}

            //var models = await query.Select(x => new ClassAttendanceVm()
            //{
            //    ClassName = x.ScheduleModels.Select(x => x.ClassModel.Name).ToString(),
            //    ScheduleTile = x.ScheduleModels.Select(x => x.Title).ToString(),
            //    Subject = x.ScheduleModels.Select(x => x.SubjectModel.Name).ToString(),
            //    Date = x.ScheduleModels.Select(x => x.DateTime.Date).ToString(),
            //    Time = x.ScheduleModels.Select(x => x.StartTime.ToString("hh:mm")) + "-" + x.ScheduleModels.Select(x => x.EndTime.ToString("hh:mm")),
            //}).ToListAsync();

            var models = await query.Select(x => new ClassAttendanceVm()
            {
                ScheduleId = x.Id,
                ClassName = x.ClassModel.Name,
                ScheduleTile = x.Title,
                Subject = x.SubjectModel.Name,
                Date = x.DateTime.Date.ToString(),
                Time = x.StartTime.ToString("hh:mm") + "-" + x.EndTime.ToString("hh:mm"),
                //Status = this.TakeAttendanceStatus(x.DateTime)
            }).ToListAsync();
            foreach (var model in models)
            {
                model.Status = this.TakeAttendanceStatus(DateTime.Parse(model.Date));
            }

            return models;
        }

        public List<ScheduleAttendanceVm> GetScheduleAttendance(int scheduleId)
        {
            var attendances = _context.StudentAttendanceModels.Where(x => x.ScheduleId == scheduleId);
            var scheduleDate = _context.ScheduleModels.First(x => x.Id == scheduleId).DateTime;
            var models = new List<ScheduleAttendanceVm>();
            foreach (var item in attendances)
            {
                var student = _context.AppUserModels.Find(item.UserId);
                var record = new ScheduleAttendanceVm()
                {
                    AttendanceId = item.Id,
                    ScheduleId = scheduleId,
                    StudentName = student.FullName,
                    SpecificId = "SL" + student.SpecificId.ToString(),
                    SubmitInRequire = this.CheckNumberOfSubmit(scheduleId, item.UserId),
                    IsPresent = item.IsPresent,
                };
                if (item.IsPresent == false)
                {
                    if (DateTime.Now.Date < scheduleDate.Date) record.Status = "Future";
                    else record.Status = "Absent";
                }
                else { record.Status = "Present"; };
                models.Add(record);
            }
            return models;
        }

        public int IsFulFillTest(int scheduleId, string userId)
        {
            var noOfTest = _context.TestModels
                .Where(x => x.ScheduleId == scheduleId)
                .Count();

            var noOfSumitted = _context.submitModels
                .Where(x => x.TestModels.ScheduleId == scheduleId)
                .Where(x => x.UserId == userId)
                .Count();

            if (noOfSumitted >= noOfTest)
            {
                var attendanceRecord = _context.StudentAttendanceModels
                    .Where(x => x.ScheduleId == scheduleId)
                    .Where(x => x.UserId == userId).First();
                attendanceRecord.IsPresent = true;

                _context.StudentAttendanceModels.Update(attendanceRecord);
                return _context.SaveChanges();
            }
            else
            {
                var attendanceRecord = _context.StudentAttendanceModels
                    .Where(x => x.ScheduleId == scheduleId)
                    .Where(x => x.UserId == userId).First();
                attendanceRecord.IsPresent = false;
                _context.StudentAttendanceModels.Update(attendanceRecord);
                return _context.SaveChanges();
            }
        }

        public string TakeAttendanceStatus(DateTime date)
        {
            if (DateTime.Now.Date < date.Date)
            {
                return "Future";
            }
            return "Avaliale";
        }
    }
}