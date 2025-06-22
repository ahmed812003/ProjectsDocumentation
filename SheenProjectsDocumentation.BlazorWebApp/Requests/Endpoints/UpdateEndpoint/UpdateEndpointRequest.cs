using ProjectsDocumentation.BlazorWebApp.Enums;

namespace ProjectsDocumentation.BlazorWebApp.Requests.Endpoints.UpdateEndpoint
{
    public class UpdateEndpointRequest
    {
        public int Id { get; set; }
        public string EndpointUrl { get; set; }

        public Method Method { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
