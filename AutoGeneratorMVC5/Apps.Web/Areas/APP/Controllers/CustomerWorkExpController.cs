using System.Collections.Generic;
using Apps.Web.Core;
using Apps.Locale;
using System.Web.Mvc;
using Apps.Common;
using Apps.Models.App;
using Microsoft.Practices.Unity;
using Apps.BLL.App;
using Apps.BLL.Sys;

namespace Apps.Web.Areas.App.Controllers
{
    public class CustomerWorkExpController : BaseController
    {
        #region BLLs
        [Dependency]
        public App_CustomerWorkExpBLL m_BLL { get; set; }
        [Dependency]
        public EnumDictionaryBLL enumDictionaryBLL { get; set; }
        [Dependency]
        public App_CustomerBLL _App_CustomerBLL { get; set; }
        #endregion

        ValidationErrors errors = new ValidationErrors();

        #region 列表
        public ActionResult Index(string CustomerId)
        {
            ViewBag.CustomerId = CustomerId;
            return View();
        }
        [HttpPost]
        public JsonResult GetList(GridPager pager, string queryStr, string CustomerId)
        {
            List<App_CustomerWorkExpModel> list = m_BLL.GetList(ref pager, queryStr, CustomerId);
            foreach (var Item in list)
            {
                Item.PK_App_Customer_CustomerName = _App_CustomerBLL.GetCustomerName(Item.PK_App_Customer_CustomerName);
            }
            GridRows<App_CustomerWorkExpModel> grs = new GridRows<App_CustomerWorkExpModel>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #endregion

        #region 创建
        public ActionResult Create(string CustomerId)
        {
            ViewBag.CustomerId = CustomerId;
            ViewBag.PK_App_Customer_CustomerName = new SelectList(_App_CustomerBLL.m_Rep.FindList(), "Id", "CustomerName");
            App_CustomerWorkExpModel app_CustomerWorkExpModel = new App_CustomerWorkExpModel()
            {
                PK_App_Customer_CustomerName = CustomerId
            };
            return View(app_CustomerWorkExpModel);
        }

        [HttpPost]
        public JsonResult Create(App_CustomerWorkExpModel model)
        {
            model.Id = ResultHelper.NewId;
            model.CreateTime = ResultHelper.NowTime;
            model.CreateUserName = GetUserId();
            model.ModificationTime = ResultHelper.NowTime;
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.Create(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "创建", "App_CustomerWorkExp");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "创建", "App_CustomerWorkExp");
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
        public ActionResult Edit(string id)
        {
            App_CustomerWorkExpModel entity = m_BLL.GetById(id);
            ViewBag.PK_App_Customer_CustomerName = new SelectList(_App_CustomerBLL.m_Rep.FindList(), "Id", "CustomerName");
            return View(entity);
        }

        [HttpPost]
        public JsonResult Edit(App_CustomerWorkExpModel model)
        {
            if (model != null && ModelState.IsValid)
            {

                model.ModificationUserName = GetUserId();
                model.ModificationTime = ResultHelper.NowTime;
                if (m_BLL.Edit(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "修改", "App_CustomerWorkExp");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "修改", "App_CustomerWorkExp");
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
        public ActionResult Details(string id)
        {
            App_CustomerWorkExpModel entity = m_BLL.GetById(id);
            entity.PK_App_Customer_CustomerName = _App_CustomerBLL.GetCustomerName(entity.PK_App_Customer_CustomerName);
            return View(entity);
        }

        #endregion

        #region 删除
        [HttpPost]
        public JsonResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (m_BLL.Delete(ref errors, id))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "App_CustomerWorkExp");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "App_CustomerWorkExp");
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

