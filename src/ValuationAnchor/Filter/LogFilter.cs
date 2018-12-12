using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
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

        }
    }
}
