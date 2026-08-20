using Ex06.ProductManagement.Interfaces;
using Ex06.ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex06.ProductManagement.Repositories
{
    public class ProductRepository:IRepository<Product>
    {
        private readonly List<Product> _products = new();
        private readonly Dictionary<string, Product> _productById = new();
        public void Add(Product entity)
        {
            if (_productById.ContainsKey(entity.ProductId))
                throw new InvalidOperationException(
                    $"Mã sản phẩm {entity.ProductId} đã tồn tại.");

            _products.Add(entity);
            _productById.Add(entity.ProductId, entity);
        }

        public bool Update(Product entity)
        {
            if (!_productById.TryGetValue(entity.ProductId, out var existing))
                return false;

            existing.ProductName = entity.ProductName;
            existing.Category = entity.Category;
            existing.Price = entity.Price;
            existing.Quantity = entity.Quantity;
            existing.Supplier = entity.Supplier;
            existing.Tags = entity.Tags;
            return true;
        }

        public bool Delete(string id)
        {
            if (!_productById.TryGetValue(id, out var product))
                return false;

            _products.Remove(product);
            _productById.Remove(id);
            return true;
        }

        public Product? GetById(string id)
        {
            _productById.TryGetValue(id, out var product);
            return product;
        }

        public IReadOnlyList<Product> GetAll()
        {
            return _products.AsReadOnly();
        }

    }
}
