using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Data.Configurations
{
    public class StudentAttendanceConfiguration : IEntityTypeConfiguration<StudentAttendanceModel>
    {
        public void Configure(EntityTypeBuilder<StudentAttendanceModel> builder)
        {
            builder.ToTable("StudentAttendances");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.ScheduleModel).WithMany(x => x.StudentAttendanceModels).HasForeignKey(x => x.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}