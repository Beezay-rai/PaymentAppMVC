namespace PayementMVC.Models
{
    public class OpenApiModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public VisibilityEnum Visibility { get; set; }
    }



    public enum VisibilityEnum
    {
        Public,
        Private
    }
}
