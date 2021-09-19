using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Smart_ELearning.Data;
using Smart_ELearning.Models;
using Smart_ELearning.Services.Interfaces;
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

        public int ChangeAttendanceStatus(int attendanceId, bool isPresent)
        {
            var studentAttendance = _context.StudentAttendanceModels
                .Find(attendanceId);
            studentAttendance.IsPresent = isPresent;
            _context.StudentAttendanceModels.Update(studentAttendance);
            return _context.SaveChanges();

            //_context.Entry<StudentAttendanceModel>(studentAttendance).State = EntityState.Detached;
            //_context.Entry<StudentAttendanceModel>().State = EntityState.Modified;
        }

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
    }
}