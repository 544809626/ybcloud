namespace ValuationAnchor.Models
{
    using System.ComponentModel;

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
        /// Token验证失败
        /// </summary>
        [Description("Token验证失败")]
        TokenError = 105,

        /// <summary>
        /// 登录失败
        /// </summary>
        LoginError = 106,
        /// <summary>
        /// 日志信息验证失败，没有日志信息
        /// </summary>
        LogError = 107,

        /// <summary>
        /// 参数错误
        /// </summary>
        ParameterError = 108,

        /// <summary>
        /// 签名时间过期
        /// </summary>
        SignTimeError = 109,
        /// <summary>
        /// 签名错误
        /// </summary>
        SignError = 110,
        #endregion
    }

}
