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
    public class SubjectService : ISubjectService
    {
        private readonly ApplicationDbContext _context;

        public SubjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Delete(int id)
        {
            var subject = await _context.SubjectModels.FindAsync(id);
            if (subject == null) throw new Exception("Cound not found");
            _context.SubjectModels.Remove(subject);
            return await _context.SaveChangesAsync();
        }

        public async Task<ICollection<SubjectModel>> GetAll()
        {
            var data = await _context.SubjectModels.ToListAsync();

            return data;
        }

        public async Task<SubjectModel> GetById(int id)
        {
            var model = await _context.SubjectModels.FindAsync(id);
            if (model == null) throw new Exception("Not found");
            return model;
        }

        public async Task<int> Upsert(SubjectModel model)
        {
            if (model.Id == 0)
            {
                _context.SubjectModels.Add(model);
            }
            else
            {
                var classFromDb = await _context.SubjectModels.FindAsync(model.Id);
                if (classFromDb == null) throw new Exception($"Could not found class id{model.Id}");
                else
                {
                    _context.Entry<SubjectModel>(classFromDb).State = EntityState.Detached;
                    _context.Entry<SubjectModel>(model).State = EntityState.Modified;
                }
            }
            return await _context.SaveChangesAsync();
        }
    }
}