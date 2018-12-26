using Apps.BLL.Sys;
using Apps.Common;
using Apps.WebApi.Core;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Apps.WebApi.Controllers
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SysConfigurationController : BaseApiController
    {
        #region BLLs
        /// <summary>
        /// 系统用户
        /// </summary>
        [Dependency]
        public SysConfigurationBLL sysConfigurationBLL { get; set; }
        #endregion

        #region 获取配置信息
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public object GetSysConfiguration(string UserId)
        {
            LogHandler.WriteServiceLog(UserId, "", "开始", "GetSysConfiguration", "SysConfigurationController");
            string ErrorMsg = "";
            var sysConfigurationVm = sysConfigurationBLL.GetSysConfiguration(UserId, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "ErrorMsg:" + ErrorMsg, "结束", "GetSysConfiguration", "SysConfigurationController");
            if (sysConfigurationVm == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, sysConfigurationVm)
                    );
            }
        }
        #endregion
    }
}