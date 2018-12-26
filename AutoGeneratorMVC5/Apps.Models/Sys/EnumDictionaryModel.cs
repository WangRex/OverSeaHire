using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.Sys
{
    public partial class EnumDictionaryModel
    {
        [Display(Name = "主键")]
        public override int Id { get; set; }

        [Display(Name = "关联ID")]
        public override int? parentId { get; set; }

        [Display(Name = "表名")]
        public override string TableName { get; set; }

        [Display(Name = "名字")]
        public override string ItemName { get; set; }

        [Display(Name = "值")]
        public override string ItemValue { get; set; }

        [Display(Name = "排序")]
        public override int? SortCode { get; set; }

        [Display(Name = "创建时间")]
        public override DateTime CreateTime { get; set; }

        [Display(Name = "更新时间")]
        public override DateTime ModificationTime { get; set; }

        [Display(Name = "创建人姓名")]
        public override string CreateUserName { get; set; }

        [Display(Name = "修改人姓名")]
        public override string ModificationUserName { get; set; }

        [Display(Name = "枚举照片")]
        public override string ItemPhoto { get; set; }

    }
}

