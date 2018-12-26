
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using Apps.WebApi.Core;
using Apps.WebApi.Models;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;


namespace Apps.WebApi.Controllers
{

    /// <summary>
    /// H5支付宝支付 页面端调用例子参考：AlipaySample.cshtml
    /// AOPSDK.dll为自己封装，请勿使用阿里官网SDK替换,主要区别为阿里提供的返回表单，我给换成了返回JSON
    /// </summary>
    public class WebPayController : BaseApiController
    {
        ////沙盒测试
        //const string postUrl = "https://openapi.alipaydev.com/gateway.do";
        //const string app_id = "2016080500176886";
        //const string merchant_private_key =
        //"MIIEpQIBAAKCAQEA6GDxix5VIeF+if0+1zUy5QFxnUh5HRngKk44ACy82WUD0gxwYR00zg6HtGJCfaa9o7R9NCvnnd2pSBMptsohJDMSPJ3M/X5kXuLYyk1sXf/5DjiKs48gIL73Rzl0xyzD0NXV1DqKQil4tVJF9cHPesgejCjVw9nuyGnMDeZjFnZmbRAbKraNMuiPBuaoTSVJsEF6KttPpqfwvW2h2mCfbKqrdWI/s0oTAn2w38BLujSp9lMtTUqdu69sI+Pp+ofzhmDZxCK/KOkr+GdtHy9x2Wl4rLHK00027MSDyUwhupGsn3ZDT5USB2q7KhsayZdtW+/vx+zsgVrTMDfXMYYnaQIDAQABAoIBAQDQQqnXg5zKm5xbsQJTv5LYWL3pNx2MbjdP0wAvb+jkcLrG0m0ZLCK6FZ2blYB/uiM1hzm6uyp+ej0PolT9RBQFrFvIxagE+/SacXXXgSIA8LrV+uib2kbx8hQN2jmFk+H+1NYpuTlV6HMNFiorAcKgCKAP/zztpPBy3xaI0pS1eDxNIG85h+m8dqEB6qU0a8/XGc2/VToPo9wmvLiVXzX1ucwPspR7GiXhbcIyWA2h8dLXUfy0ZUZb94HYO6kbJa0ajaJB/+s3Is92ZkxSNaK6fMtquMijJAR4x2QHUx7KbpNGHHaE3BPypO77ivhTMjlu4p/eXoL9ipgW2jYLWCYBAoGBAPURpoyufq/s+jk31vbOoQBe0bAzgfBtVxdma2gL0JF/5+0K07Dl8ujWQE7ks+f/4mJPKem6RBjr8HSj4xPE1KC9X2eL6BXeFSgGgzDcO/FT2Fiv1XsD/7eMwfMgP/WbPLhO+Zw7mASeDRkI66811ePIn3q7aMVFMGDY5wilo3ZBAoGBAPK+Yz1PwqPE4HbV6zhq4IMrbnyDlfwX6GWijCWMQt4B0oSz0A/UdsgkVb1wsqoTMd7eQ0nvp1ZvdQ4fP/BB4jP0wg0/GhEqTfPE9LHNCyo4wy71BwLQ7zBQ5kUog1JhIaWMQcq6zRDDuB5fZkcS0ZZVHO+OEYItF2Nl4o/WYHcpAoGBAL3cR9DT2xhGmvC7PxIEsR0NWJyOuwteWkupsGpyRMqEclTlv21cKN2UJ0w3yN1jE6sgM5N9GAbKu4ZR9bm6ExTYwdIBxPM0E+Xnbbnr1ZC4aXMD/nsIdRNpvFS0VnjcJKWRobYVQUfKftE3ZQhfx+5p3owCJ0A1wy40vICpesNBAoGAKARi79SaTc1DA9Q4NtDHulgKmtRGgYyfb0HgwL/ith6uydmqzzFDc7Mpv1U43vjTG96gUSwE9ibhsTZNoBn8ZHxREUX50iKbPziU7krTMF6zLqaORVSUWe68R2JqY8ZHebcpkXZCICVj0P4BKm5bxuC5KHTPRhqQ5H4DX5sDxCECgYEApNHbMyicW8qma/WDuhntksb3jLVoveLL1Bu8p1Z2/udZmf36fwT4/n8Af69SVQeQef9aee6DVOgiY9BvpA037z7A+VbD/79SgWqbecYLqdM2hyV7tUzb0F3a/g1/K1YzTyHGpWTwWSM1oW+bTxFBO9iWlaDH+yj0rY7to5arVQc=";
        //const string alipay_public_key =
        //"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA6GDxix5VIeF+if0+1zUy5QFxnUh5HRngKk44ACy82WUD0gxwYR00zg6HtGJCfaa9o7R9NCvnnd2pSBMptsohJDMSPJ3M/X5kXuLYyk1sXf/5DjiKs48gIL73Rzl0xyzD0NXV1DqKQil4tVJF9cHPesgejCjVw9nuyGnMDeZjFnZmbRAbKraNMuiPBuaoTSVJsEF6KttPpqfwvW2h2mCfbKqrdWI/s0oTAn2w38BLujSp9lMtTUqdu69sI+Pp+ofzhmDZxCK/KOkr+GdtHy9x2Wl4rLHK00027MSDyUwhupGsn3ZDT5USB2q7KhsayZdtW+/vx+zsgVrTMDfXMYYnaQIDAQAB";
        //-------------------------------------------------------------------------------------------------------------
        //线上配置
        const string postUrl = "https://openapi.alipay.com/gateway.do";
        const string app_id = "2017051807272624";
        //应用私钥
        const string merchant_private_key =
        "MIIEowIBAAKCAQEA1JPcJRwM1SXLo6n6+YfdkJG2yV2lTdYE6/dAWMEtUbNDwWb2nWqPlbfk5TEOO8DueU0KpcAqyxUd9Z4Hn1QUuPOvwSlnXpiBWHN8qqvA1AJR7lhYxHQZgy4HuBz+Gskc1/7JgJJQSQ/IJCcCJ78UHD2o5krO62AyEjsK2I0+AnevFlMLnL8TQdRfZ+OR4TsjtzcnJicebygvP+chjSdr1Hjm8knVVGG5NgAo7p4sQDKC8zEaIyzGVgLNJeVdEqQJW3qih/2PIr/8AviCNZwnMdX7jEyTDb+nPD5bRiCAWajcxzXMg55YoszksLUuxVDV24YZRbdav1u0Iqod04gEwQIDAQABAoIBAQDOe5j4itiEKaQ2IyPfQXObu44a5lVs15gMRCxCidGlbEVCFOszWJCD0w4I+a9jpzF8rbLL8W8fuDFTpN6uipNeW4W6UKdRoTzyV6sedJpm1KIUq8f9evBVFV72HWNNTVAxFhFQS86kSSoUw7c6OEYmcFYg60U2KEg6oRMV6Yv0A9ruN6dxq8rxFuGqzozFm19YYgWxWOsY6L7FHCT3AwOArsGaW71OBSB8z1SQZZ0LKRGaderhngIegmhesKI8LmUkvzlh6+UEHPqwDqJPDX+dqrvBv1u4q61tED53ODnIlzR18ywaKWVnIcgIOjRP6/5iXauIu5Yf0EuK6fdC51nhAoGBAPjvZevvh3/ErD3cMunIhh5QSXCm4zVn7b2H8R+Xho89XM1+Z7aShz950jUNNJWM/lUWfEu9wXPz/ei5I4QfjGJ2lwTxhnEH1PPdxwn1v0XGQeUtWhUxkBVXfFrl4JAMeuaWcar4XTfql3loGOEFgmN9zEemUugMCZeFrqbMbt3lAoGBANqcT6rtbWGWMf0aq1NpTiO3j935gAFwereKUDm3gSVpojsl9R53+hcnDBw9xO9e6bbT8wujJUCfNT4+kgUqppgv2zO8zyDOvCmC7cldOn+GQXCmwi36I+pMjcWyUUqYw3PLBh+buGxMHULmWTkcRnyq8DCVn6pBeH7WVrhLAr2tAoGAelDXWcjGYYd3wqWR9sFq9TYp/8pg1fsNHBXR6t3/Zh6WnPpBZ7oz7oKjQSIN9nXi+lgkFutURO9ckZ92zhLaXf87UViD4MIiZvlQNZqks1opQry4CLXjwDjOh7NwVhdrxCWkEIgc9b61UxhNMTQNz2kemQ4mqpJPnD45hQfqaekCgYAeeYvhmBeXcwik8ALSLevmrsAvgYiGqtdwnqdwZFRskr064Z+6D5+deqQFi9bYR7Ls+b738FZRG+CgPLi+X6O5s3IrVDbLrAYvA0GCwKxW+rBEs/p9zlETaHibBrCuZX1ZtwOzhFHBRxsNKG9ntrlcx7eUDkejxFy30cllB4/qQQKBgBjAzSL3SwPew2zVTEWX7PvicNqHeeHyPApvo9UMrWZGTKASX4epmynCIASGLKwVSiSgH8CLcrqY+IDx7anBaE7QhrlsYEmXSidrhv46XdQZfxCg9s4lwPzm7QlSbJUHYHhU/wrxXwR9VvGZTD31LQ30fP+pDX0cYn4jW/fl+WkO";
        //支付宝公钥
        const string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqVEpoiPDm8qnZmWrcycaHXlfQckwD5nWXW5hw2Itonmtop5bhoi5ikbB5NYLZ9vPGXgFVTBbupci72VZHpqdPJzGwvbhFYBRqkzkhgOluzie3s+OfFldON6OJ74su6359zuRDKG2NzT0raMOxtTNPgbOYUAl3xL7Glhrl/FAaDWpOccrDSIUnQ7RbxMSB4b/b0geqpPAL3I+Oufq3sC1h1NWl2qFnKGaN1W3er4Vl/0v2RRgYNiIaYL1xhavFhnVNXBftvNhzZG+gKT+Qrmtw5JkuoDo/BZ1+fxA3CwDqvPnXslq4TnW7GbNndQA/zLOIhCYKiw+D/8u5UX1tTxU7wIDAQAB";
        //------------------------------------------------------
        ////线上配置2
        //const string postUrl = "https://openapi.alipay.com/gateway.do";
        //const string app_id = "2017051807272660";
        ////应用私钥
        //const string merchant_private_key =
        //"MIIEpQIBAAKCAQEA6GDxix5VIeF+if0+1zUy5QFxnUh5HRngKk44ACy82WUD0gxwYR00zg6HtGJCfaa9o7R9NCvnnd2pSBMptsohJDMSPJ3M/X5kXuLYyk1sXf/5DjiKs48gIL73Rzl0xyzD0NXV1DqKQil4tVJF9cHPesgejCjVw9nuyGnMDeZjFnZmbRAbKraNMuiPBuaoTSVJsEF6KttPpqfwvW2h2mCfbKqrdWI/s0oTAn2w38BLujSp9lMtTUqdu69sI+Pp+ofzhmDZxCK/KOkr+GdtHy9x2Wl4rLHK00027MSDyUwhupGsn3ZDT5USB2q7KhsayZdtW+/vx+zsgVrTMDfXMYYnaQIDAQABAoIBAQDQQqnXg5zKm5xbsQJTv5LYWL3pNx2MbjdP0wAvb+jkcLrG0m0ZLCK6FZ2blYB/uiM1hzm6uyp+ej0PolT9RBQFrFvIxagE+/SacXXXgSIA8LrV+uib2kbx8hQN2jmFk+H+1NYpuTlV6HMNFiorAcKgCKAP/zztpPBy3xaI0pS1eDxNIG85h+m8dqEB6qU0a8/XGc2/VToPo9wmvLiVXzX1ucwPspR7GiXhbcIyWA2h8dLXUfy0ZUZb94HYO6kbJa0ajaJB/+s3Is92ZkxSNaK6fMtquMijJAR4x2QHUx7KbpNGHHaE3BPypO77ivhTMjlu4p/eXoL9ipgW2jYLWCYBAoGBAPURpoyufq/s+jk31vbOoQBe0bAzgfBtVxdma2gL0JF/5+0K07Dl8ujWQE7ks+f/4mJPKem6RBjr8HSj4xPE1KC9X2eL6BXeFSgGgzDcO/FT2Fiv1XsD/7eMwfMgP/WbPLhO+Zw7mASeDRkI66811ePIn3q7aMVFMGDY5wilo3ZBAoGBAPK+Yz1PwqPE4HbV6zhq4IMrbnyDlfwX6GWijCWMQt4B0oSz0A/UdsgkVb1wsqoTMd7eQ0nvp1ZvdQ4fP/BB4jP0wg0/GhEqTfPE9LHNCyo4wy71BwLQ7zBQ5kUog1JhIaWMQcq6zRDDuB5fZkcS0ZZVHO+OEYItF2Nl4o/WYHcpAoGBAL3cR9DT2xhGmvC7PxIEsR0NWJyOuwteWkupsGpyRMqEclTlv21cKN2UJ0w3yN1jE6sgM5N9GAbKu4ZR9bm6ExTYwdIBxPM0E+Xnbbnr1ZC4aXMD/nsIdRNpvFS0VnjcJKWRobYVQUfKftE3ZQhfx+5p3owCJ0A1wy40vICpesNBAoGAKARi79SaTc1DA9Q4NtDHulgKmtRGgYyfb0HgwL/ith6uydmqzzFDc7Mpv1U43vjTG96gUSwE9ibhsTZNoBn8ZHxREUX50iKbPziU7krTMF6zLqaORVSUWe68R2JqY8ZHebcpkXZCICVj0P4BKm5bxuC5KHTPRhqQ5H4DX5sDxCECgYEApNHbMyicW8qma/WDuhntksb3jLVoveLL1Bu8p1Z2/udZmf36fwT4/n8Af69SVQeQef9aee6DVOgiY9BvpA037z7A+VbD/79SgWqbecYLqdM2hyV7tUzb0F3a/g1/K1YzTyHGpWTwWSM1oW+bTxFBO9iWlaDH+yj0rY7to5arVQc=";
        ////支付宝公钥
        //const string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqVEpoiPDm8qnZmWrcycaHXlfQckwD5nWXW5hw2Itonmtop5bhoi5ikbB5NYLZ9vPGXgFVTBbupci72VZHpqdPJzGwvbhFYBRqkzkhgOluzie3s+OfFldON6OJ74su6359zuRDKG2NzT0raMOxtTNPgbOYUAl3xL7Glhrl/FAaDWpOccrDSIUnQ7RbxMSB4b/b0geqpPAL3I+Oufq3sC1h1NWl2qFnKGaN1W3er4Vl/0v2RRgYNiIaYL1xhavFhnVNXBftvNhzZG+gKT+Qrmtw5JkuoDo/BZ1+fxA3CwDqvPnXslq4TnW7GbNndQA/zLOIhCYKiw+D/8u5UX1tTxU7wIDAQAB";



