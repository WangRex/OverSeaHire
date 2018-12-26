using Apps.BLL.App;
using Apps.BLL.Sys;
using Apps.Common;
using Apps.Models.App;
using Apps.WebApi.Core;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Apps.WebApi.Controllers
{
    /// <summary>
    /// 应聘申请
    /// </summary>
    public class ApplyJobController : BaseApiController
    {
        #region BLLs
        /// <summary>
        /// 系统用户
        /// </summary>
        [Dependency]
        public SysUserBLL _SysUserBLL { get; set; }
        /// <summary>
        /// 需求
        /// </summary>
        [Dependency]
        public App_RequirementBLL app_RequirementBLL { get; set; }
        /// <summary>
        /// 应聘申请
        /// </summary>
        [Dependency]
        public App_ApplyJobBLL app_ApplyJobBLL { get; set; }
        /// <summary>
        /// 应聘申请步骤
        /// </summary>
        [Dependency]
        public App_ApplyJobStepBLL applyJobStepBLL { get; set; }
        #endregion

        #region 提交应聘申请
        /// <summary>
        /// 提交应聘申请
        /// </summary>
        /// <param name="applyJobPost"></param>
        /// <returns></returns>
        [HttpPost]
        public object CreateApplyJob(ApplyJobPost applyJobPost)
        {
            LogHandler.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString(), "开始", "CreateApplyJob", "ApplyJobController");
            string ErrorMsg = "";
            var applyJobId = app_ApplyJobBLL.CreateApplyJob(applyJobPost, ref ErrorMsg);
            LogHandler.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "CreateApplyJob", "ApplyJobController");
            if (!string.IsNullOrEmpty(applyJobId))
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, applyJobId, 1)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
        }
        #endregion

        #region 修改应聘申请
        /// <summary>
        /// 修改应聘申请
        /// </summary>
        /// <param name="editApplyJobPost"></param>
        /// <returns></returns>
        [HttpPost]
        public object EditApplyJob(EditApplyJobPost editApplyJobPost)
        {
            LogHandler.WriteServiceLog(editApplyJobPost.UserId, editApplyJobPost.ToString(), "开始", "EditApplyJob", "ApplyJobController");
            string ErrorMsg = "";
            var applyJobVm = app_ApplyJobBLL.EditApplyJob(editApplyJobPost, ref ErrorMsg);
            LogHandler.WriteServiceLog(editApplyJobPost.UserId, editApplyJobPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "EditApplyJob", "ApplyJobController");
            if (applyJobVm != null)
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, applyJobVm, 1)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
        }
        #endregion

        #region 获取应聘申请
        /// <summary>
        /// 获取应聘申请
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="CustomerId"></param>
        /// <param name="PageNum"></param>
        /// <param name="RecordNum"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetApplyJobs(string UserId, string CustomerId, int PageNum, int RecordNum)
        {
            LogHandler.WriteServiceLog(UserId, "CustomerId:" + CustomerId + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetApplyJobs", "ApplyJobController");
            string ErrorMsg = "";
            int DataCount = 0;
            var applyJobVms = app_ApplyJobBLL.GetApplyJobs(UserId, CustomerId, PageNum, RecordNum, ref DataCount, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "CustomerId:" + CustomerId + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetApplyJobs", "ApplyJobController");
            if (applyJobVms != null)
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, applyJobVms, DataCount)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
        }
        #endregion

        #region 获取应聘申请流程
        /// <summary>
        /// 获取应聘申请流程
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetApplyStep(string UserId)
        {
            LogHandler.WriteServiceLog(UserId, "", "开始", "GetApplyStep", "ApplyJobController");
            string ErrorMsg = "";
            var applySteps = applyJobStepBLL.GetApplyStep(UserId, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "ErrorMsg:" + ErrorMsg, "结束", "GetApplyStep", "ApplyJobController");
            if (applySteps == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, applySteps)
                    );
            }
        }
        #endregion

        #region 获取应聘申请详情
        /// <summary>
        /// 获取应聘申请详情
        /// </summary>
        /// <param name="UserId">登录人信息</param>
        /// <param name="ApplyJobId">应聘申请主键</param>
        /// <param name="Longitude">当前位置经度</param>
        /// <param name="Latitude">当前位置纬度</param>
        /// <returns></returns>
        [HttpGet]
        public object GetApplyJob(string UserId, string ApplyJobId, string Longitude = null, string Latitude = null)
        {
            LogHandler.WriteServiceLog(UserId, "ApplyJobId:" + ApplyJobId + ",Longitude:" + Longitude + ",Latitude:" + Latitude, "开始", "GetApplyJob", "ApplyJobController");
            string ErrorMsg = "";
            var applyJobVms = app_ApplyJobBLL.GetApplyJob(UserId, ApplyJobId, Longitude, Latitude, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "ApplyJobId:" + ApplyJobId + ",Longitude:" + Longitude + ",Latitude:" + Latitude + ",ErrorMsg:" + ErrorMsg, "结束", "GetApplyJob", "ApplyJobController");
            if (applyJobVms != null)
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, applyJobVms, 1)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
        }
        #endregion

        #region 获取应聘申请步骤操作记录列表
        /// <summary>
        /// 获取应聘申请步骤操作记录列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ApplyJobId"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetApplyJobRecords(string UserId, string ApplyJobId)
        {
            LogHandler.WriteServiceLog(UserId, "ApplyJobId:" + ApplyJobId, "开始", "GetApplyJobRecords", "ApplyJobController");
            string ErrorMsg = "";
            int DataCount = 0;
            var applyJobRecordVms = app_ApplyJobBLL.GetApplyJobRecords(UserId, ApplyJobId, ref DataCount, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "ApplyJobId:" + ApplyJobId + ",ErrorMsg:" + ErrorMsg, "结束", "GetApplyJobRecords", "ApplyJobController");
            if (applyJobRecordVms == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, applyJobRecordVms, DataCount)
                    );
            }
        }
        #endregion

        #region 忽略职位
        /// <summary>
        /// 忽略职位
        /// </summary>
        /// <param name="applyJobPost"></param>
        /// <returns></returns>
        [HttpPost]
        public object IgnoreApplyJob(ApplyJobPost applyJobPost)
        {
            LogHandler.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString(), "开始", "IgnoreApplyJob", "ApplyJobController");
            string ErrorMsg = "";
            var applyJobId = app_ApplyJobBLL.IgnoreApplyJob(applyJobPost, ref ErrorMsg);
            LogHandler.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "IgnoreApplyJob", "ApplyJobController");
            if (!string.IsNullOrEmpty(applyJobId))
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, applyJobId, 1)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
        }
        #endregion

        #region 个人中心--我的面试--应聘申请
        /// <summary>
        /// 个人中心--我的面试--应聘申请
        /// </summary>
        /// <param name="UserId">登录人主键</param>
        /// <param name="EmployerId">雇主主键</param>
        /// <param name="Flag">0:应聘申请, 1:面试中的</param>
        /// <param name="PageNum"></param>
        /// <param name="RecordNum"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetApplyMyJobs(string UserId, string EmployerId, string Flag, int PageNum, int RecordNum)
        {
            LogHandler.WriteServiceLog(UserId, "EmployerId:" + EmployerId + ",Flag:" + Flag + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetApplyMyJobs", "ApplyJobController");
            string ErrorMsg = "";
            int DataCount = 0;
            var applyJobUserVms = app_ApplyJobBLL.GetApplyMyJobs(UserId, EmployerId, Flag, PageNum, RecordNum, ref DataCount, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "EmployerId:" + EmployerId + ",Flag:" + Flag + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetApplyMyJobs", "ApplyJobController");
            if (applyJobUserVms != null)
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, applyJobUserVms, DataCount)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
        }
        #endregion
    }
}