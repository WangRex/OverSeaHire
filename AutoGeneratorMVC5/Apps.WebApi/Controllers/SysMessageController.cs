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
using System.Web.Http.Cors;

namespace Apps.WebApi.Controllers
{
    /// <summary>
    /// 系统消息
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SysMessageController : BaseApiController
    {
        #region BLLs
        /// <summary>
        /// 系统用户
        /// </summary>
        [Dependency]
        public SysMessageBLL sysMessageBLL { get; set; }
        #endregion

        #region 获取消息列表
        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="UserId">登录人</param>
        /// <param name="EnumMessageType">消息类型</param>
        /// <param name="SwitchBtnRead">是否已读</param>
        /// <param name="PageNum">0不分页</param>
        /// <param name="RecordNum"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetSysMessages(string UserId, string EnumMessageType, string SwitchBtnRead, int PageNum, int RecordNum)
        {
            LogHandler.WriteServiceLog(UserId, "EnumMessageType:" + EnumMessageType + ",SwitchBtnRead:" + SwitchBtnRead + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetSysMessages", "SysMessageController");
            string ErrorMsg = "";
            int DataCount = 0;
            var sysMessages = sysMessageBLL.GetSysMessages(UserId, EnumMessageType, SwitchBtnRead, PageNum, RecordNum, ref DataCount, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "EnumMessageType:" + EnumMessageType + ",SwitchBtnRead:" + SwitchBtnRead + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetSysMessages", "SysMessageController");
            if (sysMessages != null)
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, sysMessages, DataCount)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
        }
        #endregion

        #region 获取消息详情
        /// <summary>
        /// 获取消息详情
        /// </summary>
        /// <param name="UserId">登录人</param>
        /// <param name="SysMessageId">消息主键</param>
        /// <returns></returns>
        [HttpGet]
        public object GetSysMessage(string UserId, string SysMessageId)
        {
            LogHandler.WriteServiceLog(UserId, "SysMessageId:" + SysMessageId, "开始", "GetSysMessage", "SysMessageController");
            string ErrorMsg = "";
            var sysMessage = sysMessageBLL.GetSysMessage(UserId, SysMessageId, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "SysMessageId:" + SysMessageId + ",ErrorMsg:" + ErrorMsg, "结束", "GetSysMessage", "SysMessageController");
            if (sysMessage != null)
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, sysMessage, 1)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
        }
        #endregion

        #region 获取未读消息总数
        /// <summary>
        /// 获取未读消息总数
        /// </summary>
        /// <param name="UserId">登录人</param>
        /// <returns></returns>
        [HttpGet]
        public object GetUnreadSysMessageCount(string UserId)
        {
            LogHandler.WriteServiceLog(UserId, "", "开始", "GetUnreadSysMessageCount", "SysMessageController");
            string ErrorMsg = "";
            int DataCount = 0;
            var sysMessageCount = sysMessageBLL.GetUnreadSysMessageCount(UserId, ref DataCount, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetUnreadSysMessageCount", "SysMessageController");
            return Json(
                ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, sysMessageCount, DataCount)
                );
        }
        #endregion
    }
}