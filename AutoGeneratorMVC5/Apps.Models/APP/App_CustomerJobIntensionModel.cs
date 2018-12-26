using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.App
{
    #region App_CustomerJobIntensionModel
    public partial class App_CustomerJobIntensionModel
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

        [Display(Name = "用户")]
        public override string PK_App_Customer_CustomerName { get; set; }

        [Display(Name = "期望职位")]
        public override string EnumPositionType { get; set; }

        [Display(Name = "期望月薪")]
        public override string ExpectSalary { get; set; }

        [Display(Name = "期望国家")]
        public override string ExpectCountry { get; set; }

        [Display(Name = "掌握技能")]
        public override string Skills { get; set; }

        [Display(Name = "出国意向")]
        public override string Intention { get; set; }

        [Display(Name = "求职方式")]
        public override string EnumApplyWay { get; set; }

        [Display(Name = "现居地")]
        public override string CurrentPlace { get; set; }

        [Display(Name = "外语等级")]
        public override string EnumForeignLangGrade { get; set; }

        [Display(Name = "出国经历")]
        public override string AbroadExp { get; set; }

        public override string ToString()
        {
            return "PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName + ",EnumPositionType:" + EnumPositionType + ",ExpectSalary:" + ExpectSalary
     + ",ExpectCountry:" + ExpectCountry + ",Skills:" + Skills + ",Intention:" + Intention
     + ",EnumApplyWay:" + EnumApplyWay + ",CurrentPlace:" + CurrentPlace + ",EnumForeignLangGrade:" + EnumForeignLangGrade
     + ",AbroadExp:" + AbroadExp;
        }
    }
    #endregion

    #region 求职意向
    public class JobIntensionPost
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public string PK_App_Customer_CustomerName { get; set; }
        /// <summary>
        /// 期望职位
        /// </summary>
        public string EnumPositionType { get; set; }
        /// <summary>
        /// 期望月薪
        /// </summary>
        public string ExpectSalary { get; set; }
        /// <summary>
        /// 期望国家
        /// </summary>
        public string ExpectCountry { get; set; }
        /// <summary>
        /// 掌握技能
        /// </summary>
        public string Skills { get; set; }
        /// <summary>
        /// 出国意向
        /// </summary>
        public string Intention { get; set; }
        /// <summary>
        /// 求职方式
        /// </summary>
        public string EnumApplyWay { get; set; }
        /// <summary>
        /// 现居地
        /// </summary>
        public string CurrentPlace { get; set; }
        /// <summary>
        /// 外语等级
        /// </summary>
        public string EnumForeignLangGrade { get; set; }
        /// <summary>
        /// 出国经历
        /// </summary>
        public string AbroadExp { get; set; }
        /// <summary>
        /// 证书
        /// </summary>
        public List<CertificatePost> certificatePosts { get; set; }
        public override string ToString()
        {
            return "Id:" + Id + ",PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName + ",EnumPositionType:" + EnumPositionType + ",ExpectSalary:" + ExpectSalary
             + ",ExpectCountry:" + ExpectCountry + ",Skills:" + Skills + ",Intention:" + Intention
             + ",EnumApplyWay:" + EnumApplyWay + ",CurrentPlace:" + CurrentPlace + ",EnumForeignLangGrade:" + EnumForeignLangGrade
             + ",AbroadExp:" + AbroadExp;
        }
    }
    #endregion

    #region 用户求职意向
    public class CustomerJobIntensionVm : JobIntensionPost
    {
        public string PositionNames { get; set; }
    }
    #endregion
}

