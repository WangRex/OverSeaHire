using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.App
{
    #region App_CompanyModel
    public partial class App_CompanyModel
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

        [Display(Name = "公司全称")]
        public override string CompanyName { get; set; }

        [Display(Name = "公司简称")]
        public override string CompanyShortName { get; set; }

        [Display(Name = "所属行业")]
        public override string Industry { get; set; }

        [Display(Name = "公司规模")]
        public override string EnumCompanySize { get; set; }

        [Display(Name = "营业执照")]
        public override string BusinessLicence { get; set; }

        [Display(Name = "是否审核")]
        public override string SwitchBtnApply { get; set; }

        [Display(Name = "企业管理员")]
        public override string PK_App_Customer_CustomerName { get; set; }

        public override string ToString()
        {
            return "CompanyName:" + CompanyName + ",CompanyShortName:" + CompanyShortName + ",Industry:" + Industry
             + ",EnumCompanySize:" + EnumCompanySize + ",BusinessLicence:" + BusinessLicence + ",SwitchBtnApply:" + SwitchBtnApply
             + ",PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName;
            ;
        }
    }
    #endregion

    #region 企业认证
    public class CompanyPost
    {
        public string Id { get; set; }
        /// <summary>
        /// 登录人主键
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 公司全称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 公司简称
        /// </summary>
        public string CompanyShortName { get; set; }
        /// <summary>
        /// 所属行业
        /// </summary>
        public string Industry { get; set; }
        /// <summary>
        /// 公司规模
        /// </summary>
        public string EnumCompanySize { get; set; }
        /// <summary>
        /// 营业执照
        /// </summary>
        public string BusinessLicence { get; set; }
        /// <summary>
        /// 是否审核
        /// </summary>
        public string SwitchBtnApply { get; set; }
        /// <summary>
        /// 企业管理员
        /// </summary>
        public string PK_App_Customer_CustomerName { get; set; }

        public override string ToString()
        {
            return "CompanyName:" + CompanyName + ",CompanyShortName:" + CompanyShortName + ",Industry:" + Industry
                + ",EnumCompanySize:" + EnumCompanySize + ",BusinessLicence:" + BusinessLicence + ",SwitchBtnApply:" + SwitchBtnApply
                + ",PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName;
        }
    }
    #endregion

    #region 企业认证
    public class CompanyVm : CompanyPost
    {
        /// <summary>
        /// 公司规模
        /// </summary>
        public string CompanySize { get; set; }
        /// <summary>
        /// 企业管理员
        /// </summary>
        public string App_Customer_CustomerName { get; set; }

        public override string ToString()
        {
            return "CompanyName:" + CompanyName + ",CompanyShortName:" + CompanyShortName + ",Industry:" + Industry
                + ",EnumCompanySize:" + EnumCompanySize + ",BusinessLicence:" + BusinessLicence + ",SwitchBtnApply:" + SwitchBtnApply
                + ",CompanySize:" + CompanySize + ",App_Customer_CustomerName:" + App_Customer_CustomerName
                + ",PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName;
        }
    }
    #endregion
}

