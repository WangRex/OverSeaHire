using Apps.Common;
using Apps.Models;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using Apps.Models.App;

namespace Apps.BLL.App
{
    public  partial class App_OfficeBLL
    {

        #region 获取办事处名称
        /// <summary>
        /// 获取办事处名称
        /// </summary>
        /// <param name="OfficeID"></param>
        /// <returns></returns>
        public string GetOfficeName(string OfficeID)
        {
             var OfficeName = string.Empty;
             var _App_OfficeModel = GetById(OfficeID);
             if(null != _App_OfficeModel)
             {
                 OfficeName = _App_OfficeModel.OfficeName;
             }
            return OfficeName;
        }
        #endregion
        #region 获取联系人
        /// <summary>
        /// 获取联系人
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        public string GetContactName(string ContactID)
        {
             var ContactName = string.Empty;
             var _App_OfficeModel = GetById(ContactID);
             if(null != _App_OfficeModel)
             {
                 ContactName = _App_OfficeModel.ContactName;
             }
            return ContactName;
        }
        #endregion
    }
 }

