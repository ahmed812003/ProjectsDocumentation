using ProjectsDocumentation.BlazorWebApp.Dtos.Dashboard;
using ProjectsDocumentation.BlazorWebApp.Dtos.Projects.Details;
using ProjectsDocumentation.BlazorWebApp.Dtos.Projects.Update;
using ProjectsDocumentation.BlazorWebApp.Requests.Projects;

namespace ProjectsDocumentation.BlazorWebApp.Interfaces.Projects
{
    public interface IProjectService
    {
        public Task<DashboardDto> GetProjectListAsync();
        public Task<int> AddProjectAsync(UpsertProjectRequest request);
        Task<int> UpdateProjectAsync(int id, UpsertProjectRequest request);
        public Task<ProjectDetailsDto> GetProjectDetailsAsync(int Id);
        public Task<ProjectForUpdateDto> GetProjectByIdAsync(int id);
        public Task<List<ProjectTablesDto>> GetProjectReflectionTables(int projectId);
    }
}
