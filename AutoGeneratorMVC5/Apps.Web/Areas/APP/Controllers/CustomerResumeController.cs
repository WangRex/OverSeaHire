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
    public class CustomerResumeController : BaseController
    {
        #region BLLs
        [Dependency]
        public App_CustomerBLL m_BLL { get; set; }
        [Dependency]
        public SysUserBLL sysUserBLL { get; set; }
        [Dependency]
        public EnumDictionaryBLL enumDictionaryBLL { get; set; }
        [Dependency]
        public App_CustomerWorkExpBLL app_CustomerWorkExpBLL { get; set; }
        [Dependency]
        public App_PositionBLL app_PositionBLL { get; set; }
        [Dependency]
        public App_ApplyJobBLL app_ApplyJobBLL { get; set; }
        [Dependency]
        public App_ApplyJobStepBLL app_ApplyJobStepBLL { get; set; }
        [Dependency]
        public App_RequirementBLL app_RequirementBLL { get; set; }
        [Dependency]
        public SysRoleBLL sysRoleBLL { get; set; }
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
        public JsonResult GetList(GridPager pager, CustomerResumeQuery customerResumeQuery)
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
            List<App_CustomerModel> list = m_BLL.GetResumeList(ref pager, customerResumeQuery);
            foreach (var Item in list)
            {
                Item.EnumCustomerLevel = enumDictionaryBLL.GetDicName("APP_Customer.EnumCustomerLevel", Item.EnumCustomerLevel);
                Item.EnumCustomerType = enumDictionaryBLL.GetDicName("APP_Customer.EnumCustomerType", Item.EnumCustomerType);
                Item.OwnerName = m_BLL.GetCustomerName(Item.ParentId);
            }
            GridRows<App_CustomerModel> grs = new GridRows<App_CustomerModel>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #endregion

        #region 创建
        [SupportFilter]
        public ActionResult Create()
        {
            ViewBag.EnumCustomerLevel = new SelectList(enumDictionaryBLL.GetDropDownList("APP_Customer.EnumCustomerLevel"), "ItemValue", "ItemName");
            ViewBag.EnumCustomerType = new SelectList(enumDictionaryBLL.GetDropDownList("APP_Customer.EnumCustomerType"), "ItemValue", "ItemName");
            ViewBag.EnumForeignLangGrade = new SelectList(enumDictionaryBLL.GetDropDownList("App_CustomerJobIntension.EnumForeignLangGrade"), "ItemValue", "ItemName");
            ViewBag.EnumDriverLicence = new SelectList(enumDictionaryBLL.GetDropDownList("App_CustomerWorkmate.EnumDriverLicence"), "ItemValue", "ItemName");
            ViewBag.Sex = new SelectList(enumDictionaryBLL.GetDropDownList("APP_Customer.Sex"), "ItemValue", "ItemName");
            ViewBag.AbroadExp = new SelectList(enumDictionaryBLL.GetDropDownList("App_CustomerJobIntension.AbroadExp"), "ItemValue", "ItemName");
            return View();
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Create(string strCustomerResumePost, string eduPosts, string workPosts, string memberPosts)
        {
            //自己反序列化处理，更灵活处理
            CustomerResumePost customerResumePost = JsonConvert.DeserializeObject<CustomerResumePost>(strCustomerResumePost);
            List<EduExpPost> listEdu = JsonConvert.DeserializeObject<List<EduExpPost>>(eduPosts);
            List<WorkExpPost> listWork = JsonConvert.DeserializeObject<List<WorkExpPost>>(workPosts);
            List<FamilyPost> listMember = JsonConvert.DeserializeObject<List<FamilyPost>>(memberPosts);
            string strUserId = GetUserId();
            string ErrorMsg = "";
            customerResumePost.eduExpPosts = listEdu;
            customerResumePost.workExpPosts = listWork;
            customerResumePost.familyPosts = listMember;
            customerResumePost.UserId = strUserId;
            string CustomerId = m_BLL.CreateCustomerResume(customerResumePost, ref ErrorMsg);
            if (CustomerId != null)
            {
                LogHandler.WriteServiceLog(GetUserId(), "Id" + CustomerId + ",CreateTime:" + ResultHelper.NowTime, "成功", "创建", "APP_Customer");
                return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
            }
            else
            {
                string ErrorCol = errors.Error;
                LogHandler.WriteServiceLog(GetUserId(), "Id" + CustomerId + ",CreateTime:" + ResultHelper.NowTime + "," + ErrorMsg, "失败", "创建", "APP_Customer");
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
            }
        }
        #endregion

        #region 修改
        [SupportFilter]
        public ActionResult Edit(string id)
        {
            App_CustomerModel entity = m_BLL.GetById(id);
            ViewBag.EnumCustomerLevel = new SelectList(enumDictionaryBLL.GetDropDownList("APP_Customer.EnumCustomerLevel"), "ItemValue", "ItemName");
            ViewBag.EnumCustomerType = new SelectList(enumDictionaryBLL.GetDropDownList("APP_Customer.EnumCustomerType"), "ItemValue", "ItemName");
            ViewBag.EnumForeignLangGrade = new SelectList(enumDictionaryBLL.GetDropDownList("App_CustomerJobIntension.EnumForeignLangGrade"), "ItemValue", "ItemName");
            ViewBag.EnumDriverLicence = new SelectList(enumDictionaryBLL.GetDropDownList("App_CustomerWorkmate.EnumDriverLicence"), "ItemValue", "ItemName");
            ViewBag.Sex = new SelectList(enumDictionaryBLL.GetDropDownList("APP_Customer.Sex"), "ItemValue", "ItemName");
            ViewBag.AbroadExp = new SelectList(enumDictionaryBLL.GetDropDownList("App_CustomerJobIntension.AbroadExp"), "ItemValue", "ItemName");
            entity.JobIntensionNames = app_PositionBLL.GetNames(entity.JobIntension);
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Edit(App_CustomerModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.ModificationUserName = GetUserId();
                model.ModificationTime = ResultHelper.NowTime;
                if (model.SwitchBtnPassport == null)
                {
                    model.SwitchBtnPassport = "0";
                }
                else
                {
                    model.SwitchBtnPassport = "1";
                }
                if (model.SwitchBtnRecommend == null)
                {
                    model.SwitchBtnRecommend = "0";
                }
                else
                {
                    model.SwitchBtnRecommend = "1";
                }
                if (m_BLL.Edit(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "修改", "APP_Customer");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "修改", "APP_Customer");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
            }
        }
        #endregion

        #region 详细
        [SupportFilter]
        public ActionResult Details(string id)
        {
            App_CustomerModel entity = m_BLL.GetById(id);
            entity.EnumCustomerLevel = enumDictionaryBLL.GetDicName("APP_Customer.EnumCustomerLevel", entity.EnumCustomerLevel);
            entity.EnumCustomerType = enumDictionaryBLL.GetDicName("APP_Customer.EnumCustomerType", entity.EnumCustomerType);
            entity.EnumForeignLangGrade = enumDictionaryBLL.GetDicName("App_Customer.EnumForeignLangGrade", entity.EnumForeignLangGrade);
            entity.EnumDriverLicence = enumDictionaryBLL.GetDicName("App_Customer.EnumDriverLicence", entity.EnumDriverLicence);
            entity.AbroadExp = enumDictionaryBLL.GetDicName("App_CustomerJobIntension.AbroadExp", entity.AbroadExp);
            entity.SwitchBtnPassport = entity.SwitchBtnPassport == "1" ? "有" : "无";
            entity.SwitchBtnRecommend = entity.SwitchBtnRecommend == "1" ? "推荐" : "无";
            entity.JobIntension = app_PositionBLL.GetNames(entity.JobIntension);
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
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "APP_Customer");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "APP_Customer");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail));
            }


        }
        #endregion

        #region 获取相关订单列表
        public ActionResult ViewOrder(string CustomerId)
        {
            ViewBag.CustomerId = CustomerId;
            return View();
        }
        [HttpPost]
        public JsonResult ViewOrder(GridPager pager, CustomerResumeQuery customerResumeQuery)
        {
            List<App_ApplyJobModel> list = app_ApplyJobBLL.GetApplyJobList(ref pager, customerResumeQuery);
            foreach (var Item in list)
            {
                Item.App_Requirement_Title = app_RequirementBLL.GetTitle(Item.PK_App_Requirement_Title);
                var customer = m_BLL.m_Rep.GetById(Item.PK_App_Customer_CustomerName);
                if (null != customer)
                {
                    Item.App_Customer_CustomerName = customer.CustomerName;
                }
                Item.EnumApplyStatus = enumDictionaryBLL.GetDicName("App_ApplyJob.EnumApplyStatus", Item.EnumApplyStatus);
                Item.EnumPromisePayWay = enumDictionaryBLL.GetDicName("App.EnumPayWay", Item.EnumPromisePayWay);
                Item.EnumServicePayWay = enumDictionaryBLL.GetDicName("App.EnumPayWay", Item.EnumServicePayWay);
                Item.EnumTailPayWay = enumDictionaryBLL.GetDicName("App.EnumPayWay", Item.EnumTailPayWay);
                Item.CurrentStepName = app_ApplyJobStepBLL.GetStepName(Item.CurrentStep);
            }
            GridRows<App_ApplyJobModel> grs = new GridRows<App_ApplyJobModel>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #endregion

        #region 获取职位列表
        public ActionResult ViewPosition()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ViewPosition(GridPager pager, CustomerResumeQuery customerResumeQuery)
        {
            List<App_ApplyJobModel> list = app_ApplyJobBLL.GetApplyJobList(ref pager, customerResumeQuery);
            foreach (var Item in list)
            {
                Item.App_Requirement_Title = app_RequirementBLL.GetTitle(Item.PK_App_Requirement_Title);
                var customer = m_BLL.m_Rep.GetById(Item.PK_App_Customer_CustomerName);
                if (null != customer)
                {
                    Item.App_Customer_CustomerName = customer.CustomerName;
                }
                Item.EnumApplyStatus = enumDictionaryBLL.GetDicName("App_ApplyJob.EnumApplyStatus", Item.EnumApplyStatus);
                Item.EnumPromisePayWay = enumDictionaryBLL.GetDicName("App.EnumPayWay", Item.EnumPromisePayWay);
                Item.EnumServicePayWay = enumDictionaryBLL.GetDicName("App.EnumPayWay", Item.EnumServicePayWay);
                Item.EnumTailPayWay = enumDictionaryBLL.GetDicName("App.EnumPayWay", Item.EnumTailPayWay);
            }
            GridRows<App_ApplyJobModel> grs = new GridRows<App_ApplyJobModel>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #endregion
    }
}
