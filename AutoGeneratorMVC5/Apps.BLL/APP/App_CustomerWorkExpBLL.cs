using Apps.Common;
using Apps.Models;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using Apps.Models.App;

namespace Apps.BLL.App
{
    public partial class App_CustomerWorkExpBLL
    {
        public List<App_CustomerWorkExpModel> GetList(ref GridPager pager, string queryStr, string CustomerId)
        {
            IQueryable<App_CustomerWorkExp> queryData = m_Rep.GetList(EF => EF.PK_App_Customer_CustomerName == CustomerId);
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = queryData.Where(
                                a => a.Id.Contains(queryStr)
                                || a.CreateUserName.Contains(queryStr)
                                || a.ModificationUserName.Contains(queryStr)
                                || a.StartDate.Contains(queryStr)
                                || a.EndDate.Contains(queryStr)
                                || a.Company.Contains(queryStr)
                                || a.Position.Contains(queryStr)
                                );
            }
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
    }
}

