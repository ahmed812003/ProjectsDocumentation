namespace ProjectsDocumentation.BlazorWebApp.Requests.Endpoints.AddEndpointVersion
{
    public class AddEndpointVersionRequest
    {
        public string Version { get; set; }
        public string Description { get; set; }
        public List<AddEnpointVersionResponseRequest> AddEnpointResponseRequests { get; set; }
        public AddEnpointVersionRequestRequest AddEnpointRequestRequest { get; set; }
        public List<int> ReflectionTableIds { get; set; }
    }
}
