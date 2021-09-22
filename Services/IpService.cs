using Microsoft.EntityFrameworkCore;
using Smart_ELearning.Data;
using Smart_ELearning.Models;
using Smart_ELearning.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Services
{
    public class IpService : IIpService
    {
        private readonly ApplicationDbContext _context;

        public IpService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int ChangeStatus(int id)
        {
            var record = _context.IpInfos.Find(id);
            record.IsBlock = !record.IsBlock;
            _context.IpInfos.Update(record);
            return _context.SaveChanges();
        }

        public List<IpInfo> GetAll()
        {
            var data = _context.IpInfos.ToList();
            return data;
        }

        public List<IpInfo> GetBlockList()
        {
            var data = _context.IpInfos.Where(x => x.IsBlock == true);
            return data.ToList();
        }

        public List<IpInfo> GetWhiteList()
        {
            var data = _context.IpInfos.Where(x => x.IsBlock == false);
            return data.ToList();
        }
    }
}