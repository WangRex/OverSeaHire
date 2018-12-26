using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.WebApi.Models
{
    public class PayModel
    {
        /// <summary>
        /// 支付订单号
        /// </summary>
        [Description("支付宝定义：out_trade_no")]
        public string PayOrderNo { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        [Description("支付宝定义：total_fee")]
        public string PayAmount { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        [Description("支付宝定义：body")]
        public string Body { get; set; }

        /// <summary>
        /// 订单名称
        /// </summary>
        [Description("支付宝定义：subject")]
        public string OrderName { get; set; }

        /// <summary>
        /// 公用回传参数 ( 如果用户请求时传递了该参数，则返回给商户时会回传该参数 )
        /// </summary>
        [Description("支付宝定义：extra_common_param")]
        public string ExtraCommonParam { get; set; }

        /// <summary>
        /// 当前网站域名，用于拼接支付宝回调地址
        /// </summary>
        [Description("自定义：address")]
        public string Address { get; set; }

    }
}
