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
    public class PositionController : BaseController
    {
        #region BLLs
        [Dependency]
        public App_PositionBLL m_BLL { get; set; }
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
        [SupportFilter(ActionName = "Index")]
        public JsonResult GetList(string id)
        {
            if (id == null)
                id = "0";
            List<App_PositionModel> list = m_BLL.GetList(id);
            var json = from r in list
                       select new App_PositionModel()
                       {
                           Id = r.Id,
                           Name = r.Name,
                           SortCode = r.SortCode,
                           ParentName = m_BLL.GetName(r.ParentId),
                           Description = r.Description,
                           CreateTime = r.CreateTime,
                           state = (m_BLL.GetList(r.Id).Count > 0) ? "closed" : "open"
                       };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 创建
        [SupportFilter]
        public ActionResult Create(string id)
        {
            ViewBag.ParentID = id;
            var list = m_BLL.m_Rep.Find(SS => SS.Id.Equals(id));
            if (list == null)
            {
                App_PositionModel entity = new App_PositionModel();
                return View(entity);
            }
            else
            {
                App_PositionModel entity = new App_PositionModel()
                {
                    ParentName = list.Name,
                    ParentId = id,
                };
                return View(entity);
            }
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Create(App_PositionModel model)
        {
            model.Id = ResultHelper.NewId;
            model.CreateTime = ResultHelper.NowTime;
            model.CreateUserName = GetUserId();
            model.ModificationTime = ResultHelper.NowTime;
            if (model != null && ModelState.IsValid)
            {
                if (model.ParentId == null)
                {
                    model.ParentId = "0";
                }
                if (model.SwitchBtnCommonUse == null)
                {
                    model.SwitchBtnCommonUse = "0";
                }
                else
                {
                    model.SwitchBtnCommonUse = "1";
                }
                if (m_BLL.Create(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "创建", "App_Position");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "创建", "App_Position");
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
            App_PositionModel entity = m_BLL.GetById(id);
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Edit(App_PositionModel model)
        {
            if (model != null && ModelState.IsValid)
            {

                model.ModificationUserName = GetUserId();
                model.ModificationTime = ResultHelper.NowTime;
                if (model.SwitchBtnCommonUse == null)
                {
                    model.SwitchBtnCommonUse = "0";
                }
                else
                {
                    model.SwitchBtnCommonUse = "1";
                }
                if (m_BLL.Edit(ref errors, model))
                {
                    //如果设置当前职位为常用，则需要给上辈节点改为常用
                    if (model.SwitchBtnCommonUse == "1")
                    {
                        var pos = m_BLL.GetById(model.ParentId);
                        while (pos != null)
                        {
                            pos.ModificationUserName = GetUserId();
                            pos.ModificationTime = ResultHelper.NowTime;
                            pos.SwitchBtnCommonUse = "1";
                            m_BLL.Edit(ref errors, pos);
                            pos = m_BLL.GetById(pos.ParentId);
                        }
                    }
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "修改", "App_Position");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "修改", "App_Position");
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
            App_PositionModel entity = m_BLL.GetById(id);
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
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "App_Position");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "App_Position");
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

        #region 获取职位树
        /// <summary>
        /// 获取职位树
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPosCombTree()
        {
            var list = m_BLL.GetAppPositionComboTree();
            return Json(list);
        }
        #endregion
    }
}

