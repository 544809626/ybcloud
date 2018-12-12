using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SensitivewordApi.Common;
using SensitivewordApi.Helper.EnumHelpers;

namespace SensitivewordApi.Helper.ReturnHelpers
{
    /// <summary>
    /// 公用返回结果
    /// </summary>
    public class OutResult
    {
        private static ResultCode DefaultCode = ResultCode.Success;

        public OutResult()
        {
            code = (int)DefaultCode;
            msg = DefaultCode.GetDesc();
        }

        /// <summary>
        /// 请求返回码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 请求返回信息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 请求返回数据
        /// </summary>
        public object data { get; set; }
    }
}
