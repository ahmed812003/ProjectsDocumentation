namespace ProjectsDocumentation.BlazorWebApp.Requests.Endpoints.UpdateEndpointVersion
{
    public class UpdateEndpointVersionRequest
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public List<UpdateEnpointVersionResponseRequest> UpdateEnpointResponseRequests { get; set; }
        public UpdateEnpointVersionRequestRequest UpdateEnpointRequestRequest { get; set; }
        public List<int> ReflectionTableIds { get; set; }
    }
}
