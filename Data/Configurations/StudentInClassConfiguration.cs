using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Data.Configurations
{
    public class StudentInClassConfiguration : IEntityTypeConfiguration<StudentInClassModel>
    {
        public void Configure(EntityTypeBuilder<StudentInClassModel> builder)
        {
            builder.ToTable("StudentInClasses");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.ClassModel).WithMany(x => x.StudentInClassModels).HasForeignKey(x => x.ClassId);
            builder.HasOne(x => x.AppUserModel).WithMany(x => x.StudentInClassModels).HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}