
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Serialization;
using ValuationAnchor.Filter;
using NuGet.Protocol;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using SensitivewordApi.Common;
using SensitivewordApi.Helper.ReturnHelpers;
using SensitivewordApi.Helper.EnumHelpers;
using NLog.Web;
using ValuationAnchor.Filters;
using SensitivewordApi.Common.AllConfig;
using ValuationAnchor.Helpers;

namespace ValuationAnchor
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            //数据库连接配置区域
            services.AddDbContext<MsSqlDbContext>(options => options.UseSqlServer(Configuration["DbConn:UCenterConn"]));

            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);


            DapperHelper.SensitiveWordConnStr = Configuration["DbConn:UCenterConn"];

            services.Configure<RedisConfig>(Configuration.GetSection("Redis"));
            //redis配置
            RedisHelper.RedisConfig = new RedisConfig()
            {
                Host = Configuration["Redis:Host"],
                Port = int.Parse(Configuration["Redis:Port"]),
                Prefix = Configuration["Redis:Prefix"],
                TimeOut = int.Parse(Configuration["Redis:TimeOut"]),
                DataBase = int.Parse(Configuration["Redis:database"])
            };
            services.AddMvc(options =>
                {
//#if  !DEBUG

                   options.Filters.Add(typeof(CheckRequestFilter));
//#endif
                    options.Filters.Add(typeof(LogFilter));
                })
                .AddJsonOptions(x =>
                {
                    //Json不使用驼峰命名
                    x.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //引入NLog日志组件

            loggerFactory.AddNLog();


            env.ConfigureNLog("nlog.config");//加载NLog配置文件

            //启用静态文件
            app.UseStaticFiles();

            //异常处理
            app.UseExceptionHandler(
                builder =>
                {
                    builder.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                            var error = context.Features.Get<IExceptionHandlerFeature>();
                            if (error != null)
                            {
                                var sb = new StringBuilder();

                                if (context.Request.HasFormContentType)
                                {
                                    foreach (KeyValuePair<string, StringValues> keyValuePair in context.Request.Form)
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

                                loggerFactory.CreateLogger<Startup>().LogError("请求：{path}，参数：{par}，出错信息：{error}",
                                    context.Request.Path + context.Request.QueryString, sb.ToString(),
                                    error.Error.ToString().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0]);

                                var result = new JsonResult(new OutResult()
                                {
                                    code = (int)ResultCode.UnknowError,
                                    msg = ResultCode.UnknowError.GetDesc() + " " + error.Error.Message,
                                    data = ""
                                });

                                await context.Response.WriteAsync(result.Value.ToJson()).ConfigureAwait(false);
                            }
                        });
                });

            //中间件组件
            app.UseMiddleware<ServerRunTimeMiddlewareHelper>();

            app.UseMvc();
        }
    }
}
