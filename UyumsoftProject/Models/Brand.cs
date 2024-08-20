using System.ComponentModel.DataAnnotations;

namespace UyumsoftProject.Models
{
    public class Brand
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
