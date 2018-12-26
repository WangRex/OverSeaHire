using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Apps.IDAL;
using Apps.BLL.Core;
using Apps.Common;
using Apps.Models.Sys;
using Apps.Models;
using System.Transactions;
using Apps.IBLL;
using Apps.Locale;
namespace Apps.BLL.Sys
{
    public partial class SysLogBLL
    {
        #region 获取日志列表
        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="queryStr"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SysLogModel> GetListByUser(ref GridPager pager, string queryStr, string userId)
        {
            IQueryable<SysLog> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(a => a.Message.Contains(queryStr) || a.Module.Contains(queryStr) && a.Operator == userId);
            }
            else
            {
                queryData = m_Rep.GetList();
            }
            pager.totalRows = queryData.Count();
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
        #endregion

        #region 写入日志
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="oper">操作人</param>
        /// <param name="mes">操作信息</param>
        /// <param name="result">结果</param>
        /// <param name="type">类型</param>
        /// <param name="module">操作模块</param>
        public bool WriteServiceLog(string oper, string mes, string result, string type, string module)
        {
            SysLog entity = new SysLog();
            entity.Id = ResultHelper.NewId;
            entity.Operator = oper;
            entity.Message = mes;
            entity.Result = result;
            entity.Type = type;
            entity.Module = module;
            entity.CreateTime = ResultHelper.NowTime;
            return m_Rep.Create(entity);
        }
        #endregion

    }
}
