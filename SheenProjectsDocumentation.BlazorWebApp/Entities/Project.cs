namespace ProjectsDocumentation.BlazorWebApp.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        //navigation properties
        public virtual ICollection<ProjectModel> ProjectModels { get; set; }
        public virtual ICollection<ProjectTable> ProjectTables { get; set; }
    }
}
