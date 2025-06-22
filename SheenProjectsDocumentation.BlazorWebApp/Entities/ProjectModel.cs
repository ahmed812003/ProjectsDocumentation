using ProjectsDocumentation.BlazorWebApp.Enums;

namespace ProjectsDocumentation.BlazorWebApp.Entities
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ModelName { get; set; }
        public int ModelTypeId { get; set; }

        //navigation properties
        public virtual Project Project { get; set; }
        public virtual LookupValue ModelType { get; set; }
        public virtual ICollection<Endpoint> Endpoints { get; set; }
    }
}
