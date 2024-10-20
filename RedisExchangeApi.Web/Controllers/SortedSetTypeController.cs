using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _db;
        private string SortedSetType = "sortedSetNames";

        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb(3);
        }

        public IActionResult Index()
        {
            HashSet<string> names = new HashSet<string>();
            if (_db.KeyExists(SortedSetType))
            {
                //default k -> b
                /*_db.SortedSetScan(SortedSetType).ToList().ForEach(x=>
                {
                    names.Add(x.ToString());
                });*/

                //b -> k
                _db.SortedSetRangeByRank(SortedSetType, order: Order.Descending).ToList().ForEach(x =>
                {
                    names.Add(x.ToString());
                });
            }
            return View(names);
        }

        [HttpPost]
        public IActionResult Add(string name, int score)
        {
            _db.KeyExpire(SortedSetType, DateTime.Now.AddMinutes(1));
            _db.SortedSetAdd(SortedSetType, name, score);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            _db.SortedSetRemove(SortedSetType, name);
            return RedirectToAction("Index");
        }
    }
}
