using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SensitivewordApi.Common
{
    public static class Util
    {
        #region 关联数据库
        /// <summary>
        /// 关联数据库
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static dynamic DbConn(string type)
        {
            dynamic conn = null;
            switch (type)
            {
                case "orc_wzt":
                    conn = DapperHelper.GetMssqlConnection();
                    break;
                    //case "Cloud_Core":
                    //    conn = DapperHelper.GetMssqlCoreConnection();
                    //    break;
                    //case "Cloud_UCenter":
                    //    conn = DapperHelper.GetMssqlUcenterConnection();
                    //break;
            }
            return conn;
        }
        #endregion


        #region Unicode10转汉字
        /// <summary>
        /// Unicode10转汉字
        /// </summary>
        /// <param name="countText"></param>
        /// <returns></returns>
        public static string Unicode10ToGB(string countText)
        {
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(countText, "&#([\\w]{5});");
            if (mc != null && mc.Count > 0)
            {
                foreach (System.Text.RegularExpressions.Match m2 in mc)
                {
                    string v = m2.Value;
                    string temp = v.Substring(2, v.Length - 3);
                    try
                    {
                        char word = (char)Convert.ToInt32(temp);
                        countText = countText.Replace(v, word.ToString());
                    }
                    catch (Exception ex)
                    {
                        return countText; //throw ;
                    }
                }
            }
            return countText;
        }

        /// <summary>
        /// Unicode16转汉字
        /// </summary>
        /// <param name="countText"></param>
        /// <returns></returns>
        public static string Unicode16ToGB(string countText)
        {

            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(countText, "&#x(\\w{1,4});");
            if (mc != null && mc.Count > 0)
            {
                foreach (System.Text.RegularExpressions.Match m2 in mc)
                {
                    string v = m2.Value;
                    string a = m2.Groups[1].Value;

                    var b = int.Parse(a, System.Globalization.NumberStyles.HexNumber);

                    try
                    {
                        char word = (char)b;
                        countText = countText.Replace(v, word.ToString());
                    }
                    catch (Exception ex)
                    {
                        return countText; //throw ;
                    }
                }
            }
            return countText;
        }

        #endregion


        #region 移除Html
        /// <summary>
        /// 移除Html
        /// </summary>
        /// <param name="HTMLStr"></param>
        /// <returns></returns>
        public static string ParseTags(string HTMLStr)
        {
            return System.Text.RegularExpressions.Regex.Replace(HTMLStr, "<[^>]*>", "");
        }

        /// <summary>
        /// 移除Html
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public static string Html2Text(string htmlStr)
        {
            try
            {
                if (String.IsNullOrEmpty(htmlStr))
                {
                    return "";
                }
                string regEx_style = "<style[^>]*?>[\\s\\S]*?<\\/style>"; //定义style的正则表达式 
                string regEx_script = "<script[^>]*?>[\\s\\S]*?<\\/script>"; //定义script的正则表达式 
                string regEx_html = "<[^>]+>"; //定义HTML标签的正则表达式 
                htmlStr = Regex.Replace(htmlStr, regEx_style, "");//删除css
                htmlStr = Regex.Replace(htmlStr, regEx_script, "");//删除js
                htmlStr = Regex.Replace(htmlStr, regEx_html, "");//删除html标记
                htmlStr = Regex.Replace(htmlStr, "\\s*|\t|\r|\n", "");//去除tab、空格、空行
                htmlStr = htmlStr.Replace(" ", "");
                htmlStr = htmlStr.Replace("\"", "");//去除异常的引号" " "
                htmlStr = htmlStr.Replace("\"", "");
                return htmlStr.Trim();
            }
            catch (Exception)
            {
                return htmlStr;
            }
        }
        #endregion
    }
}
