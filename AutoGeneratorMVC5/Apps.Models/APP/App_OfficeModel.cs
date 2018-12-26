using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.App
{
    #region App_OfficeModel
    public partial class App_OfficeModel
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

        [Display(Name = "客服电话")]
        public override string Tel { get; set; }

        [Display(Name = "办事处名称")]
        public override string OfficeName { get; set; }

        [Display(Name = "经度")]
        public override string Longitude { get; set; }

        [Display(Name = "纬度")]
        public override string Latitude { get; set; }

        [Display(Name = "联系人")]
        public override string ContactName { get; set; }

        [Display(Name = "联系人电话")]
        public override string ContactPhone { get; set; }

        [Display(Name = "微信号")]
        public override string ContactWeChat { get; set; }

        [Display(Name = "邮箱")]
        public override string ContactMail { get; set; }

        [Display(Name = "地址")]
        public override string OfficeAddress { get; set; }

        public override string ToString()
        {
            return "Tel:" + Tel + ",OfficeName:" + OfficeName + ",Longitude:" + Longitude
             + ",Latitude:" + Latitude + ",ContactName:" + ContactName + ",ContactPhone:" + ContactPhone
             + ",ContactWeChat:" + ContactWeChat + ",ContactMail:" + ContactMail + ",OfficeAddress:" + OfficeAddress;
        }
    }
    #endregion

    #region 办事处信息
    public class OfficeVm
    {
        /// <summary>
        /// 客服电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 办事处名称
        /// </summary>
        public string OfficeName { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        public string ContactWeChat { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string ContactMail { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string OfficeAddress { get; set; }

        public override string ToString()
        {
            return "Tel:" + Tel + ",OfficeName:" + OfficeName + ",Longitude:" + Longitude
             + ",Latitude:" + Latitude + ",ContactName:" + ContactName + ",ContactPhone:" + ContactPhone
             + ",ContactWeChat:" + ContactWeChat + ",ContactMail:" + ContactMail + ",OfficeAddress:" + OfficeAddress;
        }

    } 
    #endregion
}
