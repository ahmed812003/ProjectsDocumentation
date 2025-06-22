namespace ProjectsDocumentation.BlazorWebApp.Requests.Endpoints.UpdateEndpointVersion
{
    public class UpdateEnpointVersionRequestRequest
    {
        public int Id { get; set; }
        public string Header { get; set; }

        public string Body { get; set; }

        public string QuerParameters { get; set; }
    }
}
