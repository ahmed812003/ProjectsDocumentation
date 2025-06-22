using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectsDocumentation.BlazorWebApp.Entities;

namespace ProjectsDocumentation.BlazorWebApp.DbContext.Configurations
{
    public class LookupTitleConfiguration : IEntityTypeConfiguration<LookupTitle>
    {
        public void Configure(EntityTypeBuilder<LookupTitle> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            
            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(e => e.Title).IsUnique();

            builder.ToTable("LookupTitles");
        }
    }
}
