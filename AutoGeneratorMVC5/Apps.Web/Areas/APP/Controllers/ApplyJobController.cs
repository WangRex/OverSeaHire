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
using Apps.Models;
using Apps.DAL.Sys;
using System;
using System.Linq;

namespace Apps.Web.Areas.App.Controllers
{
    public class ApplyJobController : BaseController
    {
        #region BLLs
        [Dependency]
        public IApp_ApplyJobBLL m_BLL { get; set; }
        [Dependency]
        public EnumDictionaryBLL enumDictionaryBLL { get; set; }
        [Dependency]
        public App_RequirementBLL _App_RequirementBLL { get; set; }
        [Dependency]
        public App_CustomerBLL _App_CustomerBLL { get; set; }
        [Dependency]
        public App_ApplyJobStepBLL applyJobStepBLL { get; set; }
        [Dependency]
        public App_ApplyJobRecordBLL applyJobRecordBLL { get; set; }
        [Dependency]
        public SysMessageBLL sysMessageBLL { get; set; }
        [Dependency]
        public SysMessageRepository sysMessageRepository { get; set; }
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
            List<App_ApplyJobModel> list = m_BLL.GetList(ref pager, queryStr);
            foreach (var Item in list)
            {
                Item.App_Requirement_Title = _App_RequirementBLL.GetTitle(Item.PK_App_Requirement_Title);
                var customer = _App_CustomerBLL.m_Rep.GetById(Item.PK_App_Customer_CustomerName);
                if (null != customer)
                {
                    Item.App_Customer_CustomerName = customer.CustomerName;
                }
                Item.ApplyStatus = enumDictionaryBLL.GetDicName("App_ApplyJob.EnumApplyStatus", Item.EnumApplyStatus);
                Item.EnumPromisePayWay = enumDictionaryBLL.GetDicName("App.EnumPayWay", Item.EnumPromisePayWay);
                Item.EnumServicePayWay = enumDictionaryBLL.GetDicName("App.EnumPayWay", Item.EnumServicePayWay);
                Item.EnumTailPayWay = enumDictionaryBLL.GetDicName("App.EnumPayWay", Item.EnumTailPayWay);
            }
            GridRows<App_ApplyJobModel> grs = new GridRows<App_ApplyJobModel>();
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
            ViewBag.EnumApplyStatus = new SelectList(enumDictionaryBLL.GetDropDownList("App_ApplyJob.EnumApplyStatus"), "ItemValue", "ItemName");
            ViewBag.EnumPromisePayWay = new SelectList(enumDictionaryBLL.GetDropDownList("App.EnumPayWay"), "ItemValue", "ItemName");
            ViewBag.EnumServicePayWay = new SelectList(enumDictionaryBLL.GetDropDownList("App.EnumPayWay"), "ItemValue", "ItemName");
            ViewBag.EnumTailPayWay = new SelectList(enumDictionaryBLL.GetDropDownList("App.EnumPayWay"), "ItemValue", "ItemName");
            return View();
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Create(App_ApplyJobModel model)
        {
            model.Id = ResultHelper.NewId;
            model.CreateTime = ResultHelper.NowTime;
            model.CreateUserName = GetUserId();
            model.ModificationTime = ResultHelper.NowTime;
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.Create(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "创建", "App_ApplyJob");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "创建", "App_ApplyJob");
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
            App_ApplyJobModel entity = m_BLL.GetById(id);
            ViewBag.PK_App_Requirement_Title = new SelectList(_App_RequirementBLL.m_Rep.FindList(), "Id", "Title");
            entity.App_Requirement_Title = _App_RequirementBLL.GetTitle(entity.PK_App_Requirement_Title);
            ViewBag.PK_App_Customer_CustomerName = new SelectList(_App_CustomerBLL.m_Rep.FindList(), "Id", "CustomerName");
            entity.App_Customer_CustomerName = _App_CustomerBLL.GetCustomerName(entity.PK_App_Customer_CustomerName);
            ViewBag.EnumApplyStatus = new SelectList(enumDictionaryBLL.GetDropDownList("App_ApplyJob.EnumApplyStatus"), "ItemValue", "ItemName");
            ViewBag.EnumPromisePayWay = new SelectList(enumDictionaryBLL.GetDropDownList("App.EnumPayWay"), "ItemValue", "ItemName");
            ViewBag.EnumServicePayWay = new SelectList(enumDictionaryBLL.GetDropDownList("App.EnumPayWay"), "ItemValue", "ItemName");
            ViewBag.EnumTailPayWay = new SelectList(enumDictionaryBLL.GetDropDownList("App.EnumPayWay"), "ItemValue", "ItemName");
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Edit(App_ApplyJobModel model)
        {
            string strUserId = GetUserId();
            var now = ResultHelper.NowTime;
            if (model != null && ModelState.IsValid)
            {
                model.ModificationUserName = strUserId;
                model.ModificationTime = now;
                if (m_BLL.Edit(ref errors, model))
                {
                    var customer = _App_CustomerBLL.m_Rep.GetById(model.PK_App_Customer_CustomerName);
                    customer.ModificationTime = now;
                    customer.ModificationUserName = strUserId;
                    if ("0".Equals(model.EnumApplyStatus))
                    {
                        //如果修改了申请，只要是进行中，则把用户锁定
                        customer.SwitchBtnInterview = "1";
                    }
                    else
                    {
                        //如果修改了申请，只要不是进行中，则把用户锁定解除
                        customer.SwitchBtnInterview = "0";
                    }
                    _App_CustomerBLL.m_Rep.Edit(customer);
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "修改", "App_ApplyJob");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "修改", "App_ApplyJob");
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

        #region 下一步
        [SupportFilter]
        public ActionResult NextStep(string ApplyJobId)
        {
            App_ApplyJobModel entity = m_BLL.GetById(ApplyJobId);
            ViewBag.Result = new SelectList(enumDictionaryBLL.GetDropDownList("App_ApplyJobRecord.Result"), "ItemValue", "ItemName");
            var iNextStep = Utils.ObjToInt(entity.CurrentStep, 0) + 1;
            var now = ResultHelper.NowTime;
            App_ApplyJobRecordModel applyJobRecordModel = new App_ApplyJobRecordModel()
            {
                Id = ResultHelper.NewId,
                CreateTime = now,
                CreateUserName = GetUserId(),
                ModificationTime = now,
                ModificationUserName = GetUserId(),
                PK_App_ApplyJob_Id = ApplyJobId,
                PK_App_Customer_CustomerName = entity.PK_App_Customer_CustomerName,
                Step = iNextStep.ToString(),
                Result = "进行中",
                Content = applyJobStepBLL.GetStepName(iNextStep.ToString()) + "进行中",
            };
            if (iNextStep == 9)
            {
                applyJobRecordModel.Result = "已完成";
            }
            return View(applyJobRecordModel);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult NextStep(App_ApplyJobRecordModel model)
        {
            string strUserId = GetUserId();
            if (model != null && ModelState.IsValid)
            {
                var rtnFlag = m_BLL.NextStep(strUserId, model);
                if (rtnFlag)
                {
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
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
            App_ApplyJobModel entity = m_BLL.GetById(id);
            entity.PK_App_Requirement_Title = _App_RequirementBLL.GetTitle(entity.PK_App_Requirement_Title);
            var customer = _App_CustomerBLL.m_Rep.GetById(entity.PK_App_Customer_CustomerName);
            if (null != customer)
            {
                entity.App_Customer_CustomerName = customer.CustomerName;
            }
            entity.EnumApplyStatus = enumDictionaryBLL.GetDicName("App_ApplyJob.EnumApplyStatus", entity.EnumApplyStatus);
            entity.EnumPromisePayWay = enumDictionaryBLL.GetDicName("App.EnumPayWay", entity.EnumPromisePayWay);
            entity.EnumServicePayWay = enumDictionaryBLL.GetDicName("App.EnumPayWay", entity.EnumServicePayWay);
            entity.EnumTailPayWay = enumDictionaryBLL.GetDicName("App.EnumPayWay", entity.EnumTailPayWay);
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
                //判断如果还在进行中，则不允许删除
                var model = m_BLL.GetById(id);
                if (model.EnumApplyStatus == "0")
                {
                    string ErrorCol = "面试还在进行中，不可删除";
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "App_ApplyJob");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
                if (m_BLL.Delete(ref errors, id))
                {
                    //删除所有的关联记录数据
                    var applyJobRecordIds = applyJobRecordBLL.m_Rep.FindList(EF => EF.PK_App_ApplyJob_Id == id).Select(EF => EF.Id);
                    applyJobRecordBLL.m_Rep.Delete(applyJobRecordIds);
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "App_ApplyJob");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "App_ApplyJob");
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

        #region 拒绝应聘申请
        /// <summary>
        /// 拒绝应聘申请
        /// </summary>
        /// <param name="applyJobId"></param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter]
        public JsonResult RejectApplyJob(string applyJobId)
        {
            LogHandler.WriteServiceLog(GetUserId(), "applyJobId:" + applyJobId, "开始", "RejectApplyJob", "App_ApplyJob");
            var now = ResultHelper.NowTime;
            string strUserId = GetUserId();
            //首先更新申请主信息
            App_ApplyJobModel entity = m_BLL.GetById(applyJobId);
            entity.ModificationTime = now;
            entity.ModificationUserName = strUserId;
            entity.EnumApplyStatus = "4";
            try
            {
                m_BLL.Edit(ref errors, entity);
                var customer = _App_CustomerBLL.m_Rep.GetById(entity.PK_App_Customer_CustomerName);
                //如果拒绝了申请，则把用户锁定解除
                customer.ModificationTime = now;
                customer.ModificationUserName = strUserId;
                customer.SwitchBtnInterview = "0";
                _App_CustomerBLL.m_Rep.Edit(customer);
                var account = GetAccount();
                sysMessageRepository.CrtSysMessage(account.Id, entity.PK_App_Customer_CustomerName, applyJobId, "应聘申请提醒", "非常遗憾，您的面试申请被驳回，请选择其他职位或重新提交", "1", "0", "");
                LogHandler.WriteServiceLog(GetUserId(), "applyJobId:" + applyJobId + ",ErrorMsg:拒绝成功", "结束", "RejectApplyJob", "App_ApplyJob");
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode("拒绝申请成功", true, 1)
                    );
            }
            catch (Exception ex)
            {
                LogHandler.WriteServiceLog(GetUserId(), "applyJobId:" + applyJobId + ",ErrorMsg:拒绝请求失败," + ex.Message, "结束", "RejectApplyJob", "App_ApplyJob");
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode("拒绝申请失败", false, 1)
                    );
            }
        }
        #endregion
    }
}
