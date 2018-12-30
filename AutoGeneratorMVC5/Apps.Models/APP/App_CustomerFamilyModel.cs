using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.App
{
    #region App_CustomerFamilyModel
    public partial class App_CustomerFamilyModel
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

        [Display(Name = "姓名")]
        public override string Name { get; set; }

        [Display(Name = "年龄")]
        public override int Age { get; set; }

        [Display(Name = "关系")]
        public override string Relation { get; set; }

        [Display(Name = "职业")]
        public override string Occupation { get; set; }

        public override string ToString()
        {
            return "PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName + ",Name:" + Name + ",Age:" + Age
                + ",Relation:" + Relation + ",Occupation:" + Occupation;
        }
    }
    #endregion

    #region 家庭关系
    public class FamilyPost
    {
        /// <summary>
        /// 用户
        /// </summary>
        public string PK_App_Customer_CustomerName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 关系
        /// </summary>
        public string Relation { get; set; }
        /// <summary>
        /// 职业
        /// </summary>
        public string Occupation { get; set; }

        public override string ToString()
        {
            return "PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName + ",Name:" + Name + ",Age:" + Age
                + ",Relation:" + Relation + ",Occupation:" + Occupation;
        }
    }
    #endregion
}

