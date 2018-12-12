using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SensitivewordApi.Common
{
    /// <summary>
    /// APi返回结果Code代码   Description中是异常的描述信息
    /// 备注:原则上，我们是不允许开发人员在接口中自定义异常信息的，
    /// 如果需要返回额外的提示信息，只能做信息的追加操作
    /// </summary>
    public enum ResultCode
    {
        #region 全局返回码区域
        /// <summary>
        /// 请求成功
        /// </summary>
        [Description("请求成功")]
        Success = 100,
        /// <summary>
        /// 请求入参异常
        /// </summary>
        [Description("请求入参异常")]
        InParaError = 101,
        /// <summary>
        /// 系统服务未知异常
        /// </summary>
        [Description("系统服务未知异常")]
        UnknowError = 102,
        /// <summary>
        /// 没有数据
        /// </summary>
        [Description("没有数据")]
        NoData = 103,

        /// <summary>
        /// 没有传入日志信息
        /// </summary>
        [Description("没有传入日志信息")]
        LogError = 104,
        /// <summary>
        /// Token验证失败
        /// </summary>
        [Description("Token验证失败")]
        TokenError = 105,

        /// <summary>
        /// AppID不存在
        /// </summary>
        [Description("AppID不存在")]
        AppIdError = 106,

        /// <summary>
        /// 签名时间格式不正确
        /// </summary>
        [Description("签名时间格式不正确")]
        TokenTimeError = 107,

        /// <summary>
        /// 签名时间与服务器时间相差10分钟
        /// </summary>
        [Description("签名时间与服务器时间相差10分钟")]
        SignTimeError = 108,

        /// <summary>
        /// 签名错误
        /// </summary>
        [Description("签名错误")]
        SignError = 109,
        #endregion
    }
}
