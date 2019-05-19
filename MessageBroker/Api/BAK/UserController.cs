using MessageShared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Web.Http;

namespace MessageBroker
{
    public class aUser
    { 
        public string UserName { get; set; }
        public string Password { get; set; } 
    }

    public class UserController : ApiController
    {
        static readonly svcLogService.svcLogServiceClient _log;
        static readonly mCacheService.mCacheServiceClient _cache;
        static UserController()
        {
            try
            {
                _log = LogProvider.init();
                _log.Write(Guid.NewGuid().ToString());
            }
            catch { }
            try
            {
                _cache = CacheFindProvider.init();
                //mCacheReply response = _cache.Find(new oCacheFind() { });
            }
            catch { }
        }


        Product[] products = new Product[]
        {
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
        };

        public IEnumerable<Product> GetAll()
        {
            _log.Write(Guid.NewGuid().ToString());
            return products;
        }

        public Product GetProductById(int id)
        {
            var product = products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return product;
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return products.Where(p => string.Equals(p.Category, category,
                    StringComparison.OrdinalIgnoreCase));
        }
    }
}
