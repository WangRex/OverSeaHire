using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Common
{
    public class WeixinLuyinHelper
    {
        #region 请求Url，不发送数据
        /// <summary>
        /// 请求Url，不发送数据
        /// </summary>
        public static string RequestUrl(string url)
        {
            return RequestUrl(url, "POST");
        }
        #endregion


        #region 请求Url，不发送数据
        /// <summary>
        /// 请求Url，不发送数据
        /// </summary>
        public static string RequestUrl(string url, string method)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = method;
            request.ContentType = "text/html";
            request.Headers.Add("charset", "utf-8");

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }
        #endregion



        #region 获取Json字符串某节点的值
        /// <summary>
        /// 获取Json字符串某节点的值
        /// </summary>
        public static string GetJsonValue(string jsonStr, string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                key = "\"" + key.Trim('"') + "\"";
                int index = jsonStr.IndexOf(key) + key.Length + 1;
                if (index > key.Length + 1)
                {
                    //先截逗号，若是最后一个，截“｝”号，取最小值
                    int end = jsonStr.IndexOf(',', index);
                    if (end == -1)
                    {
                        end = jsonStr.IndexOf('}', index);
                    }

                    result = jsonStr.Substring(index, end - index);
                    result = result.Trim(new char[] { '"', ' ', '\'' }); //过滤引号或空格
                }
            }
            return result;
        }
        #endregion



        /// <summary>
        /// 获取Token
        /// </summary>
        public static string GetToken(string appid, string secret)
        {
            string strJson = RequestUrl(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret));
            return GetJsonValue(strJson, "access_token");
        }



        #region 获取jsapi_ticket
        /// <summary>
        /// 获取Token
        /// </summary>
        public static string Getjsapi_ticketToken(string access_token)
        {
            string strJson = RequestUrl(string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", access_token));
            return GetJsonValue(strJson, "ticket");
        }
        #endregion



        //#region 生成签名
        //public static SignatureModel GetSignature(string jsapi_ticket)
        //{
        //    var timestamp = GetTimeStamp();
        //    var noncestr = new Random().Next(10000).ToString();
        //    var uri = "http://m.wanwushuo.com/Home/WeixinLuyin";
        //    string oldstr = "jsapi_ticket=" + jsapi_ticket + "&noncestr=" + noncestr + "&timestamp=" + timestamp + "&url=" + uri;

        //    byte[] cleanBytes = Encoding.UTF8.GetBytes(oldstr);
        //    byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);

        //    string newstr = BitConverter.ToString(hashedBytes).Replace("-", "");
        //    SignatureModel SignatureModel = new SignatureModel();
        //    SignatureModel.noncestr = noncestr;
        //    SignatureModel.Signature = newstr;
        //    SignatureModel.timestamp = timestamp;
        //    return SignatureModel;
        //}

        //#endregion

        //获取当前时间段额时间戳
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }
    }
}
