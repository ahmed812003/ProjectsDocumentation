using ProjectsDocumentation.BlazorWebApp.Enums;

namespace ProjectsDocumentation.BlazorWebApp.Entities
{
    public class EndpointRequest
    {
        public int Id { get; set; }
        public int EndpointVersionId { get; set; }
        public string Headers { get; set; } // JSON string to store headers  
        public string Body { get; set; } // JSON string to store request body  
        public string QueryParameters { get; set; } // JSON string to store query parameters  

        //navigation properties
        public virtual EndpointVersion EndpointVersion { get; set; }
    }
}
