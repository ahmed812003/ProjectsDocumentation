using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ProjectsDocumentation.BlazorWebApp.DbContext.Configurations
{
    public class EndpointConfiguration : IEntityTypeConfiguration<Entities.Endpoint>
    {
        public void Configure(EntityTypeBuilder<Entities.Endpoint> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            
            builder.Property(e => e.EndpointUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(e => e.Name)
               .IsRequired()
               .HasMaxLength(255);

            builder.Property(e => e.ProjectModelId).IsRequired();
            
            builder.Property(e => e.MethodId).IsRequired();

            // Foreign key to ProjectModel
            builder.HasOne(e => e.ProjectModel)
                .WithMany(pm => pm.Endpoints)
                .HasForeignKey(e => e.ProjectModelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Foreign key to LookupValue (Method)
            builder.HasOne(e => e.Method)
                .WithMany(lv => lv.EndpointsAsMethod)
                .HasForeignKey(e => e.MethodId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.ProjectModelId, e.EndpointUrl, e.MethodId }).IsUnique();

            builder.ToTable("Endpoints");
        }
    }
}
