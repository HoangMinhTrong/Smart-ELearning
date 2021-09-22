using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Smart_ELearning.Data.Configurations;
using Smart_ELearning.Data.DataSeed;
using Smart_ELearning.Models;

namespace Smart_ELearning.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUserModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new AppUserConfiguration());
            builder.ApplyConfiguration(new ClassConfiguration());
            builder.ApplyConfiguration(new QuestionConfiguration());
            builder.ApplyConfiguration(new StudentAttendanceConfiguration());
            builder.ApplyConfiguration(new StudentInClassConfiguration());
            builder.ApplyConfiguration(new SubjectConfiguration());
            builder.ApplyConfiguration(new SubmitConfiguration());
            builder.ApplyConfiguration(new TestConfiguration());
            builder.ApplyConfiguration(new ScheduleConfiguration());
            builder.ApplyConfiguration(new SubmitDetailConfiguration());

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
            //
        }

        public DbSet<AppUserModel> AppUserModels { get; set; }
        public DbSet<ClassModel> ClassModels { get; set; }
        public DbSet<QuestionModel> QuestionModels { get; set; }
        public DbSet<ScheduleModel> ScheduleModels { get; set; }
        public DbSet<StudentAttendanceModel> StudentAttendanceModels { get; set; }
        public DbSet<StudentInClassModel> StudentInClassModels { get; set; }
        public DbSet<SubjectModel> SubjectModels { get; set; }
        public DbSet<SubmitModel> submitModels { get; set; }
        public DbSet<TestModel> TestModels { get; set; }
        public DbSet<SubmitDetailModel> SubmitDetailModels { get; set; }
        public DbSet<IpInfo> IpInfos { get; set; }
    }
}