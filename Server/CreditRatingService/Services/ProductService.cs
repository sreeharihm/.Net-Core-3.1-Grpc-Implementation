using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ProductService;

namespace CreditRatingService.Services
{
    public class ProductCrudService : ProductCrud.ProductCrudBase
    {
        private readonly ILogger<ProductCrudService> _logger;
        private readonly List<Product> _products = new List<Product>();
        private int _iCount=0;
        public ProductCrudService(ILogger<ProductCrudService> logger)
        {
            _logger = logger;
            _products.Add(new Product()
            {
                ProductId = _iCount++,
                Name ="KitKat",
                Amount=10,
                Brand="Nestle",
                Value=2.33f
            });
        }
        public override Task<ProductList> GetAll(Empty request, ServerCallContext context)
        {
            ProductList p1 = new ProductList();
            p1.Products.Add(_products);
            return Task.FromResult(p1);
        }
        public override Task<Product> GetById(ProductId request, ServerCallContext context)
        {
            var product = _products.Where(x=>x.ProductId.Equals(request)).FirstOrDefault();
            return Task.FromResult(product) ;
        }
        public override Task<Product> Create(Product request, ServerCallContext context)
        {
            request.ProductId = _iCount++;
            _products.Add(request);
            return Task.FromResult(request);
        }
        public override Task<Product> Update(Product request, ServerCallContext context)
        {
            var productToUpdate = (from p in _products where p.ProductId == request.ProductId select p).FirstOrDefault();
            if (productToUpdate != null)
            {
                productToUpdate.Name = request.Name;
                productToUpdate.Amount = request.Amount;
                productToUpdate.Brand = request.Brand;
                productToUpdate.Value = request.Value;
                return Task.FromResult(request);
            }
            return Task.FromException<Product>(new EntryPointNotFoundException());
        }
        public override Task<Empty> Delete(ProductId productId, ServerCallContext context)
        {
            var productToDelete = (from p in _products where p.ProductId == productId.ProductId_ select p).FirstOrDefault();
            if (productToDelete == null)
            {
                return Task.FromException<Empty>(new EntryPointNotFoundException());
            }
            _products.Remove(productToDelete);
            return Task.FromResult(new Empty());
        }
    }
}
