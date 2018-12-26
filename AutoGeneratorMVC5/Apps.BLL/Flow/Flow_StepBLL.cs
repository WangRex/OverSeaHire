using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.Common;
using System.Transactions;
using Apps.Models.Flow;
using Apps.IBLL.Flow;
using Apps.IDAL.Flow;
using Apps.BLL.Core;
using Apps.Locale;

namespace Apps.BLL.Flow
{
    public partial class Flow_StepBLL
    {
        public override List<Flow_StepModel> GetList(ref GridPager pager, string formId)
        {

            IQueryable<Flow_Step> queryData = null;

            queryData = m_Rep.GetList(a => a.FormId == formId);
        
            pager.totalRows = queryData.Count();
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
      
    }
}
