namespace Smart_ELearning.Services.Interfaces
{
    public interface ISubmissionService
    {
        string GetIpAddress();

        int CheckFakeAddress();

        int IsDuplicate(int testId);
    }
}