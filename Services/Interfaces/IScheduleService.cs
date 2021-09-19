using Smart_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_ELearning.ViewModels;
using Smart_ELearning.ViewModels.ScheduleViewModels;

namespace Smart_ELearning.Services.Interfaces
{
    public interface IScheduleService
    {
        List<ScheduleModel> GetAll();

        List<ScheduleVM> GetClassSchedule(int classId);

        List<ScheduleVM> GetDisplay();

        int Upsert(ScheduleViewModel model);

        bool Delete(int classId);

        ScheduleModel GetById(int? classId);

        List<TestToScheduleViewModel> GetScheduleToTest(int scheduleid);
    }
}