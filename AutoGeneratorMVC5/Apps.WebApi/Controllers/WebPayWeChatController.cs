/**
* 命名空间: Apps.WebApi.Controllers
*
* 功 能： N/A
* 类 名： WebPayWeChatController
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017-11-27 17:06:16 王仁禧 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：大连安琪科技有限公司 　　　　　　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using Apps.Common;
using Apps.WebApi.Core;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Http;
using tenpay;

namespace Apps.WebApi.Controllers
{
    /// <summary>
    /// 支付接口
    /// </summary>
    public class WebPayWeChatController : BaseApiController
    {
        #region 统一下单（App）
        /// <summary>
        /// 统一下单（App）
        /// </summary> 	
        /// <param name="OrderId">订单id</param>
        /// <param name="price">支付金额</param>
        /// <returns></returns>
        [HttpGet]
        public object payApp(string OrderId, string price)
        {
            LogHandler.WriteServiceLog("API", "订单id ：" + OrderId + "价格 ：" + price, "成功", "进入方法", "WebPayWeChatController.payApp()");
            Response response = new Response();
            UnifiedOrder order = new UnifiedOrder();
            order.appid = WeChatPara.APP_ID;
            order.body = "胖姐包子铺-服务费";
            order.mch_id = WeChatPayPara.WXPay_MCH_ID;
            order.nonce_str = TenpayUtil.getNoncestr();
            order.notify_url = WeChatPayPara.WXPay_NOTIFY_URL;
            order.out_trade_no = OrderId;
            order.trade_type = "APP";
            order.spbill_create_ip = TenpayUtil.GetHostAddress();
            order.total_fee = (int)Convert.ToDecimal(price) * 100;

            TenpayUtil tenpay = new TenpayUtil();
            string prepay_id = tenpay.getPrepay_id(order, WeChatPayPara.WXPay_KEY);

            string timeStamp = TenpayUtil.getTimestamp();
            string nonceStr = TenpayUtil.getNoncestr();

            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("appid", WeChatPara.APP_ID);
            sParams.Add("partnerid", WeChatPayPara.WXPay_MCH_ID);
            sParams.Add("prepayid", prepay_id);
            sParams.Add("timestamp", timeStamp);
            sParams.Add("noncestr", nonceStr);
            sParams.Add("package", "Sign=WXPay");

            string paySign = tenpay.getsign(sParams, WeChatPayPara.WXPay_KEY);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("appid", WeChatPara.APP_ID);
            dic.Add("partnerid", WeChatPayPara.WXPay_MCH_ID);
            dic.Add("prepayid", prepay_id);
            dic.Add("nonceStr", nonceStr);
            dic.Add("timeStamp", timeStamp);
            dic.Add("package", "Sign=WXPay");
            dic.Add("sign", paySign);
            dic.Add("out_trade_no", OrderId);

            return Json(dic);
        }
        #endregion

        #region 统一下单（H5）
        /// <summary>
        /// 统一下单（H5）
        /// </summary> 	
        /// <param name="OrderId">订单id</param>
        /// <param name="price">支付金额</param>
        /// <returns></returns>
        [HttpGet]
        public object payH5(string OrderId, string price)
        {
            LogHandler.WriteServiceLog("API", "订单ID ：" + OrderId + "支付金额 ：" + price, "成功", "进入方法", "WebPayWeChatController.payH5()");
            Response response = new Response();
            UnifiedOrder order = new UnifiedOrder();
            order.appid = WeChatSystem.appid;
            order.body = "宽带管家-服务费（h5）";
            order.mch_id = WeChatSystem.mch_id;
            order.nonce_str = TenpayUtil.getNoncestr();
            order.notify_url = "http://www.baidu.com";
            order.out_trade_no = OrderId;
            order.trade_type = "MWEB";
            order.scene_info = "{\"h5_info\": {\"type\":\"Android\",\"app_name\": \"王者荣耀\",\"package_name\": \"com.tencent.tmgp.sgame\"}}";
            order.spbill_create_ip = TenpayUtil.GetHostAddress();
            order.total_fee = (int)Convert.ToDecimal(price) * 100;
            //order.total_fee = 1;

            TenpayUtil tenpay = new TenpayUtil();
            string mweb_url = tenpay.getMweb_url(order, WeChatSystem.api_key);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("mweb_url", mweb_url);
            dic.Add("status", "success");

            return Json(dic);
        }
        #endregion

        #region 统一下单（微信）
        /// <summary>
        /// 统一下单（微信）
        /// </summary> 	
        /// <param name="OrderId">订单id</param>
        /// <param name="price">支付金额</param>
        /// <param name="openid">openid</param>
        /// <returns></returns>
        [HttpGet]
        public object payWechat(string OrderId, decimal price, string openid)
        {
            LogHandler.WriteServiceLog("API", "订单ID ：" + OrderId + "支付金额 ：" + price + ",openid:" + openid, "成功", "进入方法", "WebPayWeChatController.payWechat()");
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            TenpayUtil tenpay = new TenpayUtil();
            string total_fee = (price * 100).ToString("f0");
            string wx_nonceStr = Guid.NewGuid().ToString().Replace("-", "");    //Interface_WxPay.getNoncestr();
            string timeStamp = TenpayUtil.getTimestamp();
            string spbill_create_ip = TenpayUtil.GetHostAddress();

            dic.Add("appid", WeChatPara.APP_ID);
            dic.Add("mch_id", WeChatPayPara.WXPay_MCH_ID);//财付通帐号商家
            //dic.Add("device_info", "013467007045711");//可为空
            dic.Add("nonce_str", wx_nonceStr);
            dic.Add("body", WeChatPayPara.WXPay_Body);
            dic.Add("out_trade_no", OrderId);
            dic.Add("total_fee", total_fee);
            dic.Add("spbill_create_ip", spbill_create_ip);
            dic.Add("openid", openid);//openid
            dic.Add("notify_url", WeChatPayPara.WXPay_NOTIFY_URL);//异步回掉
            dic.Add("trade_type", "JSAPI");   //用户的公网ip，不是商户服务器IP
            dic.Add("limit_pay", "no_credit");   //no_credit--可限制用户不能使用信用卡支付

            string get_sign = HttpUtil.BuildRequest(dic, WeChatPayPara.WXPay_KEY);

            string _req_data = "<xml>";
            _req_data += "<appid>" + WeChatPara.APP_ID + "</appid>";
            _req_data += "<mch_id>" + WeChatPayPara.WXPay_MCH_ID + "</mch_id>";
            _req_data += "<nonce_str>" + wx_nonceStr + "</nonce_str>";
            _req_data += "<body>" + dic["body"] + "</body>";
            _req_data += "<out_trade_no>" + OrderId + "</out_trade_no>";
            _req_data += "<total_fee>" + total_fee + "</total_fee>";
            _req_data += "<limit_pay>no_credit</limit_pay>";
            _req_data += "<spbill_create_ip>" + spbill_create_ip + "</spbill_create_ip>";
            _req_data += "<notify_url>"+ WeChatPayPara.WXPay_NOTIFY_URL + "</notify_url>";
            _req_data += "<trade_type>JSAPI</trade_type>";
            _req_data += "<openid>" + openid + "</openid>";
            _req_data += "<sign>" + get_sign + "</sign>";
            _req_data += "</xml>";

            var result = HttpUtil.HttpPost("https://api.mch.weixin.qq.com/pay/unifiedorder", _req_data.Trim(), false, 300);
            SortedDictionary<string, string> dica = HttpUtil.GetInfoFromXml(result);
            string return_code = dica["return_code"];
            Response response = new Response();
            if (return_code.Equals("SUCCESS"))
            {
                string result_code = dica["result_code"];
                if (result_code.Equals("SUCCESS"))
                {
                    string prepay_id = dica["prepay_id"];
                    LogHandler.WriteServiceLog("API", "获取prepayid:" + prepay_id, "成功", "prepayid", "WebPayWeChatController.payWechat()");
                    SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
                    sParams.Add("appId", WeChatPara.APP_ID);
                    sParams.Add("nonceStr", wx_nonceStr);
                    sParams.Add("package", "prepay_id=" + prepay_id);
                    sParams.Add("signType", "MD5");
                    sParams.Add("timeStamp", timeStamp);

                    string paySign = tenpay.getsign(sParams, WeChatPayPara.WXPay_KEY);
                    sParams.Add("paySign", paySign);
                    response.Code = 0;
                    response.Message = "获取prepay_id成功";
                    response.Data = sParams;
                }
                else
                {
                    LogHandler.WriteServiceLog("API", "err_code_des:" + dica["err_code_des"], "失败", "prepayid", "WebPayWeChatController.payWechat()");
                    response.Code = 1;
                    response.Message = "获取prepay_id失败";
                    response.Data = new
                    {
                        PS_OrderDetail = dica
                    };
                }
            }
            else
            {
                LogHandler.WriteServiceLog("API", "err_code_des:" + dica["return_msg"], "失败", "prepayid", "WebPayWeChatController.payWechat()");
                response.Code = 1;
                response.Message = "获取prepay_id失败";
                response.Data = new
                {
                    PS_OrderDetail = dica
                };
            }
            return Json(response);
        }
        #endregion

        #region 企业向用户付款到零钱
        /// <summary>
        /// 企业向用户付款到零钱
        /// </summary>
        /// <param name="Bill_No">订单Id</param>
        /// <param name="toOpenid">openid</param>
        /// <param name="Charge_Amt">支付金额（必须是数字）</param>
        /// <param name="userName">姓名（收款人姓名）</param>
        /// <param name="title">备注（如果没有的话，默认传：员工提现到零钱）</param>
        /// <returns></returns>
        [HttpPost]
        public object PayToUser(string Bill_No, string toOpenid, decimal Charge_Amt, string userName, string title)
        {
            string result = TenpayToUserUtil.EnterprisePay(Bill_No, toOpenid, Charge_Amt, userName, title);
            SortedDictionary<string, string> dica = HttpUtil.GetInfoFromXml(result);
            string return_code = dica["return_code"];
            Response response = new Response();

            if (return_code.Equals("SUCCESS"))
            {
                response.Code = 0;
                response.Message = "提现成功";
                response.Data = new
                {
                    PS_OrderDetail = dica
                };
            }
            else
            {
                response.Code = 1;
                response.Message = "提现失败";
                response.Data = new
                {
                    PS_OrderDetail = dica
                };
            }
            return Json(response);
        }
        #endregion

        #region 申请退款
        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="Bill_No">订单Id</param>
        /// <param name="Charge_Amt">原订单金额</param>
        /// <param name="Total">退款金额(小于等于金额)</param>
        /// <returns></returns>
        [HttpPost]
        public object payback(string Bill_No, decimal Charge_Amt, decimal Total)
        {
            string result = TenpaybackUtil.Payback(Bill_No, Charge_Amt, Total);
            SortedDictionary<string, string> dica = HttpUtil.GetInfoFromXml(result);
            string return_code = dica["return_code"];

            Response response = new Response();
            if (return_code.Equals("SUCCESS"))
            {
                string result_code = dica["result_code"];
                if (result_code.Equals("SUCCESS"))
                {
                    response.Code = 0;
                    response.Message = "退款申请成功";
                    response.Data = new
                    {
                        PS_OrderDetail = dica
                    };
                }
                else
                {
                    response.Code = 1;
                    response.Message = "退款申请失败";
                    response.Data = new
                    {
                        PS_OrderDetail = dica
                    };
                }
            }
            else
            {
                response.Code = 1;
                response.Message = "退款申请失败";
                response.Data = new
                {
                    PS_OrderDetail = dica
                };
            }
            return Json(response);
        }
        #endregion

        #region 退款查询
        /// <summary>
        /// 退款查询
        /// </summary>
        /// <param name="Bill_No">订单号</param>
        /// <returns></returns>
        [HttpPost]
        public object paybackCheck(string Bill_No)
        {
            string result = TenpaybackUtil.PaybackCheck(Bill_No);
            string return_code = JsonHelper.GetJsonValue(result, "return_code");
            Response response = new Response();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (return_code.Equals("SUCCESS"))
            {
                dic.Add("partner_trade_no", JsonHelper.GetJsonValue(result, "partner_trade_no"));
                response.Code = 0;
                response.Message = "退款申请成功";
                response.Data = new
                {
                    PS_OrderDetail = dic
                };
            }
            else
            {
                dic.Add("return_msg", JsonHelper.GetJsonValue(result, "partner_trade_no"));
                response.Code = 1;
                response.Message = "退款申请成功";
                response.Data = new
                {
                    PS_OrderDetail = dic
                };
            }
            return Json(response);
        }
        #endregion

        #region 微信授权登录app
        /// <summary>
        /// 微信授权登录app
        /// </summary>
        /// <param name="Code">Code</param>
        /// <returns></returns>
        [HttpGet]
        public object getOpenid(string Code)
        {
            LogHandler.WriteServiceLog("API", "Code：" + Code, "成功", "进入方法", "WebPayWeChatController.getOpenid()");
            Response response = new Response();

            string strJson = RequestUrl(string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                 WeChatPara.APP_ID, WeChatPara.APP_SECRET, Code), "GET");

            string openid = JsonHelper.GetJsonValue(strJson, "access_token");
            if (string.IsNullOrEmpty(openid))
            {
                response.Code = 0;
                response.Message = "获取成功";
                response.Data = new
                {
                    openid = openid
                };
            }
            else
            {
                response.Code = 1;
                response.Message = "获取失败";
                response.Data = null;
            }
            return Json(response);
        }
        #endregion

        #region 请求Url，不发送数据

        /// <summary>
        /// 请求Url，不发送数据
        /// </summary>
        public static string RequestUrl(string url, string method)
        {
            // 设置参数
            var request = WebRequest.Create(url) as HttpWebRequest;
            var cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = method;
            request.ContentType = "text/html";
            request.Headers.Add("charset", "utf-8");
            //发送请求并获取相应回应数据
            var response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            var sr = new StreamReader(responseStream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();

            return content;
        }

        #endregion
    }
}