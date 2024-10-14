using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            /*
                memory de "zaman" adinda data var mi kontrol ediyoruz.
                TryGetValue ile kontrol et, eger zaman yoksa zamanCache olarak olustur
             */
            
            if(!_memoryCache.TryGetValue("zaman", out string zamanCache))
            {
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions();
                /*
                 *  Bu sekilde oldugunda 10 saniye icinde dataya erismeye calısırsak
                 *  dataya erisim saglayabilecegiz, fakat bunu max 1 dk icinde yapabilirz
                 *  cunku absoluteExpiration'un suresi 1dk 
                 *  
                 *  Not: SlidingExpiration u yalniz kullanabilirsin
                 */


                cacheOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
                cacheOptions.SlidingExpiration = TimeSpan.FromSeconds(10);

                _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), cacheOptions);
            }
            return View();
        }

        public IActionResult Show()
        {
            //_memoryCache.Remove("zaman");

            /*
             *  zaman'i almaya calis, yoksa olustur (datetime.now ile)
                _memoryCache.GetOrCreate("zaman", option =>
                {
                    return DateTime.Now.ToString();
                });
            */
            ViewBag.zaman = _memoryCache.Get<string>("zaman");
            return View();
        }
    }
}
