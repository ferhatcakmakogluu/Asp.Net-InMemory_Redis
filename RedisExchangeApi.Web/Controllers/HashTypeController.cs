using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class HashTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _db;
        private string HashTypeKey = "hashDictionary";

        public HashTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb(4);
        }
        public IActionResult Index()
        {
            Dictionary<string,string> dic = new Dictionary<string,string>();
            if (_db.KeyExists(HashTypeKey))
            {
                _db.HashGetAll(HashTypeKey).ToList().ForEach(x =>
                {
                    dic.Add(x.Name, x.Value);
                });
            }
            return View(dic);
        }

        [HttpPost]
        public IActionResult Add(string key, string value)
        {
            _db.HashSet(HashTypeKey, key, value);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string key)
        {
            _db.HashDelete(HashTypeKey, key);
            return RedirectToAction("Index");
        }
    }
}
