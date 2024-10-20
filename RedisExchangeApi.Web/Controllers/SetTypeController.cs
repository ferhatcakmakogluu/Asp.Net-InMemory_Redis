using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _db;
        private string SetKey = "hashNames";

        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb(2);
        }
        public IActionResult Index()
        {
            //uniq, sirasiz
            HashSet<string> namesList = new HashSet<string>();
            if (_db.KeyExists(SetKey))
            {
                _db.SetMembers(SetKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }

            return View(namesList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            //verinin omru
            //sayfa her yenilendiginde 5dk ekler
            //timeout u kaldirmak icin (omur eklenmemesi icin)
            /*
                if(!_db.KeyExist(SetKey)){
                    _db.KeyExpire(SetKey,DateTime.Now.AddMinutes(5));
                 }
             */
            _db.KeyExpire(SetKey,DateTime.Now.AddMinutes(5));
            _db.SetAdd(SetKey, name);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteItem(string name)
        {
            await _db.SetRemoveAsync(SetKey, name);
            return RedirectToAction("Index");
        }
    }
}
