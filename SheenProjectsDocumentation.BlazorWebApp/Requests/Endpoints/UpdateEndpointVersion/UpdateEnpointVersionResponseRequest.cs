using ProjectsDocumentation.BlazorWebApp.Enums;

namespace ProjectsDocumentation.BlazorWebApp.Requests.Endpoints.UpdateEndpointVersion
{
    public class UpdateEnpointVersionResponseRequest
    {
        public int Id { get; set; }
        public string Response { get; set; }
        public StatusCode StatusCode { get; set; }
    }
}
