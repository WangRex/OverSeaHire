using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Common;
using Apps.IBLL;
using Apps.Models.Sys;
using Microsoft.Practices.Unity;
using Apps.Web;
using Apps.Web.Core;
using Apps.Locale;
using Apps.IBLL.Sys;
using Apps.BLL.Sys;

namespace Apps.Web.Controllers
{
    public class SysPositionController : BaseController
    {
        #region BLLs
        [Dependency]
        public SysPositionBLL m_BLL { get; set; }
        [Dependency]
        public SysRoleBLL _SysRoleBLL { get; set; }
        [Dependency]
        public ISysStructBLL structBLL { get; set; }
        #endregion

        ValidationErrors errors = new ValidationErrors();

        #region 列表
        [SupportFilter]
        public ActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public JsonResult GetList(GridPager pager, string queryStr, string DepId)
        {
            //获取当前登录用户，如果是指定的人事管理用户账号，则需要按照部门显示用户
            var _Account = GetAccount();
            if (!_SysRoleBLL.ToBeCheckAuthorityRole(_Account.RoleId, "超级管理员") && !_SysRoleBLL.ToBeCheckAuthorityRole(_Account.RoleId, "总账号"))
            {
                if (string.IsNullOrEmpty(DepId))
                {
                    DepId = structBLL.GetCompanyId(_Account.DepId);
                }
            }
            List<SysPositionModel> list = m_BLL.GetList(ref pager, queryStr, DepId);
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new SysPositionModel()
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Remark = r.Remark,
                            Sort = r.Sort,
                            CreateTime = r.CreateTime,
                            Enable = r.Enable,
                            MemberCount = r.MemberCount,
                            DepId = r.DepId,
                            DepName = structBLL.GetFullDepName(r.DepId),
                            ComName = structBLL.GetCompanyName(r.DepId)
                        }).ToArray()
            };
            return Json(json);
        }
        [HttpPost]
        public JsonResult GetPosListByComTree(string depId)
        {
            List<SysPositionModel> list = m_BLL.GetPosListByDepId(ref setNoPagerAscBySort, depId);
            var json = from r in list
                       select new SysPositionEditModel()
                       {
                           id = r.Id,
                           text = r.Name,
                           state = "open"
                       };


            return Json(json);
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
        public JsonResult Create(SysPositionModel model)
        {
            string ErrorCol = "";
            if (string.IsNullOrEmpty(model.DepId))
            {
                ErrorCol = "部门不能为空";
                LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "创建", "SysPosition");
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
            }
            string[] DepIds = model.DepId.Split(',');
            if (DepIds.Length > 0)
            {
                for (int i = 0; i < DepIds.Length; i++)
                {
                    var _NowDT = ResultHelper.NowTime;
                    model.Id = ResultHelper.NewId;
                    model.CreateTime = _NowDT;
                    model.CreateUserName = GetUserId();
                    model.ModificationTime = _NowDT;
                    model.DepId = DepIds[i];
                    if (model != null)
                    {
                        //判断，如果相同部门下，有相同的名字的职位，不允许创建
                        var _SysPosCheck = m_BLL.m_Rep.Find(EF => EF.Name.Equals(model.Name) && EF.DepId.Equals(model.DepId));
                        if (null != _SysPosCheck)
                        {
                            var Dept = structBLL.GetById(model.DepId);
                            ErrorCol += "部门" + Dept.Name + "下已经存在" + model.Name + "职位";
                            LogHandler.WriteServiceLog(GetUserId(), "部门" + Dept.Name + "下已经存在职位" + model.Name, "失败", "创建", "SysPosition");
                            return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + "," + ErrorCol));
                        }
                        if (m_BLL.Create(ref errors, model))
                        {
                            LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "创建", "SysPosition");
                        }
                        else
                        {
                            ErrorCol += errors.Error;
                            LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "创建", "SysPosition");
                            return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
                        }
                    }
                }
                if (string.IsNullOrEmpty(ErrorCol))
                {
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(1, ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, "部门不能为空"));
            }
        }
        #endregion

        #region 修改
        [SupportFilter]
        public ActionResult Edit(string id)
        {

            SysPositionModel entity = m_BLL.GetById(id);
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Edit(SysPositionModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.ModificationUserName = GetUserId();
                model.ModificationTime = ResultHelper.NowTime;
                //判断，如果相同部门下，有相同的名字的职位，不允许创建
                var _SysPosCheck = m_BLL.m_Rep.Find(EF => EF.Name.Equals(model.Name) && EF.DepId.Equals(model.DepId) && EF.Id != model.Id);
                if (null != _SysPosCheck)
                {
                    var Dept = structBLL.GetById(model.DepId);
                    var ErrorCol = "部门" + Dept.Name + "下已经存在" + model.Name + "职位";
                    LogHandler.WriteServiceLog(GetUserId(), "部门" + Dept.Name + "下已经存在职位" + model.Name, "失败", "修改", "SysPosition");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + "," + ErrorCol));
                }
                if (m_BLL.Edit(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "修改", "SysPosition");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "修改", "SysPosition");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":" + ErrorCol));
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

            SysPositionModel entity = m_BLL.GetById(id);
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
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "SysPosition");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "SysPosition");
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
