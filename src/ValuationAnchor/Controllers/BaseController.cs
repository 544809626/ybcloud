using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SensitivewordApi.Common;
using SensitivewordApi.Common.Extend.ControllerExtend;
using SensitivewordApi.Helper.EnumHelpers;
using SensitivewordApi.Helper.ReturnHelpers;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ValuationAnchor.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>

    public class BaseController : Controller
    {
        private ILogger<dynamic> _logger;


        /// <summary>
        /// 构造函数 初始化数据库上下文
        /// </summary>
        public BaseController(ILogger<dynamic> logger)
        {
            _logger = logger;
            Code = ResultCode.Success;
            ExMsg = null;
            Data = "";
        }

        /// <summary>
        /// 返回码
        /// </summary>
        protected ResultCode Code { get; set; }

        //返回的数据
        protected object Data { get; set; }

        //异常信息
        protected string ExMsg { get; set; }



        //成功
        public JsonResult Result_Ok(object data, string msg = "请求成功")
        {
            OutResult outJson = new OutResult();
            outJson.data = data;
            outJson.code = (int)ResultCode.Success;
            outJson.msg = msg;

            return this.ObjToJson(outJson);
        }

        //错误
        public JsonResult Result_Error(string msg = "")
        {
            OutResult outJson = new OutResult();
            outJson.data = null;
            outJson.code = (int)ResultCode.UnknowError;
            outJson.msg = msg;

            return this.ObjToJson(outJson);
        }
        /// <summary>
        /// 基类获取返回数据对象
        /// </summary>
        /// <returns></returns>
        protected OutResult ReturnObj()
        {
            OutResult outJson = new OutResult();
            outJson.data = Data;
            outJson.code = (int)Code;
            outJson.msg = Code.GetDesc() + (string.IsNullOrEmpty(ExMsg) ? "" : ExMsg);
            return outJson;
        }

        /// <summary>
        /// 父类直接返回序列化好的结果
        /// </summary>
        /// <returns></returns>
        public JsonResult ReturnJson()
        {
            return this.ObjToJson(ReturnObj());
        }
    }
}
