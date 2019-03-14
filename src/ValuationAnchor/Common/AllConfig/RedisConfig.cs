using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensitivewordApi.Common.AllConfig
{
    public class RedisConfig
    {
        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 超时时间，单位毫秒
        /// </summary>
        public int TimeOut { get; set; }

        /// <summary>
        /// Redis中的数据库
        /// </summary>
        public int DataBase { get; set; }

        /// <summary>
        /// 缓存Key前缀
        /// </summary>
        public string Prefix { get; set; }
    }

    public class TokenValue
    {
        public string Token { get; set; }
    }
}
