using System.Collections.Generic;

namespace UyumsoftProject.Models
{
    public class CartViewModel
    {
        public Cart Cart { get; set; }
        public List<Address> Addresses { get; set; }
        public int SelectedAddressId { get; set; }
    }
}