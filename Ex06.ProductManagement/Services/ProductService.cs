using Ex06.ProductManagement.Models;
using Ex06.ProductManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex06.ProductManagement.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repository;
        private readonly HashSet<string> _allTags = new();
        private readonly SortedDictionary<string, List<Product>> _productsByCategory = new();

        public ProductService(ProductRepository repository)
        {
            _repository = repository;
        }

        public void AddProduct(Product product)
        {
            _repository.Add(product);

            foreach (var tag in product.Tags)
                _allTags.Add(tag);

            AddToCategory(product);
        }

        private void AddToCategory(Product product)
        {
            if (!_productsByCategory.TryGetValue(
                    product.Category, out var products))
            {
                products = new List<Product>();
                _productsByCategory.Add(product.Category, products);
            }

            products.Add(product);
        }
        private void RebuildAuxiliaryCollections()
        {
            _allTags.Clear();
            _productsByCategory.Clear();

            foreach (var p in _repository.GetAll())
            {
                foreach (var tag in p.Tags)
                {
                    _allTags.Add(tag);
                }
                AddToCategory(p);
            }
        }
        public bool UpdateProduct(Product product)
        {
            bool success = _repository.Update(product);
            if (success)
            {
                RebuildAuxiliaryCollections();
            }
            return success;
        }

        public bool DeleteProduct(string id)
        {
            bool success = _repository.Delete(id);
            if (success)
            {
                RebuildAuxiliaryCollections();
            }
            return success;
        }

        public bool CheckIdExists(string id)
        {
            return _repository.GetById(id) != null;
        }

        public Product? GetById(string id)
        {
            return _repository.GetById(id);
        }

        public IReadOnlyList<Product> GetAllProducts()
        {
            return _repository.GetAll();
        }

        public List<Product> SearchByName(string keyword)
        {
            return _repository.GetAll()
                .Where(p => p.ProductName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Product> FilterByCategory(string category)
        {
            return _repository.GetAll()
                .Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Product> FilterLowStock(int threshold = 5)
        {
            return _repository.GetAll()
                .Where(p => p.Quantity <= threshold)
                .ToList();
        }

        public List<Product> SortByPrice(bool ascending = true)
        {
            return ascending
                ? _repository.GetAll().OrderBy(p => p.Price).ToList()
                : _repository.GetAll().OrderByDescending(p => p.Price).ToList();
        }

        public decimal GetTotalInventoryValue()
        {
            return _repository.GetAll().Sum(p => p.Price * p.Quantity);
        }

        public void SeedSampleData()
        {
            var sampleList = new List<Product>
        {
            new() { ProductId = "P001", ProductName = "Laptop Dell Inspiron", Category = "Laptop", Price = 18500000m, Quantity = 10, Supplier = "Dell", Tags = new() { "laptop", "dell", "office" } },
            new() { ProductId = "P002", ProductName = "Laptop HP ProBook", Category = "Laptop", Price = 21000000m, Quantity = 5, Supplier = "HP", Tags = new() { "laptop", "hp", "business" } },
            new() { ProductId = "P003", ProductName = "Mouse Logitech M331", Category = "Mouse", Price = 450000m, Quantity = 20, Supplier = "Logitech", Tags = new() { "mouse", "silent", "wireless" } },
            new() { ProductId = "P004", ProductName = "Keyboard Logitech K120", Category = "Keyboard", Price = 250000m, Quantity = 30, Supplier = "Logitech", Tags = new() { "keyboard", "usb", "office" } },
            new() { ProductId = "P005", ProductName = "Monitor Dell 24", Category = "Monitor", Price = 4500000m, Quantity = 4, Supplier = "Dell", Tags = new() { "monitor", "ips", "fhd" } },
            new() { ProductId = "P006", ProductName = "Laptop Lenovo ThinkPad", Category = "Laptop", Price = 25000000m, Quantity = 3, Supplier = "Lenovo", Tags = new() { "laptop", "thinkpad", "workstation" } },
            new() { ProductId = "P007", ProductName = "Mouse Rapoo M100", Category = "Mouse", Price = 300000m, Quantity = 15, Supplier = "Rapoo", Tags = new() { "mouse", "bluetooth", "wireless" } },
            new() { ProductId = "P008", ProductName = "Keyboard Corsair K60", Category = "Keyboard", Price = 250000m, Quantity = 6, Supplier = "Corsair", Tags = new() { "keyboard", "mechanical", "gaming" } },
            new() { ProductId = "P009", ProductName = "Monitor LG 27", Category = "Monitor", Price = 6500000m, Quantity = 2, Supplier = "LG", Tags = new() { "monitor", "2k", "144hz" } },
            new() { ProductId = "P010", ProductName = "Webcam Logitech C920", Category = "Webcam", Price = 1800000m, Quantity = 8, Supplier = "Logitech", Tags = new() { "webcam", "fhd", "streaming" } }
        };

            foreach (var product in sampleList)
            {
                AddProduct(product);
            }
        }
        public IEnumerable<object> GetCategoryStatistics()
        {
            return _repository.GetAll()
                .GroupBy(p => p.Category, StringComparer.OrdinalIgnoreCase)
                .Select(g => new
                {
                    Category = g.Key,
                    ProductCount = g.Count(),
                    TotalQuantity = g.Sum(p => p.Quantity),
                    TotalValue = g.Sum(p => p.Price * p.Quantity)
                })
                .OrderBy(x => x.Category);
        }

        public SortedDictionary<string, List<Product>> GetProductsGroupedByCategory()
        {
            return _productsByCategory;
        }

        public IReadOnlyCollection<string> GetAllTags()
        {
            return _allTags;
        }

        public List<Product> FindProductsByTag(string tag)
        {
            return _repository.GetAll()
                .Where(p => p.Tags.Contains(tag))
                .ToList();
        }

        public int CountProductsByTag(string tag)
        {
            return _repository.GetAll()
                .Count(p => p.Tags.Contains(tag));
        }
    }
}
