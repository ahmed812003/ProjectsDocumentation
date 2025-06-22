using Microsoft.EntityFrameworkCore;
using ProjectsDocumentation.BlazorWebApp.DbContext;

namespace ProjectsDocumentation.BlazorWebApp.Extensions
{
    public static class DIServiceConfiguration
    {
        public static IServiceCollection RegisteredDbContext(this IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            services.AddDbContext<DataBaseContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("DefaultConnectionString"),
                    ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnectionString"))
                ));
            return services;
        }
    }
}
