using System.Collections.Generic;
using AgriEnergyConnect.Models;

namespace AgriEnergyConnect.Models
{
    public class Farmer
    {
        public int FarmerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
