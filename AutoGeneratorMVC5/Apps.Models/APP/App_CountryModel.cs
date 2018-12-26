using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.App
{
    #region App_CountryModel
    public partial class App_CountryModel
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

        [Display(Name = "国家名称")]
        public override string Name { get; set; }

        [Display(Name = "国旗图片")]
        public override string CountryImg { get; set; }

        [Display(Name = "汇率")]
        public override string ExchangeRate { get; set; }

        [Display(Name = "热门")]
        public override string SwitchBtnHot { get; set; }

        public override string ToString()
        {
            return "Name:" + Name + ",CountryImg:" + CountryImg + ",ExchangeRate:" + ExchangeRate
                + ",SwitchBtnHot:" + SwitchBtnHot;
        }
    }
    #endregion

    #region 国家
    public class CountryVm
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CountryImg { get; set; }
        public string ExchangeRate { get; set; }
        public string SwitchBtnHot { get; set; }
        public override string ToString()
        {
            return "Name:" + Name + ",CountryImg:" + CountryImg + ",ExchangeRate:" + ExchangeRate
                + ",SwitchBtnHot:" + SwitchBtnHot;
        }
    }
    #endregion
}

