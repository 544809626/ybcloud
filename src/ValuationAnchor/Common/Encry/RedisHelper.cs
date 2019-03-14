using System;
using StackExchange.Redis;
using Newtonsoft.Json;
using SensitivewordApi.Common.AllConfig;

namespace ValuationAnchor.Helpers
{
    /// <summary>
    /// Redis缓存帮助类
    /// </summary>
    public class RedisHelper
    {
        private static ConnectionMultiplexer _connection;
        public static RedisConfig RedisConfig;
        private static readonly object Locker = new object();
        public RedisHelper()
        {
            RedisConfig = ConfigGetHelper.GetAppSettings<RedisConfig>("appsettings.json", "Redis");
        }

        /// <summary>
        /// 属性赋值
        /// </summary>
        public static ConnectionMultiplexer Connection
        {
            get
            {
                if (_connection == null || !_connection.IsConnected)
                {
                    lock (Locker)
                    {
                        if (_connection == null || !_connection.IsConnected)
                        {
                            _connection = ConnectionMultiplexer.Connect(new ConfigurationOptions()
                            {
                                EndPoints = { { RedisConfig.Host, RedisConfig.Port } },
                                ConnectTimeout = RedisConfig.TimeOut,
                                DefaultDatabase = 1,
                                AbortOnConnectFail = false
                            });

                            _connection.PreserveAsyncOrder = false;
                        }
                    }
                }

                return _connection;
            }
        }

        /// <summary>
        /// 容器赋值
        /// </summary>

        private static IDatabase Database
        {
            get { return Connection.GetDatabase(); }
        }

        /// <summary>
        ///  判断当前Key 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsExists(RedisKey key, bool prefix = true)
        {
            if (!prefix)
            {
                return Database.KeyExists(key);
            }
            return Database.KeyExists(key);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存key值</param>
        /// <param name="value">缓存项</param>
        /// <param name="expiry">超时选择</param>
        /// <returns></returns>
        public static bool Set(RedisKey key, string value, TimeSpan? expiry = null)
        {
            return Database.StringSet(RedisConfig.Prefix + key.ToString(), value, expiry);
        }

        public static bool SetObj<T>(RedisKey key, T value, TimeSpan? expiry = null)
        {
            return Database.StringSet(key, JsonConvert.SerializeObject(value), expiry);
        }

        public static RedisValue Get(RedisKey key, bool prefix = true)
        {
            if (!prefix)
            {
                return Database.StringGet(key.ToString());
            }
            return Database.StringGet(RedisConfig.Prefix + key.ToString());
        }
        public static T GetObj<T>(RedisKey key)
        {
            var value = Database.StringGet(key.ToString());

            return value.HasValue ? JsonConvert.DeserializeObject<T>(value) : default(T);
        }

        /// <summary>
        /// 删除相应key的缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        public static bool Delete(RedisKey key, RedisValue value)
        {
            return Database.SetRemove(RedisConfig.Prefix + key.ToString(), value);
        }

        public static RedisValue[] GetListByKey(RedisKey key)
        {
            if (Database.KeyExists(RedisConfig.Prefix + key.ToString()))
            {
                return Database.SetMembers(RedisConfig.Prefix + key.ToString());
            }
            return null;
        }

        public static bool DeleteKey(RedisKey key)
        {
            return Database.KeyDelete(RedisConfig.Prefix + key.ToString());
        }

        public static RedisValue[] GetRoomsgListByKey(RedisKey key, int startindex, int endindex)
        {
            return Database.ListRange(RedisConfig.Prefix + key.ToString(), startindex, endindex);
        }



        public static long SetRoomsgByKey_RightPush(RedisKey key, byte[] msg)
        {
            return Database.ListRightPush(RedisConfig.Prefix + key.ToString(), msg);
        }
        public static long SetRoomsgByKey_LeftPush(RedisKey key, byte[] msg)
        {
            return Database.ListLeftPush(RedisConfig.Prefix + key.ToString(), msg);
        }



        /// <summary>
        /// 设置Token
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool SetToken(RedisKey key, RedisValue value, TimeSpan? expiry = null)
        {
            return Database.StringSet(key.ToString(), value, expiry);
        }

        /// <summary>
        /// 取Token
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static RedisValue GetToken(RedisKey key)
        {
            return Database.StringGet(key.ToString());
        }
        
        /// <summary>
        /// List设置集合缓存-右
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long SetRightList(RedisKey key, RedisValue value)
        {
            return Database.ListRightPush(RedisConfig.Prefix + key.ToString(), value, When.Always, CommandFlags.HighPriority);
        }
        public static long SetRightListNoPrefix(RedisKey key, RedisValue value)
        {
            return Database.ListRightPush(key.ToString(), value, When.Always, CommandFlags.HighPriority);
        }
        /// <summary>
        /// 根据Value值获得其score
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double? GetScorebyValue(RedisKey key, RedisValue value)
        {
            return Database.SortedSetScore(RedisConfig.Prefix + key.ToString(), value);
        }

        public static bool SetSortSet(RedisKey key, RedisValue value, double score)
        {
            return Database.SortedSetAdd(RedisConfig.Prefix + key.ToString(), value, score);
        }

    }
}

