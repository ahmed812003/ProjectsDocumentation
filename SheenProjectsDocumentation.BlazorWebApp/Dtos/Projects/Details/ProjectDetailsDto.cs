namespace ProjectsDocumentation.BlazorWebApp.Dtos.Projects.Details
{
    public class ProjectDetailsDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public int TotalNumberOfTables { get; set; }
        public int TotalNumberOfModels { get; set; }
        public int TotalNumberOfEndPoints { get; set; }
        public List<ProjectModelsDetailsDto> ProjectModelsDtos { get; set; }
        public List<ProjectTablesDto> ProjectTables { get; set; }
    }
}
