using ProjectsDocumentation.BlazorWebApp.Enums;

namespace ProjectsDocumentation.BlazorWebApp.Entities
{
    public class EndpointResponse
    {
        public int Id { get; set; }
        public int EndpointVersionId { get; set; }
        public int StatusCodeId { get; set; }
        public string EndpointResponseData { get; set; }

        //navigation properties
        public virtual EndpointVersion EndpointVersion { get; set; }
        public virtual LookupValue StatusCode { get; set; }
    }
}
