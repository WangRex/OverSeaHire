using System.Collections.Generic;
using Apps.Web.Core;
using Apps.Locale;
using System.Web.Mvc;
using Apps.Common;
using Apps.Models.App;
using Microsoft.Practices.Unity;
using Apps.IBLL.App;
using Apps.BLL.App;
using Apps.BLL.Sys;

namespace Apps.Web.Areas.App.Controllers
{
    public class RequirementController : BaseController
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

        #region 创建
        [SupportFilter]
        public ActionResult Create()
        {
            ViewBag.PK_App_Position_Name = new SelectList(_App_PositionBLL.m_Rep.FindList(), "Id", "Name");
            ViewBag.WorkLimitSex = new SelectList(enumDictionaryBLL.GetDropDownList("App_Requirement.WorkLimitSex"), "ItemValue", "ItemName");
            ViewBag.EnumWorkLimitDegree = new SelectList(enumDictionaryBLL.GetDropDownList("App_Requirement.EnumWorkLimitDegree"), "ItemValue", "ItemName");
            ViewBag.TransactProvince = new SelectList(sysAreasBLL.GetList("0"), "Id", "Name");
            ViewBag.PK_App_Customer_CustomerName = new SelectList(_App_CustomerBLL.m_Rep.FindList(), "Id", "CustomerName");
            ViewBag.PK_App_Country_Name = new SelectList(_App_CountryBLL.m_Rep.FindList(), "Id", "Name");
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
            entity.App_Position_Name = _App_PositionBLL.GetName(entity.PK_App_Position_Name);
            ViewBag.PK_App_Position_Name = new SelectList(_App_PositionBLL.m_Rep.FindList(), "Id", "Name");
            ViewBag.WorkLimitSex = new SelectList(enumDictionaryBLL.GetDropDownList("App_Requirement.WorkLimitSex"), "ItemValue", "ItemName");
            ViewBag.EnumWorkLimitDegree = new SelectList(enumDictionaryBLL.GetDropDownList("App_Requirement.EnumWorkLimitDegree"), "ItemValue", "ItemName");
            ViewBag.TransactProvince = new SelectList(sysAreasBLL.GetList("0"), "Id", "Name");
            ViewBag.PK_App_Customer_CustomerName = new SelectList(_App_CustomerBLL.m_Rep.FindList(), "Id", "CustomerName");
            ViewBag.PK_App_Country_Name = new SelectList(_App_CountryBLL.m_Rep.FindList(), "Id", "Name");
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        [ValidateInput(false)]
        public JsonResult Edit(App_RequirementModel model)
        {
            if (model != null && ModelState.IsValid)
            {

                model.ModificationUserName = GetUserId();
                model.ModificationTime = ResultHelper.NowTime;
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
                if (m_BLL.Edit(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "修改", "App_Requirement");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "修改", "App_Requirement");
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
            entity.PK_App_Position_Name = _App_PositionBLL.GetName(entity.PK_App_Position_Name);
            entity.WorkLimitSex = enumDictionaryBLL.GetDicName("App_Requirement.WorkLimitSex", entity.WorkLimitSex);
            entity.EnumWorkLimitDegree = enumDictionaryBLL.GetDicName("App_Requirement.EnumWorkLimitDegree", entity.EnumWorkLimitDegree);
            entity.PK_App_Customer_CustomerName = _App_CustomerBLL.GetCustomerName(entity.PK_App_Customer_CustomerName);
            entity.PK_App_Country_Name = _App_CountryBLL.GetName(entity.PK_App_Country_Name);
            return View(entity);
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

        #region 获取简历列表
        public ActionResult IndexCustomerResume(string id)
        {
            ViewBag.ReqId = id;
            //获取需求信息，默认筛选条件赋值
            var req = m_BLL.m_Rep.GetById(id);
            ViewBag.WorkLimitAgeHigh = req.WorkLimitAgeHigh;
            ViewBag.WorkLimitAgeLow = req.WorkLimitAgeLow;
            ViewBag.Sex = req.WorkLimitSex;
            return View();
        }
        [HttpPost]
        public JsonResult GetCustomerResumeList(GridPager pager, CustomerResumeQuery customerResumeQuery)
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
            var queryData = m_BLL.GetResumeList(ref pager, customerResumeQuery);
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

        #region 提交应聘申请
        /// <summary>
        /// 提交应聘申请【后台批量】
        /// 目的是可以一次性给多人报名
        /// </summary>
        /// <param name="applyJobPost"></param>
        /// <returns></returns>
        [HttpPost]
        public object CreateApplyJobs(ApplyJobPost applyJobPost)
        {
            string strUserId = GetUserId();
            applyJobPost.UserId = strUserId;
            LogHandler.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString(), "开始", "CreateApplyJobs", "ApplyJobController");
            string ErrorMsg = "";
            var iApplyJobCount = app_ApplyJobBLL.CreateApplyJobs(applyJobPost, ref ErrorMsg);
            LogHandler.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "CreateApplyJobs", "ApplyJobController");
            if (iApplyJobCount != 0)
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, iApplyJobCount, 1)
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
    }
}
