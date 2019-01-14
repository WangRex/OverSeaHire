using Apps.Common;
using System.Linq;
using System.Collections.Generic;
using Apps.Models.Sys;
using Microsoft.Practices.Unity;
using Apps.DAL.Sys;
using Apps.DAL.App;
using Apps.Models;

namespace Apps.BLL.Sys
{
    public partial class SysMessageBLL
    {
        #region Reps
        [Dependency]
        public SysLogRepository sysLog { get; set; }
        [Dependency]
        public App_CustomerRepository customerRepository { get; set; }
        #endregion

        #region 获取消息列表
        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="EnumMessageType"></param>
        /// <param name="SwitchBtnRead"></param>
        /// <param name="PageNum"></param>
        /// <param name="RecordNum"></param>
        /// <param name="DataCount"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public List<SysMessageVm> GetSysMessages(string UserId, string EnumMessageType, string SwitchBtnRead, int PageNum, int RecordNum, ref int DataCount, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "EnumMessageType:" + EnumMessageType + ",SwitchBtnRead:" + SwitchBtnRead + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetSysMessages", "SysMessageBLL");
            List<SysMessageVm> sysMessageVms = new List<SysMessageVm>();
            var sysMessages = m_Rep.FindList(EF => EF.PK_App_Customer_CustomerName == UserId).ToList();
            if (!string.IsNullOrEmpty(EnumMessageType))
            {
                sysMessages = sysMessages.Where(EF => EF.EnumMessageType == EnumMessageType).ToList();
            }
            if (!string.IsNullOrEmpty(SwitchBtnRead))
            {
                sysMessages = sysMessages.Where(EF => EF.SwitchBtnRead == SwitchBtnRead).ToList();
            }
            DataCount = sysMessages.Count;
            if (PageNum > 0)
            {
                sysMessages = sysMessages.Skip((PageNum - 1) * RecordNum).Take(RecordNum).ToList();
            }
            sysMessageVms = sysMessages.Select(EF => new SysMessageVm
            {
                Id = EF.Id,
                CreateTime = EF.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                BusinessTable = EF.BusinessTable,
                BusinessID = EF.BusinessID,
                PK_App_Customer_CustomerName = EF.PK_App_Customer_CustomerName,
                App_Customer_CustomerName = customerRepository.GetCustomerName(EF.PK_App_Customer_CustomerName),
                SwitchBtnRead = EF.SwitchBtnRead,
                Title = EF.Title,
                Content = EF.Content,
                EnumMessageType = EF.EnumMessageType,
                SwitchBtnButton = EF.SwitchBtnButton,
                ShowMessage = EF.ShowMessage,
                WorkerId = EF.WorkerId,
            }).ToList();
            ErrorMsg = "获取消息成功";
            sysLog.WriteServiceLog(UserId, "EnumMessageType:" + EnumMessageType + ",SwitchBtnRead:" + SwitchBtnRead + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetSysMessages", "SysMessageBLL");
            return sysMessageVms;
        }
        #endregion

