using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValuationAnchor.Models;
using SensitivewordApi.Helper.ReturnHelpers;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SensitivewordApi.Common.AllConfig;
using StackExchange.Redis;
using ValuationAnchor.Helpers;
using SensitivewordApi.Common.Extend.HttpExtend;
using ValuationAnchor.Extend.EnumExtend;

namespace ValuationAnchor.Filters
{
    /// <summary>
    /// 请求安全的校验
    /// 说明-此处校验逻辑是：
    /// </summary>
    public class CheckRequestFilter : IAuthorizationFilter
    {
        private readonly ILogger<CheckRequestFilter> _logger;

        public CheckRequestFilter(ILogger<CheckRequestFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 请求安全校验
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            #region token校验区域

            var urlStr = context.HttpContext.Request.GetUri().LocalPath;


            if (!(urlStr.ToLower().StartsWith("/api/token")))
            {
                var tokenStr = context.HttpContext.GetToken();
               //var tokenStr = HttpContextExtend.GetTokenInfo();
                var value = new RedisValue();
                if (!string.IsNullOrEmpty(tokenStr))
                {
                    value = RedisHelper.GetToken(ResourceRedisKeyEnum.ValuationAnchorApiToken.GetDesc());

                    _logger.LogDebug("url:" + urlStr + " token:" + tokenStr + " appid:" + value);
                }

                if (string.IsNullOrEmpty(tokenStr) || value.IsNullOrEmpty)
                {
                    _logger.LogDebug("url:" + urlStr + ",身份认证失败！token:" + tokenStr + " appid:" + value);

                    context.Result = new JsonResult(new OutResult()
                    {
                        code = (int)ResultCode.TokenError,
                        msg = ResultCode.TokenError.GetDesc(),
                        data = ""
                    });
                }
            }
        }
        #endregion
    }
}