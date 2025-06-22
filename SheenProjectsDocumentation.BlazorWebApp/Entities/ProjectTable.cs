namespace ProjectsDocumentation.BlazorWebApp.Entities
{
    public class ProjectTable
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string TableName { get; set; }

        //navigation properties
        public virtual Project Project { get; set; }
        public virtual ICollection<EndpointReflection> EndpointReflections { get; set; }
    }
}
