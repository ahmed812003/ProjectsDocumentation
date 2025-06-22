namespace ProjectsDocumentation.BlazorWebApp.Entities
{
    public class EndpointReflection
    {
        public int Id { get; set; }
        public int EndpointVersionId { get; set; }
        public int ProjectTableId { get; set; }
        public virtual EndpointVersion EndpointVersion { get; set; }
        public virtual ProjectTable ProjectTable { get; set; }
    }
}
