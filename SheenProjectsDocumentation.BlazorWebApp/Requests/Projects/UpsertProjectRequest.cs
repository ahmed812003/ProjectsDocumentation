namespace ProjectsDocumentation.BlazorWebApp.Requests.Projects
{
    public class UpsertProjectRequest
    {
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public List<UpsertProjectModelRequest> ProjectModelsRequest { get; set; }
        public List<UpsertProjectTablesRequest> ProjectTablesRequest { get; set; }
    }
}
