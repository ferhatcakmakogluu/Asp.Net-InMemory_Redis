using StackExchange.Redis;

namespace RedisExchangeApi.Web.Services
{
    public class RedisService 
    {
        private readonly string _redisHost;
        private readonly string _redisPort;
        private readonly IConfiguration _configuration;
        public IDatabase _database { get; set; }
        private ConnectionMultiplexer _redis;

        public RedisService(IConfiguration configuration)
        {
            _configuration = configuration;
            _redisHost = _configuration["Redis:Host"];
            _redisPort = _configuration["Redis:Port"];
        }

        public void Connect()
        {
            var configString = $"{_redisHost}:{_redisPort}";
            _redis = ConnectionMultiplexer.Connect(configString);
        }

        public IDatabase GetDb(int db)
        {
            return _redis.GetDatabase(db);
        }
    }
}
