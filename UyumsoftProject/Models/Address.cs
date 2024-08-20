using System.ComponentModel.DataAnnotations;

namespace UyumsoftProject.Models
{
    public class Address
    {
        public int Id { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        public string UserId { get; set; }
        public string Name { get; set; }
        public string AddressDetail { get; set; }
        public int PostCode { get; set; }
    }
}
