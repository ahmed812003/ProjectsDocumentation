using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectsDocumentation.BlazorWebApp.Entities;

namespace ProjectsDocumentation.BlazorWebApp.DbContext.Configurations
{
    public class EndpointVersionConfiguration : IEntityTypeConfiguration<EndpointVersion>
    {
        public void Configure(EntityTypeBuilder<EndpointVersion> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            
            builder.Property(e => e.Description).HasMaxLength(1000);
            
            builder.Property(e => e.Version)
                .IsRequired()
                .HasMaxLength(50);
            
            builder.Property(e => e.EndpointId).IsRequired();

            // Foreign key to Endpoint
            builder.HasOne(e => e.Endpoint)
                .WithMany(ep => ep.EndpointVersions)
                .HasForeignKey(e => e.EndpointId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.EndpointId, e.Version }).IsUnique();

            builder.ToTable("EndpointVersions");
        }
    }
}
