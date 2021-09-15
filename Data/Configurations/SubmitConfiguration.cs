using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Data.Configurations
{
    public class SubmitConfiguration : IEntityTypeConfiguration<SubmitModel>
    {
        public void Configure(EntityTypeBuilder<SubmitModel> builder)
        {
            builder.ToTable("Submits");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.StudentAttendanceModel).WithOne(x => x.SubmitModel).HasForeignKey<StudentAttendanceModel>(x => x.SubmitId);
        }
    }
}