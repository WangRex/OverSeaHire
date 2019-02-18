using System;
using Apps.Common;
using System.Collections.Generic;
using Apps.Models.App;
namespace Apps.IBLL.App
{
    public partial interface IApp_ApplyJobBLL : IBaseBLL<App_ApplyJobModel>
    {
        bool NextStep(string strUserId, App_ApplyJobRecordModel model);
    }
}
