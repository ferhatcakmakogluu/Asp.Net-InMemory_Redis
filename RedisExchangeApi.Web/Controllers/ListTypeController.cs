using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _db;
        private string ListKey = "names";

        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb(1);
        }

        public IActionResult Index()
        {
            List<string> namesList = new List<string>();
            if (_db.KeyExists(ListKey))
            {
                //default start:0 end: -1
                _db.ListRange(ListKey).ToList().ForEach(x=>
                {
                    namesList.Add(x.ToString());
                });
            }
            return View(namesList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            _db.ListRightPush(ListKey, name);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            _db.ListRemoveAsync(ListKey,name).Wait();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteFirstItem()
        {
            _db.ListLeftPop(ListKey);
            return RedirectToAction("Index");
        }
    }
}
