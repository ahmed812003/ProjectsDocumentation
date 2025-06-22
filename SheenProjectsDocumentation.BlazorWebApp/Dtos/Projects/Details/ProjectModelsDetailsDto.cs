using ProjectsDocumentation.BlazorWebApp.Enums;

namespace ProjectsDocumentation.BlazorWebApp.Dtos.Projects.Details
{
    public class ProjectModelsDetailsDto
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public ModelType ModelType { get; set; }
    }
}
