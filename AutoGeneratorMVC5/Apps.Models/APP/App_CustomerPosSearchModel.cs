using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.App
{
    public partial class App_CustomerPosSearchModel
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

        [Display(Name = "内容")]
        public override string Content { get; set; }

        [Display(Name = "搜索人")]
        public override string PK_App_Customer_CustomerName { get; set; }

    public override string ToString()    {
        return "Content:" + Content + ",PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName;          }
     }
}

