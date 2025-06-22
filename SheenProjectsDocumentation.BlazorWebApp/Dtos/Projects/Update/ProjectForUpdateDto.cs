namespace ProjectsDocumentation.BlazorWebApp.Dtos.Projects.Update
{
    public class ProjectForUpdateDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public List<ProjectModelForUpdateDto> ProjectModelForUpdateDtos { get; set; }
        public List<ProjectTableForUpdateDto> ProjectTableForUpdateDtos { get; set; }
    }
}
