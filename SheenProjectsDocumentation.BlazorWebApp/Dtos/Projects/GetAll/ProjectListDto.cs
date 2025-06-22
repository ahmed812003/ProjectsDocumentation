namespace ProjectsDocumentation.BlazorWebApp.Dtos.Projects.GetAll
{
    public class ProjectListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalNumberOfModels { get; set; }
        public int TotalNumberOfTables { get; set; }
    }
}
