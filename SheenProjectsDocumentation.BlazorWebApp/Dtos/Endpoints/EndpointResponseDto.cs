using ProjectsDocumentation.BlazorWebApp.Enums;

namespace ProjectsDocumentation.BlazorWebApp.Dtos.Endpoints
{
    public class EndpointResponseDto
    {
        public StatusCode StatusCode { get; set; }
        public int Id { get; set; }
        public string Response { get; set; }
    }
}
