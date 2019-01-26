using Apps.Common;
using Apps.Models;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using Apps.Models.App;

namespace Apps.BLL.App
{
    public partial class App_RequirementInviteBLL
    {
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
    }
}

