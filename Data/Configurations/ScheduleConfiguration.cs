using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Data.Configurations
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<ScheduleModel>
    {
        public void Configure(EntityTypeBuilder<ScheduleModel> builder)
        {
            builder.ToTable("Schedules");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.ClassModel).WithMany(x => x.ScheduleModels).HasForeignKey(x => x.ClassId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.SubjectModel).WithMany(x => x.ScheduleModels).HasForeignKey(x => x.SubjectId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}