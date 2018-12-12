using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SensitivewordApi.Common.ModelExtend
{
    /// <summary>
    /// 模型扩展
    /// </summary>
    public static class ModelExtend
    {
        /// <summary>
        /// 获取实体验证的错误信息
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static string ValidationSummary(this ModelStateDictionary modelState)
        {
            StringBuilder errStr = new StringBuilder();
            //有异常才进入处理(节省点性能吧)
            if (modelState.ErrorCount > 0)
            {
                var errorList = modelState.Where(a => a.Value.Errors.Any()).Select(s => new
                {
                    s.Key,
                    s.Value.Errors
                });//取出异常信息集合

                foreach (var item in errorList)
                {
                    var errKey = item.Key;
                    var error = item.Errors;
                    errStr.AppendFormat("属性：{0}出现异常，异常信息是：", errKey);
                    foreach (var err in error)
                    {
                        //错误信息不为空
                        if (!string.IsNullOrEmpty(err.ErrorMessage))
                        {
                            errStr.AppendFormat(" 1:{0} ", err.ErrorMessage);
                        }
                        if (err.Exception != null)
                        {
                            errStr.AppendFormat("2:{0} ", err.Exception.Message);
                        };
                    }
                }
            }
            return errStr.ToString();
        }
    }
}
