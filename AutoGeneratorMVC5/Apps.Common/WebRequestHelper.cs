/**
* 命名空间: Apps.Common
*
* 功 能： N/A
* 类 名： WebRequestHelper
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2018-1-9 20:12:00 王仁禧 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：大连安琪科技有限公司 　　　　　　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace Apps.Common
{
    public class WebRequestHelper
    {
        #region 发送Get请求
        /// <summary>
        /// 发送Get请求
        /// 如果返回的数据是以出现异常开头就说明有异常
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        [HttpGet]
        public static string GetData(string Url)
        {
            HttpWebRequest request = WebRequest.Create(Url) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.Headers.Add("App-Key", ConstantEywa.APP_KEY);
            request.Headers.Add("AuthToken", "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiLkvaAiLCJjcmVhdGVkIjoxNTEwODIxNzE4MDAxLCJleHAiOjMyNjE0ODIxNzE3fQ.fW9TSuLZoUHxa80tNvacfCc-N5Dp2ipGMMOuFYbx0vmZmQD80VOCZ9dVmV5e0Yn9ATF8xTBhejIjs-MXKHJS5w");
            request.KeepAlive = false;
            string returnValue = "";
            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
                returnValue = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return "出现异常：" + ex.Message;
            }
            return returnValue;
        }
        #endregion

        #region 发送Post请求
        /// <summary>
        /// 发送Post请求
        /// 如果返回的数据是以出现异常开头就说明有异常
        /// </summary>
        /// <param name="Url">"http://api.myeywa.com/locations"</param>
        /// <param name="postData">postData = "{\"name\" : \"纳美人之家\",\"description\" : \"潘多拉上的一颗神树\"}"</param>
        /// <returns></returns>
        [HttpGet]
        public static string Post(string Url, string postData)
        {
            Random r = new Random();
            int i = r.Next(10000, 99999);
            string Random = i.ToString();
            Random = "13562";
            TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
            string Timestamp = Convert.ToInt32(ts.TotalSeconds).ToString();
            Timestamp = "1503641189";
            //以字节方式存储
            byte[] data = Encoding.Default.GetBytes("IVWmSJJSDNPGGIvt" + Random + Timestamp);
            System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            //得到哈希值
            byte[] result = sha1.ComputeHash(data);
            //转换成为字符串的显示
            string Signature = BitConverter.ToString(result).Replace("-", "");
            WebRequest request = WebRequest.Create(Url);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            request.Headers.Add("App-Key", "tsqbls6WVq5cbtD5");
            request.Headers.Add("Nonce", Random);
            request.Headers.Add("Timestamp", Timestamp);
            request.Headers.Add("Signature", Signature.ToLower());
            string returnValue = "";
            try
            {
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse resp = request.GetResponse();
                dataStream = resp.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                returnValue = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                resp.Close();
            } catch (Exception ex)
            {
                return "出现异常：" + ex.Message;
            }
            return returnValue;
        }
        #endregion

        #region 发送Get请求，获取图片二进制
        /// <summary>
        /// 发送Get请求
        /// 如果返回的数据是以出现异常开头就说明有异常
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        [HttpGet]
        public static byte[] GetImage(string Url)
        {
            HttpWebRequest request = WebRequest.Create(Url) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "image/jpeg";
            //request.Headers.Add("App-Key", ConstantEywa.APP_KEY);
            request.Headers.Add("AuthToken", "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiLkvaAiLCJjcmVhdGVkIjoxNTEwODIxNzE4MDAxLCJleHAiOjMyNjE0ODIxNzE3fQ.fW9TSuLZoUHxa80tNvacfCc-N5Dp2ipGMMOuFYbx0vmZmQD80VOCZ9dVmV5e0Yn9ATF8xTBhejIjs-MXKHJS5w");
            request.KeepAlive = false;
            byte[] outBytes = new byte[128];
            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();
                var contentLength = response.ContentLength;
                outBytes = ReadFully(responseStream);
            }
            catch (Exception ex)
            {
                return outBytes;
            }
            return outBytes;
        }
        private static byte[] ReadFully(Stream stream)
        {
            byte[] buffer = new byte[128];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        /// <summary>
        /// 二进制转字符串
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static string byte2hex(byte[] b)
        {
            byte[] data = new byte[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                data[i] = Convert.ToByte(b[i]);
            }
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }
        #endregion
    }
}
