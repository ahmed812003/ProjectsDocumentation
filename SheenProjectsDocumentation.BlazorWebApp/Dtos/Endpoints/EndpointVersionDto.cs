using ProjectsDocumentation.BlazorWebApp.Dtos.Projects.Details;

namespace ProjectsDocumentation.BlazorWebApp.Dtos.Endpoints
{
    public class EndpointVersionDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public List<ReflectionTableDto> ReflectionTables { get; set; }
        public List<EndpointResponseDto> EndpointResponseDtos { get; set; }
        public List<EndpointRequestDto> EndpointRequestDtos { get; set; }
    }
}
