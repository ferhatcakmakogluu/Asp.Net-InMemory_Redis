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

                /*
                 * CacheItemPriority ile 4 tane oncelik geliyor
                 * Oncelik siralamasina gore -> NeverRemove > High > Normal > Low
                 * 
                 * NeverRemove = Memory dolsa bile ASLA silme
                 * High = Onemli veri, silme
                 * Normal = Low ve High arasinde onem derecesi
                 * Low = Onemsiz veri silebilirsin
                 * 
                 * NeverRemove'de exception sikintisi olabilir. Memory doldugunda silemeyecegi icin exp firlatir
                 * Onun disinda exp problemi olmaz
                 */
                cacheOptions.Priority = CacheItemPriority.NeverRemove;

                /*
                 * Memory den silinen datalarin hangi sebeple silindigini RegisterPostEvictionCallback
                 * ile anlayabiliriz.
                 * Ornegin, AbsoluteExpiration ile suresi dolmus datalar silindiginde, silinme sebebi olarak
                 * "Expired (süresi bitti)" sebebini aliriz
                 */
                cacheOptions.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    _memoryCache.Set("callback", $"{key}->{value} => sebep: {reason}");
                });

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
            _memoryCache.TryGetValue("callback", out string callback);
            ViewBag.callback = _memoryCache.Get<string>("callback");
            return View();
        }
    }
}
