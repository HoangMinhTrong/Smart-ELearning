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
    public class ClassService : IClassService
    {
        private readonly ApplicationDbContext _context;

        public ClassService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Upsert(ClassModel model)
        {
            if (model.Id == 0)
            {
                _context.ClassModels.Add(model);
            }
            else
            {
                var classFromDb = await _context.ClassModels.FindAsync(model.Id);
                if (classFromDb == null) throw new Exception($"Could not found class id{model.Id}");
                else
                {
                    _context.Entry<ClassModel>(classFromDb).State = EntityState.Detached;
                    _context.Entry<ClassModel>(model).State = EntityState.Modified;
                }
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int classId)
        {
            var classFromDb = await _context.ClassModels.FindAsync(classId);
            if (classFromDb == null) throw new Exception($"Could not found class id{classId}");

            _context.ClassModels.Remove(classFromDb);

            return await _context.SaveChangesAsync();
        }

        public async Task<ICollection<ClassModel>> GetAll()
        {
            var query = _context.ClassModels;
            var classes = await query.ToListAsync();

            return classes;
        }

        public async Task<ClassModel> GetById(int? classId)
        {
            var classModel = await _context.ClassModels.FindAsync(classId);
            if (classModel == null) throw new Exception("Cound not found");

            return classModel;
        }
    }
}