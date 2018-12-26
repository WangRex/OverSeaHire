using Apps.Common;
using Apps.Models;
using Apps.Models.Sys;
using Microsoft.Practices.Unity;
using Apps.DAL.Sys;
using System;

namespace Apps.BLL.Sys
{
    public partial class SysFeedbackBLL
    {
        #region Reps
        [Dependency]
        public SysLogRepository sysLog { get; set; }
        [Dependency]
        public EnumDictionaryBLL enumDictionary { get; set; }
        #endregion

        #region 创建反馈
        /// <summary>
        /// 创建反馈
        /// </summary>
        /// <param name="sysFeedbackPost"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public bool CreateSysFeedback(SysFeedbackPost sysFeedbackPost, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(sysFeedbackPost.PK_App_Customer_CustomerName, sysFeedbackPost.ToString(), "开始", "CreateSysFeedback", "SysFeedbackBLL");
            string customerId = sysFeedbackPost.PK_App_Customer_CustomerName;
            var now = ResultHelper.NowTime;
            SysFeedback sysFeedback = new SysFeedback();
            sysFeedback.Id = ResultHelper.NewId;
            sysFeedback.CreateTime = now;
            sysFeedback.CreateUserName = sysFeedbackPost.PK_App_Customer_CustomerName;
            sysFeedback.ModificationTime = now;
            sysFeedback.ModificationUserName = sysFeedbackPost.PK_App_Customer_CustomerName;
            sysFeedback.PK_App_Customer_CustomerName = customerId;
            sysFeedback.ImgList = sysFeedbackPost.ImgList;
            sysFeedback.Content = sysFeedbackPost.Content;
            try
            {
                m_Rep.Create(sysFeedback);
                ErrorMsg = "反馈成功";
                sysLog.WriteServiceLog(sysFeedbackPost.PK_App_Customer_CustomerName, sysFeedbackPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "CreateSysFeedback", "SysFeedbackBLL");
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = "反馈出现异常";
                sysLog.WriteServiceLog(sysFeedbackPost.PK_App_Customer_CustomerName, sysFeedbackPost.ToString() + ",ErrorMsg:" + ErrorMsg + ex.Message, "结束", "CreateSysFeedback", "SysFeedbackBLL");
                return false;
            }
        } 
        #endregion
    }
}

