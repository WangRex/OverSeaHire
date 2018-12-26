using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace tenpay
{
    public class TenpaybackUtil
    {
        /// <summary>
        /// 企业退款
        /// </summary>

        public static string TENPAY = "1";
        public static string APPID = WeChatSystem.appid;              //开发者应用ID
        public static string PARTNER = WeChatSystem.mch_id;          //商户号
        public static string APPSECRET = WeChatSystem.appsercret;      //开发者应用密钥
        public static string PARTNER_KEY = WeChatSystem.api_key;   //商户秘钥

        public const string URL = "https://api.mch.weixin.qq.com/secapi/pay/refund";

        /// <summary>
        /// 退款
        /// </summary>       
        /// <returns></returns>
        public static string Payback(string Bill_No, decimal Charge_Amt,decimal total)
        {
            string Bill_No1 = PARTNER + HttpUtil.getTimestamp() + Bill_No;  //订单号组成 商户号 + 随机时间串 + 记录ID

            //设置package订单参数
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            string total_fee = (Charge_Amt * 100).ToString("f0");
            string total_feetotal = (total * 100).ToString("f0");
            string wx_nonceStr = Guid.NewGuid().ToString().Replace("-", "");    //Interface_WxPay.getNoncestr();

            dic.Add("appid", APPID);
            dic.Add("mch_id", PARTNER);//微信商户id
            //dic.Add("device_info", "013467007045711");//可为空
            dic.Add("nonce_str", wx_nonceStr);
            dic.Add("out_trade_no", Bill_No);
            dic.Add("out_refund_no", Bill_No1);//微信退款订单账号（商户退款单号）
            dic.Add("total_fee", total_fee);//微信退款订单账号(总金额)
            dic.Add("refund_fee", total_feetotal);//微信退款金额
            //生成签名

            string get_sign = HttpUtil.BuildRequest(dic, PARTNER_KEY);


            //Vincent._Log.SaveMessage("第一步 get_sign：" + get_sign);

            string _req_data = "<xml>";
            _req_data += "<appid>" + APPID + "</appid>";
            _req_data += "<mch_id>" + PARTNER + "</mch_id>";
            _req_data += "<nonce_str>" + wx_nonceStr + "</nonce_str>";
            _req_data += "<out_trade_no>" + Bill_No + "</out_trade_no>";
            _req_data += "<out_refund_no>" + Bill_No1 + "</out_refund_no>";
            _req_data += "<total_fee>" + total_fee + "</total_fee>";
            _req_data += "<refund_fee>" + total_feetotal + "</refund_fee>";
            _req_data += "<sign>" + get_sign + "</sign>";
            _req_data += "</xml>";


            var result = HttpUtil.HttpPost(URL, _req_data.Trim(), true, 300);
            //var result = HttpPost(URL, _req_data, Encoding.UTF8);

            return result;

            //ReturnValue retValue = StreamReaderUtils.StreamReader(URL, Encoding.UTF8.GetBytes(_req_data), System.Text.Encoding.UTF8, true);
            //Vincent._Log.SaveMessage("返回结果：" + retValue.ErrorCode);
            //return retValue.ErrorCode;            
        }
        
        /// <summary>
        /// 退款订单查询
        /// </summary>
        /// <param name="Bill_No">订单号(创建订单时的订单号)</param>
        /// <returns></returns>
        public static string PaybackCheck(string Bill_No)
        {
            string url = "https://api.mch.weixin.qq.com/pay/refundquery";


            //设置package订单参数
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            string wx_nonceStr = Guid.NewGuid().ToString().Replace("-", "");    //Interface_WxPay.getNoncestr();

            dic.Add("appid", APPID);
            dic.Add("mch_id", PARTNER);//微信商户id
            dic.Add("nonce_str", wx_nonceStr);
            dic.Add("out_trade_no", Bill_No);

            //生成签名
            string get_sign = HttpUtil.BuildRequest(dic, PARTNER_KEY);

            string _req_data = "<xml>";
            _req_data += "<appid>" + APPID + "</appid>";
            _req_data += "<mch_id>" + PARTNER + "</mch_id>";
            _req_data += "<nonce_str>" + wx_nonceStr + "</nonce_str>";
            _req_data += "<out_trade_no>" + Bill_No + "</out_trade_no>";
            _req_data += "<sign>" + get_sign + "</sign>";
            _req_data += "</xml>";

            var result = HttpUtil.HttpPost(url, _req_data.Trim(), false, 300);

            return result;
        }
    }
}