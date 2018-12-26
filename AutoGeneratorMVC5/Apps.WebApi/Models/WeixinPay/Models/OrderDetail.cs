using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tenpay
{
    /// <summary>
    /// 微信订单明细实体对象
    /// </summary>
    [Serializable]
    public class OrderDetail
    {
        /// <summary>
        /// 返回状态码，SUCCESS/FAIL 此字段是通信标识，非交易标识，交易是否成功需要查看trade_state来判断
        /// </summary>
        public string return_code = "";

        /// <summary>
        /// 返回信息返回信息，如非空，为错误原因 签名失败 参数格式校验错误
        /// </summary>
        public string return_msg = "";

        /// <summary>
        /// 公共号ID(微信分配的公众账号 ID)
        /// </summary>
        public string appid = "";

        /// <summary>
        /// 商户号(微信支付分配的商户号)
        /// </summary>
        public string mch_id = "";

        /// <summary>
        /// 随机字符串，不长于32位
        /// </summary>
        public string nonce_str = "";

        /// <summary>
        /// 签名
        /// </summary>
        public string sign = "";

        /// <summary>
        /// 业务结果,SUCCESS/FAIL
        /// </summary>
        public string result_code = "";

        /// <summary>
        /// 错误代码
        /// </summary>
        public string err_code = "";

        /// <summary>
        /// 错误代码描述
        /// </summary>
        public string err_code_des = "";

        /// <summary>
        /// 交易状态
        ///SUCCESS—支付成功
        ///REFUND—转入退款
        ///NOTPAY—未支付
        ///CLOSED—已关闭
        ///REVOKED—已撤销
        ///USERPAYING--用户支付中
        ///NOPAY--未支付(输入密码或确认支付超时) PAYERROR--支付失败(其他原因，如银行返回失败)
        /// </summary>
        public string trade_state = "";

        /// <summary>
        /// 微信支付分配的终端设备号
        /// </summary>
        public string device_info = "";

        /// <summary>
        /// 用户在商户appid下的唯一标识
        /// </summary>
        public string openid = "";

        /// <summary>
        /// 用户是否关注公众账号，Y-关注，N-未关注，仅在公众账号类型支付有效
        /// </summary>
        public string is_subscribe = "";

        /// <summary>
        /// 交易类型,JSAPI、NATIVE、MICROPAY、APP
        /// </summary>
        public string trade_type = "";

        /// <summary>
        /// 银行类型，采用字符串类型的银行标识
        /// </summary>
        public string bank_type = "";

        /// <summary>
        /// 订单总金额，单位为分
        /// </summary>
        public string total_fee = "";

        /// <summary>
        /// 现金券支付金额<=订单总金额，订单总金额-现金券金额为现金支付金额
        /// </summary>
        public string coupon_fee = "";

        /// <summary>
        /// 货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY
        /// </summary>
        public string fee_type = "";

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id = "";

        /// <summary>
        /// 商户系统的订单号，与请求一致。
        /// </summary>
        public string out_trade_no = "";

        /// <summary>
        /// 商家数据包，原样返回
        /// </summary>
        public string attach = "";

        /// <summary>
        /// 支付完成时间，格式为yyyyMMddhhmmss，如2009年12月27日9点10分10秒表示为20091227091010。
        /// 时区为GMT+8 beijing。该时间取自微信支付服务器
        /// </summary>
        public string time_end = "";

    }
}