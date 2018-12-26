using System;

namespace tenpay
{
    /// <summary>
    /// 微信订单明细实体对象(114)
    /// </summary>
    [Serializable]
    public class WeChatSystem
    {
        
        /// <summary>
        /// 公共号ID(微信分配的公众账号 ID)
        /// </summary>
        public static string appid = "wxbcbe8ab1f628f11a";

        /// <summary>
        /// 公共号秘钥
        /// </summary>
        public static string appsercret = "c6ed065807fc773725305c8673f494b4";

        /// <summary>
        /// 商户号(微信支付分配的商户号)
        /// </summary>
        public static string mch_id = "1487638742";

        /// <summary>
        /// 支付接口秘钥key
        /// </summary>
        public static string api_key = "8af3ed1d21c59f91a8f63d3b4237a447";

        /// <summary>
        /// 证书路径
        /// </summary>
        public static string path = "D:\\weixin\\apiclient_cert.p12";

        /// <summary>
        /// 服务器公网ip（在调用企业向用户付款的接口时，需要检查微信商户的api安全选项中是否添加服务ip为白名单）
        /// </summary>
        public static string ip = "218.60.80.165";
    }
}