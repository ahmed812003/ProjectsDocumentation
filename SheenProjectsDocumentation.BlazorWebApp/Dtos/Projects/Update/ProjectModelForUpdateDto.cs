using ProjectsDocumentation.BlazorWebApp.Enums;

namespace ProjectsDocumentation.BlazorWebApp.Dtos.Projects.Update
{
    public class ProjectModelForUpdateDto
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public ModelType ModelType { get; set; }
    }
}
