using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apps.Models.App
{
    #region App_RequirementModel
    public partial class App_RequirementModel
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
        public override string Title { get; set; }

        [Display(Name = "副标题")]
        public override string SubTitle { get; set; }

        [Display(Name = "职位类型")]
        public override string PK_App_Position_Name { get; set; }

        [Display(Name = "职位类型名称")]
        public string App_Position_Name { get; set; }

        [Display(Name = "工作地点")]
        public override string WorkPlace { get; set; }

        [Display(Name = "工作要求性别")]
        public override string WorkLimitSex { get; set; }

        [Display(Name = "工作要求年龄(最低)")]
        public override int WorkLimitAgeLow { get; set; }

        [Display(Name = "工作要求年龄(最高)")]
        public override int WorkLimitAgeHigh { get; set; }

        [Display(Name = "工作要求学历")]
        public override string EnumWorkLimitDegree { get; set; }

        [Display(Name = "报酬(最低)")]
        public override string SalaryLow { get; set; }

        [Display(Name = "报酬(最高)")]
        public override string SalaryHigh { get; set; }

        [Display(Name = "保证金")]
        public override string PromiseMoney { get; set; }

        [Display(Name = "服务费")]
        public override string ServiceMoney { get; set; }

        [Display(Name = "总服务费")]
        public override string TotalServiceMoney { get; set; }

        [Display(Name = "发布时间")]
        public override string PublishDate { get; set; }

        [Display(Name = "招聘人数")]
        public override int TotalHire { get; set; }

        [Display(Name = "职位标签")]
        public override string Tag { get; set; }

        [Display(Name = "发布人")]
        public override string PK_App_Customer_CustomerName { get; set; }

        [Display(Name = "描述")]
        public override string Description { get; set; }

        [Display(Name = "推荐")]
        public override string SwitchBtnRecommend { get; set; }

        [Display(Name = "公司")]
        public override string CompanyName { get; set; }

        [Display(Name = "国家")]
        public override string PK_App_Country_Name { get; set; }

        [Display(Name = "年限")]
        public override int TotalYear { get; set; }

        [Display(Name = "办理省份")]
        public override string TransactProvince { get; set; }

        [Display(Name = "应聘次数")]
        public override int ApplyCount { get; set; }

        [Display(Name = "服务费尾款")]
        public override string ServiceTailMoney { get; set; }

        [Display(Name = "是否开放")]
        public override string SwitchBtnOpen { get; set; }

        [Display(Name = "查看次数")]
        public override int ClickNumber { get; set; }

        [Display(Name = "税前时薪")]
        public override string PreTaxSalary { get; set; }

        [Display(Name = "每周工时")]
        public override string WorkHourPerWeek { get; set; }

        [Display(Name = "工签")]
        public override string EnumWorkPermit { get; set; }

        /// <summary>
        /// 包括应聘的职位，系统推荐的职位，雇主邀请面试的职位，办理中的职位
        /// </summary>
        [Display(Name = "职位类型")]
        public string ReqType { get; set; }

        public override string ToString()
        {
            return "Title:" + Title + ",SubTitle:" + SubTitle + ",EnumPositionType:" + PK_App_Position_Name
                + ",WorkPlace:" + WorkPlace + ",WorkLimitSex:" + WorkLimitSex + ",WorkLimitAgeLow:" + WorkLimitAgeLow
                + ",WorkLimitAgeHigh:" + WorkLimitAgeHigh + ",EnumWorkLimitDegree:" + EnumWorkLimitDegree + ",SalaryLow:" + SalaryLow
                + ",SalaryHigh:" + SalaryHigh + ",PromiseMoney:" + PromiseMoney + ",ServiceMoney:" + ServiceMoney
                + ",ServiceTailMoney:" + ServiceTailMoney
                + ",TotalServiceMoney:" + TotalServiceMoney + ",PublishDate:" + PublishDate + ",TotalHire:" + TotalHire
                + ",Tag:" + Tag + ",PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName + ",Description:" + Description
                + ",SwitchBtnRecommend:" + SwitchBtnRecommend + ",CompanyName:" + CompanyName + ",PK_App_Country_Name:" + PK_App_Country_Name
                + ",TotalYear:" + TotalYear + ",TransactProvince:" + TransactProvince + ",ApplyCount:" + ApplyCount
                + ",ServiceTailMoney:" + ServiceTailMoney + ",SwitchBtnOpen:" + SwitchBtnOpen + ",ClickNumber:" + ClickNumber
                + ",PreTaxSalary:" + PreTaxSalary + ",WorkHourPerWeek:" + WorkHourPerWeek + ",EnumWorkPermit:" + EnumWorkPermit
                + ",ReqType:" + ReqType;
        }
    }
    #endregion

    #region 查询用
    public class RequireSearchForm
    {
        public string UserId { get; set; }
        public string PublishUserId { get; set; }
        public string QueryStr { get; set; }
        public string PK_App_Position_Name { get; set; }
        public string Country { get; set; }
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
        public string TotalYear { get; set; }
        //传过来的是省的代码，多个省
        public string TransactProvince { get; set; }
        public string IsRecommend { get; set; }
        public string IsLatest { get; set; }
        public string IsHighSalary { get; set; }
        public string IsHot { get; set; }
        public string SwitchBtnOpen { get; set; }
        public int PageNum { get; set; }
        public int RecordNum { get; set; }

        public override string ToString()
        {
            return "UserId:" + UserId + ",QueryStr:" + QueryStr + ",PK_App_Position_Name:" + PK_App_Position_Name
                + ",Country:" + Country + ",SalaryMin:" + SalaryMin + ",SalaryMax:" + SalaryMax
                + ",TotalYear:" + TotalYear + ",TransactProvince:" + TransactProvince + ",IsRecommend:" + IsRecommend
                + ",IsLatest:" + IsLatest + ",IsHot:" + IsHot + ",SwitchBtnOpen:" + SwitchBtnOpen
                + ",IsHighSalary:" + IsHighSalary + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum;
        }
    }
    #endregion

    #region 需求列表返回
    public class RequirementVm
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string CountryName { get; set; }
        public string CountryImg { get; set; }
        public string SalaryLow { get; set; }
        public string SalaryHigh { get; set; }
        public string PublishDate { get; set; }
        public List<string> Tag { get; set; }
        public string CompanyName { get; set; }
        /// <summary>
        /// 应聘数量
        /// </summary>
        public string ApplyCount { get; set; }
        public string PK_App_Position_Name { get; set; }
        public string PositionName { get; set; }
        public string TransactProvinceCode { get; set; }
        public string TransactProvinceName { get; set; }
        public string SwitchBtnOpen { get; set; }
        /// <summary>
        /// 查看数量
        /// </summary>
        public int ClickNumber { get; set; }
        /// <summary>
        /// 面试数量
        /// </summary>
        public int InterviewNumber { get; set; }
        /// <summary>
        /// 办理数量
        /// </summary>
        public int CompleteNumber { get; set; }
    }
    #endregion

    #region 需求列表返回
    public class RequirementHomePage
    {
        public RequirementHomePage()
        {
            requirementVms = new List<RequirementVm>();
            positionTreeVms = new List<PositionTreeVm>();
        }
        public List<RequirementVm> requirementVms { get; set; }
        public List<PositionTreeVm> positionTreeVms { get; set; }
    }
    #endregion

    #region 需求详情
    public class RequirementDetailVm
    {
        public RequirementDetailVm()
        {
            RecommendUsers = new List<ApplyJobUserVm>();
        }
        public string Id { get; set; }
        public string Title { get; set; }
        public string PK_App_Position_Name { get; set; }
        public string PositionName { get; set; }
        public string WorkPlace { get; set; }
        public string WorkLimitSex { get; set; }
        public string WorkLimitAgeLow { get; set; }
        public string WorkLimitAgeHigh { get; set; }
        public string EnumWorkLimitDegree { get; set; }
        public string WorkLimitDegree { get; set; }
        public string SalaryLow { get; set; }
        public string SalaryHigh { get; set; }
        public string PromiseMoney { get; set; }
        public string ServiceMoney { get; set; }
        public string ServiceTailMoney { get; set; }
        public string TotalServiceMoney { get; set; }
        public string PublishDate { get; set; }
        public string TotalHire { get; set; }
        public List<string> Tag { get; set; }
        /// <summary>
        /// 发布人主键
        /// </summary>
        public string PublishUserId { get; set; }
        /// <summary>
        /// 发布人姓名
        /// </summary>
        public string PublishUserName { get; set; }
        /// <summary>
        /// 发布人头像
        /// </summary>
        public string PublishUserPhoto { get; set; }
        /// <summary>
        /// 发布人公司
        /// </summary>
        public string PublishUserCompany { get; set; }
        public string PublisherPhone { get; set; }
        public string PublisherWeChatNumber { get; set; }
        public string Description { get; set; }
        public string SwitchBtnRecommend { get; set; }
        public string CompanyName { get; set; }
        public string CountryName { get; set; }
        public string CountryImg { get; set; }
        public string TotalYear { get; set; }
        public string ApplyCount { get; set; }
        public string RequirementCollId { get; set; }
        public int ClickNumber { get; set; }
        public string SwitchBtnOpen { get; set; }
        /// <summary>
        /// XX人申请
        /// </summary>
        public int ApplyingCount { get; set; }
        /// <summary>
        /// XX人面试
        /// </summary>
        public int InterviewCount { get; set; }
        public List<ApplyJobUserVm> RecommendUsers { get; set; }
    }
    #endregion

    #region 发布需求
    public class RequirementPost
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        /// <summary>
        /// 工种
        /// </summary>
        public string PK_App_Position_Name { get; set; }
        /// <summary>
        /// 职位名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string PK_App_Country_Name { get; set; }
        /// <summary>
        /// 税前时薪
        /// </summary>
        public string PreTaxSalary { get; set; }
        /// <summary>
        /// 每周工时
        /// </summary>
        public string WorkHourPerWeek { get; set; }
        /// <summary>
        /// 工作要求性别
        /// </summary>
        public string WorkLimitSex { get; set; }
        /// <summary>
        /// 工作要求年龄最小
        /// </summary>
        public int WorkLimitAgeLow { get; set; }
        /// <summary>
        /// 工作要求年龄最大
        /// </summary>
        public int WorkLimitAgeHigh { get; set; }
        /// <summary>
        /// 学历要求
        /// </summary>
        public string EnumWorkLimitDegree { get; set; }
        /// <summary>
        /// 招聘人数
        /// </summary>
        public int TotalHire { get; set; }
        /// <summary>
        /// 职位描述标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 发布人主键
        /// </summary>
        public string PublishUserId { get; set; }
        /// <summary>
        /// 描述内容
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否开放
        /// </summary>
        public string SwitchBtnOpen { get; set; }
        /// <summary>
        /// 工签
        /// </summary>
        public string EnumWorkPermit { get; set; }
    }
    #endregion

    #region 编辑初始化发布需求
    public class RequirementInitPost : RequirementPost
    {
        public string App_Position_Name { get; set; }
        public string App_Country_Name { get; set; }
    }
    #endregion

    #region 雇主端查询用
    public class RecommendUserSearchForm
    {
        public string UserId { get; set; }
        /// <summary>
        /// 雇主主键
        /// </summary>
        public string EmployerId { get; set; }
        public string QueryStr { get; set; }
        public string PK_App_Position_Name { get; set; }
        /// <summary>
        /// 系统推荐，是的话传1
        /// </summary>
        public string IsRecommend { get; set; }
        /// <summary>
        /// 最新简历，是的话传1
        /// </summary>
        public string IsLatest { get; set; }
        /// <summary>
        /// 有工作视频，是的话传1
        /// </summary>
        public string HaveVideo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IsHot { get; set; }
        /// <summary>
        /// 智能排序（可以传Age, DriverLicence, WorkExp, AbroadExp）
        /// </summary>
        public string IntelligenceSort { get; set; }
        public int PageNum { get; set; }
        public int RecordNum { get; set; }

        public override string ToString()
        {
            return "UserId:" + UserId + ",QueryStr:" + QueryStr + ",PK_App_Position_Name:" + PK_App_Position_Name
                + ",IsRecommend:" + IsRecommend + ",IsLatest:" + IsLatest + ",HaveVideo:" + HaveVideo
                + ",IsHot:" + IsHot + ",IntelligenceSort:" + IntelligenceSort
                + ",IntelligenceSort:" + IntelligenceSort + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum;
        }
    }
    #endregion

    #region 【后台】获取职位列表查询条件
    public class RequirementQuery
    {
        public string Title { get; set; }
        public string Sex { get; set; }
        /// <summary>
        /// 工种
        /// </summary>
        public string PositionId { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 工人主键
        /// </summary>
        public string CustomerId { get; set; }
        public int AgeLow { get; set; }
        public int AgeHigh { get; set; }
        public int SallaryLow { get; set; }
        public int SallaryHigh { get; set; }
        public string Tag { get; set; }
        public bool AdminFlag { get; set; }
        /// <summary>
        /// 查询条件
        /// Applyed:已经应聘的,InterView:面试中的,Recommend:推荐的,Invite:雇主邀请的
        /// </summary>
        public string QueryFlag { get; set; }
        /// <summary>
        /// 职位发布人
        /// </summary>
        public string PublisherId { get; set; }
        public override string ToString()
        {
            return "Title:" + Title + ",Sex:" + Sex
                + ",Country:" + Country + ",AgeLow:" + AgeLow + ",AdminFlag:" + AdminFlag
                + ",QueryFlag:" + QueryFlag + ",AgeHigh:" + AgeHigh + ",Tag:" + Tag
                + ",SallaryLow:" + SallaryLow + ",SallaryHigh:" + SallaryHigh
                + ",CustomerId:" + CustomerId + ",PublisherId:" + PublisherId;
        }
    }
    #endregion

    #region 【后台】职位列表用
    public class RequirementInfoVm
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Position { get; set; }
        public string Country { get; set; }
        public string Sex { get; set; }
        public string AgeLimit { get; set; }
        public string YearSalary { get; set; }
        public int TotalHire { get; set; }
        public string Tag { get; set; }
        public string TotalServiceMoney { get; set; }
        public string PublishDate { get; set; }
        public string ReqType { get; set; }
        public string ReqTypeName { get; set; }
    }
    #endregion

    #region 【后台】职位详情用
    public class RequirementDetailsVm : RequirementInfoVm
    {
        public string Description { get; set; }
        public string PromiseMoney { get; set; }
        public string ServiceTailMoney { get; set; }
        public string ServiceMoney { get; set; }
    }
    #endregion
}