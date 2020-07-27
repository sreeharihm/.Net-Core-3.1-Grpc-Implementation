using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService;

namespace CreditRatingApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly GrpcChannel channel;
        public ProductController() {
            channel =  GrpcChannel.ForAddress("https://localhost:60002");
        }
        [HttpGet]
        public List<Product> GetAll()
        {
            var client = new ProductCrud.ProductCrudClient(channel);
            return client.GetAll(new Empty()).Products.ToList();
        }
    }
}
