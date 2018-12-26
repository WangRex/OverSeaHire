using Apps.Common;
using Apps.Models;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using Apps.Models.Sys;
using Microsoft.Practices.Unity;
using Apps.DAL.Sys;

namespace Apps.BLL.Sys
{
    public partial class SysConfigurationBLL
    {

        #region Reps
        [Dependency]
        public SysLogRepository sysLog { get; set; }
        #endregion

        #region 获取网站名称
        /// <summary>
        /// 获取网站名称
        /// </summary>
        /// <param name="WebID"></param>
        /// <returns></returns>
        public string GetWebName(string WebID)
        {
            var WebName = string.Empty;
            var _SysConfigurationModel = GetById(WebID);
            if (null != _SysConfigurationModel)
            {
                WebName = _SysConfigurationModel.WebName;
            }
            return WebName;
        }
        #endregion

        #region 获取域名
        /// <summary>
        /// 获取域名
        /// </summary>
        /// <param name="ComID"></param>
        /// <returns></returns>
        public string GetComName(string ComID)
        {
            var ComName = string.Empty;
            var _SysConfigurationModel = GetById(ComID);
            if (null != _SysConfigurationModel)
            {
                ComName = _SysConfigurationModel.ComName;
            }
            return ComName;
        }
        #endregion

        #region 获取公司名
        /// <summary>
        /// 获取公司名
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public string GetCompanyName(string CompanyID)
        {
            var CompanyName = string.Empty;
            var _SysConfigurationModel = GetById(CompanyID);
            if (null != _SysConfigurationModel)
            {
                CompanyName = _SysConfigurationModel.CompanyName;
            }
            return CompanyName;
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
            var _SysConfigurationModel = GetById(ContactID);
            if (null != _SysConfigurationModel)
            {
                ContactName = _SysConfigurationModel.ContactName;
            }
            return ContactName;
        }
        #endregion

        #region 获取系统配置信息
        /// <summary>
        /// 获取系统配置信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public SysConfigurationVm GetSysConfiguration(string UserId, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "", "开始", "GetSysConfiguration", "SysConfigurationBLL");
            SysConfigurationVm sysConfigurationVm = new SysConfigurationVm();
            var sysConfiguration = m_Rep.GetList().FirstOrDefault();
            if (null == sysConfiguration)
            {
                ErrorMsg = "获取系统配置为空";
                sysLog.WriteServiceLog(UserId, "ErrorMsg:" + ErrorMsg, "结束", "GetSysConfiguration", "SysConfigurationBLL");
                return null;
            }
            sysConfigurationVm.ContactName = sysConfiguration.ContactName;
            sysConfigurationVm.ContactPhone = sysConfiguration.ContactPhone;
            sysConfigurationVm.ContactWeChat = sysConfiguration.ContactWeChat;
            ErrorMsg = "获取系统配置成功";
            sysLog.WriteServiceLog(UserId, "ErrorMsg:" + ErrorMsg, "结束", "GetSysConfiguration", "SysConfigurationBLL");
            return sysConfigurationVm;
        }
        #endregion
    }
}

