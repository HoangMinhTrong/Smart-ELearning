using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_ELearning.Models;
using Smart_ELearning.ViewModels;
using Smart_ELearning.ViewModels.Test;

namespace Smart_ELearning.Services.Interfaces
{
    public interface ITestService
    {
        List<TestModel> GetAll();

        int Upsert(TestViewModel testViewModel);

        bool Delete(int classId);

        TestModel GetById(int? classId);

        Task<int> CreateTestToSchedule(TestModel model);

        StudentTestVm GetTestQuestion(int testId);

        SubmitTestVM SubmitDeatilRecord(int submitid);

        Task<SubmitModel> AddSubmitRecord(StudentTestVm submitTestVm);

        List<SubmitDetailVm> GetSubmitDetail(int submitId);

        List<TestResult> GetTestResults(int testId);

        int ChangeTestStatus(int id);
    }
}