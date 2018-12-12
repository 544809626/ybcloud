using SensitivewordApi.Common.Extend.HttpExtend;

namespace ValuationAnchor.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SensitivewordApi.Common;
    using Microsoft.AspNetCore.Http.Internal;
    using SensitivewordApi.Helper.SensitiveWordHelper;
    using Dapper;
    using Microsoft.Extensions.Logging;

    [Route("api/[controller]")]
    public class FinanceController : BaseController
    {
        private string _msg = "请求成功";
        private ILogger<dynamic> _logger;

        public FinanceController(ILogger<dynamic> logger) : base(logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public dynamic Get()
        {
            try
            {
                using (DbConnection conn = DapperHelper.GetMssqlConnection())
                {
                    string strSelect = $"select * from money_data";
                    dynamic sw = conn.Query(strSelect).ToList();

                    if (sw.Count > 0)
                        return new { code = Code = ResultCode.Success, msg = "查询成功！", data = sw };
                    else
                        return new { code = ResultCode.NoData, msg = "", data = false };//暂无数据
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}--Finance/Get 接口异常==" + ex.Message);
                return new { code = ResultCode.UnknowError, msg = ex.Message, data = false };
            }
        }

        [HttpGet("code")]
        public JsonResult SelectStockFinanceInfo(string code)
        {
            try
            {
                using (DbConnection conn = DapperHelper.GetMssqlConnection())
                {
                    var strSelect = $"select * from money_data where Stockcode like '%" + code + "%' ";
                    dynamic list = conn.Query(strSelect).ToList();
                 
                    return this.Result_Ok(new { a = list });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}--Finance/StockFinanceInfo  异常==" + ex.Message);
                //异常处理
                return this.Result_Error("请求失败");
            }
        }



    }
}
