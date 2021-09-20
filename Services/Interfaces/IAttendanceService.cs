using Smart_ELearning.Models;
using Smart_ELearning.ViewModels.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Services.Interfaces
{
    public interface IAttendanceService
    {
        void GenerateAttendanceList(int scheduleId, int classId);

        Task<int> ChangeAttendanceStatus(List<ScheduleAttendanceVm> request);

        string CheckNumberOfSubmit(int scheduleId, string userId);

        List<ScheduleAttendanceVm> GetScheduleAttendance(int scheduleId);

        Task<List<ClassAttendanceVm>> GetClassAttendace(int classId);

        //List<ClassAttendanceVm> GetClassAttendace(int classId);

        string TakeAttendanceStatus(DateTime date);

        StudentAttendanceModel GetById(int id);

        int IsFulFillTest(int scheduleId, string userId);
    }
}