        #region 获取消息详情
        /// <summary>
        /// 获取消息详情
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SysMessageId"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public SysMessageVm GetSysMessage(string UserId, string SysMessageId, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "SysMessageId:" + SysMessageId, "开始", "GetSysMessage", "SysMessageBLL");
            var now = ResultHelper.NowTime;
            SysMessageVm sysMessageVm = new SysMessageVm();
            var sysMessage = m_Rep.GetById(SysMessageId);
            if (null == sysMessage)
            {
                ErrorMsg = "获取消息为空";
                sysLog.WriteServiceLog(UserId, "SysMessageId:" + SysMessageId + ",ErrorMsg:" + ErrorMsg, "结束", "GetSysMessage", "SysMessageBLL");
                return null;
            }
            sysMessageVm.Id = sysMessage.Id;
            sysMessageVm.CreateTime = sysMessage.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            sysMessageVm.BusinessTable = sysMessage.BusinessTable;
            sysMessageVm.BusinessID = sysMessage.BusinessID;
            sysMessageVm.PK_App_Customer_CustomerName = sysMessage.PK_App_Customer_CustomerName;
            sysMessageVm.App_Customer_CustomerName = customerRepository.GetCustomerName(sysMessage.PK_App_Customer_CustomerName);
            sysMessageVm.SwitchBtnRead = sysMessage.SwitchBtnRead;
            sysMessageVm.Title = sysMessage.Title;
            sysMessageVm.Content = sysMessage.Content;
            sysMessageVm.EnumMessageType = sysMessage.EnumMessageType;
            sysMessageVm.SwitchBtnButton = sysMessage.SwitchBtnButton;
            sysMessageVm.ShowMessage = sysMessage.ShowMessage;
            sysMessageVm.WorkerId = sysMessage.WorkerId;
            //看过详情了，改成已读
            sysMessage.ModificationUserName = UserId;
            sysMessage.ModificationTime = now;
            sysMessage.SwitchBtnRead = "1";
            m_Rep.Edit(sysMessage);
            ErrorMsg = "获取消息成功";
            sysLog.WriteServiceLog(UserId, "SysMessageId:" + SysMessageId + ",ErrorMsg:" + ErrorMsg, "结束", "GetSysMessage", "SysMessageBLL");
            return sysMessageVm;
        }
        #endregion

        #region 【后台】获取消息列表
        /// <summary>
        /// 【后台】获取消息列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="sysMessageQuery"></param>
        /// <returns></returns>
        public List<SysMessageVm> GetSysMessageVms(ref GridPager pager, SysMessageQuery sysMessageQuery)
        {
            sysLog.WriteServiceLog(sysMessageQuery.UserId, sysMessageQuery.ToString(), "开始", "GetSysMessageVms", "SysMessageBLL");
            List<SysMessageVm> sysMessageVms = new List<SysMessageVm>();
            //获取当前登录人所辖属的所有工人列表
            IQueryable<App_Customer> queryData = customerRepository.GetList(EF => EF.ParentId != null);
            if (!sysMessageQuery.AdminFlag)
            {
                queryData = queryData.Where(EF => EF.ParentId == sysMessageQuery.CustomerId);
            }
            //获取所有的用户主键集合
            var arrCustomerId = queryData.Select(EF => EF.Id).ToArray();
            var sysMessages = m_Rep.FindList(EF => arrCustomerId.Contains(EF.PK_App_Customer_CustomerName)).ToList();
            if (!string.IsNullOrEmpty(sysMessageQuery.EnumMessageType))
            {
                sysMessages = sysMessages.Where(EF => EF.EnumMessageType == sysMessageQuery.EnumMessageType).ToList();
            }
            if (!string.IsNullOrEmpty(sysMessageQuery.SwitchBtnRead))
            {
                sysMessages = sysMessages.Where(EF => EF.SwitchBtnRead == sysMessageQuery.SwitchBtnRead).ToList();
            }
            pager.totalRows = sysMessages.Count;
            if (pager.page > 0)
            {
                sysMessages = sysMessages.Skip((pager.page - 1) * pager.rows).Take(pager.rows).ToList();
            }
            sysMessageVms = sysMessages.Select(EF => new SysMessageVm
            {
                Id = EF.Id,
                CreateTime = EF.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                BusinessTable = EF.BusinessTable,
                BusinessID = EF.BusinessID,
                PK_App_Customer_CustomerName = EF.PK_App_Customer_CustomerName,
                App_Customer_CustomerName = customerRepository.GetCustomerName(EF.PK_App_Customer_CustomerName),
                SwitchBtnRead = EF.SwitchBtnRead,
                Title = EF.Title,
                Content = EF.Content,
                EnumMessageType = EF.EnumMessageType,
                SwitchBtnButton = EF.SwitchBtnButton,
                ShowMessage = EF.ShowMessage,
                WorkerId = EF.WorkerId,
            }).ToList();
            sysLog.WriteServiceLog(sysMessageQuery.UserId, sysMessageQuery.ToString(), "结束", "GetSysMessageVms", "SysMessageBLL");
            return sysMessageVms;
        }
        #endregion
    }
}

