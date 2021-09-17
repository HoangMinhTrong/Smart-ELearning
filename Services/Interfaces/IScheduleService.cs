
using Smart_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_ELearning.ViewModels;

namespace Smart_ELearning.Services.Interfaces
{
    public interface IScheduleService
    {
        List<ScheduleModel> GetAll();

        int Upsert(ScheduleViewModel model);

        bool Delete(int classId);

        ScheduleModel GetById(int? classId);
    }
}