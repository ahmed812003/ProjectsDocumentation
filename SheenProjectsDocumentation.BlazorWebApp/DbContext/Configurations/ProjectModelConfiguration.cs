using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectsDocumentation.BlazorWebApp.Entities;

namespace ProjectsDocumentation.BlazorWebApp.DbContext.Configurations
{
    public class ProjectModelConfiguration : IEntityTypeConfiguration<ProjectModel>
    {
        public void Configure(EntityTypeBuilder<ProjectModel> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            
            builder.Property(e => e.ModelName)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.Property(e => e.ProjectId).IsRequired();
            
            builder.Property(e => e.ModelTypeId).IsRequired();

            // Foreign key to Project
            builder.HasOne(e => e.Project)
                .WithMany(p => p.ProjectModels)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Foreign key to LookupValue (ModelType)
            builder.HasOne(e => e.ModelType)
                .WithMany(lv => lv.ProjectModels)
                .HasForeignKey(e => e.ModelTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.ProjectId, e.ModelName, e.ModelTypeId }).IsUnique();

            builder.ToTable("ProjectModels");
        }
    }
}
