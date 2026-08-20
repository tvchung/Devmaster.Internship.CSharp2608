using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex06.ProductManagement.Models
{
    public class Product
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Supplier { get; set; } = string.Empty;
        public HashSet<string> Tags { get; set; } = new();
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{ProductId,-8} | {ProductName,-25} | " +
                   $"{Category,-15} | {Price,12:N0} | " +
                   $"{Quantity,5} | {Supplier,-15}";
        }

    }
}
