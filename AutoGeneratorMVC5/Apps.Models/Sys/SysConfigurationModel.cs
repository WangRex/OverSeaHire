using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.Sys
{
    public partial class SysConfigurationModel
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

        [Display(Name = "网站名称")]
        public override string WebName { get; set; }

        [Display(Name = "域名")]
        public override string ComName { get; set; }

        [Display(Name = "公司名")]
        public override string CompanyName { get; set; }

        [Display(Name = "联系人")]
        public override string ContactName { get; set; }

        [Display(Name = "联系人电话")]
        public override string ContactPhone { get; set; }

        [Display(Name = "微信号")]
        public override string ContactWeChat { get; set; }

        [Display(Name = "邮箱")]
        public override string ContactMail { get; set; }

        public override string ToString()
        {
            return "Tel:" + Tel + ",WebName:" + WebName + ",ComName:" + ComName
             + ",CompanyName:" + CompanyName + ",ContactName:" + ContactName + ",ContactPhone:" + ContactPhone
             + ",ContactWeChat:" + ContactWeChat + ",ContactMail:" + ContactMail;
        }
    }

    #region 获取配置信息
    public class SysConfigurationVm
    {
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 联系人手机号
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 联系微信号
        /// </summary>
        public string ContactWeChat { get; set; }

        public override string ToString()
        {
            return "ContactName:" + ContactName + ",ContactPhone:" + ContactPhone
             + ",ContactWeChat:" + ContactWeChat;
        }
    }
    #endregion
}

