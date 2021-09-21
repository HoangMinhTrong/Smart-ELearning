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
using System.Text.RegularExpressions;

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
            if (lastedStudentAccount == null || lastedStudentAccount.SpecificId < 100) specificId = 101;
            else specificId = lastedStudentAccount.SpecificId + 1;

            var fullNameToEmail = String.Concat(request.FullName.Where(c => !Char.IsWhiteSpace(c)));
            fullNameToEmail = fullNameToEmail.ToLower();
            fullNameToEmail = Regex.Replace(fullNameToEmail, "à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ|/g", "a");
            fullNameToEmail = Regex.Replace(fullNameToEmail, "è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ|/g", "e");
            fullNameToEmail = Regex.Replace(fullNameToEmail, "ì|í|ị|ỉ|ĩ|/g", "i");
            fullNameToEmail = Regex.Replace(fullNameToEmail, "ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ|/g", "o");
            fullNameToEmail = Regex.Replace(fullNameToEmail, "ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ|/g", "u");
            fullNameToEmail = Regex.Replace(fullNameToEmail, "ỳ|ý|ỵ|ỷ|ỹ|/g", "y");
            fullNameToEmail = Regex.Replace(fullNameToEmail, "đ", "d");
            account.FullName = request.FullName;
            account.SpecificId = specificId;
            account.Email = fullNameToEmail + specificId.ToString() + "@smartlearning.com";
            account.EmailConfirmed = true;
            account.PhoneNumberConfirmed = true;
            account.NormalizedUserName = request.FullName.ToUpper();
            account.UserName = fullNameToEmail + specificId.ToString() + "@smartlearning.com";

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
            //await _context.SaveChangesAsync();
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