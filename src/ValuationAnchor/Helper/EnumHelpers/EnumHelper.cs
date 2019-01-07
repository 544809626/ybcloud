using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Net;
namespace SensitivewordApi.Helper.EnumHelpers
{
    /// <summary>
    /// 获取枚举头部描述帮助方法
    /// </summary>
    public static class EnumHelper
    {
        
        /// <summary>
        /// 获取枚举描述信息
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetDesc(this Enum en)
        {
            //返回信息
            string strDesc = en.ToString();
            //获取信息Type
            Type type = en.GetType();
            //获取成员类型信息集合
            MemberInfo[] memberInfos = type.GetMember(en.ToString());
            if (memberInfos != null && memberInfos.Length > 0)
            {
                //获取自定义属性集合
                IEnumerable<Attribute> attrs = memberInfos[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attrs != null && attrs.Any())
                {
                    strDesc = ((DescriptionAttribute)attrs.FirstOrDefault()).Description;
                }
            }
            return strDesc;
        }
    }

    


}
