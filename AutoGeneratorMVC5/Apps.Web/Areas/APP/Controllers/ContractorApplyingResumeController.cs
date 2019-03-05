using System.Collections.Generic;
using Apps.Web.Core;
using Apps.Locale;
using System.Web.Mvc;
using Apps.Common;
using Microsoft.Practices.Unity;
using Apps.BLL.Sys;
using Apps.BLL.App;
using Apps.Models.App;
using Newtonsoft.Json;
using System;

namespace Apps.Web.Areas.App.Controllers
{
    /// <summary>
    /// 外派公司-面试中的简历
    /// </summary>
    public class ContractorApplyingResumeController : BaseController
    {
        #region BLLs
        [Dependency]
        public App_CustomerBLL m_BLL { get; set; }
        [Dependency]
        public SysUserBLL sysUserBLL { get; set; }
        [Dependency]
        public EnumDictionaryBLL enumDictionaryBLL { get; set; }
        [Dependency]
        public App_CustomerWorkExpBLL app_CustomerWorkExpBLL { get; set; }
        [Dependency]
        public App_PositionBLL app_PositionBLL { get; set; }
        [Dependency]
        public App_ApplyJobBLL app_ApplyJobBLL { get; set; }
        [Dependency]
        public App_CountryBLL app_CountryBLL { get; set; }
        [Dependency]
        public App_ApplyJobStepBLL app_ApplyJobStepBLL { get; set; }
        [Dependency]
        public App_RequirementBLL app_RequirementBLL { get; set; }
        [Dependency]
        public App_RequirementInviteBLL app_RequirementInviteBLL { get; set; }
        [Dependency]
        public SysRoleBLL sysRoleBLL { get; set; }
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
        public JsonResult GetList(GridPager pager, RequirementQuery requirementQuery)
        {
            //先获取当前用户的对应customerId
            if ("1" == Session["IdFlag"] as string || Session["ohadmin"] as string == "1")
            {
                requirementQuery.AdminFlag = true;
            }
            else
            {
                //如果登录的账号不是ohadmin角色的，则按照他自己创建的显示
                requirementQuery.CustomerId = Session["PK_App_Customer_CustomerName"] as string;
            }
            List<App_RequirementModel> list = app_RequirementBLL.GetContractorRequirementList(ref pager, requirementQuery);
            List<RequirementInfoVm> requirementInfoVms = new List<RequirementInfoVm>();
            foreach (var Item in list)
            {
                requirementInfoVms.Add(new RequirementInfoVm()
                {
                    Id = Item.Id,
                    Title = Item.Title,
                    Position = app_PositionBLL.GetNames(Item.PK_App_Position_Name),
                    Country = app_CountryBLL.GetName(Item.PK_App_Country_Name),
                    Sex = Item.WorkLimitSex,
                    AgeLimit = Item.WorkLimitAgeLow + "-" + Item.WorkLimitAgeHigh,
                    YearSalary = Utils.ObjToDecimal(Item.SalaryLow, 0) / 10000 + "万-" + Utils.ObjToDecimal(Item.SalaryHigh, 0) / 10000 + "万",
                    TotalHire = Item.TotalHire,
                    Tag = Item.Tag,
                    TotalServiceMoney = Utils.ObjToDecimal(Item.TotalServiceMoney, 0) / 10000 + "万",
                    PublishDate = Item.PublishDate,
                    ReqType = Item.ReqType,
                });
            }
            GridRows<RequirementInfoVm> grs = new GridRows<RequirementInfoVm>();
            grs.rows = requirementInfoVms;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #endregion

        #region 详细
        public ActionResult Details(string id, string flagWin)
        {
            App_RequirementModel entity = app_RequirementBLL.GetById(id);
            RequirementDetailsVm requirementDetailsVm = new RequirementDetailsVm();
            requirementDetailsVm.Id = entity.Id;
            requirementDetailsVm.Title = entity.Title;
            requirementDetailsVm.Position = app_PositionBLL.GetNames(entity.PK_App_Position_Name);
            requirementDetailsVm.Country = app_CountryBLL.GetName(entity.PK_App_Country_Name);
            requirementDetailsVm.Sex = entity.WorkLimitSex;
            requirementDetailsVm.AgeLimit = entity.WorkLimitAgeLow + "-" + entity.WorkLimitAgeHigh;
            requirementDetailsVm.YearSalary = Utils.ObjToDecimal(entity.SalaryLow, 0) / 10000 + "万-" + Utils.ObjToDecimal(entity.SalaryHigh, 0) / 10000 + "万";
            requirementDetailsVm.TotalHire = entity.TotalHire;
            requirementDetailsVm.Tag = entity.Tag;
            requirementDetailsVm.TotalServiceMoney = entity.TotalServiceMoney;
            requirementDetailsVm.PublishDate = entity.PublishDate;
            requirementDetailsVm.Description = entity.Description;
            requirementDetailsVm.PromiseMoney = entity.PromiseMoney;
            requirementDetailsVm.ServiceTailMoney = entity.ServiceTailMoney;
            requirementDetailsVm.ServiceMoney = entity.ServiceMoney;
            return View(requirementDetailsVm);
        }
        #endregion

        #region 获取符合条件工人列表
        /// <summary>
        /// 获取符合条件工人列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult IndexResume(string id)
        {
            App_RequirementModel entity = app_RequirementBLL.GetById(id);
            entity.PK_App_Position_Name = app_PositionBLL.GetName(entity.PK_App_Position_Name);
            entity.EnumWorkLimitDegree = enumDictionaryBLL.GetDicName("App_Requirement.EnumWorkLimitDegree", entity.EnumWorkLimitDegree);
            entity.PK_App_Customer_CustomerName = m_BLL.GetCustomerName(entity.PK_App_Customer_CustomerName);
            entity.PK_App_Country_Name = app_CountryBLL.GetName(entity.PK_App_Country_Name);
            ViewBag.ReqId = id;
            return View(entity);
        }
        [HttpPost]
        [SupportFilter(ActionName = "IndexResume")]
        public JsonResult GetIndexResume(GridPager pager, CustomerResumeQuery customerResumeQuery)
        {
            //此处获取的简历就是所有的工人，无需权限
            customerResumeQuery.AdminFlag = true;
            customerResumeQuery.QueryFlag = "Applying";
            string strCustomerId = Session["PK_App_Customer_CustomerName"] as string;
            var queryData = app_RequirementBLL.GetReqResumeList(ref pager, customerResumeQuery);
            List<App_CustomerModel> list = m_BLL.CreateModelList(ref queryData);
            var customerResumeVms = m_BLL.TransCustomerResume(list, strCustomerId, true, true);
            GridRows<CustomerResumeVm> grs = new GridRows<CustomerResumeVm>();
            grs.rows = customerResumeVms;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #endregion

        #region 外派公司同意拒绝
        /// <summary>
        /// 外派公司同意拒绝
        /// </summary>
        /// <param name="ReqId"></param>
        /// <param name="CustomerId"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter(ActionName = "UpdateApplywResult")]
        public JsonResult UpdateApplywResult(string ReqId, string CustomerId, string Flag)
        {
            var now = ResultHelper.NowTime;
            string strUserId = GetUserId(), applyJobId = "";
            ApplyJobPost applyJobPost = new ApplyJobPost();
            applyJobPost.CustomerId = CustomerId;
            applyJobPost.RequirementId = ReqId;
            applyJobPost.UserId = strUserId;
            var app_ApplyJob = app_ApplyJobBLL.m_Rep.Find(EF => EF.PK_App_Requirement_Title == ReqId && EF.PK_App_Customer_CustomerName == CustomerId && EF.EnumApplyStatus == "0" && EF.CurrentStep == "3");
            app_ApplyJob.ModificationTime = now;
            app_ApplyJob.ModificationUserName = strUserId;
            try
            {
                string ErrorMsg = "";
                if ("1".Equals(Flag))
                {
                    //如果同意，则更新状态为0,并且处理下一步。
                    app_ApplyJob.EnumApplyStatus = "0";
                    app_ApplyJobBLL.m_Rep.Edit(app_ApplyJob);
                    App_ApplyJobModel entity = app_ApplyJobBLL.GetById(app_ApplyJob.Id);
                    ViewBag.Result = new SelectList(enumDictionaryBLL.GetDropDownList("App_ApplyJobRecord.Result"), "ItemValue", "ItemName");
                    var iNextStep = Utils.ObjToInt(entity.CurrentStep, 0) + 1;
                    App_ApplyJobRecordModel applyJobRecordModel = new App_ApplyJobRecordModel()
                    {
                        Id = ResultHelper.NewId,
                        CreateTime = now,
                        CreateUserName = strUserId,
                        ModificationTime = now,
                        ModificationUserName = strUserId,
                        PK_App_ApplyJob_Id = app_ApplyJob.Id,
                        PK_App_Customer_CustomerName = entity.PK_App_Customer_CustomerName,
                        Step = iNextStep.ToString(),
                        Result = "进行中",
                        Content = app_ApplyJobStepBLL.GetStepName(iNextStep.ToString()) + "进行中",
                    };
                    if (iNextStep == 9)
                    {
                        applyJobRecordModel.Result = "已完成";
                    }
                    app_ApplyJobBLL.NextStep(strUserId, applyJobRecordModel);
                    ErrorMsg = "面试已通过成功";
                }
                else
                {
                    app_ApplyJob.EnumApplyStatus = "4";
                    app_ApplyJobBLL.m_Rep.Edit(app_ApplyJob);
                    var customer = m_BLL.m_Rep.GetById(CustomerId);
                    customer.SwitchBtnInterview = "0";
                    customer.ModificationTime = now;
                    customer.ModificationUserName = strUserId;
                    m_BLL.m_Rep.Edit(customer);
                    ErrorMsg = "面试未通过成功";
                }
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, applyJobId, 1)
                );
            }
            catch (Exception ex)
            {
                return Json(
                   ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ex.Message)
                   );
            }
        }
        #endregion
    }
}
