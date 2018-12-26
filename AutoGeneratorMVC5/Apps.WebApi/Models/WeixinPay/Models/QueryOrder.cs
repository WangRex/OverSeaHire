using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tenpay
{
    /// <summary>
    /// 微信订单查询接口请求实体对象
    /// </summary>
    [Serializable]
    public class QueryOrder
    {
        /// <summary>
        /// 公共号ID(微信分配的公众账号 ID)
        /// </summary>
        public string appid = "";

        /// <summary>
        /// 商户号(微信支付分配的商户号)
        /// </summary>
        public string mch_id = "";

        /// <summary>
        /// 微信订单号，优先使用
        /// </summary>
        public string transaction_id = "";

        /// <summary>
        /// 商户系统内部订单号
        /// </summary>
        public string out_trade_no = "";

        /// <summary>
        /// 随机字符串，不长于 32 位
        /// </summary>
        public string nonce_str = "";

        /// <summary>
        /// 签名，参与签名参数：appid，mch_id，transaction_id，out_trade_no，nonce_str，key
        /// </summary>
        public string sign = "";
    }
}