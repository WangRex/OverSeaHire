using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace tenpay
{
    public class TenpayUtil
    {
        /// <summary>
        /// 统一支付接口
        /// </summary>
        const string UnifiedPayUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        /// <summary>
        /// 网页授权接口
        /// </summary>
        const string access_tokenUrl = "https://api.weixin.qq.com/sns/oauth2/access_token";

        /// <summary>
        /// app获取openid、accesstoken
        /// </summary>
        const string access_tokenUrlapp = "";
        /// <summary>
        /// 微信订单查询接口
        /// </summary>
        const string OrderQueryUrl = "https://api.mch.weixin.qq.com/pay/orderquery";

        /// <summary>
        /// 随机串
        /// </summary>
        public static string getNoncestr()
        {
            Random random = new Random();
            return MD5Util.GetMD5(random.Next(1000).ToString(), "GBK").ToLower().Replace("s", "S");
        }

        /// <summary>
        /// 时间截，自1970年以来的秒数
        /// </summary>
        public static string getTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 网页授权接口
        /// </summary>
        public static string getAccess_tokenUrl()
        {
            return access_tokenUrl;
        }

        /// <summary>
        /// 获取微信签名
        /// </summary>
        /// <param name="sParams"></param>
        /// <returns></returns>
        public string getsign(SortedDictionary<string, string> sParams, string key)
        {
            int i = 0;
            string sign = string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in sParams)
            {
                if (temp.Value == "" || temp.Value == null || temp.Key.ToLower() == "sign")
                {
                    continue;
                }
                i++;
                sb.Append(temp.Key.Trim() + "=" + temp.Value.Trim() + "&");
            }
            sb.Append("key=" + key.Trim() + "");
            string signkey = sb.ToString();
            sign = MD5Util.GetMD5(signkey, "utf-8");

            return sign;
        }

        /// <summary>
        /// post数据到指定接口并返回数据
        /// </summary>
        public string PostXmlToUrl(string url, string postData)
        {
            string returnmsg = "";
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                returnmsg = wc.UploadString(url, "POST", postData);
            }
            return returnmsg;
        }

        /// <summary>
        /// 获取prepay_id
        /// </summary>
        public string getPrepay_id(UnifiedOrder order, string key)
        {
            string prepay_id = "";
            string post_data = getUnifiedOrderXml(order, key);
            string request_data = PostXmlToUrl(UnifiedPayUrl, post_data);
            SortedDictionary<string, string> requestXML = GetInfoFromXml(request_data);
            foreach (KeyValuePair<string, string> k in requestXML)
            {
                if (k.Key == "prepay_id")
                {
                    prepay_id = k.Value;
                    break;
                }
            }
            return prepay_id;
        }

        /// <summary>
        /// 获取prepay_id
        /// </summary>
        public string getMweb_url(UnifiedOrder order, string key)
        {
            string prepay_id = "";
            string post_data = getUnifiedOrderXml(order, key);
            string request_data = PostXmlToUrl(UnifiedPayUrl, post_data);
            SortedDictionary<string, string> requestXML = GetInfoFromXml(request_data);
            foreach (KeyValuePair<string, string> k in requestXML)
            {
                if (k.Key == "mweb_url")
                {
                    prepay_id = k.Value;
                    break;
                }
            }
            return prepay_id;
        }

        /// <summary>
        /// 获取微信订单明细
        /// </summary>
        public OrderDetail getOrderDetail(QueryOrder queryorder, string key)
        {
            string post_data = getQueryOrderXml(queryorder, key);
            string request_data = PostXmlToUrl(OrderQueryUrl, post_data);
            OrderDetail orderdetail = new OrderDetail();
            SortedDictionary<string, string> requestXML = GetInfoFromXml(request_data);
            foreach (KeyValuePair<string, string> k in requestXML)
            {
                switch (k.Key)
                {
                    case "retuen_code":
                        orderdetail.result_code = k.Value;
                        break;
                    case "return_msg":
                        orderdetail.return_msg = k.Value;
                        break;
                    case "appid":
                        orderdetail.appid = k.Value;
                        break;
                    case "mch_id":
                        orderdetail.mch_id = k.Value;
                        break;
                    case "nonce_str":
                        orderdetail.nonce_str = k.Value;
                        break;
                    case "sign":
                        orderdetail.sign = k.Value;
                        break;
                    case "result_code":
                        orderdetail.result_code = k.Value;
                        break;
                    case "err_code":
                        orderdetail.err_code = k.Value;
                        break;
                    case "err_code_des":
                        orderdetail.err_code_des = k.Value;
                        break;
                    case "trade_state":
                        orderdetail.trade_state = k.Value;
                        break;
                    case "device_info":
                        orderdetail.device_info = k.Value;
                        break;
                    case "openid":
                        orderdetail.openid = k.Value;
                        break;
                    case "is_subscribe":
                        orderdetail.is_subscribe = k.Value;
                        break;
                    case "trade_type":
                        orderdetail.trade_type = k.Value;
                        break;
                    case "bank_type":
                        orderdetail.bank_type = k.Value;
                        break;
                    case "total_fee":
                        orderdetail.total_fee = k.Value;
                        break;
                    case "coupon_fee":
                        orderdetail.coupon_fee = k.Value;
                        break;
                    case "fee_type":
                        orderdetail.fee_type = k.Value;
                        break;
                    case "transaction_id":
                        orderdetail.transaction_id = k.Value;
                        break;
                    case "out_trade_no":
                        orderdetail.out_trade_no = k.Value;
                        break;
                    case "attach":
                        orderdetail.attach = k.Value;
                        break;
                    case "time_end":
                        orderdetail.time_end = k.Value;
                        break;
                    default:
                        break;
                }
            }
            return orderdetail;
        }

        /// <summary>
        /// 获取客户端ip地址
        /// </summary>
        /// <returns></returns>
        public static string GetHostAddress()
        {
            string userHostAddress = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 把XML数据转换为SortedDictionary<string, string>集合
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        protected SortedDictionary<string, string> GetInfoFromXml(string xmlstring)
        {
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlstring);
                XmlElement root = doc.DocumentElement;
                int len = root.ChildNodes.Count;
                for (int i = 0; i < len; i++)
                {
                    string name = root.ChildNodes[i].Name;
                    if (!sParams.ContainsKey(name))
                    {
                        sParams.Add(name.Trim(), root.ChildNodes[i].InnerText.Trim());
                    }
                }
            }
            catch { }
            return sParams;
        }

        /// <summary>
        /// 微信统一下单接口xml参数整理
        /// </summary>
        /// <param name="order">微信支付参数实例</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        protected string getUnifiedOrderXml(UnifiedOrder order, string key)
        {
            string return_string = string.Empty;
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("appid", order.appid);
            sParams.Add("attach", order.attach);
            sParams.Add("body", order.body);
            sParams.Add("device_info", order.device_info);
            sParams.Add("mch_id", order.mch_id);
            sParams.Add("nonce_str", order.nonce_str);
            sParams.Add("notify_url", order.notify_url);
            sParams.Add("openid", order.openid);
            sParams.Add("out_trade_no", order.out_trade_no);
            sParams.Add("spbill_create_ip", order.spbill_create_ip);
            sParams.Add("total_fee", order.total_fee.ToString());
            sParams.Add("trade_type", order.trade_type);
            order.sign = getsign(sParams, key);
            sParams.Add("sign", order.sign);

            //拼接成XML请求数据
            StringBuilder sbPay = new StringBuilder();
            foreach (KeyValuePair<string, string> k in sParams)
            {
                if (k.Key == "attach" || k.Key == "body" || k.Key == "sign")
                {
                    sbPay.Append("<" + k.Key + "><![CDATA[" + k.Value + "]]></" + k.Key + ">");
                }
                else
                {
                    sbPay.Append("<" + k.Key + ">" + k.Value + "</" + k.Key + ">");
                }
            }
            return_string = string.Format("<xml>{0}</xml>", sbPay.ToString());
            byte[] byteArray = Encoding.UTF8.GetBytes(return_string);
            return_string = Encoding.GetEncoding("GBK").GetString(byteArray);
            return return_string;

        }

        /// <summary>
        /// 微信订单查询接口XML参数整理
        /// </summary>
        /// <param name="queryorder">微信订单查询参数实例</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        protected string getQueryOrderXml(QueryOrder queryorder, string key)
        {
            string return_string = string.Empty;
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("appid", queryorder.appid);
            sParams.Add("mch_id", queryorder.mch_id);
            sParams.Add("transaction_id", queryorder.transaction_id);
            sParams.Add("out_trade_no", queryorder.out_trade_no);
            sParams.Add("nonce_str", queryorder.nonce_str);
            queryorder.sign = getsign(sParams, key);
            sParams.Add("sign", queryorder.sign);

            //拼接成XML请求数据
            StringBuilder sbPay = new StringBuilder();
            foreach (KeyValuePair<string, string> k in sParams)
            {
                if (k.Key == "attach" || k.Key == "body" || k.Key == "sign")
                {
                    sbPay.Append("<" + k.Key + "><![CDATA[" + k.Value + "]]></" + k.Key + ">");
                }
                else
                {
                    sbPay.Append("<" + k.Key + ">" + k.Value + "</" + k.Key + ">");
                }
            }
            return_string = string.Format("<xml>{0}</xml>", sbPay.ToString().TrimEnd(','));
            return return_string;
        }

        
    }
}