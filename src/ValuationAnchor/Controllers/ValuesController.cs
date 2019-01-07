using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace ValuationAnchor.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public async Task<string> Get()
        {
            // return new string[] { "value1", "value2" };
            var list =await GetWebJson<JsonResult>("http://money.finance.sina.com.cn/q/view/newFLJK.php?param=class");
            return list;
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
                            //foreach (var s in list)
                            //{
                            //    if (s.Length <20) continue;
                            //    var ss = s;
                            //}
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
    }
}
