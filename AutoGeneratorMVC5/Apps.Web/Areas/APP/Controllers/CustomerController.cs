using System.Collections.Generic;
using Apps.Web.Core;
using Apps.Locale;
using System.Web.Mvc;
using Apps.Common;
using Microsoft.Practices.Unity;
using Apps.BLL.Sys;
using Apps.BLL.App;
using Apps.Models.App;

namespace Apps.Web.Areas.APP.Controllers
{
    public class CustomerController : BaseController
    {
        #region BLLs
        [Dependency]
        public App_CustomerBLL m_BLL { get; set; }
        [Dependency]
        public EnumDictionaryBLL enumDictionaryBLL { get; set; }
        [Dependency]
        public App_CustomerWorkExpBLL app_CustomerWorkExpBLL { get; set; }
        [Dependency]
        public App_PositionBLL app_PositionBLL { get; set; }
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
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            List<App_CustomerModel> list = m_BLL.GetList(ref pager, queryStr);
            foreach (var Item in list)
            {
                Item.EnumCustomerLevel = enumDictionaryBLL.GetDicName("APP_Customer.EnumCustomerLevel", Item.EnumCustomerLevel);
                Item.EnumCustomerType = enumDictionaryBLL.GetDicName("APP_Customer.EnumCustomerType", Item.EnumCustomerType);
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
            ViewBag.Sex = new SelectList(enumDictionaryBLL.GetDropDownList("App.Sex"), "ItemValue", "ItemName");
            ViewBag.AbroadExp = new SelectList(enumDictionaryBLL.GetDropDownList("App_CustomerJobIntension.AbroadExp"), "ItemValue", "ItemName");
            return View();
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Create(App_CustomerModel model)
        {
            model.Id = ResultHelper.NewId;
            model.CreateTime = ResultHelper.NowTime;
            model.CreateUserName = GetUserId();
            model.ModificationTime = ResultHelper.NowTime;
            if (model != null && ModelState.IsValid)
            {
                if (m_BLL.Create(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "创建", "APP_Customer");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "创建", "APP_Customer");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail));
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
            ViewBag.Sex = new SelectList(enumDictionaryBLL.GetDropDownList("App.Sex"), "ItemValue", "ItemName");
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
    }
}
