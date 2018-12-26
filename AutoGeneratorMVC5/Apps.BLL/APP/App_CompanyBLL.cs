using Apps.Common;
using Apps.Models;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using Apps.Models.App;

namespace Apps.BLL.App
{
    public  partial class App_CompanyBLL
    {

        #region 获取公司全称
        /// <summary>
        /// 获取公司全称
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public string GetCompanyName(string CompanyID)
        {
             var CompanyName = string.Empty;
             var _App_CompanyModel = GetById(CompanyID);
             if(null != _App_CompanyModel)
             {
                 CompanyName = _App_CompanyModel.CompanyName;
             }
            return CompanyName;
        }
        #endregion
        #region 获取公司简称
        /// <summary>
        /// 获取公司简称
        /// </summary>
        /// <param name="CompanyShortID"></param>
        /// <returns></returns>
        public string GetCompanyShortName(string CompanyShortID)
        {
             var CompanyShortName = string.Empty;
             var _App_CompanyModel = GetById(CompanyShortID);
             if(null != _App_CompanyModel)
             {
                 CompanyShortName = _App_CompanyModel.CompanyShortName;
             }
            return CompanyShortName;
        }
        #endregion
    }
 }

