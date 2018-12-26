using System;
using System.ComponentModel.DataAnnotations;

namespace Apps.Models.Sys
{
    public partial class SysRoleModel
    {

        public override string Id { get; set; }
        [NotNullExpression]
        [Display(Name = "角色名称")]
        public override string Name { get; set; }

        [Display(Name = "说明")]
        public override string Description { get; set; }

        [Display(Name = "创建时间")]
        public override DateTime CreateTime { get; set; }

        [Display(Name = "创建人")]
        public override string CreatePerson { get; set; }

        [Required]
        [Display(Name = "角色code")]
        public override string RoleCode { get; set; }

        [Display(Name = "修改时间")]
        public override DateTime ModificationTime { get; set; }

        [Display(Name = "修改时间")]
        public override string ModificationPerson { get; set; }

        [Display(Name = "拥有的用户")]
        public string UserName { get; set; }

        //用户分配角色
        public string Flag { get; set; }

        public override string ToString()
        {
            return "Name:" + Name + ",Description:" + Description + ",CreatePerson:" + CreatePerson
                + ",RoleCode:" + RoleCode;
        }
    }
}
