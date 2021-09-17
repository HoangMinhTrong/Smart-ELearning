using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Smart_ELearning.Data;
using Smart_ELearning.Models;
using Smart_ELearning.Services.Interfaces;

using Microsoft.AspNetCore.Identity;
using Smart_ELearning.ViewModels.AccountViewModels;

namespace Smart_ELearning.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public StudentService(
            UserManager<AppUserModel> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<int> AssignStudentToClass(AssignStudentToClassRequest request)
        {
            var account = new AppUserModel();

            var specificId = 0;
            var lastedStudentAccount = await _context.AppUserModels.OrderBy(x => x.SpecificId).LastOrDefaultAsync();
            if (lastedStudentAccount == null) specificId = 101;
            else specificId = lastedStudentAccount.SpecificId + 1;

            account.FullName = request.FullName;
            account.SpecificId = specificId;
            account.Email = String.Concat(request.FullName.Where(c => !Char.IsWhiteSpace(c))) + "@smartlearning.com";
            account.EmailConfirmed = true;
            account.PhoneNumberConfirmed = true;
            account.NormalizedUserName = request.FullName.ToUpper();
            account.UserName = String.Concat(request.FullName.Where(c => !Char.IsWhiteSpace(c))) + "@smartlearning.com";

            IdentityResult result = _userManager.CreateAsync(account, "Default@123").GetAwaiter().GetResult();
            if (!_roleManager.RoleExistsAsync("Student").Result)
            {
                var role = new IdentityRole()
                {
                    Name = "Student",
                };
                _roleManager.CreateAsync(role).GetAwaiter().GetResult();
            }

            var addRoleResult = await _userManager.AddToRoleAsync(account, "Student");
            await _context.SaveChangesAsync();
            // Tạo tạm cái role

            var studentInClass = new StudentInClassModel()
            {
                ClassId = request.ClassId,
                UserId = account.Id
            };
            await _context.StudentInClassModels.AddAsync(studentInClass);

            return await _context.SaveChangesAsync();
        }

        public async Task<ICollection<StudentInClassVM>> GetStudentInClass(int classId)
        {
            var query = _context.StudentInClassModels.Include(x => x.AppUserModel).Include(x => x.ClassModel)
                .Where(x => x.ClassId == classId).AsQueryable();

            var listStudent = await query.Select(x => new StudentInClassVM()
            {
                Id = x.AppUserModel.Id,
                SpecificId = "SL" + x.AppUserModel.SpecificId.ToString(),
                FullName = x.AppUserModel.FullName,
                Email = x.AppUserModel.Email
            }).ToListAsync();

            return listStudent;
        }

        public async Task<int> RemoveStudentInStudent(string studentId, int classId)
        {
            var studentInClass = await _context.StudentInClassModels
                .FirstOrDefaultAsync(x => x.ClassId == classId && x.UserId == studentId);
            if (studentInClass != null)
                _context.StudentInClassModels.Remove(studentInClass);

            return await _context.SaveChangesAsync();
        }
    }
}