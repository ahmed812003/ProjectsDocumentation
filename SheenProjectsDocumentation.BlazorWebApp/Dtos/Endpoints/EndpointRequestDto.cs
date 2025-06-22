using ProjectsDocumentation.BlazorWebApp.Enums;

namespace ProjectsDocumentation.BlazorWebApp.Dtos.Endpoints
{
    public class EndpointRequestDto
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }

        public string Query { get; set; }
    }
}
