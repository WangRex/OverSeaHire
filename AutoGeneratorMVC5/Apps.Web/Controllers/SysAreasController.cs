﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2017-04-11 22:05:42 by HD
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Apps.IBLL;
using Apps.Common;
using Apps.Models;
using Apps.Models.Sys;
using System.Text;
using Apps.Web.Core;
using Apps.Locale;
using Apps.IBLL.Sys;

namespace Apps.Web.Controllers
{
    public class SysAreasController : BaseController
    {
        [Dependency]
        public ISysAreasBLL m_BLL { get; set; }
        ValidationErrors errors = new ValidationErrors();


        [SupportFilter]
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        [SupportFilter(ActionName="Index")]
        public JsonResult GetList(string id)
        {
            if (id == null)
                id = "0";
            List<SysAreasModel> list = m_BLL.GetList(id);
            var json = from r in list
                       select new SysAreasModel()
                       {
                           Id = r.Id,
		                Name = r.Name,
		                ParentId = r.ParentId,
		                Sort = r.Sort,
		                Enable = r.Enable,
		                IsMunicipality = r.IsMunicipality,
		                IsHKMT = r.IsHKMT,
		                IsOther = r.IsOther,
                           CreateTime = r.CreateTime,
                           state = (m_BLL.GetList(r.Id).Count > 0) ? "closed" : "open"
                       };


            return Json(json);
        }
        [HttpPost]
        public JsonResult GetListByParentId(string id)
        {
            if (id == null)
                id = "0";
            List<SysAreasModel> list = m_BLL.GetList(id);
            StringBuilder sb = new StringBuilder("");
            foreach (var i in list)
            {
                sb.AppendFormat("<option value='{0}'>{1}</option>", i.Id, i.Name);
            }

            return Json(sb.ToString());
        }
        #region 创建
        [SupportFilter]
        public ActionResult Create(string id)
        {
            
            SysAreasModel entity = new SysAreasModel()
            {
                ParentId = id,
                Enable = true
            };
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Create(SysAreasModel model)
        {
            model.Id = ResultHelper.NewId;
            model.CreateTime = ResultHelper.NowTime;
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.Create(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "创建", "SysAreas");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "创建", "SysAreas");
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
            
            SysAreasModel entity = m_BLL.GetById(id);
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Edit(SysAreasModel model)
        {
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.Edit(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "修改", "SysAreas");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "修改", "SysAreas");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":"+ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
            }
        }
        #endregion

        #region 详细

        public ActionResult Details(string id)
        {
            //获取父级
            List<SysAreasModel> list = m_BLL.GetList("0");

            foreach (var model in list)
            {
                model.clildren = m_BLL.GetList(model.Id);
                foreach (var m in model.clildren)
                {
                    m.clildren = m_BLL.GetList(m.Id);
                }
            }

            return View(list);
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
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "SysAreas");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "SysAreas");
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
