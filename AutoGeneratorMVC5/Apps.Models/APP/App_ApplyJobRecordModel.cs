using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.App
{
    public partial class App_ApplyJobRecordModel
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
        public override string PK_App_ApplyJob_Id { get; set; }

        [Display(Name = "应聘人")]
        public override string PK_App_Customer_CustomerName { get; set; }

        [Display(Name = "应聘状态")]
        public override string EnumApplyStatus { get; set; }

        [Display(Name = "步骤数")]
        public override string Step { get; set; }

        [Display(Name = "结果")]
        public override string Result { get; set; }

        [Display(Name = "内容")]
        public override string Content { get; set; }

        [Display(Name = "发生时间")]
        public override string HappenDate { get; set; }

        [Display(Name = "配置时间")]
        public override string ConfigDate { get; set; }

        [Display(Name = "配置地点")]
        public override string ConfigPlace { get; set; }

        public override string ToString()
        {
            return "PK_App_ApplyJob_Id:" + PK_App_ApplyJob_Id + ",PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName + ",EnumApplyStatus:" + EnumApplyStatus
     + ",Step:" + Step + ",Result:" + Result + ",Content:" + Content
     + ",HappenDate:" + HappenDate + ",ConfigDate:" + ConfigDate + ",ConfigPlace:" + ConfigPlace
    ;
        }
    }

    #region 应聘步骤记录
    public class ApplyJobRecordVm
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string PK_App_ApplyJob_Id { get; set; }
        /// <summary>
        /// 应聘人
        /// </summary>
        public string PK_App_Customer_CustomerName { get; set; }
        /// <summary>
        /// 应聘状态
        /// </summary>
        public string EnumApplyStatus { get; set; }
        /// <summary>
        /// 步骤数
        /// </summary>
        public string Step { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 发生时间
        /// </summary>
        public string HappenDate { get; set; }
        /// <summary>
        /// 配置时间
        /// </summary>
        public string ConfigDate { get; set; }
        /// <summary>
        /// 配置地点
        /// </summary>
        public string ConfigPlace { get; set; }

        public override string ToString()
        {
            return "PK_App_ApplyJob_Id:" + PK_App_ApplyJob_Id + ",PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName + ",EnumApplyStatus:" + EnumApplyStatus
                 + ",Step:" + Step + ",Result:" + Result + ",Content:" + Content
                 + ",HappenDate:" + HappenDate + ",ConfigDate:" + ConfigDate + ",ConfigPlace:" + ConfigPlace;
        }
    }
    #endregion
}

