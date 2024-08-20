using System.Collections.Generic;

namespace UyumsoftProject.Models
{
    public class ProductViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public int? SelectedBrandId { get; set; }
        public List<int> SelectedBrandIds { get; set; } = new List<int>();
        public int? SelectedCategoryId { get; set; }
        public List<int> SelectedCategoryIds { get; set; } = new List<int>();
        public string SearchTerm { get; set; }
    }
}