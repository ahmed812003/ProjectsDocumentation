using ProjectsDocumentation.BlazorWebApp.Enums;

namespace ProjectsDocumentation.BlazorWebApp.Dtos.Endpoints
{
    public class EndpointForUpdateDto
    {
        public int Id { get; set; }
        public Method Method { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
    }
}
