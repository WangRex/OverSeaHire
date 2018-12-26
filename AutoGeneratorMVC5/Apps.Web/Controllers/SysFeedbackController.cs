using System.Collections.Generic;
using Apps.Web.Core;
using Apps.Locale;
using System.Web.Mvc;
using Apps.Common;
using Apps.Models.Sys;
using Microsoft.Practices.Unity;
using Apps.BLL.Sys;
using Apps.IBLL.Sys;
using Apps.BLL.App;

namespace Apps.Web.Controllers
{
    public class SysFeedbackController : BaseController
    {
        #region BLLs
        [Dependency]
        public ISysFeedbackBLL m_BLL { get; set; }
        [Dependency]
        public EnumDictionaryBLL enumDictionaryBLL { get; set; }
        [Dependency]
        public App_CustomerBLL _App_CustomerBLL { get; set; }
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
            List<SysFeedbackModel> list = m_BLL.GetList(ref pager, queryStr);
            foreach (var Item in list)
            {
                Item.PK_App_Customer_CustomerName = _App_CustomerBLL.GetCustomerName(Item.PK_App_Customer_CustomerName);
            }
            GridRows<SysFeedbackModel> grs = new GridRows<SysFeedbackModel>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #endregion

        #region 创建
        [SupportFilter]
        public ActionResult Create()
        {
            ViewBag.PK_App_Customer_CustomerName = new SelectList(_App_CustomerBLL.m_Rep.FindList(), "Id", "CustomerName");
            return View();
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Create(SysFeedbackModel model)
        {
            model.Id = ResultHelper.NewId;
            model.CreateTime = ResultHelper.NowTime;
            model.CreateUserName = GetUserId();
            model.ModificationTime = ResultHelper.NowTime;
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.Create(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "创建", "SysFeedback");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "创建", "SysFeedback");
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
            SysFeedbackModel entity = m_BLL.GetById(id);
            ViewBag.PK_App_Customer_CustomerName = new SelectList(_App_CustomerBLL.m_Rep.FindList(), "Id", "CustomerName");
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Edit(SysFeedbackModel model)
        {
            if (model != null && ModelState.IsValid)
            {

                model.ModificationUserName = GetUserId();
                model.ModificationTime = ResultHelper.NowTime;
                if (m_BLL.Edit(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "修改", "SysFeedback");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "修改", "SysFeedback");
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
            SysFeedbackModel entity = m_BLL.GetById(id);
            entity.PK_App_Customer_CustomerName = _App_CustomerBLL.GetCustomerName(entity.PK_App_Customer_CustomerName);
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
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "SysFeedback");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "SysFeedback");
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

