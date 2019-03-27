using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace ValuationAnchor.Filter
{
    public class LogFilter
        : IActionFilter
    {
        private ILogger<LogFilter> logger;

        public LogFilter(ILogger<LogFilter> _logger)
        {
            logger = _logger;
        }

        /// <summary>
        ///从写完成结果 
        /// 备注:此处请求结束后，进行相应的日志信息记录
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                //logger = context.HttpContext.GetLogInfo();
                //var logAction = Mapper.Map<CloudLogAction>(logger);
                //logAction.ActionType = context.HttpContext.GetLogAction();
                //logAction.Remark = context.HttpContext.GetLogRemark();

                ////如果动作类型不为空并且使用POST请求
                //if (!string.IsNullOrEmpty(logAction.ActionType) && context.HttpContext.Request.Method == "POST")
                //{
                //    if (string.IsNullOrEmpty(logAction.Ip))
                //    {
                //        logAction.Ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
                //    }
                //    if (!string.IsNullOrEmpty(context.HttpContext.GetLogUserName()))
                //    {
                //        logAction.UserName = context.HttpContext.GetLogUserName();
                //    }
                //    if (!string.IsNullOrEmpty(context.HttpContext.GetLogProject()))
                //    {
                //        logAction.Product = context.HttpContext.GetLogProject();
                //    } 
                //    _logger.LogError(string.Format("IP={0}, logAction.UserName={1},logAction.Product={2}", logAction.Ip, logAction.UserName, logAction.Product));
                //}
            }
            catch (Exception ex)
            {
                logger.LogError("保存日志出错：" + ex);
            }
            logger.LogInformation(context.Result.ToJson());
        }
    }

}
