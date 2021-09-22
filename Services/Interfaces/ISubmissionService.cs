using Smart_ELearning.Models;

namespace Smart_ELearning.Services.Interfaces
{
    public interface ISubmissionService
    {
        string GetIpAddress();

        int CheckFakeAddress();

        int IsDuplicate(int testId);

        int Delete(int id);

        int IsExpired(int testId);

        SubmitModel GetById(int id);
    }
}