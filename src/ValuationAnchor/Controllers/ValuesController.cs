using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Extensions.Logging;
using ValuationAnchor.Helpers;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json;

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
            var token = await GetToken();
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogInformation("-----token is null");
                return null;
            }
            else
            {
                var list = await GetWebJson<JsonResult>("http://money.finance.sina.com.cn/q/view/newFLJK.php?param=class");
                _logger.LogInformation("-----Data");
                return list;
            }
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
        public async static Task<string> GetToken()
        {
            string tokenStr = "";
            string appId = "201609021530300235";
            string signKey = "0987654321";
            string signTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string random = new Random().Next(100000, 999999).ToString();
            string signCode = EncryptWithMD5(appId + signTime + random + signKey);
            Dictionary<string, string> parm = new Dictionary<string, string>();
            parm.Add("appId", appId);
            parm.Add("signTime", signTime);
            parm.Add("random", random);
            parm.Add("signCode", signCode);
            Dictionary<string, string> parmHead = new Dictionary<string, string>();
            parmHead.Add("loginfo", "{\"USERNAME\":\"\",\"PROJECT\":\"test\",\"PLATFORM\":\"web\",\"IP\":\"192.168.1.1\",\"DEVICENUM\":\"\"}");
            dynamic token = await AsyncPost<dynamic>("http://localhost:37304/" + "api/Token", parm, parmHead);
            if (token != null && token.code == 57600)
            {
                tokenStr = token.data;
            }
            return tokenStr;
        }

        public async static Task<T> AsyncPost<T>(string url, Dictionary<string, string> postPara, Dictionary<string, string> head)
        {
            using (HttpClient client = new HttpClient())
            {
                //添加额外的请求头
                if (head.Any())
                {
                    foreach (var item in head)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }
                HttpResponseMessage task = await client.PostAsync(new Uri(url), new FormUrlEncodedContent(postPara));
                string postStr = task.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(postStr))
                {
                    return default(T);
                }
                else
                {
                    try
                    {
                        //解析返回
                        return JsonStrToObj<T>(postStr);
                    }
                    catch (Exception ex)
                    {

                        return default(T);
                    }
                }
            }
        }

        public static T JsonStrToObj<T>(string jsonStr)
        {
            if (!string.IsNullOrEmpty(jsonStr))
            {
                var obj = JsonConvert.DeserializeObject<T>(jsonStr);
                return obj;
            }
            else
            {
                return default(T);
            }
        }

        public static string EncryptWithMD5(string str)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(str);

            byte[] hash = md5.ComputeHash(inputBytes);


            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString().ToLower();
        }
    }
}
