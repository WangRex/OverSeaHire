using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.Sys
{
    #region SysMessageModel
    public partial class SysMessageModel
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

        [Display(Name = "业务主键")]
        public override string BusinessID { get; set; }

        [Display(Name = "业务表名")]
        public override string BusinessTable { get; set; }

        [Display(Name = "用户")]
        public override string PK_App_Customer_CustomerName { get; set; }

        [Display(Name = "阅读")]
        public override string SwitchBtnRead { get; set; }

        [Display(Name = "标题")]
        public override string Title { get; set; }

        [Display(Name = "内容")]
        public override string Content { get; set; }

        [Display(Name = "消息类型")]
        public override string EnumMessageType { get; set; }

        [Display(Name = "按钮")]
        public override string SwitchBtnButton { get; set; }

        [Display(Name = "显示文字")]
        public override string ShowMessage { get; set; }

        [Display(Name = "被邀请人主键")]
        public override string WorkerId { get; set; }

        public override string ToString()
        {
            return "BusinessID:" + BusinessID + ",BusinessTable:" + BusinessTable + ",PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName
                + ",SwitchBtnRead:" + SwitchBtnRead + ",Title:" + Title + ",Content:" + Content
                + ",EnumMessageType:" + EnumMessageType + ",SwitchBtnButton:" + SwitchBtnButton + ",ShowMessage:" + ShowMessage
                + ",WorkerId:" + WorkerId;
        }
    }
    #endregion

    #region 消息
    public class SysMessageVm
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 业务主键
        /// </summary>
        public string BusinessID { get; set; }
        /// <summary>
        /// 业务表名
        /// </summary>
        public string BusinessTable { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public string PK_App_Customer_CustomerName { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public string App_Customer_CustomerName { get; set; }
        /// <summary>
        /// 阅读
        /// </summary>
        public string SwitchBtnRead { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string EnumMessageType { get; set; }
        /// <summary>
        /// 按钮
        /// </summary>
        public string SwitchBtnButton { get; set; }
        /// <summary>
        /// 显示文字
        /// </summary>
        public string ShowMessage { get; set; }
        /// <summary>
        /// 被邀请人主键
        /// </summary>
        public string WorkerId { get; set; }

        public override string ToString()
        {
            return "BusinessID:" + BusinessID + ",BusinessTable:" + BusinessTable + ",PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName
                + ",SwitchBtnRead:" + SwitchBtnRead + ",Title:" + Title + ",Content:" + Content
                + ",EnumMessageType:" + EnumMessageType + ",SwitchBtnButton:" + SwitchBtnButton + ",ShowMessage:" + ShowMessage
                + ",WorkerId:" + WorkerId;
        }

    }
    #endregion

    #region 消息列表查询
    public class SysMessageQuery
    {
        public string UserId { get; set; }
        public bool AdminFlag { get; set; }
        public string CustomerId { get; set; }
        public string EnumMessageType { get; set; }
        public string SwitchBtnRead { get; set; }
        public override string ToString()
        {
            return "UserId:" + UserId + ",AdminFlag:" + AdminFlag
                 + ",CustomerId:" + CustomerId + ",EnumMessageType:" + EnumMessageType
                 + ",EnumMessageType:" + EnumMessageType;
        }
    }
    #endregion
}

