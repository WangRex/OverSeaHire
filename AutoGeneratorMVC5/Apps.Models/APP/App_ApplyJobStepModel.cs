using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.App
{
    public partial class App_ApplyJobStepModel
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

        [Display(Name = "名称")]
        public override string Name { get; set; }

        [Display(Name = "图标")]
        public override string Icon { get; set; }

        [Display(Name = "描述")]
        public override string Description { get; set; }

        [Display(Name = "步骤数")]
        public override string Step { get; set; }

        public override string ToString()
        {
            return "Name:" + Name + ",Icon:" + Icon + ",Description:" + Description
                + ",Step:" + Step;
        }
    }

    #region App_ApplyJobStepVm
    public class App_ApplyJobStepVm
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public string Step { get; set; }

        public override string ToString()
        {
            return "Name:" + Name + ",Icon:" + Icon + ",Description:" + Description
                + ",Step:" + Step;
        }
    }
    #endregion
}

