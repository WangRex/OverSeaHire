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
                Item.EnumApplyStatus = enumDictionaryBLL.GetDicName("App_ApplyJob.EnumApplyStatus", Item.EnumApplyStatus);
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
            if (model != null && ModelState.IsValid)
            {

                model.ModificationUserName = GetUserId();
                model.ModificationTime = ResultHelper.NowTime;
                if (m_BLL.Edit(ref errors, model))
                {
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
            var now = ResultHelper.NowTime;
            var iPreStep = Utils.ObjToInt(model.Step, 0) - 1;
            if (model != null && ModelState.IsValid)
            {
                //首先更新申请主信息
                App_ApplyJobModel entity = m_BLL.GetById(model.PK_App_ApplyJob_Id);
                entity.ModificationTime = now;
                entity.ModificationUserName = GetUserId();
                entity.CurrentStep = model.Step;
                if (model.Step == "9")
                {
                    //如果步骤到9了，就改成已完成
                    entity.EnumApplyStatus = "1";
                }
                m_BLL.Edit(ref errors, entity);
                SysMessage sysMessage = new SysMessage()
                {
                    Id = ResultHelper.NewId,
                    CreateTime = now,
                    CreateUserName = GetUserId(),
                    ModificationTime = now,
                    ModificationUserName = GetUserId(),
                    BusinessTable = "ApplyJob",
                    BusinessID = model.PK_App_ApplyJob_Id,
                    PK_App_Customer_CustomerName = model.PK_App_Customer_CustomerName,
                    SwitchBtnRead = "0",
                    EnumMessageType = "1",
                    Title = "应聘进度更新",
                };
                sysMessage.WorkerId = model.PK_App_Customer_CustomerName;
                //增加判断，如果这个邀请的是个工友，则需要推送消息给工人
                var customer = _App_CustomerBLL.m_Rep.GetById(model.PK_App_Customer_CustomerName);
                if (!string.IsNullOrEmpty(customer.ParentId))
                {
                    sysMessage.PK_App_Customer_CustomerName = customer.ParentId;
                }
                if ("3".Equals(model.Step))
                {
                    sysMessage.Content = "您的应聘进度有更新——待面试";
                }
                if ("4".Equals(model.Step))
                {
                    sysMessage.Content = "您的应聘进度有更新——待考试";
                }
                if ("5".Equals(model.Step))
                {
                    sysMessage.Content = "您的应聘进度有更新——待支付服务费";
                }
                if ("6".Equals(model.Step))
                {
                    sysMessage.Content = "您的应聘进度有更新——待审核材料";
                }
                if ("7".Equals(model.Step))
                {
                    sysMessage.Content = "您的应聘进度有更新——待签约";
                }
                if ("8".Equals(model.Step))
                {
                    sysMessage.Content = "您的应聘进度有更新——待支付尾款";
                }
                if ("9".Equals(model.Step))
                {
                    sysMessage.Content = "您的应聘完成啦~~祝一切顺利";
                }
                sysMessageBLL.m_Rep.Create(sysMessage);
                model.ModificationUserName = GetUserId();
                model.ModificationTime = ResultHelper.NowTime;
                if (applyJobRecordBLL.Create(ref errors, model))
                {
                    //成功后把上一个步骤的状态改成已完成
                    var PreStep = iPreStep.ToString();
                    App_ApplyJobRecord applyJobRecord = applyJobRecordBLL.m_Rep.Find(EF => EF.PK_App_ApplyJob_Id == model.PK_App_ApplyJob_Id && EF.Step == PreStep);
                    applyJobRecord.Result = "已完成";
                    applyJobRecord.ModificationTime = now;
                    applyJobRecord.ModificationUserName = GetUserId();
                    if (string.IsNullOrEmpty(applyJobRecord.HappenDate))
                    {
                        applyJobRecord.HappenDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    applyJobRecordBLL.m_Rep.Edit(applyJobRecord);
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime, "成功", "创建记录", "App_ApplyJob");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",CreateTime" + model.CreateTime + "," + ErrorCol, "失败", "创建记录", "App_ApplyJob");
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
                if (m_BLL.Delete(ref errors, id))
                {
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
    }
}
