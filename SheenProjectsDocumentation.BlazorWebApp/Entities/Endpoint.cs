using ProjectsDocumentation.BlazorWebApp.Enums;

namespace ProjectsDocumentation.BlazorWebApp.Entities
{
    public class Endpoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EndpointUrl { get; set; }
        public int ProjectModelId { get; set; }
        public int MethodId { get; set; }

        //navigation properties
        public virtual ProjectModel ProjectModel { get; set; }
        public virtual LookupValue Method { get; set; }
        public virtual ICollection<EndpointVersion> EndpointVersions { get; set; }
    }
}
