using IDistributedCachRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IDistributedCachRedisApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions distributedCache = new DistributedCacheEntryOptions();
            distributedCache.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            _distributedCache.SetString("name", "ferhatcakmakoglu61", distributedCache);


            await _distributedCache.SetStringAsync("id", "210905024");
            return View();
        }

        public async Task<IActionResult> Show()
        {
            //string name = _distributedCache.GetString("name");
            //string id = await _distributedCache.GetStringAsync("id");


            Byte[] byteProduct = _distributedCache.Get("product:1");
            string jsonProduct = Encoding.UTF8.GetString(byteProduct);


            //string jsonProduct = _distributedCache.GetString("product:1");
            Product p = JsonSerializer.Deserialize<Product>(jsonProduct);
            //ViewBag.name = name;
            //ViewBag.id = id;
            ViewBag.product = p;
            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("name");
            return View();
        }

        public async Task<IActionResult> Complex()
        {
            DistributedCacheEntryOptions distributedCache = new DistributedCacheEntryOptions();
            distributedCache.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            Product product = new Product { Id = 1, Name = "Kalem", Price = 100 };
            string jsonProduct = JsonSerializer.Serialize(product);

            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
            _distributedCache.Set("product:1", byteProduct);
            //await _distributedCache.SetStringAsync("product:1", jsonProduct, distributedCache);

            return View();
        }

        public IActionResult ImageCach()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/trabzon.jpg");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("trabzon", imageByte);
            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] imageByte = _distributedCache.Get("trabzon");
            return File(imageByte, "image/jpg");
        }
    }
}
