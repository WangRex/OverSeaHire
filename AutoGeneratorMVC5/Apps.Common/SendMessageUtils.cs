/**
* 命名空间: Apps.Common
*
* 功 能： N/A
* 类 名： SendMessageUtils
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017-8-21 10:26:44 王仁禧 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：大连安琪科技有限公司 　　　　　　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Apps.Common
{
    /// <summary>
    /// 发送短信
    /// </summary>
    public static class SendMessageUtils
    {
        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="mobile">手机号</param>
        [HttpGet]
        public static Response sendNotify(string mobile)
        {
            var response = new Response();

            Random r = new Random();
            int i = r.Next(10000, 99999);
            string Random = i.ToString();

            TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
            string Timestamp = Convert.ToInt32(ts.TotalSeconds).ToString();

            //以字节方式存储
            byte[] data = Encoding.Default.GetBytes(Constant.APP_SECRET + Random + Timestamp);
            System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            //得到哈希值
            byte[] result = sha1.ComputeHash(data);
            //转换成为字符串的显示
            string Signature = BitConverter.ToString(result).Replace("-", "");

            WebRequest request = WebRequest.Create("http://api.sms.ronghub.com/sendNotify.json");
            request.Method = "POST";
            string postData = "mobile=" + mobile + "&templateId=" + Constant.TEMPLATE_NOTIFY + "&region=86";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            request.Headers.Add("App-Key", Constant.APP_KEY);
            request.Headers.Add("Nonce", Random);
            request.Headers.Add("Timestamp", Timestamp);
            request.Headers.Add("Signature", Signature);
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse resp = request.GetResponse();
            dataStream = resp.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            ReturnJson _ReturnJson = jsonSerializer.Deserialize<ReturnJson>(responseFromServer);
            var code = _ReturnJson.code;
            var sessionId = _ReturnJson.sessionId;
            if (code == 200)
            {
                response.Code = 0;
                response.Message = "发送通知成功！";
            }
            else
            {
                response.Code = 1;
                response.Message = "发送通知失败！";
            }
            reader.Close();
            dataStream.Close();
            resp.Close();
            response.Data = _ReturnJson;

            return response;
        }
    }
}
