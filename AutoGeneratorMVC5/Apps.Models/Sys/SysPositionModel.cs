using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.Sys
{
    public partial class SysPositionEditModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public string state { get; set; }
    }
    public partial class SysPositionModel
    {
        [MaxWordsExpression(50)]
        [Display(Name = "ID")]
        public override string Id { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "职位名称")]
        public override string Name { get; set; }

        [MaxWordsExpression(500)]
        [Display(Name = "职位说明")]
        public override string Remark { get; set; }

        [Display(Name = "排序")]
        public override int Sort { get; set; }

        [Display(Name = "创建时间")]
        public override DateTime CreateTime { get; set; }

        [Display(Name = "状态")]
        public override bool Enable { get; set; }

        [Display(Name = "职位允许人数")]
        public override int MemberCount { get; set; }

        [Display(Name = "部门")]
        public override string DepId { get; set; }
        //部门名
        public string DepName { get; set; }
        //公司名
        public string ComName { get; set; }

        public string Flag { get; set; }

        [Display(Name = "修改时间")]
        public override DateTime ModificationTime { get; set; }

        [Display(Name = "修改人")]
        public override string ModificationUserName { get; set; }

        [Display(Name = "创建人")]
        public override string CreateUserName { get; set; }
    }
}
