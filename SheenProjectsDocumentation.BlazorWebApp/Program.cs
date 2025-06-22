using ProjectsDocumentation.BlazorWebApp.Components;
using ProjectsDocumentation.BlazorWebApp.DbContext;
using ProjectsDocumentation.BlazorWebApp.Entities.Seeders;
using ProjectsDocumentation.BlazorWebApp.Extensions;
using ProjectsDocumentation.BlazorWebApp.Interfaces.Endpoints;
using ProjectsDocumentation.BlazorWebApp.Interfaces.Projects;
using ProjectsDocumentation.BlazorWebApp.Services.Endpoints;
using ProjectsDocumentation.BlazorWebApp.Services.Projects;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.RegisteredDbContext();


ServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
DataBaseContext dataBaseContext = serviceProvider.GetRequiredService<DataBaseContext>();
LookupTitleSeeder lookupTitleSeeder = new LookupTitleSeeder(dataBaseContext);
lookupTitleSeeder.SeedData();

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IEndpointService, EndpointService>();
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
