using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SensitivewordApi.Common
{
    public static class DapperHelper
    {
        public static string SensitiveWordConnStr;

        #region 用户中心数据库
        /// <summary>
        /// 资讯数据库
        /// </summary>
        /// <returns></returns>
        public static DbConnection GetMssqlConnection(bool readOnly = true)
        {
            return new SqlConnection(SensitiveWordConnStr + (readOnly ? "ApplicationIntent=ReadOnly;" : ""));
        }
        #endregion
    }
}
