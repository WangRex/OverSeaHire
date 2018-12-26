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
    public class TenpayToUserUtil
    {
        /// <summary>
        /// 企业付款给个人，直接入帐到微信钱包中
        /// </summary>

        public static string TENPAY = "1";
        public static string APPID = WeChatSystem.appid;              //开发者应用ID
        public static string PARTNER = WeChatSystem.mch_id;          //商户号
        public static string APPSECRET = WeChatSystem.appsercret;      //开发者应用密钥
        public static string PARTNER_KEY = WeChatSystem.api_key;   //商户秘钥
        public static string spbill_create_ip = WeChatSystem.ip;   //公用网ip

        public const string URL = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";

        /// <summary>
        /// 企业付款给个人
        /// </summary>       
        /// <returns></returns>
        public static string EnterprisePay(string Bill_No, string toOpenid, decimal Charge_Amt, string userName, string title)
        {

            //公众账号appid mch_appid 是 wx8888888888888888 String 微信分配的公众账号ID（企业号corpid即为此appId） 
            //商户号 mchid 是 1900000109 String(32) 微信支付分配的商户号 
            //设备号 device_info 否 013467007045764 String(32) 微信支付分配的终端设备号 
            //随机字符串 nonce_str 是 5K8264ILTKCH16CQ2502SI8ZNMTM67VS String(32) 随机字符串，不长于32位 
            //签名 sign 是 C380BEC2BFD727A4B6845133519F3AD6 String(32) 签名，详见签名算法 
            //商户订单号 partner_trade_no 是 10000098201411111234567890 String 商户订单号，需保持唯一性 
            //用户openid openid 是 oxTWIuGaIt6gTKsQRLau2M0yL16E String 商户appid下，某用户的openid 
            //校验用户姓名选项 check_name 是 OPTION_CHECK String NO_CHECK：不校验真实姓名 
            //FORCE_CHECK：强校验真实姓名（未实名认证的用户会校验失败，无法转账） 
            //OPTION_CHECK：针对已实名认证的用户才校验真实姓名（未实名认证用户不校验，可以转账成功） 
            //收款用户姓名 re_user_name 可选 马花花 String 收款用户真实姓名。 
            // 如果check_name设置为FORCE_CHECK或OPTION_CHECK，则必填用户真实姓名 
            //金额 amount 是 10099 int 企业付款金额，单位为分 
            //企业付款描述信息 desc 是 理赔 String 企业付款操作说明信息。必填。 
            //Ip地址 spbill_create_ip 是 192.168.0.1 String(32) 调用接口的机器Ip地址 

            //Bill_No = PARTNER + HttpUtil.getTimestamp() + Bill_No;  //订单号组成 商户号 + 随机时间串 + 记录ID

            //设置package订单参数
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            string total_fee = (Charge_Amt * 100).ToString("f0");
            string wx_nonceStr = Guid.NewGuid().ToString().Replace("-", "");    //Interface_WxPay.getNoncestr();

            dic.Add("mch_appid", APPID);
            dic.Add("mchid", PARTNER);//财付通帐号商家
            //dic.Add("device_info", "013467007045711");//可为空
            dic.Add("nonce_str", wx_nonceStr);
            dic.Add("partner_trade_no", Bill_No);
            dic.Add("openid", toOpenid);
            dic.Add("check_name", "NO_CHECK");
            dic.Add("amount", total_fee);
            dic.Add("desc", title);//商品描述
            dic.Add("spbill_create_ip", spbill_create_ip);   //用户的公网ip，不是商户服务器IP
            //生成签名

            string get_sign = HttpUtil.BuildRequest(dic, PARTNER_KEY);


            //Vincent._Log.SaveMessage("第一步 get_sign：" + get_sign);

            string _req_data = "<xml>";
            _req_data += "<mch_appid>" + APPID + "</mch_appid>";
            _req_data += "<mchid>" + PARTNER + "</mchid>";
            _req_data += "<nonce_str>" + wx_nonceStr + "</nonce_str>";
            _req_data += "<partner_trade_no>" + Bill_No + "</partner_trade_no>";
            _req_data += "<openid>" + toOpenid + "</openid>";
            _req_data += "<check_name>NO_CHECK</check_name>";
            _req_data += "<amount>" + total_fee + "</amount>";
            _req_data += "<desc>" + title + "</desc>";
            _req_data += "<spbill_create_ip>"+ spbill_create_ip + "</spbill_create_ip>";
            _req_data += "<sign>" + get_sign + "</sign>";
            _req_data += "</xml>";


            var result = HttpUtil.HttpPost(URL, _req_data.Trim(), true, 300);
            //var result = HttpPost(URL, _req_data, Encoding.UTF8);

            return result;

            //ReturnValue retValue = StreamReaderUtils.StreamReader(URL, Encoding.UTF8.GetBytes(_req_data), System.Text.Encoding.UTF8, true);
            //Vincent._Log.SaveMessage("返回结果：" + retValue.ErrorCode);
            //return retValue.ErrorCode;            
        }
    }
}