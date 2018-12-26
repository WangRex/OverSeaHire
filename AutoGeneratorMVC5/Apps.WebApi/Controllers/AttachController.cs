using Apps.BLL.Sys;
using Apps.Common;
using Apps.Models;
using Apps.Models.Sys;
using Apps.WebApi.Core;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Apps.WebApi.Controllers
{
    /// <summary>
    /// 附件
    /// </summary>
    public class AttachController : BaseApiController
    {
        #region BLLs
        /// <summary>
        /// 附件
        /// </summary>
        [Dependency]
        public SysAttachBLL _SysAttachBLL { get; set; }
        #endregion

        #region 创建/编辑附件
        /// <summary>
        /// 创建/编辑附件
        /// EnumType:2身份证正面,3身份证反面
        /// </summary>
        /// <param name="_SysAttachViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public object CreateAttach(SysAttachViewModel _SysAttachViewModel)
        {
            LogHandler.WriteServiceLog("API", _SysAttachViewModel.ToString(), "成功", "进入方法", "AttachController.CreateAttach()");
            Response response = new Response();
            SysAttach _SysAttach = new SysAttach();
            if (!string.IsNullOrEmpty(_SysAttachViewModel.Id))
            {
                _SysAttach = _SysAttachBLL.m_Rep.Find(SA => SA.Id.Equals(_SysAttachViewModel.Id));
                if (null != _SysAttach)
                {
                    _SysAttach.ModificationTime = ResultHelper.NowTime;
                    _SysAttach.BusinessID = _SysAttachViewModel.BusinessID;
                    _SysAttach.AttachPath = _SysAttachViewModel.AttachPath;
                    _SysAttach.FileName = _SysAttachViewModel.FileName;
                    _SysAttach.ExtName = Utils.GetPhotoExt(_SysAttachViewModel.AttachPath);
                    _SysAttach.EnumType = _SysAttachViewModel.EnumType;
                    if (_SysAttachBLL.m_Rep.Edit(_SysAttach))
                    {
                        LogHandler.WriteServiceLog("API", "编辑附件信息成功", "成功", "编辑附件", "AttachController.CreateAttach()");
                        response.Code = 0;
                        response.Message = "编辑附件信息成功";
                        response.Data = _SysAttach;
                    }
                    else
                    {
                        LogHandler.WriteServiceLog("API", "编辑附件信息失败", "失败", "编辑附件", "AttachController.CreateAttach()");
                        response.Code = 2;
                        response.Message = "编辑附件信息失败";
                        response.Data = null;
                    }
                }
            }
            _SysAttach = _SysAttachBLL.m_Rep.Find(SA => SA.BusinessID.Equals(_SysAttachViewModel.BusinessID) && SA.FileName.Equals(_SysAttachViewModel.FileName) && SA.EnumType.Equals(_SysAttachViewModel.EnumType));
            if (null != _SysAttach)
            {
                _SysAttach.ModificationTime = ResultHelper.NowTime;
                _SysAttach.AttachPath = _SysAttachViewModel.AttachPath;
                _SysAttach.ExtName = Utils.GetPhotoExt(_SysAttachViewModel.AttachPath);
                if (_SysAttachBLL.m_Rep.Edit(_SysAttach))
                {
                    LogHandler.WriteServiceLog("API", "编辑附件信息成功", "成功", "编辑附件", "AttachController.CreateAttach()");
                    response.Code = 0;
                    response.Message = "编辑附件信息成功";
                    response.Data = _SysAttach;
                }
                else
                {
                    LogHandler.WriteServiceLog("API", "编辑附件信息失败", "失败", "编辑附件", "AttachController.CreateAttach()");
                    response.Code = 2;
                    response.Message = "编辑附件信息失败";
                    response.Data = null;
                }
            }
            else
            {
                _SysAttach = new SysAttach();
                _SysAttach.Id = ResultHelper.NewId;
                _SysAttach.CreateTime = ResultHelper.NowTime;
                _SysAttach.ModificationTime = ResultHelper.NowTime;
                _SysAttach.BusinessID = _SysAttachViewModel.BusinessID;
                _SysAttach.AttachPath = _SysAttachViewModel.AttachPath;
                _SysAttach.FileName = _SysAttachViewModel.FileName;
                _SysAttach.ExtName = Utils.GetPhotoExt(_SysAttachViewModel.AttachPath);
                _SysAttach.EnumType = _SysAttachViewModel.EnumType;
                if (_SysAttachBLL.m_Rep.Create(_SysAttach))
                {
                    LogHandler.WriteServiceLog("API", "创建附件信息成功", "成功", "创建附件", "AttachController.CreateAttach()");
                    response.Code = 0;
                    response.Message = "创建附件信息成功";
                    response.Data = _SysAttach;
                }
                else
                {
                    LogHandler.WriteServiceLog("API", "创建附件信息失败", "失败", "创建附件", "AttachController.CreateAttach()");
                    response.Code = 2;
                    response.Message = "创建附件信息失败";
                    response.Data = null;
                }
            }
            return Json(response);
        }
        #endregion

        #region 删除附件
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="AttachId"></param>
        /// <returns></returns>
        [HttpGet]
        public object DeleteAttach(string AttachId)
        {
            LogHandler.WriteServiceLog("API", "AttachId:" + AttachId, "成功", "进入方法", "AttachController.DeleteAttach()");
            Response response = new Response();
            SysAttach _SysAttach = _SysAttachBLL.m_Rep.Find(SA => SA.Id.Equals(AttachId));
            if (null == _SysAttach)
            {
                LogHandler.WriteServiceLog("API", "要删除的附件不存在", "失败", "删除附件", "AttachController.DeleteAttach()");
                response.Code = 2;
                response.Message = "要删除的附件不存在";
                response.Data = null;
                return Json(response);
            }
            if (_SysAttachBLL.m_Rep.Delete(_SysAttach))
            {
                LogHandler.WriteServiceLog("API", "删除附件信息成功", "成功", "删除附件", "AttachController.DeleteAttach()");
                response.Code = 0;
                response.Message = "删除附件信息成功";
                response.Data = _SysAttach;
            }
            else
            {
                LogHandler.WriteServiceLog("API", "删除附件信息失败", "失败", "删除附件", "AttachController.DeleteAttach()");
                response.Code = 2;
                response.Message = "删除附件信息失败";
                response.Data = null;
            }
            return Json(response);
        }
        #endregion

        #region 获取附件列表
        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <param name="_GridPager"></param>
        /// <param name="queryStr"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetAttachListGrid(string BusinessID, GridPager _GridPager, string queryStr)
        {
            LogHandler.WriteServiceLog("API", "BusinessID:" + BusinessID + ",_GridPager:" + _GridPager + ",queryStr:" + queryStr, "成功", "进入方法", "AttachController.GetAttachListGrid()");
            Response response = new Response();
            var _SysAttachList = _SysAttachBLL.m_Rep.FindList(SA => SA.BusinessID.Equals(BusinessID)).OrderByDescending(SA => SA.ModificationTime).ToList();
            if (!string.IsNullOrEmpty(queryStr))
            {
                _SysAttachList = _SysAttachList.Where(EF => EF.FileName.Contains(queryStr)).ToList();
            }
            var _ISysAttachList = _SysAttachList.Select(SA =>
                new SysAttachViewModel
                {
                    Id = SA.Id,
                    BusinessID = SA.BusinessID,
                    AttachPath = SA.AttachPath,
                    FileName = SA.FileName,
                    ExtName = SA.ExtName,
                });
            if (_ISysAttachList.Count() == 0)
            {
                LogHandler.WriteServiceLog("API", "获取的附件列表为空", "成功", "附件列表", "AttachController.GetAttachList()");
                return Json(new
                {
                    rows = _ISysAttachList.ToList(),
                    total = 0
                });
            }
            var total = _ISysAttachList.Count();
            if (_GridPager.page <= 1)
            {
                _ISysAttachList = _ISysAttachList.Take(_GridPager.rows);
            }
            else
            {
                _ISysAttachList = _ISysAttachList.Skip((_GridPager.page - 1) * _GridPager.rows).Take(_GridPager.rows);
            }
            LogHandler.WriteServiceLog("API", "获取附件列表成功", "成功", "附件列表", "AttachController.GetAttachList()");
            return Json(new
            {
                rows = _ISysAttachList.ToList(),
                total = total
            });
        }
        #endregion

        #region 获取附件列表
        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <param name="_GridPager"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetAttachList(string BusinessID, GridPager _GridPager)
        {
            LogHandler.WriteServiceLog("API", "BusinessID:" + BusinessID + ",_GridPager:" + _GridPager, "成功", "进入方法", "AttachController.GetAttachList()");
            Response response = new Response();
            var _ISysAttachList = _SysAttachBLL.m_Rep.FindList(SA => SA.BusinessID.Equals(BusinessID)).OrderByDescending(SA => SA.ModificationTime).ToList().Select(SA =>
                new SysAttachViewModel
                {
                    Id = SA.Id,
                    BusinessID = SA.BusinessID,
                    AttachPath = SA.AttachPath,
                    FileName = SA.FileName,
                    ExtName = SA.ExtName,
                });
            if (_ISysAttachList.Count() == 0)
            {
                LogHandler.WriteServiceLog("API", "获取的附件列表为空", "成功", "附件列表", "AttachController.GetAttachList()");
                response.Code = 1;
                response.Message = "获取的附件列表为空";
                response.Data = null;
                return Json(response);
            }
            var total = _ISysAttachList.Count();
            if (_GridPager.page <= 1)
            {
                _ISysAttachList = _ISysAttachList.Take(_GridPager.rows);
            }
            else
            {
                _ISysAttachList = _ISysAttachList.Skip((_GridPager.page - 1) * _GridPager.rows).Take(_GridPager.rows);
            }

            LogHandler.WriteServiceLog("API", "获取附件列表成功", "成功", "附件列表", "AttachController.GetAttachList()");
            response.Code = 0;
            response.Message = "获取附件列表成功";
            response.Data = new
            {
                rows = _ISysAttachList.ToList(),
                total = total
            };
            return Json(response);
        }
        #endregion

        #region 获取附件
        /// <summary>
        /// 获取附件
        /// </summary>
        /// <param name="AttachId"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetAttach(string AttachId)
        {
            LogHandler.WriteServiceLog("API", "AttachId:" + AttachId, "成功", "进入方法", "AttachController.GetAttach()");
            Response response = new Response();
            SysAttach _SysAttach = _SysAttachBLL.m_Rep.Find(SA => SA.Id.Equals(AttachId));
            if (null == _SysAttach)
            {
                LogHandler.WriteServiceLog("API", "附件不存在", "失败", "删除附件", "AttachController.GetAttach()");
                response.Code = 2;
                response.Message = "附件不存在";
                response.Data = null;
                return Json(response);
            }
            LogHandler.WriteServiceLog("API", "获取附件信息成功", "成功", "获取附件", "AttachController.DeleteAttach()");
            response.Code = 0;
            response.Message = "获取附件信息成功";
            response.Data = _SysAttach;
            return Json(response);
        }
        #endregion
    }
}