using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            
            db.StringSet("name", "ferhat cakmakoglu");
            db.StringSet("ziyaretci", 100);

            return View();
        }

        public IActionResult Show()
        {
            var value = db.StringGet("name");//ferhat cakmakoglu

            var newValue = db.StringGetRange("name", 0, 8);//ferhat ca

            var length = db.StringLength("name"); //17
            db.StringIncrement("ziyaretci",1);

            //await async ikilisini kullanmamak icin Result kullandik, sonucu alir
            //sonucu almak istemiyorsak ve async await ikilisini kullanmka istemiyorsan, Wait kullan
            var count = db.StringDecrementAsync("ziyaretci", 10).Result;

            if (value.HasValue)
            {
                //ViewBag.name = name.ToString();
                ViewBag.name = newValue.ToString();
                ViewBag.count = length.ToString();
            }

            return View();
        }
    }
}
