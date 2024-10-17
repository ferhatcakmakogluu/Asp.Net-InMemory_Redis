using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

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
            string name = _distributedCache.GetString("name");
            string id = await _distributedCache.GetStringAsync("id");
            ViewBag.name = name;
            ViewBag.id = id;
            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("name");
            return View();
        }
    }
}
