using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectsDocumentation.BlazorWebApp.Entities;

namespace ProjectsDocumentation.BlazorWebApp.DbContext.Configurations
{
    public class EndpointRequestConfiguration : IEntityTypeConfiguration<EndpointRequest>
    {
        public void Configure(EntityTypeBuilder<EndpointRequest> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.EndpointVersionId).IsRequired();

            // JSON string properties for storing complex data
            builder.Property(e => e.Headers).HasColumnType("JSON");

            builder.Property(e => e.Body).HasColumnType("JSON");

            builder.Property(e => e.QueryParameters).HasColumnType("JSON");

            // Foreign key to EndpointVersion
            builder.HasOne(e => e.EndpointVersion)
                .WithMany(ev => ev.EndpointRequests)
                .HasForeignKey(e => e.EndpointVersionId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.ToTable("EndpointRequests");
        }
    }

}
