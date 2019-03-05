using Apps.Common;
using Apps.Models;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using Apps.Models.App;
using Microsoft.Practices.Unity;
using Apps.DAL.App;

namespace Apps.BLL.App
{
    public partial class App_RequirementInviteBLL
    {
        #region Reps
        [Dependency]
        public App_RequirementRepository requirementRepository { get; set; }
        #endregion

        #region 发起面试邀请
        /// <summary>
        /// 发起面试邀请
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="InitiatorId"></param>
        /// <param name="ReqId"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public bool RequirementInvite(string UserId, string InitiatorId, string ReqId, string CustomerId)
        {
            var now = ResultHelper.NowTime;
            App_RequirementInvite app_RequirementInvite = new App_RequirementInvite();
            app_RequirementInvite.Id = ResultHelper.NewId;
            app_RequirementInvite.CreateTime = now;
            app_RequirementInvite.CreateUserName = UserId;
            app_RequirementInvite.ModificationTime = now;
            app_RequirementInvite.ModificationUserName = UserId;
            app_RequirementInvite.PK_App_Requirement_Title = ReqId;
            app_RequirementInvite.Inviter = CustomerId;
            app_RequirementInvite.InitiatorId = InitiatorId;
            return m_Rep.Create(app_RequirementInvite);
        }
        #endregion

        #region 判断用户是否有邀请中(并且未同意拒绝)的职位
        /// <summary>
        /// 判断用户是否有邀请中(并且未同意拒绝)的职位
        /// </summary>
        /// <param name="applyJobPost"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public bool IsInviting(ApplyJobPost applyJobPost, ref string ErrorMsg)
        {
            string ReqId = applyJobPost.RequirementId,
                //这里的顾客主键是逗号拼接的
                customerId = applyJobPost.CustomerId;
            var Req = requirementRepository.GetById(ReqId);
            if (null == Req)
            {
                ErrorMsg = "申请的需求不存在";
                return true;
            }
            if (string.IsNullOrEmpty(customerId))
            {
                ErrorMsg = "请先选择工人";
                return true;
            }
            var arrCustomerId = customerId.Split(',');
            var iCount = m_Rep.FindList(EF => EF.PK_App_Requirement_Title == ReqId && arrCustomerId.Contains(EF.Inviter) && EF.SwitchBtnAgree == null).Count();
            if (iCount > 0)
            {
                ErrorMsg = "雇主已邀请工人参加面试，请到左侧<邀请的职位>中确认";
                return true;
            }
            else
            {
                ErrorMsg = "可应聘职位";
                return false;
            }
        }
        #endregion
    }
}

