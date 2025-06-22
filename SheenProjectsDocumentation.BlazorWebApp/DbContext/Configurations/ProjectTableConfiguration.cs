using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectsDocumentation.BlazorWebApp.Entities;

namespace ProjectsDocumentation.BlazorWebApp.DbContext.Configurations
{
    public class ProjectTableConfiguration : IEntityTypeConfiguration<ProjectTable>
    {
        public void Configure(EntityTypeBuilder<ProjectTable> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
           
            builder.Property(e => e.TableName)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.Property(e => e.ProjectId).IsRequired();

            // Foreign key to Project
            builder.HasOne(e => e.Project)
                .WithMany(p => p.ProjectTables)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.ProjectId, e.TableName }).IsUnique();

            builder.ToTable("ProjectTables");
        }
    }
}
