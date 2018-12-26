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
    public class ApplyedRequirementController : BaseController
    {
        #region BLLs
        [Dependency]
        public App_RequirementBLL m_BLL { get; set; }
        [Dependency]
        public EnumDictionaryBLL enumDictionaryBLL { get; set; }
        [Dependency]
        public App_CustomerBLL _App_CustomerBLL { get; set; }
        [Dependency]
        public App_CountryBLL _App_CountryBLL { get; set; }
        [Dependency]
        public SysAreasBLL sysAreasBLL { get; set; }
        [Dependency]
        public App_PositionBLL _App_PositionBLL { get; set; }
        [Dependency]
        public SysUserBLL sysUserBLL { get; set; }
        [Dependency]
        public SysRoleBLL sysRoleBLL { get; set; }
        [Dependency]
        public App_ApplyJobBLL app_ApplyJobBLL { get; set; }
        #endregion

        #region 获取已经应聘的职位列表
        [SupportFilter]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [SupportFilter(ActionName = "Index")]
        public JsonResult GetApplyedReqList(GridPager pager, RequirementQuery requirementQuery)
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
            List<App_RequirementModel> list = m_BLL.GetRequirementList(ref pager, requirementQuery);
            foreach (var Item in list)
            {
                Item.PK_App_Position_Name = _App_PositionBLL.GetNames(Item.PK_App_Position_Name);
                Item.EnumWorkLimitDegree = enumDictionaryBLL.GetDicName("App_Requirement.EnumWorkLimitDegree", Item.EnumWorkLimitDegree);
                Item.PK_App_Customer_CustomerName = _App_CustomerBLL.GetCustomerName(Item.PK_App_Customer_CustomerName);
                Item.PK_App_Country_Name = _App_CountryBLL.GetName(Item.PK_App_Country_Name);
            }
            GridRows<App_RequirementModel> grs = new GridRows<App_RequirementModel>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #endregion

        #region 详细
        [SupportFilter]
        public ActionResult Details(string id)
        {
            App_RequirementModel entity = m_BLL.GetById(id);
            entity.PK_App_Position_Name = _App_PositionBLL.GetName(entity.PK_App_Position_Name);
            entity.WorkLimitSex = enumDictionaryBLL.GetDicName("App_Requirement.WorkLimitSex", entity.WorkLimitSex);
            entity.EnumWorkLimitDegree = enumDictionaryBLL.GetDicName("App_Requirement.EnumWorkLimitDegree", entity.EnumWorkLimitDegree);
            entity.PK_App_Customer_CustomerName = _App_CustomerBLL.GetCustomerName(entity.PK_App_Customer_CustomerName);
            entity.PK_App_Country_Name = _App_CountryBLL.GetName(entity.PK_App_Country_Name);
            return View(entity);
        }
        #endregion

        #region 获取工人列表
        [SupportFilter]
        public ActionResult IndexResume(string id)
        {
            App_RequirementModel entity = m_BLL.GetById(id);
            entity.PK_App_Position_Name = _App_PositionBLL.GetName(entity.PK_App_Position_Name);
            entity.WorkLimitSex = enumDictionaryBLL.GetDicName("App_Requirement.WorkLimitSex", entity.WorkLimitSex);
            entity.EnumWorkLimitDegree = enumDictionaryBLL.GetDicName("App_Requirement.EnumWorkLimitDegree", entity.EnumWorkLimitDegree);
            entity.PK_App_Customer_CustomerName = _App_CustomerBLL.GetCustomerName(entity.PK_App_Customer_CustomerName);
            entity.PK_App_Country_Name = _App_CountryBLL.GetName(entity.PK_App_Country_Name);
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

        #region 取消应聘申请
        [HttpPost]
        [SupportFilter(ActionName = "CancelApply")]
        public JsonResult CancelApply(string ReqId, string CustomerId)
        {
            var applyJob = app_ApplyJobBLL.m_Rep.Find(EF => EF.PK_App_Requirement_Title == ReqId && EF.PK_App_Customer_CustomerName == CustomerId && EF.EnumApplyStatus == "0");

            if (null == applyJob)
            {
                return Json(
                   ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode("获取的应聘申请为空")
                   );
            }
            applyJob.ModificationTime = ResultHelper.NowTime;
            applyJob.ModificationUserName = GetUserId();
            applyJob.EnumApplyStatus = "2";
            try
            {
                app_ApplyJobBLL.m_Rep.Edit(applyJob);
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode("取消成功", applyJob.Id, 1)
                );
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