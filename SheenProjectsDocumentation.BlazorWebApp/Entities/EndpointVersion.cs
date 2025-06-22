namespace ProjectsDocumentation.BlazorWebApp.Entities
{
    public class EndpointVersion
    {
        public int Id { get; set; }
        public int EndpointId { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }

        //navigation properties
        public virtual Endpoint Endpoint { get; set; }
        public virtual ICollection<EndpointReflection> EndpointReflections { get; set; }
        public virtual ICollection<EndpointResponse> EndpointResponses { get; set; }
        public virtual ICollection<EndpointRequest> EndpointRequests { get; set; }
    }
}
