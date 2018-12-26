using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.Sys
{
    public partial class SysAttachModel
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

        [Display(Name = "业务")]
        public override string BusinessID { get; set; }

        [Display(Name = "附件")]
        public override string AttachPath { get; set; }

        [Display(Name = "文件名")]
        public override string FileName { get; set; }

        [Display(Name = "后缀")]
        public override string ExtName { get; set; }

        [Display(Name = "类型")]
        public override string EnumType { get; set; }

    }
}

