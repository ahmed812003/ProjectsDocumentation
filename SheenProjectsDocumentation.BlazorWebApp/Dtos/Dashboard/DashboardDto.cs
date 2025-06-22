using ProjectsDocumentation.BlazorWebApp.Dtos.Projects.GetAll;

namespace ProjectsDocumentation.BlazorWebApp.Dtos.Dashboard
{
    public class DashboardDto
    {
        public int TotalNumberOfProjects { get; set; }
        public int TotalNumberOfModels { get; set; }
        public int TotalNumberOfTables { get; set; }
        public List<ProjectListDto> ProjectListDtos { get; set; }
    }
}
