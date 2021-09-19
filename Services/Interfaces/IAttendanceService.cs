using Smart_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Services.Interfaces
{
    public interface IAttendanceService
    {
        void GenerateAttendanceList(int scheduleId, int classId);

        int ChangeAttendanceStatus(int attendanceId, bool isPresent);

        //public Task<AttendanceVm> GetScheduleAttendance(int scheduleId);
    }
}