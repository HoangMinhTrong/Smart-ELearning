
using Smart_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Smart_ELearning.Services.Interfaces
{
    public interface IScheduleService
    {
        List<ScheduleModel> GetAll();

        int Upsert(ScheduleModel model);

        bool Delete(int classId);

        ScheduleModel GetById(int? classId);
    }
}