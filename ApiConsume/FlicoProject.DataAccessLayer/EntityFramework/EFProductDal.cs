using FlicoProject.DataAccessLayer.Abstract;
using FlicoProject.DataAccessLayer.Concrete;
using FlicoProject.DataAccessLayer.Repositories;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DataAccessLayer.EntityFramework
{
    public class EFProductDal : GenericRepository<Product>, IProductDal
    {
        private readonly IDistributedCache _cache;

        public EFProductDal(Context context, IDistributedCache cache) : base(context)
        {
            _cache = cache;
        }

        public List<Product> GetList()
        {
            var cacheKey = "ProductList";
            var cachedList = _cache.GetString(cacheKey);

            if (!string.IsNullOrEmpty(cachedList))
            {
                Console.WriteLine("Data is fetched from cache.");
                return JsonConvert.DeserializeObject<List<Product>>(cachedList);
            }

            var list = base.GetList();

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            _cache.SetString(cacheKey, JsonConvert.SerializeObject(list), cacheOptions);
            Console.WriteLine("Data is fetched from database and cached.");

            return list;
        }
    }
}
