using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectsDocumentation.BlazorWebApp.Entities;

namespace ProjectsDocumentation.BlazorWebApp.DbContext.Configurations
{
    public class EndpointResponseConfiguration : IEntityTypeConfiguration<EndpointResponse>
    {
        public void Configure(EntityTypeBuilder<EndpointResponse> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.EndpointResponseData).HasColumnType("JSON");

            builder.Property(e => e.EndpointVersionId).IsRequired();

            builder.Property(e => e.StatusCodeId).IsRequired();

            // Foreign key to EndpointVersion
            builder.HasOne(e => e.EndpointVersion)
                .WithMany(ev => ev.EndpointResponses)
                .HasForeignKey(e => e.EndpointVersionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Foreign key to LookupValue (StatusCode)
            builder.HasOne(e => e.StatusCode)
                .WithMany(lv => lv.EndpointResponses)
                .HasForeignKey(e => e.StatusCodeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("EndpointResponses");
        }
    }
}