        /// <summary>
        /// 支付宝支付地址生成
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public string Alipay(PayModel payInfo)
        {
            string payStr = AlipayForm(payInfo);
            return payStr;
            //前端页面提交返回信息方式
            //1.JQUERY方式
            //jQuery("#formDiv").append(data);
            //jQuery("#alipaysubmit").submit();
            //2.JS方式
            //    const div = document.createElement('div');
            //    div.innerHTML = data.html;
            //    document.body.appendChild(div);
            //    document.forms.alipaysubmit.submit();
            //}
        }

        /// <summary>
        /// 支付成功后返回页面（仅供支付宝服务器调用）
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public object AlipayPayResult(string OrderID)
        {
            //业务逻辑，将订单改为已支付******************************

            //********************************************
            var jsonDataRef = new
            {
                data = "",
                status = 1,
                msg = "数据提交成功"
            };
            return Json(jsonDataRef);
        }

        /// <summary>
        /// 服务器异步通知页面（仅供支付宝服务器调用）
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public string NotifyUrl()
        {
            try
            {
                LogHandler.WriteServiceLog("支付宝到帐通知", "进入方法", "", "", "");
                SortedDictionary<string, string> sPara = GetRequestGet();
                LogHandler.WriteServiceLog("支付宝到帐通知", "获取参数列表成功", "", "", "");
                bool verifyResult = AlipaySignature.RSACheckV2(sPara, alipay_public_key, "UTF-8");
                LogHandler.WriteServiceLog("支付宝到帐通知", "验签结束", "", "", "");
                if (verifyResult)//验证成功
                {
                    LogHandler.WriteServiceLog("支付宝到帐通知", "验签成功", "", "", "");
                    bool isRefund = false;
                    string batch_no = string.Empty;
                    if (sPara.ContainsKey("out_biz_no"))
                    {
                        isRefund = true;
                        batch_no = sPara["out_biz_no"];
                    }

                    //商户订单号
                    string out_trade_no = httpRequestBase.Form["out_trade_no"];
                    //支付宝交易号
                    string trade_no = httpRequestBase.Form["trade_no"];
                    //交易状态
                    string trade_status = httpRequestBase.Form["trade_status"];
                    string buyer_id = httpRequestBase.Form["buyer_id"];
                    string buyer_emial = httpRequestBase.Form["buyer_logon_id"];
                    if (string.IsNullOrEmpty(out_trade_no))
                    {
                        throw new Exception("商户订单号不能为空");
                    }
                    LogHandler.WriteServiceLog("支付宝到帐通知", "获取商户订单号", out_trade_no, "", "");
                    if (trade_status == "TRADE_FINISHED")
                    {
                        LogHandler.WriteServiceLog("支付宝到帐通知", "TRADE_FINISHED", out_trade_no, "", "");
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
                        //如果有做过处理，不执行商户的业务程序

                        //处理业务逻辑  

                        //注意：
                        //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                    }
                    else if (trade_status == "TRADE_SUCCESS")
                    {
                        LogHandler.WriteServiceLog("支付宝到帐通知", "TRADE_SUCCESS", out_trade_no, "", "");
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
                        //如果有做过处理，不执行商户的业务程序

                        if (isRefund)
                        {
                            LogHandler.WriteServiceLog("支付宝到帐通知", "处理退款业务", out_trade_no, "", "");
                            //处理退款业务---退款成功

                        }
                        else
                        {
                            LogHandler.WriteServiceLog("支付宝到帐通知", "付款成功", out_trade_no, "", "");
                            //处理订单业务---付款成功

                        }

                        //注意：
                        //付款完成后，支付宝系统发送该交易状态通知
                    }
                    else if (trade_status == "TRADE_CLOSED")//未付款交易超时关闭，或支付完成后全额退款
                    {
                        LogHandler.WriteServiceLog("支付宝到帐通知", "未付款交易超时关闭", out_trade_no, "", "");
                        if (isRefund)
                        {
                            LogHandler.WriteServiceLog("支付宝到帐通知", "未付款交易退款成功", out_trade_no, "", "");
                            //处理退款业务--退款成功

                        }
                    }
                    return "success";  //必须输出success
                }
                else//验证失败
                {
                    LogHandler.WriteServiceLog("支付宝到帐通知", "验证失败", "", "", "");
                    return "failure";
                }
            }
            catch(Exception ex)
            {
                LogHandler.WriteServiceLog("支付宝到帐通知", ex.Message, "", "", "");
                return "failure";
            }
          
        }

        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        private SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = httpRequestBase.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], httpRequestBase.QueryString[requestItem[i]]);
            }
            return sArray;
        }


        /// <summary>
        ///  H5支付
        /// </summary>
        /// <param name="payInfo"></param>
        /// <returns>提交支付宝的订单</returns>
        private string AlipayForm(PayModel payInfo)
        {
            //---------------------------------------------------------------------------------------------------------------------------------------
            string sign_type = "RSA2";//加签方式 有两种RSA和RSA2 我这里使用的RSA2（支付宝推荐的）
            string version = "1.0";//固定值 不用改
            string format = "json";//固定值
            string address = httpRequestBase.Url.ToString();
            address = address.Substring(0, address.IndexOf("api"));
            IAopClient client = new DefaultAopClient(postUrl, app_id, merchant_private_key, format, version, sign_type, alipay_public_key, "UTF-8", false);
            AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
            request.SetReturnUrl(payInfo.Address);//支付后返回地址，同步请求
            request.SetNotifyUrl(address + "api/WebPay/NotifyUrl");//支付结果后台接受地址，异步请求
            request.BizContent = "{" +
            "    \'body\':\'" + payInfo.Body + "\'," +
            "    \'subject\':\'" + payInfo.OrderName + "\'," +
            "    \'out_trade_no\':\'" + payInfo.PayOrderNo + "\'," +
            "    \'timeout_express\':\'30m\'," +
            "    \'total_amount\':" + payInfo.PayAmount + "," +
            "    \'product_code\':\'QUICK_WAP_PAY\'" +
            "  }";
            AlipayTradeWapPayResponse response = client.pageExecute(request);
            string form = response.Body;
            return form;
        }
    }
}
