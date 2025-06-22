using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectsDocumentation.BlazorWebApp.Entities;

namespace ProjectsDocumentation.BlazorWebApp.DbContext.Configurations
{
    public class EndpointReflectionConfiguration : IEntityTypeConfiguration<EndpointReflection>
    {
        public void Configure(EntityTypeBuilder<EndpointReflection> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.EndpointVersionId).IsRequired();

            builder.Property(e => e.ProjectTableId).IsRequired();

            // Foreign key to EndpointVersion
            builder.HasOne(e => e.EndpointVersion)
                .WithMany(ev => ev.EndpointReflections)
                .HasForeignKey(e => e.EndpointVersionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Foreign key to ProjectTable
            builder.HasOne(e => e.ProjectTable)
                .WithMany(pt => pt.EndpointReflections)
                .HasForeignKey(e => e.ProjectTableId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.EndpointVersionId, e.ProjectTableId }).IsUnique();

            builder.ToTable("EndpointReflections");
        }
    }

}
