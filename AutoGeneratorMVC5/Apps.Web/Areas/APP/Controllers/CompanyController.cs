using System.Collections.Generic;
using System.Linq;
using Apps.Web.Core;
using Apps.Locale;
using System.Web.Mvc;
using Apps.Common;
using Apps.IBLL;
using Apps.Models.App;
using Microsoft.Practices.Unity;
using Apps.IBLL.App;
using Apps.BLL.App;
using Apps.BLL.Sys;

namespace Apps.Web.Areas.App.Controllers
{
    public class CompanyController : BaseController
    {
        #region BLLs
        [Dependency]
        public IApp_CompanyBLL m_BLL { get; set; }
        [Dependency]
        public EnumDictionaryBLL enumDictionaryBLL { get; set; }
        #endregion

        ValidationErrors errors = new ValidationErrors();
        
        #region 列表
        [SupportFilter]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
       [SupportFilter(ActionName="Index")]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            List<App_CompanyModel> list = m_BLL.GetList(ref pager, queryStr);
foreach (var Item in list)
{
Item.EnumCompanySize = enumDictionaryBLL.GetDicName("App_Company.EnumCompanySize",Item.EnumCompanySize);
}
            GridRows<App_CompanyModel> grs = new GridRows<App_CompanyModel>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #endregion

        #region 创建
        [SupportFilter]
        public ActionResult Create()
        {
ViewBag.EnumCompanySize = new SelectList(enumDictionaryBLL.GetDropDownList("App_Company.EnumCompanySize"), "ItemValue", "ItemName");
            return View();
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Create(App_CompanyModel model)
        {
            model.Id = ResultHelper.NewId;
            model.CreateTime = ResultHelper.NowTime;
            model.CreateUserName = GetUserId();
            model.ModificationTime = ResultHelper.NowTime;
            if (model != null && ModelState.IsValid)
            {

            if (model.SwitchBtnApply == null)
            {
                model.SwitchBtnApply = "0";
            }
            else            {
                model.SwitchBtnApply = "1";
            }
                if (m_BLL.Create(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "创建", "App_Company");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "创建", "App_Company");
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
            App_CompanyModel entity = m_BLL.GetById(id);
ViewBag.EnumCompanySize = new SelectList(enumDictionaryBLL.GetDropDownList("App_Company.EnumCompanySize"), "ItemValue", "ItemName");
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Edit(App_CompanyModel model)
        {
            if (model != null && ModelState.IsValid)
            {

            model.ModificationUserName = GetUserId();
            model.ModificationTime = ResultHelper.NowTime;
            if (model.SwitchBtnApply == null)
            {
                model.SwitchBtnApply = "0";
            }
            else
            {
                model.SwitchBtnApply = "1";
            }
                if (m_BLL.Edit(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "修改", "App_Company");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "修改", "App_Company");
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
            App_CompanyModel entity = m_BLL.GetById(id);
             entity.EnumCompanySize = enumDictionaryBLL.GetDicName("App_Company.EnumCompanySize",entity.EnumCompanySize);
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
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "App_Company");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "App_Company");
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
    }
}

