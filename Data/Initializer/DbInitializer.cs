using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smart_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Data.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AppUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
            }
            // Role
            if (!_roleManager.RoleExistsAsync("Admin").Result)
            {
                var role = new IdentityRole()
                {
                    Name = "Admin",
                };
                _roleManager.CreateAsync(role).GetAwaiter().GetResult();
            }
            if (!_roleManager.RoleExistsAsync("Teacher").Result)
            {
                var role = new IdentityRole()
                {
                    Name = "Teacher"
                };
                _roleManager.CreateAsync(role).GetAwaiter().GetResult();
            };
            if (!_roleManager.RoleExistsAsync("Student").Result)
            {
                var role = new IdentityRole()
                {
                    Name = "Student"
                };
                _roleManager.CreateAsync(role).GetAwaiter().GetResult();
            };

            if (_userManager.FindByEmailAsync("admin@elearning.com").Result == null)
            {
                var user = new AppUserModel()
                {
                    Email = "admin@elearning.com",
                    EmailConfirmed = true,
                    UserName = "admin@elearning.com",
                };
                IdentityResult result = _userManager.CreateAsync(user, "Default@123").GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
                }
            }
            if (_userManager.FindByEmailAsync("teacher@elearning.com").Result == null)
            {
                var teacher_user = new AppUserModel()
                {
                    Email = "teacher@elearning.com",
                    EmailConfirmed = true,
                    UserName = "teacher@elearning.com",
                    SpecificId = 1
                };
                IdentityResult result = _userManager.CreateAsync(teacher_user, "Default@123").GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(teacher_user, "Teacher").GetAwaiter().GetResult();
                }
            }
        }
    }
}