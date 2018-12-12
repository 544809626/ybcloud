using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SensitivewordApi.Common.Encry
{
    /// <summary>
    /// 加密/解密公用方法
    /// </summary>
    public class EncryHelper
    {
        /// <summary>
        /// 将指定的字符串进行MD5加密形成的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5(string str)
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

        /// <summary>
        /// 密码盐随机生产
        /// </summary>
        /// <param name="num">返回随机数位数</param>
        /// <returns></returns>
        public static string PaySaltRandom(int num = 6)
        {
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            Random randrom = new Random((int)DateTime.Now.Ticks);

            string str = "";
            for (int i = 0; i < num; i++)
            {
                str += chars[randrom.Next(chars.Length)];
            }

            return str;
        }
    }
}
