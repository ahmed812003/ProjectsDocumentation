using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectsDocumentation.BlazorWebApp.Entities;

namespace ProjectsDocumentation.BlazorWebApp.DbContext.Configurations
{
    public class LookupValueConfiguration : IEntityTypeConfiguration<LookupValue>
    {
        public void Configure(EntityTypeBuilder<LookupValue> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            
            builder.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.Property(e => e.LookupTitleId).IsRequired();

            // Foreign key to LookupTitle
            builder.HasOne(e => e.LookupTitle)
                .WithMany(lt => lt.LookupValues)
                .HasForeignKey(e => e.LookupTitleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.LookupTitleId, e.Value }).IsUnique();

            builder.ToTable("LookupValues");
        }
    }
}
