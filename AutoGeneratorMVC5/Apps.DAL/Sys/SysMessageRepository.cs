using Apps.Common;
using Apps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.DAL.Sys
{
    public partial class SysMessageRepository
    {
        #region 创建消息
        /// <summary>
        /// 创建消息
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="PK_App_Customer_CustomerName"></param>
        /// <param name="BusinessID"></param>
        /// <param name="Title"></param>
        /// <param name="Content"></param>
        /// <param name="EnumMessageType"></param>
        /// <param name="SwitchBtnButton"></param>
        /// <param name="ShowMessage"></param>
        /// <returns></returns>
        public bool CrtSysMessage(string UserId, string PK_App_Customer_CustomerName, string BusinessID, string Title, string Content, string EnumMessageType, string SwitchBtnButton, string ShowMessage)
        {
            var now = ResultHelper.NowTime;
            var sysMessage = new SysMessage();
            sysMessage.Id = ResultHelper.NewId;
            sysMessage.CreateTime = now;
            sysMessage.CreateUserName = UserId;
            sysMessage.ModificationTime = now;
            sysMessage.ModificationUserName = UserId;
            sysMessage.PK_App_Customer_CustomerName = PK_App_Customer_CustomerName;
            sysMessage.BusinessID = BusinessID;
            sysMessage.SwitchBtnButton = SwitchBtnButton;
            sysMessage.ShowMessage = ShowMessage;
            sysMessage.SwitchBtnRead = "0";
            sysMessage.EnumMessageType = EnumMessageType;
            sysMessage.Title = Title;
            sysMessage.Content = Content;
            return Create(sysMessage);
        }
        #endregion
    }
}
