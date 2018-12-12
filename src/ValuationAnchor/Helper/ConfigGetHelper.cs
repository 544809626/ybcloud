using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ValuationAnchor.Helpers
{
    /// <summary>
    /// 动态读取配置文件中 节点信息
    /// </summary>
    public class ConfigGetHelper
    {
        public static T GetAppSettings<T>(string fileName,string key) where T : class, new()
        {
            IConfiguration config = new ConfigurationBuilder()
                                .Add(new JsonConfigurationSource { Path = fileName, ReloadOnChange = true })
                                //.AddJsonFile(fileName, optional: true, reloadOnChange: true)
                                .Build();
            var appconfig = new ServiceCollection()
                            .AddOptions()
                            .Configure<T>(config.GetSection(key))
                            .BuildServiceProvider()
                            .GetService<IOptions<T>>()
                            .Value;
            return appconfig;
        }
    }
}
