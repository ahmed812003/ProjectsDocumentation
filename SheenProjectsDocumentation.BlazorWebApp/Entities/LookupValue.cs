namespace ProjectsDocumentation.BlazorWebApp.Entities
{
    public class LookupValue
    {
        public int Id { get; set; }
        public int LookupTitleId { get; set; }
        public string Value { get; set; }

        //navigation properties
        public virtual LookupTitle LookupTitle { get; set; }
        public virtual ICollection<ProjectModel> ProjectModels { get; set; }
        public virtual ICollection<Endpoint> EndpointsAsMethod { get; set; }
        public virtual ICollection<EndpointResponse> EndpointResponses { get; set; }

        public static LookupValue CreateLookupValueSeeder(int id, string value)
        {
            return new LookupValue
            {
                Id = id,
                Value = value,
            };
        }
    }
}
