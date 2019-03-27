﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Extensions.Logging;
using ValuationAnchor.Helpers;

namespace ValuationAnchor.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ValuesController : BaseController
    {


        private ILogger<dynamic> _logger;

        private string _msg = "请求成功";

        public ValuesController(ILogger<dynamic> logger) : base(logger)
        {
            _logger = logger;
        }
        // GET api/values
        [HttpGet]
        public async Task<string> Get()
        {
            // return new string[] { "value1", "value2" };
            var list = await GetWebJson<JsonResult>("http://money.finance.sina.com.cn/q/view/newFLJK.php?param=class");
            _logger.LogInformation("-----Data");
            return list;
        }

        [HttpGet]
        public JsonResult GetListJson()
        {
            var value = RedisHelper.GetToken("BlockList_11");
            return Result_Ok(value);
            //把字段存储在DB 中，每个15秒刷新一次

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }

        public static async Task<string> GetWebJson<T>(string url, Dictionary<string, string> para = null,
            Dictionary<string, string> head = null)
        {
            using (HttpClient client = new HttpClient())
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                string result = "";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    Stream myResponseStream = await response.Content.ReadAsStreamAsync();
                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("GBK"));
                    result = myStreamReader.ReadToEnd();
                    myStreamReader.Dispose();
                    myResponseStream.Dispose();
                }
                if (string.IsNullOrEmpty(result))
                {
                    return result;
                }
                else
                {
                    try
                    {
                        //解析返回
                        var str = result.Remove(0, 29);
                        if (str.Contains("gn_"))
                        {
                            var list = str.Split(':');
                            var i = 0;
                            foreach (var gn in list)
                            {

                                if (i < list.Length)
                                {
                                    RedisHelper.SetToken("BlockList_" + i, gn, TimeSpan.FromSeconds(3600));
                                    i++;
                                    
                                }
                            }
                        }
                        return str;
                    }
                    catch (Exception ex)
                    {
                       
                      
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            var result = "";
            return result;
        }


      
    }
}
