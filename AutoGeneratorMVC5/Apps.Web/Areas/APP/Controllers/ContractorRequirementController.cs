using System.Collections.Generic;
using Apps.Web.Core;
using Apps.Locale;
using System.Web.Mvc;
using Apps.Common;
using Microsoft.Practices.Unity;
using Apps.BLL.Sys;
using Apps.BLL.App;
using Apps.Models.App;
using Newtonsoft.Json;

namespace Apps.Web.Areas.App.Controllers
{
    /// <summary>
    /// 外派公司-职位管理
    /// </summary>
    public class ContractorRequirementController : BaseController
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
        public App_ApplyJobStepBLL app_ApplyJobStepBLL { get; set; }
        [Dependency]
        public App_RequirementInviteBLL app_RequirementInviteBLL { get; set; }
        #endregion

        ValidationErrors errors = new ValidationErrors();

        #region 列表
        [SupportFilter]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager, RequirementQuery requirementQuery)
        {
            //先获取当前用户的对应customerId
            var account = GetAccount();
            var sysUser = sysUserBLL.m_Rep.GetById(account.Id);
            bool ohadmin = sysRoleBLL.ToBeCheckAuthorityRoleCode(account.RoleId, "ohadmin");
            bool admin = sysRoleBLL.ToBeCheckAuthorityRoleCode(account.RoleId, "SuperAdmin");
            if (ohadmin || admin)
            {
                requirementQuery.AdminFlag = true;
            }
            else
            {
                //如果登录的账号不是ohadmin角色的，则按照他自己创建的显示
                requirementQuery.CustomerId = sysUser.PK_App_Customer_CustomerName;
            }
            List<App_RequirementModel> list = m_BLL.GetContractorRequirementList(ref pager, requirementQuery);
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

        #region 创建
        [SupportFilter]
        public ActionResult Create()
        {
            ViewBag.PK_App_Position_Name = new SelectList(app_PositionBLL.m_Rep.FindList(), "Id", "Name");
            ViewBag.WorkLimitSex = new SelectList(enumDictionaryBLL.GetDropDownList("App_Requirement.WorkLimitSex"), "ItemValue", "ItemName");
            ViewBag.EnumWorkLimitDegree = new SelectList(enumDictionaryBLL.GetDropDownList("App_Requirement.EnumWorkLimitDegree"), "ItemValue", "ItemName");
            ViewBag.TransactProvince = new SelectList(sysAreasBLL.GetList("0"), "Id", "Name");
            ViewBag.PK_App_Customer_CustomerName = new SelectList(_App_CustomerBLL.m_Rep.FindList(), "Id", "CustomerName");
            ViewBag.PK_App_Country_Name = new SelectList(app_CountryBLL.m_Rep.FindList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [SupportFilter]
        [ValidateInput(false)]
        public JsonResult Create(App_RequirementModel model)
        {
            var now = ResultHelper.NowTime;
            model.Id = ResultHelper.NewId;
            model.CreateTime = now;
            model.CreateUserName = GetUserId();
            model.ModificationTime = now;
            if (model != null && ModelState.IsValid)
            {
                model.PublishDate = now.ToString("yyyy-MM-dd");
                if (model.SwitchBtnRecommend == null)
                {
                    model.SwitchBtnRecommend = "0";
                }
                else
                {
                    model.SwitchBtnRecommend = "1";
                }
                if (model.SwitchBtnOpen == null)
                {
                    model.SwitchBtnOpen = "0";
                }
                else
                {
                    model.SwitchBtnOpen = "1";
                }
                model.PK_App_Country_Name = Session["PK_App_Customer_CustomerName"] as string;
                if (m_BLL.Create(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "创建", "App_Requirement");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "创建", "App_Requirement");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
                }
            }
            else
            {
                string ErrorCol = "";
                var AllModelStateErrors = ResultHelper.AllModelStateErrors(ModelState);
                foreach (var Error in AllModelStateErrors)
                {
                    ErrorCol += ", " + Error.Message;
                }
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
            }
        }
        #endregion

        #region 修改
        [SupportFilter]
        public ActionResult Edit(string id)
        {
            App_RequirementModel entity = m_BLL.GetById(id);
            entity.App_Position_Name = app_PositionBLL.GetName(entity.PK_App_Position_Name);
            ViewBag.PK_App_Position_Name = new SelectList(app_PositionBLL.m_Rep.FindList(), "Id", "Name");
            ViewBag.WorkLimitSex = new SelectList(enumDictionaryBLL.GetDropDownList("App_Requirement.WorkLimitSex"), "ItemValue", "ItemName");
            ViewBag.EnumWorkLimitDegree = new SelectList(enumDictionaryBLL.GetDropDownList("App_Requirement.EnumWorkLimitDegree"), "ItemValue", "ItemName");
            ViewBag.TransactProvince = new SelectList(sysAreasBLL.GetList("0"), "Id", "Name");
            ViewBag.PK_App_Customer_CustomerName = new SelectList(_App_CustomerBLL.m_Rep.FindList(), "Id", "CustomerName");
            ViewBag.PK_App_Country_Name = new SelectList(app_CountryBLL.m_Rep.FindList(), "Id", "Name");
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        [ValidateInput(false)]
        public JsonResult Edit(App_RequirementModel model)
        {
            string UserId = GetUserId();
            var now = ResultHelper.NowTime;
            if (model != null && ModelState.IsValid)
            {

                model.ModificationUserName = GetUserId();
                model.ModificationTime = now;
                if (model.SwitchBtnRecommend == null)
                {
                    model.SwitchBtnRecommend = "0";
                }
                else
                {
                    model.SwitchBtnRecommend = "1";
                }
                if (model.SwitchBtnOpen == null)
                {
                    model.SwitchBtnOpen = "0";
                }
                else
                {
                    model.SwitchBtnOpen = "1";
                    model.PublishDate = now.ToString("yyyy-MM-dd");
                }
                if (m_BLL.Edit(ref errors, model))
                {
                    //如果审核通过了，直接给符合条件的工人推送消息
                    m_BLL.SendMessageToWorker(UserId, model.Id);
                    LogHandler.WriteServiceLog(UserId, "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "修改", "App_Requirement");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(UserId, "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "修改", "App_Requirement");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ErrorCol));
                }
            }
            else
            {
                string ErrorCol = "";
                var AllModelStateErrors = ResultHelper.AllModelStateErrors(ModelState);
                foreach (var Error in AllModelStateErrors)
                {
                    ErrorCol += ", " + Error.Message;
                }
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ErrorCol));
            }
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

        #region 删除
        [HttpPost]
        [SupportFilter]
        public JsonResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (m_BLL.Delete(ref errors, id))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "App_Requirement");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "App_Requirement");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                string ErrorCol = "";
                var AllModelStateErrors = ResultHelper.AllModelStateErrors(ModelState);
                foreach (var Error in AllModelStateErrors)
                {
                    ErrorCol += ", " + Error.Message;
                }
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
            }


        }
        #endregion

        #region 添加标签
        /// <summary>
        /// 添加标签
        /// </summary>
        /// <returns></returns>
        public ActionResult AddTag(string Tags)
        {
            if (string.IsNullOrEmpty(Tags))
            {
                Tags = "";
            }
            ViewBag.Tags = Tags;
            return View();
        }
        #endregion

        #region 获取符合条件工人列表
        [SupportFilter]
        public ActionResult IndexResume(string id)
        {
            App_RequirementModel entity = m_BLL.GetById(id);
            entity.PK_App_Position_Name = app_PositionBLL.GetName(entity.PK_App_Position_Name);
            entity.WorkLimitSex = enumDictionaryBLL.GetDicName("App_Requirement.WorkLimitSex", entity.WorkLimitSex);
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
            customerResumeQuery.AdminFlag = true;
            customerResumeQuery.QueryFlag = "Recommend";
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

        #region 发起面试邀请
        /// <summary>
        /// 发起面试邀请
        /// </summary>
        /// <param name="ReqId"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RequirementInvite(string ReqId, string CustomerId)
        {
            string strUserId = GetUserId(), InitiatorId = Session["PK_App_Customer_CustomerName"] as string;
            LogHandler.WriteServiceLog(strUserId, "ReqId:" + ReqId + ",CustomerId:" + CustomerId, "开始", "RequirementInvite", "CustomerResumeController");
            var boolFlag = app_RequirementInviteBLL.RequirementInvite(strUserId, InitiatorId, ReqId, CustomerId);
            LogHandler.WriteServiceLog(strUserId, "ReqId:" + ReqId + ",CustomerId:" + CustomerId + ",boolFlag:" + boolFlag, "结束", "RequirementInvite", "CustomerResumeController");
            if (!boolFlag)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode("发起邀请失败")
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode("发起邀请成功", boolFlag, 1)
                    );
            }
        }
        #endregion
    }
}
