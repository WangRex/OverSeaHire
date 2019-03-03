using Apps.BLL.App;
using Apps.BLL.Sys;
using Apps.Common;
using Apps.Models.App;
using Apps.Web.Core;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apps.Web.Areas.APP.Controllers
{
    /// <summary>
    /// 雇主邀请职位
    /// </summary>
    public class InviteRequirementController : BaseController
    {
        #region BLLs
        [Dependency]
        public App_RequirementBLL m_BLL { get; set; }
        [Dependency]
        public EnumDictionaryBLL enumDictionaryBLL { get; set; }
        [Dependency]
        public App_CustomerBLL _App_CustomerBLL { get; set; }
        [Dependency]
        public App_CountryBLL app_CountryBLL { get; set; }
        [Dependency]
        public SysAreasBLL sysAreasBLL { get; set; }
        [Dependency]
        public App_PositionBLL app_PositionBLL { get; set; }
        [Dependency]
        public SysUserBLL sysUserBLL { get; set; }
        [Dependency]
        public SysRoleBLL sysRoleBLL { get; set; }
        [Dependency]
        public App_ApplyJobBLL app_ApplyJobBLL { get; set; }
        [Dependency]
        public App_RequirementInviteBLL app_RequirementInviteBLL { get; set; }
        #endregion

        #region 获取符合当前账号下工人工种需求的职位列表
        /// <summary>
        /// 获取符合当前账号下工人工种需求的职位列表
        /// </summary>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取符合当前账号下工人工种需求的职位列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="requirementQuery"></param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter(ActionName = "Index")]
        public JsonResult GetInviteReqList(GridPager pager, RequirementQuery requirementQuery)
        {
            if ("1" == Session["IdFlag"] as string || Session["ohadmin"] as string == "1")
            {
                requirementQuery.AdminFlag = true;
            }
            else
            {
                //如果登录的账号不是ohadmin角色的，则按照他自己创建的显示
                requirementQuery.CustomerId = Session["PK_App_Customer_CustomerName"] as string;
            }
            var queryData = m_BLL.GetInviteRequirementList(ref pager, requirementQuery);
            List<App_RequirementModel> list = m_BLL.CreateModelList(ref queryData);
            List<RequirementInfoVm> requirementInfoVms = new List<RequirementInfoVm>();
            foreach (var Item in list)
            {
                requirementInfoVms.Add(new RequirementInfoVm()
                {
                    Id = Item.Id,
                    Title = Item.Title,
                    Position = app_PositionBLL.GetNames(Item.PK_App_Position_Name),
                    Country = app_CountryBLL.GetName(Item.PK_App_Country_Name),
                    Sex = Item.WorkLimitSex,
                    AgeLimit = Item.WorkLimitAgeLow + "-" + Item.WorkLimitAgeHigh,
                    YearSalary = Utils.ObjToDecimal(Item.SalaryLow, 0) / 10000 + "万-" + Utils.ObjToDecimal(Item.SalaryHigh, 0) / 10000 + "万",
                    TotalHire = Item.TotalHire,
                    Tag = Item.Tag,
                    TotalServiceMoney = Utils.ObjToDecimal(Item.TotalServiceMoney, 0) / 10000 + "万",
                    PublishDate = Item.PublishDate,
                    ReqType = Item.ReqType,
                });
            }
            GridRows<RequirementInfoVm> grs = new GridRows<RequirementInfoVm>();
            grs.rows = requirementInfoVms;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #endregion

        #region 详细
        [SupportFilter]
        public ActionResult Details(string id)
        {
            App_RequirementModel entity = m_BLL.GetById(id);
            RequirementDetailsVm requirementDetailsVm = new RequirementDetailsVm();
            requirementDetailsVm.Id = entity.Id;
            requirementDetailsVm.Title = entity.Title;
            requirementDetailsVm.Position = app_PositionBLL.GetNames(entity.PK_App_Position_Name);
            requirementDetailsVm.Country = app_CountryBLL.GetName(entity.PK_App_Country_Name);
            requirementDetailsVm.Sex = entity.WorkLimitSex;
            requirementDetailsVm.AgeLimit = entity.WorkLimitAgeLow + "-" + entity.WorkLimitAgeHigh;
            requirementDetailsVm.YearSalary = Utils.ObjToDecimal(entity.SalaryLow, 0) / 10000 + "万-" + Utils.ObjToDecimal(entity.SalaryHigh, 0) / 10000 + "万";
            requirementDetailsVm.TotalHire = entity.TotalHire;
            requirementDetailsVm.Tag = entity.Tag;
            requirementDetailsVm.TotalServiceMoney = entity.TotalServiceMoney;
            requirementDetailsVm.PublishDate = entity.PublishDate;
            requirementDetailsVm.Description = entity.Description;
            requirementDetailsVm.PromiseMoney = entity.PromiseMoney;
            requirementDetailsVm.ServiceTailMoney = entity.ServiceTailMoney;
            requirementDetailsVm.ServiceMoney = entity.ServiceMoney;
            return View(requirementDetailsVm);
        }
        #endregion

        #region 获取符合条件工人列表
        [SupportFilter]
        public ActionResult IndexResume(string id)
        {
            App_RequirementModel entity = m_BLL.GetById(id);
            entity.PK_App_Position_Name = app_PositionBLL.GetName(entity.PK_App_Position_Name);
            entity.WorkLimitSex = enumDictionaryBLL.GetDicName("App.Sex", entity.WorkLimitSex);
            entity.EnumWorkLimitDegree = enumDictionaryBLL.GetDicName("App_Requirement.EnumWorkLimitDegree", entity.EnumWorkLimitDegree);
            entity.PK_App_Customer_CustomerName = _App_CustomerBLL.GetCustomerName(entity.PK_App_Customer_CustomerName);
            entity.PK_App_Country_Name = app_CountryBLL.GetName(entity.PK_App_Country_Name);
            ViewBag.ReqId = id;
            return View(entity);
        }
        [HttpPost]
        [SupportFilter(ActionName = "IndexResume")]
        public JsonResult GetIndexResume(GridPager pager, CustomerResumeQuery customerResumeQuery)
        {
            //先获取当前用户的对应customerId
            var account = GetAccount();
            var sysUser = sysUserBLL.m_Rep.GetById(account.Id);
            bool ohadmin = sysRoleBLL.ToBeCheckAuthorityRoleCode(account.RoleId, "ohadmin");
            bool admin = sysRoleBLL.ToBeCheckAuthorityRoleCode(account.RoleId, "SuperAdmin");
            if (ohadmin || admin)
            {
                customerResumeQuery.AdminFlag = true;
            }
            else
            {
                //如果登录的账号不是ohadmin角色的，则按照他自己创建的显示
                customerResumeQuery.CustomerId = sysUser.PK_App_Customer_CustomerName;
            }
            customerResumeQuery.QueryFlag = "Invite";
            var queryData = m_BLL.GetReqResumeList(ref pager, customerResumeQuery);
            List<App_CustomerModel> list = _App_CustomerBLL.CreateModelList(ref queryData);
            foreach (var Item in list)
            {
                Item.EnumCustomerLevel = enumDictionaryBLL.GetDicName("APP_Customer.EnumCustomerLevel", Item.EnumCustomerLevel);
                Item.EnumCustomerType = enumDictionaryBLL.GetDicName("APP_Customer.EnumCustomerType", Item.EnumCustomerType);
                Item.OwnerName = _App_CustomerBLL.GetCustomerName(Item.ParentId);
            }
            GridRows<App_CustomerModel> grs = new GridRows<App_CustomerModel>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #endregion

        #region 同意应聘申请
        /// <summary>
        /// 同意应聘申请
        /// </summary>
        /// <param name="ReqId"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter(ActionName = "RequirementInvite")]
        public JsonResult RequirementInvite(string ReqId, string CustomerId, string Flag)
        {
            var now = ResultHelper.NowTime;
            string strUserId = GetUserId(), applyJobId = "";
            ApplyJobPost applyJobPost = new ApplyJobPost();
            applyJobPost.CustomerId = CustomerId;
            applyJobPost.RequirementId = ReqId;
            applyJobPost.UserId = strUserId;
            var ReqInvite = app_RequirementInviteBLL.m_Rep.Find(EF => EF.PK_App_Requirement_Title == ReqId && EF.Inviter == CustomerId && EF.SwitchBtnAgree == null);
            ReqInvite.ModificationTime = now;
            ReqInvite.ModificationUserName = strUserId;
            ReqInvite.SwitchBtnAgree = Flag;
            //2019-02-25 Rex 修改只要代理公司点了同意，则直接创建申请，无需外派再次同意。
            ReqInvite.SwitchBtnContractorAgree = Flag;
            try
            {
                var ReqInviteFlag = app_RequirementInviteBLL.m_Rep.Edit(ReqInvite);
                string ErrorMsg = "";
                if (ReqInviteFlag)
                {
                    if ("1".Equals(Flag))
                    {
                        var applyJob = app_ApplyJobBLL.CreateApplyJobs(applyJobPost, "2", ref ErrorMsg);
                        ErrorMsg = "同意邀请成功";
                    }
                    else
                    {
                        ErrorMsg = "拒绝邀请成功";
                    }
                    return Json(
                        ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, applyJobId, 1)
                    );
                }
                else
                {
                    if ("1".Equals(Flag))
                    {
                        ErrorMsg = "同意邀请失败";
                    }
                    else
                    {
                        ErrorMsg = "拒绝邀请失败";
                    }
                    return Json(
                       ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                       );
                }
            }
            catch (Exception ex)
            {
                return Json(
                   ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ex.Message)
                   );
            }
        }
        #endregion
    }
}