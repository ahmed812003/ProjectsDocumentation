using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectsDocumentation.BlazorWebApp.Entities;

namespace ProjectsDocumentation.BlazorWebApp.DbContext.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.ProjectName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.Description)
                .IsRequired();

            builder.HasIndex(e => e.ProjectName).IsUnique();

            builder.ToTable("Projects");

        }
    }
}
