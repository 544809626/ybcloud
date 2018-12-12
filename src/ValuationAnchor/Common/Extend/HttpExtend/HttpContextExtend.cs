using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SensitivewordApi.Common.Encry;
using SensitivewordApi.Common.AllConfig;
using SensitivewordApi.Helper.EnumHelpers;
using ValuationAnchor.Helpers;

namespace SensitivewordApi.Common.Extend.HttpExtend
{
    /// <summary>
    /// HttpContext上下文的扩展
    /// </summary>
    public static class HttpContextExtend
    {

        /// <summary>
        /// 从请求上下文中获取Token信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetToken(this HttpContext context)
        {
            string token = "";
            var tokenByContext = context.Request.Headers["token"];
            if (tokenByContext.Count > 0)
            {
                token = tokenByContext.FirstOrDefault();
            }
            return token;
        }

        /// <summary>
        /// 获取appId
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns></returns>
        public static string GetAppId(this HttpContext context)
        {
            string appId = "";
            var appIdByContext = context.Request.Headers["appId"];
            if (appIdByContext.Count > 0)
            {
                appId = appIdByContext.FirstOrDefault();
            }
            return appId;
        }

        /// <summary>
        /// 获取请求头部签名时间
        /// 说明:签名时间格式:yyyyMMddHHmmss格式的14位字符串
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetSignTime(this HttpContext context)
        {
            //时间参数
            string signTime = "";
            var signTimeByContext = context.Request.Headers["signTime"];
            if (signTimeByContext.Count > 0)
            {
                signTime = signTimeByContext.FirstOrDefault();
            }
            return signTime;
        }

        /// <summary>
        /// 获取传递过来的随机数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetRandomNum(this HttpContext context)
        {
            string randomNum = "";
            var randomNumByContext = context.Request.Headers["randomNum"];
            if (randomNumByContext.Count > 0)
            {
                randomNum = randomNumByContext.FirstOrDefault();
            }
            return randomNum;
        }

        /// <summary>
        /// 获取传递过来的校验信息扩展
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetCheckInfo(this HttpContext context)
        {
            string checkInfo = "";
            var checkInfoByContext = context.Request.Headers["checkInfo"];
            if (checkInfoByContext.Count > 0)
            {
                checkInfo = checkInfoByContext.FirstOrDefault();
            }
            return checkInfo;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public static string GetTokenInfo()
        {
            string redisKey = ValuationAnchor.Models.ResourceRedisKeyEnum.ValuationAnchorApiToken.GetDesc();
            string token = string.Empty;
            try
            {
                if (RedisHelper.IsExists(redisKey))
                {
                    return RedisHelper.GetObj<TokenValue>(redisKey).Token;
                }
                //post参数
                string signKey = "0987654321"; //推送参数signKey 
                string appid = "201609021530300235"; //推送参数appid应用ID
                //请求参数
                string signTime = DateTime.Now.AddMinutes(3).ToString("yyyyMMddHHmmss"); //推送参数签名时间
                Random rdom = new Random();
                string r = rdom.Next(99999, 1000000).ToString(); //推送参数随机码
                //md5 加密
                string signCode = EncryHelper.MD5(appid + signTime + r + signKey); //推送参数签名


                token = signCode.ToString();

                RedisHelper.SetObj(redisKey, new TokenValue { Token = token }, TimeSpan.FromMinutes(30));
                return token;

            }
            catch (Exception exception)
            {
                //FileLogHelper.Write($"调取可搜接口获得token异常：{exception.Message}");
               throw new Exception("可搜接口，token获取出错！({0})", exception);
            }
        }
    }

 


}
