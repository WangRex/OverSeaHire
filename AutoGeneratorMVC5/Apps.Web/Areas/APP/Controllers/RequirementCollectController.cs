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
    public class RequirementCollectController : BaseController
    {
        #region BLLs
        [Dependency]
        public IApp_RequirementCollectBLL m_BLL { get; set; }
        [Dependency]
        public EnumDictionaryBLL enumDictionaryBLL { get; set; }
        [Dependency]
        public App_RequirementBLL _App_RequirementBLL{ get; set; }
        [Dependency]
        public App_CustomerBLL _App_CustomerBLL{ get; set; }
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
            List<App_RequirementCollectModel> list = m_BLL.GetList(ref pager, queryStr);
foreach (var Item in list)
{
Item.PK_App_Requirement_Title = _App_RequirementBLL.GetTitle(Item.PK_App_Requirement_Title);
Item.PK_App_Customer_CustomerName = _App_CustomerBLL.GetCustomerName(Item.PK_App_Customer_CustomerName);
}
            GridRows<App_RequirementCollectModel> grs = new GridRows<App_RequirementCollectModel>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #endregion

        #region 创建
        [SupportFilter]
        public ActionResult Create()
        {
ViewBag.PK_App_Requirement_Title = new SelectList(_App_RequirementBLL.m_Rep.FindList(), "Id", "Title");
ViewBag.PK_App_Customer_CustomerName = new SelectList(_App_CustomerBLL.m_Rep.FindList(), "Id", "CustomerName");
            return View();
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Create(App_RequirementCollectModel model)
        {
            model.Id = ResultHelper.NewId;
            model.CreateTime = ResultHelper.NowTime;
            model.CreateUserName = GetUserId();
            model.ModificationTime = ResultHelper.NowTime;
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.Create(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "创建", "App_RequirementCollect");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "创建", "App_RequirementCollect");
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
            App_RequirementCollectModel entity = m_BLL.GetById(id);
ViewBag.PK_App_Requirement_Title = new SelectList(_App_RequirementBLL.m_Rep.FindList(), "Id", "Title");
ViewBag.PK_App_Customer_CustomerName = new SelectList(_App_CustomerBLL.m_Rep.FindList(), "Id", "CustomerName");
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Edit(App_RequirementCollectModel model)
        {
            if (model != null && ModelState.IsValid)
            {

            model.ModificationUserName = GetUserId();
            model.ModificationTime = ResultHelper.NowTime;
                if (m_BLL.Edit(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "修改", "App_RequirementCollect");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "修改", "App_RequirementCollect");
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
            App_RequirementCollectModel entity = m_BLL.GetById(id);
             entity.PK_App_Requirement_Title = _App_RequirementBLL.GetTitle(entity.PK_App_Requirement_Title);
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
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "App_RequirementCollect");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "App_RequirementCollect");
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

