namespace ProjectsDocumentation.BlazorWebApp.Entities
{
    public class LookupTitle
    {
        public int Id { get; set; }
        public string Title { get; set; }

        //navigation properties
        public virtual ICollection<LookupValue> LookupValues { get; set; }

        public static LookupTitle CreateLookupTitleSeeder(int id, string title, List<LookupValue> lookupValues)
        {
            return new LookupTitle
            {
                Id = id,
                Title = title,
                LookupValues = lookupValues
            };
        }
    }
}
