using ProjectsDocumentation.BlazorWebApp.Enums;

namespace ProjectsDocumentation.BlazorWebApp.Requests.Projects
{
    public class UpsertProjectModelRequest
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public ModelType ModelType { get; set; }
    }
}
