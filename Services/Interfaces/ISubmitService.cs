using Smart_ELearning.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Smart_ELearning.Services.Interfaces
{
    public interface IIpService
    {
        List<IpInfo> GetAll();

        List<IpInfo> GetWhiteList();

        List<IpInfo> GetBlockList();

        int ChangeStatus(int id);
    }
}