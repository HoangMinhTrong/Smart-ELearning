using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smart_ELearning.Models;

namespace Smart_ELearning.Data.DataSeed
{
    public static class Seed
    {
        public static class ContextSeed
        {
            public static async Task SeedRolesAsync(UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager)
            {
                //Seed Roles
                await roleManager.CreateAsync(new IdentityRole(Roles.Teacher.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Student.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            }
        }

        public static void SeedData(this ModelBuilder modelBuilder)
        {
            // Identity
            PasswordHasher<AppUserModel> passwordHasher = new PasswordHasher<AppUserModel>();

            modelBuilder.Entity<AppUserModel>().HasData(
                new AppUserModel
                {
                    FullName = "Teacher",
                    UserName = "teacher@smartelearning.com",
                    NormalizedUserName = "TEACHERN@SMARTELEARNING.COM",
                    PasswordHash = passwordHasher.HashPassword(null, "Default@123"),
                    Email = "teacheradmin@smartelearning.com",
                    NormalizedEmail = "teacheradmin@smartelearning.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                }
            );
        }
    }
}