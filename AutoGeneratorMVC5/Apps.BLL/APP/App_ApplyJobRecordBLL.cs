using Apps.Common;
using Apps.Models;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using Apps.Models.App;

namespace Apps.BLL.App
{
    public partial class App_ApplyJobRecordBLL
    {

        public List<App_ApplyJobRecordModel> GetList(ref GridPager pager, string queryStr, string CustomerId, string ApplyJobId)
        {

            IQueryable<App_ApplyJobRecord> queryData = m_Rep.GetList(EF => EF.PK_App_Customer_CustomerName == CustomerId && EF.PK_App_ApplyJob_Id == ApplyJobId);
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
                                a => a.Id.Contains(queryStr)
                                || a.CreateUserName.Contains(queryStr)
                                || a.ModificationUserName.Contains(queryStr)
                                || a.ParentId.Contains(queryStr)
                                || a.EnumApplyStatus.Contains(queryStr)
                                || a.Step.Contains(queryStr)
                                || a.Result.Contains(queryStr)
                                || a.Content.Contains(queryStr)
                                || a.HappenDate.Contains(queryStr)
                                || a.ConfigDate.Contains(queryStr)
                                || a.ConfigPlace.Contains(queryStr)
                                );
            }
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
    }
}

