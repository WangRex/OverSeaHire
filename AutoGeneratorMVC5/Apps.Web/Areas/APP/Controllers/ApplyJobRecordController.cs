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
    public class ApplyJobRecordController : BaseController
    {
        #region BLLs
        [Dependency]
        public App_ApplyJobRecordBLL m_BLL { get; set; }
        [Dependency]
        public EnumDictionaryBLL enumDictionaryBLL { get; set; }
        [Dependency]
        public App_ApplyJobBLL _App_ApplyJobBLL { get; set; }
        [Dependency]
        public App_CustomerBLL _App_CustomerBLL { get; set; }
        #endregion

        ValidationErrors errors = new ValidationErrors();

        #region 列表
        public ActionResult Index(string CustomerId, string ApplyJobId)
        {
            ViewBag.CustomerId = CustomerId;
            ViewBag.ApplyJobId = ApplyJobId;
            return View();
        }
        [HttpPost]
        public JsonResult GetList(GridPager pager, string queryStr, string CustomerId, string ApplyJobId)
        {
            List<App_ApplyJobRecordModel> list = m_BLL.GetList(ref pager, queryStr, CustomerId, ApplyJobId);
            foreach (var Item in list)
            {
                Item.PK_App_Customer_CustomerName = _App_CustomerBLL.GetCustomerName(Item.PK_App_Customer_CustomerName);
                ViewBag.Result = new SelectList(enumDictionaryBLL.GetDropDownList("App_ApplyJobRecord.Result"), "ItemValue", "ItemName");
            }
            GridRows<App_ApplyJobRecordModel> grs = new GridRows<App_ApplyJobRecordModel>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #endregion

        #region 创建
        public ActionResult Create()
        {
            ViewBag.PK_App_ApplyJob_Id = new SelectList(_App_ApplyJobBLL.m_Rep.FindList(), "Id", "Id");
            ViewBag.PK_App_Customer_CustomerName = new SelectList(_App_CustomerBLL.m_Rep.FindList(), "Id", "CustomerName");
            ViewBag.Result = new SelectList(enumDictionaryBLL.GetDropDownList("App_ApplyJobRecord.Result"), "ItemValue", "ItemName");
            return View();
        }

        [HttpPost]
        public JsonResult Create(App_ApplyJobRecordModel model)
        {
            model.Id = ResultHelper.NewId;
            model.CreateTime = ResultHelper.NowTime;
            model.CreateUserName = GetUserId();
            model.ModificationTime = ResultHelper.NowTime;
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.Create(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "创建", "App_ApplyJobRecord");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "创建", "App_ApplyJobRecord");
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
            App_ApplyJobRecordModel entity = m_BLL.GetById(id);
            ViewBag.PK_App_ApplyJob_Id = new SelectList(_App_ApplyJobBLL.m_Rep.FindList(), "Id", "Id");
            ViewBag.PK_App_Customer_CustomerName = new SelectList(_App_CustomerBLL.m_Rep.FindList(), "Id", "CustomerName");
            ViewBag.Result = new SelectList(enumDictionaryBLL.GetDropDownList("App_ApplyJobRecord.Result"), "ItemValue", "ItemName");
            return View(entity);
        }

        [HttpPost]
        public JsonResult Edit(App_ApplyJobRecordModel model)
        {
            if (model != null && ModelState.IsValid)
            {

                model.ModificationUserName = GetUserId();
                model.ModificationTime = ResultHelper.NowTime;
                if (m_BLL.Edit(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "修改", "App_ApplyJobRecord");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "修改", "App_ApplyJobRecord");
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
            App_ApplyJobRecordModel entity = m_BLL.GetById(id);
            entity.PK_App_Customer_CustomerName = _App_CustomerBLL.GetCustomerName(entity.PK_App_Customer_CustomerName);
            entity.EnumApplyStatus = enumDictionaryBLL.GetDicName("App_ApplyJobRecord.EnumApplyStatus", entity.EnumApplyStatus);
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
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "App_ApplyJobRecord");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "App_ApplyJobRecord");
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

