using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.App
{
    #region App_PositionModel
    public partial class App_PositionModel
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
        [Display(Name = "关联数据名字")]
        public string ParentName { get; set; }

        [Display(Name = "职位名称")]
        public override string Name { get; set; }

        [Display(Name = "职位介绍")]
        public override string Description { get; set; }

        [Display(Name = "常用")]
        public override string SwitchBtnCommonUse { get; set; }

        [Display(Name = "开关状态")]
        public string state { get; set; }

        public override string ToString()
        {
            return "Name:" + Name + ",Description:" + Description + ",SwitchBtnCommonUse:" + SwitchBtnCommonUse; ;
        }
    }
    #endregion

    #region 职位
    public class App_PositionVm
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 职位名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 职位介绍
        /// </summary>
        public string Description { get; set; }

        public override string ToString()
        {
            return "Id:" + Id + ",Name:" + Name + ",Description:" + Description;
        }
    }
    #endregion

    #region 职位树
    public class PositionTreeVm
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 职位名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 职位介绍
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 职位孩子节点
        /// </summary>
        public List<PositionTreeVm> positionTreeVms { get; set; }

        public override string ToString()
        {
            return "Id:" + Id + ",Name:" + Name + ",Description:" + Description;
        }
    }
    #endregion

    #region 职位树【后台用】
    public class PositionComboTreeVm
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 职位名称
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 职位孩子节点
        /// </summary>
        public List<PositionComboTreeVm> children { get; set; }

        public override string ToString()
        {
            return "id:" + id + ",text:" + text;
        }
    }
    #endregion
}

