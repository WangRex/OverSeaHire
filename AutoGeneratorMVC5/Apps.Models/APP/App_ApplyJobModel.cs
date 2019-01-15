using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apps.Models.App
{
    #region App_ApplyJobModel
    public partial class App_ApplyJobModel
    {
        [Display(Name = "主键")]
        public override string Id { get; set; }

        [Display(Name = "创建时间")]
        public override DateTime CreateTime { get; set; }

        [Display(Name = "更新时间")]
        public override DateTime ModificationTime { get; set; }

        [Display(Name = "创建人姓名")]
        public override string CreateUserName { get; set; }

        [Display(Name = "修改人姓名")]
        public override string ModificationUserName { get; set; }

        [Display(Name = "排序码")]
        public override int SortCode { get; set; }

        [Display(Name = "关联数据Id")]
        public override string ParentId { get; set; }

        [Display(Name = "标题")]
        public override string PK_App_Requirement_Title { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string App_Requirement_Title { get; set; }

        [Display(Name = "应聘人")]
        public override string PK_App_Customer_CustomerName { get; set; }
        /// <summary>
        /// 应聘人
        /// </summary>
        public string App_Customer_CustomerName { get; set; }

        [Display(Name = "当前步骤")]
        public override string CurrentStep { get; set; }

        [Display(Name = "当前步骤名称")]
        public string CurrentStepName { get; set; }

        [Display(Name = "应聘状态")]
        public override string EnumApplyStatus { get; set; }
        /// <summary>
        /// 应聘状态
        /// </summary>
        public string ApplyStatus { get; set; }
        [Display(Name = "保证金")]
        public override decimal PromiseMoney { get; set; }

        [Display(Name = "保证金支付方式")]
        public override string EnumPromisePayWay { get; set; }

        [Display(Name = "服务费")]
        public override decimal ServiceMoney { get; set; }

        [Display(Name = "服务费支付方式")]
        public override string EnumServicePayWay { get; set; }

        [Display(Name = "尾款")]
        public override decimal TailMoney { get; set; }

        [Display(Name = "尾款支付方式")]
        public override string EnumTailPayWay { get; set; }

        public override string ToString()
        {
            return "PK_App_Requirement_Title:" + PK_App_Requirement_Title + ",PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName + ",CurrentStep:" + CurrentStep
                 + ",EnumApplyStatus:" + EnumApplyStatus + ",PromiseMoney:" + PromiseMoney + ",EnumPromisePayWay:" + EnumPromisePayWay
                 + ",ServiceMoney:" + ServiceMoney + ",EnumServicePayWay:" + EnumServicePayWay + ",TailMoney:" + TailMoney
                 + ",EnumTailPayWay:" + EnumTailPayWay;
        }
    }
    #endregion

    #region 提交应聘申请
    public class ApplyJobPost
    {
        /// <summary>
        /// 登录人主键
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 需求主键
        /// </summary>
        public string RequirementId { get; set; }
        /// <summary>
        /// 应聘人主键
        /// </summary>
        public string CustomerId { get; set; }
        public override string ToString()
        {
            return "UserId:" + UserId + ",RequirementId:" + RequirementId + ",CustomerId:" + CustomerId;
        }
    }
    #endregion

    #region 修改应聘申请
    public class EditApplyJobPost
    {
        /// <summary>
        /// 登录人主键
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 申请主键
        /// </summary>
        public string ApplyJobId { get; set; }
        /// <summary>
        /// 步骤数
        /// </summary>
        public string CurrentStep { get; set; }
        /// <summary>
        /// 面试结果
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 考试时间
        /// </summary>
        public string ConfigDate { get; set; }
        /// <summary>
        /// 考试地点
        /// </summary>
        public string ConfigPlace { get; set; }
        /// <summary>
        /// 状态(0:进行中,1:已完成,2:已取消)
        /// </summary>
        public string EnumApplyStatus { get; set; }
        public string EnumPromisePayWay { get; set; }
        public string EnumServicePayWay { get; set; }
        public string EnumTailPayWay { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public override string ToString()
        {
            return "UserId:" + UserId + ",ApplyJobId:" + ApplyJobId + ",CurrentStep:" + CurrentStep
                + ",Result:" + Result + ",EnumApplyStatus:" + EnumApplyStatus
                + ",ConfigDate:" + ConfigDate + ",ConfigPlace:" + ConfigPlace
                + ",EnumPromisePayWay:" + EnumPromisePayWay + ",EnumServicePayWay:" + EnumServicePayWay
                + ",EnumTailPayWay:" + EnumTailPayWay + ",Longitude:" + Longitude
                + ",Latitude:" + Latitude;
        }
    }
    #endregion

    #region 应聘申请
    public class ApplyJobVm
    {
        public ApplyJobVm()
        {
            officeVm = new OfficeVm();
            requirementDetailVm = new RequirementDetailVm();
            applyJobUserVm = new ApplyJobUserVm();
            applyJobRecordVm = new ApplyJobRecordVm();
            applyJobPayVms = new List<ApplyJobPayVm>();
            applyJobRecordVms = new List<ApplyJobRecordVm>();
        }
        public string ApplyJobId { get; set; }
        public string PK_App_Requirement_Title { get; set; }
        public string PK_App_Customer_CustomerName { get; set; }
        public string EnumApplyStatus { get; set; }
        public string ApplyStatus { get; set; }
        public string ApplyJobPromiseMoney { get; set; }
        public string PromiseMoney { get; set; }
        public string EnumPromisePayWay { get; set; }
        public string PromisePayWayName { get; set; }
        public string ServiceMoney { get; set; }
        public string EnumServicePayWay { get; set; }
        public string ServicePayWayName { get; set; }
        public string ServiceTailMoney { get; set; }
        public string EnumTailPayWay { get; set; }
        public string TailPayWayName { get; set; }
        public string CurrentStep { get; set; }
        /// <summary>
        /// 办事处信息
        /// </summary>
        public OfficeVm officeVm { get; set; }
        /// <summary>
        /// 需求信息
        /// </summary>
        public RequirementDetailVm requirementDetailVm { get; set; }
        /// <summary>
        /// 申请人信息
        /// </summary>
        public ApplyJobUserVm applyJobUserVm { get; set; }
        /// <summary>
        /// 步骤详情记录
        /// </summary>
        public ApplyJobRecordVm applyJobRecordVm { get; set; }
        /// <summary>
        /// 步骤详情记录
        /// </summary>
        public List<ApplyJobRecordVm> applyJobRecordVms { get; set; }
        /// <summary>
        /// 支付明细
        /// </summary>
        public List<ApplyJobPayVm> applyJobPayVms { get; set; }
    }
    #endregion

    #region 支付明细
    public class ApplyJobPayVm
    {
        public string Name { get; set; }
        public string Money { get; set; }
        public string Status { get; set; }
        public string PayTime { get; set; }
    }
    #endregion

    #region 获取应聘申请记录列表
    public class AppLyJobInfo
    {
        public string Id { get; set; }
        public string PK_App_Requirement_Title { get; set; }
        public string CurrentStep { get; set; }
        public string EnumApplyStatus { get; set; }
        public override string ToString()
        {
            return "Id:" + Id + ",PK_App_Requirement_Title:" + PK_App_Requirement_Title + ",CurrentStep:" + CurrentStep
                + ",EnumApplyStatus:" + EnumApplyStatus;
        }
    }
    #endregion
}

