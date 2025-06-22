using ProjectsDocumentation.BlazorWebApp.Enums;

namespace ProjectsDocumentation.BlazorWebApp.Requests.Endpoints.AddEndpoint
{
    public class AddEndpointRequest
    {
        public string EndpointUrl { get; set; }

        public Method Method { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
