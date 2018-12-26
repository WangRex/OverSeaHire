using System.Collections.Generic;
using Apps.Web.Core;
using Apps.Locale;
using System.Web.Mvc;
using Apps.Common;
using Apps.Models.Sys;
using Microsoft.Practices.Unity;
using Apps.BLL.Sys;
using Apps.Models;

namespace Apps.Web.Controllers
{
    public class EnumDictionaryController : BaseController
    {
        #region BLLs
        [Dependency]
        public EnumDictionaryBLL m_BLL { get; set; }
        #endregion

        ValidationErrors errors = new ValidationErrors();

        #region 首页跳转
        [SupportFilter]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 获取列表
        [HttpPost]
        [SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            List<EnumDictionaryModel> list = m_BLL.GetList(ref pager, queryStr);
            foreach (var Item in list)
            {
            }
            GridRows<EnumDictionaryModel> grs = new GridRows<EnumDictionaryModel>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #endregion

        #region 创建
        [SupportFilter]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Create(EnumDictionaryModel model)
        {
            if (model != null)
            {
                model.CreateTime = ResultHelper.NowTime;
                model.CreateUserName = GetUserTrueName();
                model.ModificationTime = ResultHelper.NowTime;
                if (m_BLL.Create(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",parentId" + model.parentId, "成功", "创建", "EnumDictionary");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",parentId" + model.parentId + "," + ErrorCol, "失败", "创建", "EnumDictionary");
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
            EnumDictionary _EnumDictionary = m_BLL.m_Rep.GetById(Utils.StrToInt(id, 0));
            EnumDictionaryModel entity = new EnumDictionaryModel();
            LinqHelper.ModelTrans(_EnumDictionary, entity);
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Edit(EnumDictionaryModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.ModificationUserName = GetUserTrueName();
                model.ModificationTime = ResultHelper.NowTime;
                if (m_BLL.Edit(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",parentId" + model.parentId, "成功", "修改", "EnumDictionary");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",parentId" + model.parentId + "," + ErrorCol, "失败", "修改", "EnumDictionary");
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
            EnumDictionaryModel entity = m_BLL.GetById(id);
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
                if (m_BLL.Delete(ref errors, Utils.StrToInt(id,0)))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "EnumDictionary");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "EnumDictionary");
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

