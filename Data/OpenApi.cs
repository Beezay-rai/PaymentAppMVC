using PayementMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace PayementMVC.Data
{
    public class OpenApi
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public VisibilityEnum Visibility { get; set; }
    }
}
