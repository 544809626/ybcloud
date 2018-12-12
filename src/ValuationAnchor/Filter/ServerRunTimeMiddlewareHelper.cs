using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace ValuationAnchor.Filter
{
    public class ServerRunTimeMiddlewareHelper
    {
        //请求委托
        private readonly RequestDelegate _next;

        //日志组件
        private readonly ILogger<ServerRunTimeMiddlewareHelper> _logger;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ServerRunTimeMiddlewareHelper(RequestDelegate next, ILogger<ServerRunTimeMiddlewareHelper> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// 请求监听
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();
            context.Response.OnStarting(state =>
            {
                var httpContext = (HttpContext)state;
                var elapsedMilliseconds = watch.ElapsedMilliseconds;
                httpContext.Response.Headers.Add("X-ElapsedTime", new[] { elapsedMilliseconds.ToString() });

                var sb = new StringBuilder();

                if (httpContext.Request.HasFormContentType)
                {
                    foreach (KeyValuePair<string, StringValues> keyValuePair in httpContext.Request.Form)
                    {
                        if (sb.Length == 0)
                        {
                            sb.Append(keyValuePair.Key + "=" + keyValuePair.Value);
                        }
                        else
                        {
                            sb.Append("&" + keyValuePair.Key + "=" + keyValuePair.Value);
                        }
                    }
                }

                if (elapsedMilliseconds > 2000)
                {
                    _logger.LogWarning("慢请求：{path}，时间：{time}，参数：{par}",
                        httpContext.Request.Path + httpContext.Request.QueryString, elapsedMilliseconds,
                        sb.ToString());
                }
                else
                {
                    _logger.LogDebug("请求：{path}，时间：{time}，参数：{par}",
                    httpContext.Request.Path + httpContext.Request.QueryString, elapsedMilliseconds,
                    sb.ToString());
                }

                return Task.CompletedTask;
            }, context);

            await _next(context);
        }
    }
}
