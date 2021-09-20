using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    public class ClassService : IClassService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClassService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public int Upsert(ClassModel model)
        {
            if (model.Id == 0)
            {
                _context.ClassModels.Add(model);
            }
            else
            {
                var classFromDb = _context.ClassModels.Find(model.Id);
                if (classFromDb == null) throw new Exception($"Could not found class id{model.Id}");
                else
                {
                    _context.Entry<ClassModel>(classFromDb).State = EntityState.Detached;
                    _context.Entry<ClassModel>(model).State = EntityState.Modified;
                }
            }
            return _context.SaveChanges();
        }

        public async Task<int> Delete(int classId)
        {
            var classFromDb = await _context.ClassModels.FindAsync(classId);
            if (classFromDb == null) throw new Exception($"Could not found class id{classId}");

            _context.ClassModels.Remove(classFromDb);

            return await _context.SaveChangesAsync();
        }

        public List<ClassModel> GetAll()
        {
            var query = _context.ClassModels;
            var classes = query.ToList();
            //var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return classes;
        }

        public async Task<ClassModel> GetByIdAsync(int classId)
        {
            var classModel = await _context.ClassModels.FindAsync(classId);
            if (classModel == null) throw new Exception("Cound not found");

            return classModel;
        }

        public ClassModel GetById(int classId)
        {
            return _context.ClassModels.Find(classId);
        }
    }
}