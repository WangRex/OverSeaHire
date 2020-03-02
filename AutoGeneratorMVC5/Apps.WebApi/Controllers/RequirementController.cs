using Apps.BLL.App;
using Apps.BLL.MIS;
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
    /// 需求
    /// </summary>
    public class RequirementController : BaseApiController
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
        #endregion

        #region 获取需求列表
        /// <summary>
        /// 获取需求列表
        /// </summary>
        /// <param name="requireSearchForm"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetRequirementList(RequireSearchForm requireSearchForm)
        {
            LogHandler.WriteServiceLog(requireSearchForm.UserId, requireSearchForm.ToString(), "开始", "GetRequirementList", "RequirementController");
            string ErrorMsg = "";
            int DataCount = 0;
            var app_Requirements = app_RequirementBLL.GetApp_Requirements(requireSearchForm, ref DataCount, ref ErrorMsg);
            LogHandler.WriteServiceLog(requireSearchForm.UserId, requireSearchForm.ToString() + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetRequirementList", "RequirementController");
            if (app_Requirements == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, app_Requirements, DataCount)
                    );
            }
        }
        #endregion

        #region 获取需求详情
        /// <summary>
        /// 获取需求详情
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RequirementId"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetRequirement(string UserId, string RequirementId)
        {
            LogHandler.WriteServiceLog(UserId, "RequirementId:" + RequirementId, "开始", "GetRequirement", "RequirementController");
            string ErrorMsg = "";
            var app_Requirement = app_RequirementBLL.GetApp_Requirement(UserId, RequirementId, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",ErrorMsg:" + ErrorMsg, "结束", "GetRequirement", "RequirementController");
            if (app_Requirement == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, app_Requirement)
                    );
            }
        }
        #endregion

        #region 获取热搜列表
        /// <summary>
        /// 获取热搜列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="PageNum"></param>
        /// <param name="RecordNum"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetHotSearches(string UserId, int PageNum, int RecordNum)
        {
            LogHandler.WriteServiceLog(UserId, "PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetHotSearches", "RequirementController");
            string ErrorMsg = "";
            int DataCount = 0;
            var app_Requirements = app_RequirementBLL.GetHotSearches(UserId, PageNum, RecordNum, ref DataCount, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetHotSearches", "RequirementController");
            if (app_Requirements == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, app_Requirements, DataCount)
                    );
            }
        }
        #endregion

        #region 获取国家列表
        /// <summary>
        /// 获取国家列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="PageNum"></param>
        /// <param name="RecordNum"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetCountries(string UserId, int PageNum, int RecordNum)
        {
            LogHandler.WriteServiceLog(UserId, "PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetCountries", "RequirementController");
            string ErrorMsg = "";
            int DataCount = 0;
            var countryVms = app_RequirementBLL.GetCountries(UserId, PageNum, RecordNum, ref DataCount, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetCountries", "RequirementController");
            if (countryVms == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, countryVms, DataCount)
                    );
            }
        }
        #endregion

        #region 收藏需求
        /// <summary>
        /// 收藏需求
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RequirementId"></param>
        /// <returns></returns>
        [HttpGet]
        public object CollectRequirement(string UserId, string RequirementId)
        {
            LogHandler.WriteServiceLog(UserId, "RequirementId:" + RequirementId, "开始", "CollectRequirement", "RequirementController");
            string ErrorMsg = "";
            var collectRequirementId = app_RequirementBLL.CollectRequirement(UserId, RequirementId, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",ErrorMsg:" + ErrorMsg, "结束", "CollectRequirement", "RequirementController");
            if (string.IsNullOrEmpty(collectRequirementId))
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, collectRequirementId, 1)
                    );
            }
        }
        #endregion

        #region 取消收藏需求
        /// <summary>
        /// 取消收藏需求
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RequirementCollectId"></param>
        /// <returns></returns>
        [HttpGet]
        public object UnCollectRequirement(string UserId, string RequirementCollectId)
        {
            LogHandler.WriteServiceLog(UserId, "RequirementCollectId:" + RequirementCollectId, "开始", "UnCollectRequirement", "RequirementController");
            string ErrorMsg = "";
            var flag = app_RequirementBLL.UnCollectRequirement(UserId, RequirementCollectId, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "RequirementCollectId:" + RequirementCollectId + ",ErrorMsg:" + ErrorMsg, "结束", "UnCollectRequirement", "RequirementController");
            if (!flag)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, flag, 1)
                    );
            }
        }
        #endregion

        #region 收藏需求
        /// <summary>
        /// 收藏需求
        /// </summary>
        /// <param name="requireSearchForm"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetRequirementCollections(RequireSearchForm requireSearchForm)
        {
            LogHandler.WriteServiceLog(requireSearchForm.UserId, requireSearchForm.ToString(), "开始", "GetRequirementCollections", "RequirementController");
            string ErrorMsg = "";
            int DataCount = 0;
            var app_Requirements = app_RequirementBLL.GetRequirementCollections(requireSearchForm, ref DataCount, ref ErrorMsg);
            LogHandler.WriteServiceLog(requireSearchForm.UserId, requireSearchForm.ToString() + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetRequirementCollections", "RequirementController");
            if (app_Requirements == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, app_Requirements, DataCount)
                    );
            }
        }
        #endregion

        #region 应聘过的需求
        /// <summary>
        /// 应聘过的需求
        /// </summary>
        /// <param name="requireSearchForm"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetRequirementApplieds(RequireSearchForm requireSearchForm)
        {
            LogHandler.WriteServiceLog(requireSearchForm.UserId, requireSearchForm.ToString(), "开始", "GetRequirementApplieds", "RequirementController");
            string ErrorMsg = "";
            int DataCount = 0;
            var app_Requirements = app_RequirementBLL.GetRequirementApplieds(requireSearchForm, ref DataCount, ref ErrorMsg);
            LogHandler.WriteServiceLog(requireSearchForm.UserId, requireSearchForm.ToString() + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetRequirementApplieds", "RequirementController");
            if (app_Requirements == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, app_Requirements, DataCount)
                    );
            }
        }
        #endregion

        #region 发布需求
        /// <summary>
        /// 发布需求
        /// </summary>
        /// <param name="requirementPost"></param>
        /// <returns></returns>
        [HttpPost]
        public object CreateRequirement(RequirementPost requirementPost)
        {
            LogHandler.WriteServiceLog(requirementPost.UserId, requirementPost.ToString(), "开始", "CreateRequirement", "RequirementController");
            string ErrorMsg = "";
            var flag = app_RequirementBLL.CreateRequirement(requirementPost, ref ErrorMsg);
            LogHandler.WriteServiceLog(requirementPost.UserId, requirementPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "CreateRequirement", "RequirementController");
            if (!flag)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, flag, 1)
                    );
            }
        }
        #endregion

        #region 编辑初始化需求
        /// <summary>
        /// 编辑初始化需求
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RequirementId"></param>
        /// <returns></returns>
        [HttpGet]
        public object EditRequirementInit(string UserId, string RequirementId)
        {
            LogHandler.WriteServiceLog(UserId, "RequirementId:" + RequirementId, "开始", "EditRequirementInit", "RequirementController");
            string ErrorMsg = "";
            var app_Requirement = app_RequirementBLL.EditRequirementInit(UserId, RequirementId, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",ErrorMsg:" + ErrorMsg, "结束", "EditRequirementInit", "RequirementController");
            if (app_Requirement == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, app_Requirement)
                    );
            }
        }
        #endregion

        #region 修改发布需求
        /// <summary>
        /// 修改发布需求
        /// </summary>
        /// <param name="requirementPost"></param>
        /// <returns></returns>
        [HttpPost]
        public object EditRequirement(RequirementPost requirementPost)
        {
            LogHandler.WriteServiceLog(requirementPost.UserId, requirementPost.ToString(), "开始", "EditRequirement", "RequirementController");
            string ErrorMsg = "";
            var flag = app_RequirementBLL.EditRequirement(requirementPost, ref ErrorMsg);
            LogHandler.WriteServiceLog(requirementPost.UserId, requirementPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "EditRequirement", "RequirementController");
            if (!flag)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, flag, 1)
                    );
            }
        }
        #endregion

        #region 删除需求
        /// <summary>
        /// 删除需求
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RequirementId"></param>
        /// <returns></returns>
        [HttpGet]
        public object DeleteRequirement(string UserId, string RequirementId)
        {
            LogHandler.WriteServiceLog(UserId, "RequirementId:" + RequirementId, "开始", "DeleteRequirement", "RequirementController");
            string ErrorMsg = "";
            var flag = app_RequirementBLL.DeleteRequirement(UserId, RequirementId, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",ErrorMsg:" + ErrorMsg, "结束", "DeleteRequirement", "RequirementController");
            if (!flag)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, flag, 1)
                    );
            }
        }
        #endregion

        #region 获取职位树
        /// <summary>
        /// 获取职位树
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetPositionTree(string UserId)
        {
            LogHandler.WriteServiceLog(UserId, "", "开始", "GetPositionTree", "RequirementController");
            string ErrorMsg = "";
            var positionTreeVms = app_RequirementBLL.GetPositionTree(UserId, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "ErrorMsg:" + ErrorMsg, "结束", "GetPositionTree", "RequirementController");
            if (positionTreeVms == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, positionTreeVms)
                    );
            }
        }
        #endregion

        #region 雇主端推荐人列表
        /// <summary>
        /// 雇主端推荐人列表
        /// </summary>
        /// <param name="recommendUserSearchForm"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetRecommendUserList(RecommendUserSearchForm recommendUserSearchForm)
        {
            LogHandler.WriteServiceLog(recommendUserSearchForm.UserId, recommendUserSearchForm.ToString(), "开始", "GetRecommendUserList", "RequirementController");
            string ErrorMsg = "";
            int DataCount = 0;
            var app_Requirements = app_RequirementBLL.GetRecommendUserList(recommendUserSearchForm, ref DataCount, ref ErrorMsg);
            LogHandler.WriteServiceLog(recommendUserSearchForm.UserId, recommendUserSearchForm.ToString() + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetRecommendUserList", "RequirementController");
            if (app_Requirements == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, app_Requirements, DataCount)
                    );
            }
        }
        #endregion

        #region 雇主邀请工人
        /// <summary>
        /// 雇主邀请工人
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="RequirementId"></param>
        /// <param name="WorkerId"></param>
        /// <returns></returns>
        [HttpGet]
        public object InviteWorker(string CustomerId, string RequirementId, string WorkerId)
        {
            LogHandler.WriteServiceLog(CustomerId, "RequirementId:" + RequirementId + ",WorkerId:" + WorkerId, "开始", "InviteWorker", "RequirementController");
            string ErrorMsg = "";
            var flag = app_RequirementBLL.InviteWorker(CustomerId, RequirementId, WorkerId, ref ErrorMsg);
            LogHandler.WriteServiceLog(CustomerId, "RequirementId:" + RequirementId + ",WorkerId:" + WorkerId + ",ErrorMsg:" + ErrorMsg, "结束", "InviteWorker", "RequirementController");
            if (!flag)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, flag)
                    );
            }
        }
        #endregion

        #region 获取面试中的工友列表
        /// <summary>
        /// 获取面试中的工友列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RequirementId"></param>
        /// <param name="Flag">申请: Apply,面试中:Interview</param>
        /// <param name="PageNum"></param>
        /// <param name="RecordNum"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetInterviewUsers(string UserId, string RequirementId, string Flag, int PageNum, int RecordNum)
        {
            LogHandler.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",Flag:" + Flag + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetInterviewUsers", "RequirementController");
            string ErrorMsg = "";
            int DataCount = 0;
            var applyJobUserVms = app_RequirementBLL.GetInterviewUsers(UserId, RequirementId, Flag, PageNum, RecordNum, ref DataCount, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",Flag:" + Flag + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",ErrorMsg:" + ErrorMsg, "结束", "GetInterviewUsers", "RequirementController");
            if (applyJobUserVms == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, applyJobUserVms, DataCount)
                    );
            }
        }
        #endregion

        #region 获取简历热搜列表
        /// <summary>
        /// 获取简历热搜列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="PageNum"></param>
        /// <param name="RecordNum"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetPosHotSearches(string UserId, int PageNum, int RecordNum)
        {
            LogHandler.WriteServiceLog(UserId, "PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetPosHotSearches", "RequirementController");
            string ErrorMsg = "";
            int DataCount = 0;
            var hotSearches = app_RequirementBLL.GetPosHotSearches(UserId, PageNum, RecordNum, ref DataCount, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetPosHotSearches", "RequirementController");
            if (hotSearches == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, hotSearches, DataCount)
                    );
            }
        }
        #endregion
    }
}
