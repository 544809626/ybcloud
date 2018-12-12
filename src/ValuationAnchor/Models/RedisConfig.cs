using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
namespace ValuationAnchor.Models
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
        /// 缓存Key前缀
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// 标签信息默认数据库
        /// </summary>
        public int TagRedisDb { get; set; }

        public string TagPrefix { get; set; }

    }


    public enum ResourceRedisKeyEnum
    {
        [Description("valuationAnchor_token_")]
        ValuationAnchorApiToken
    }
}
