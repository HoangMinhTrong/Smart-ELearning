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
    public class SubmissionService : ISubmissionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubmissionService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public int CheckFakeAddress()
        {
            string userId = this.GetIpAddress();
            int isFake = 1;
            string info = new WebClient().DownloadString("https://v2.api.iphub.info/guest/ip/14.243.128.251" + "?c=Fae9gi8a");
            var ipInfo = JsonConvert.DeserializeObject<dynamic>(info);
            isFake = ipInfo.block;
            return isFake;
        }

        public string GetIpAddress()
        {
            string userIp = "unknow";
            try
            {
                userIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            catch (Exception ex)
            {
            }
            return userIp;
        }
    }
}