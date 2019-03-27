using System;
using System.Collections.Generic;
using SensitivewordApi.Common.Encry;
using System.Threading.Tasks;
using ValuationAnchor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SensitivewordApi.Helper.ReturnHelpers;
using ValuationAnchor.Helpers;

namespace ValuationAnchor.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenController : BaseController
    {
        private readonly ILogger<dynamic> _logger;
        public TokenController(ILogger<dynamic> logger) : base(logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取Token信息
        /// </summary>
        /// <param name="inToken">Token入参</param>
        /// <returns></returns>
        public async Task<JsonResult> Index(InToken inToken)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("201609021530300235", "0987654321");
            var result = new OutResult();
            var dateTime = DateTime.Now;
            if (
                !DateTime.TryParseExact(inToken.SignTime, "yyyyMMddHHmmss",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out dateTime))
            {
                result.code = (int)ResultCode.ParameterError;
                result.msg = "签名时间不正确";
                return Json(result);
            }

            var nowTime = DateTime.Now;
            if (dateTime > nowTime.AddMinutes(5) || dateTime < nowTime.AddMinutes(-5))
            {
                result.code = (int)ResultCode.SignTimeError;
                result.msg = "签名时间与服务器时间相差5分钟";
                return Json(result);
            }

            var sign = EncryHelper.MD5(inToken.AppId + inToken.SignTime + inToken.Random + dic[inToken.AppId]);

            if (sign != inToken.SignCode)
            {
                result.code = (int)ResultCode.SignError;
                result.msg = "签名错误";
                return Json(result);
            }

            //生成token
            var token = Guid.NewGuid().ToString().Replace("-", "");

            _logger.LogDebug("token生成：" + token + " appid:" + inToken.AppId);
            //将token存入缓存
            RedisHelper.SetToken(token, inToken.AppId, TimeSpan.FromMinutes(300));

            _logger.LogDebug("token存入Redis：" + token + " appid:" + inToken.AppId);

            return
                Json(new OutResult
                {
                    data = token
                });
        }


    }

}
