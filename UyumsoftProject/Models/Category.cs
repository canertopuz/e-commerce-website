using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UyumsoftProject.Models
{
    public class Category
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        
        //[JsonIgnore]
        //public ICollection<Product> Products { get; set; }
    }
}
