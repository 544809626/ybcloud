using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SensitivewordApi.Helper.ReturnHelpers;
using System.Net;
namespace SensitivewordApi.Common.Extend.ControllerExtend
{
    /// <summary>
    /// CoreApi 控制器扩展
    /// </summary>
    public static class ApiControllerExtend
    {
        
        /// <summary>
        /// 返回结果扩展
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="outResult"></param>
        /// <returns></returns>
        public static JsonResult ReturnJsonResult(this Controller controller, OutResult outResult)
        {
            return controller.Json(outResult);
        }


        /// <summary>
        /// 接口返回值扩展
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static JsonResult ObjToJson(this Controller controller, OutResult result)
        {
            return controller.Json(result);
        }
    }
}
