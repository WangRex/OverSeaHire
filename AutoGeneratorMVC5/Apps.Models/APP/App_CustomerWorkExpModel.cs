using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.App
{
    #region App_CustomerWorkExpModel
    public partial class App_CustomerWorkExpModel
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

        [Display(Name = "开始时间")]
        public override string StartDate { get; set; }

        [Display(Name = "结束时间")]
        public override string EndDate { get; set; }

        [Display(Name = "公司")]
        public override string Company { get; set; }

        [Display(Name = "职位")]
        public override string Position { get; set; }

        public override string ToString()
        {
            return "PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName + ",StartDate:" + StartDate + ",EndDate:" + EndDate
                + ",Company:" + Company + ",Position:" + Position;
        }
    } 
    #endregion

    #region 工作经历
    public class WorkExpPost
    {
        /// <summary>
        /// 用户
        /// </summary>
        public string PK_App_Customer_CustomerName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }

        public override string ToString()
        {
            return "PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName + ",StartDate:" + StartDate + ",EndDate:" + EndDate
                + ",Company:" + Company + ",Position:" + Position;
        }
    }
    #endregion
}

