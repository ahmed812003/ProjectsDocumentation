using Microsoft.EntityFrameworkCore;
using ProjectsDocumentation.BlazorWebApp.Entities;

namespace ProjectsDocumentation.BlazorWebApp.DbContext
{
    public class DataBaseContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataBaseContext).Assembly);
        }

        public DbSet<LookupTitle> LookupTitles { get; set; }
        public DbSet<LookupValue> LookupValues { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectModel> ProjectModels { get; set; }
        public DbSet<ProjectTable> projectTables { get; set; }
        public DbSet<Entities.Endpoint> Endpoints { get; set; }
        public DbSet<EndpointVersion> EndpointVersions { get; set; }
        public DbSet<EndpointReflection> EndpointReflections { get; set; }
        public DbSet<EndpointResponse> EndpointResponses { get; set; }
        public DbSet<EndpointRequest> EndpointRequests { get; set; }
    }
}
