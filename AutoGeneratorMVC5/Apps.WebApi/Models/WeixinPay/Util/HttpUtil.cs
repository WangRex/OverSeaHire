using Apps.Common;
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
using System.Xml;

namespace tenpay
{
    public class HttpUtil
    {
        /// <summary>
        /// httpUtil网络请求工具类
        /// </summary>

        public static string TENPAY = "1";
        public static string APPID = WeChatSystem.appid;              //开发者应用ID
        public static string PARTNER = WeChatSystem.mch_id;          //商户号
        public static string APPSECRET = WeChatSystem.appsercret;      //开发者应用密钥
        public static string PARTNER_KEY = WeChatSystem.api_key;   //商户秘钥
        public static string PATH = WeChatPayPara.WXPay_PATH;   //证书路径

        public const string URL = "https://api.mch.weixin.qq.com/secapi/pay/refund";

        //服务器异步通知页面路径(流量卡)
        public static string WebUrl = "";
        public static readonly string NOTIFY_URL_Card_Store = "http://" + WebUrl + "/weixinpay/WXPayNotify_URL.aspx";// ConfigurationManager.AppSettings["WXPayNotify_URL_CardStore"].ToString();
        public static readonly string NOTIFY_URL_Card_User = "http://" + WebUrl + "/weixinpay/WXPayNotify_URL.aspx"; //ConfigurationManager.AppSettings["WXPayNotify_URL_CardUser"].ToString();
        public static readonly string NOTIFY_URL_HB_Store = "http://" + WebUrl + "/weixinpay/WXPayNotify_URL.aspx";// ConfigurationManager.AppSettings["WXPayNotify_URL_CardStore"].ToString();

        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public const string PROXY_URL = "http://10.152.18.220:8080";

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public const string SSLCERT_PATH = "weixin\\apiclient_cert.p12";
        public static string SSLCERT_PASSWORD = PARTNER;

     
        public static string getTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string key)
        {
            //获取过滤后的数组
            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara = FilterPara(sParaTemp);

            //组合参数数组
            string prestr = CreateLinkString(dicPara);
            //拼接支付密钥
            string stringSignTemp = prestr + "&key=" + key;


            //获得加密结果
            string myMd5Str = GetMD5(stringSignTemp.Trim());

            //返回转换为大写的加密串
            return myMd5Str.ToUpper();
        }

        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Key != "sign" && !string.IsNullOrEmpty(temp.Value))
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }

        //组合参数数组
        public static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        //加密
        public static string GetMD5(string pwd)
        {
            MD5 md5Hasher = MD5.Create();

            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(pwd));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// HttpPost请求
        /// </summary>
        /// <param name="postUrl">请求路径</param>
        /// <param name="paramData">xml参数</param>
        /// <param name="dataEncode">数据编码</param>
        /// <returns></returns>
        public static string HttpPost(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                //Vincent._Log.SaveMessage("Post提交异常：" + ex.Message);
            }
            return ret;
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }

        /// <summary>
        /// post提交支付
        /// </summary>
        /// <param name="xml">xml参数格式</param>
        /// <param name="url">请求url</param>
        /// <param name="isUseCert">是否使用证书</param>
        /// <param name="timeout">超时timeout</param>
        /// <returns></returns>
        public static string HttpPost(string url, string xml, bool isUseCert, int timeout)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = timeout * 1000;

                //设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(PROXY_URL);              //网关服务器端口:端口
                //request.Proxy = proxy;

                //设置POST的数据类型和长度
                request.ContentType = "text/xml";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                request.ContentLength = data.Length;

                //是否使用证书
                if (isUseCert)
                {
                    string path = HttpContext.Current.Request.PhysicalApplicationPath;
                    //X509Certificate2 cert = new X509Certificate2(path + SSLCERT_PATH, SSLCERT_PASSWORD);

                    //将上面的改成
                    //X509Certificate2 cert = new X509Certificate2(path + SSLCERT_PATH, SSLCERT_PASSWORD, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);//线上发布需要添加
                    X509Certificate2 cert = new X509Certificate2(WeChatPayPara.WXPay_PATH, WeChatPayPara.WXPay_MCH_ID, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);//线上发布需要添加

                    request.ClientCertificates.Add(cert);

                    //Vincent._Log.SaveMessage("WxPayApi:PostXml used cert");
                }

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                //Vincent._Log.SaveMessage("HttpService" + e.ToString());
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    //Vincent._Log.SaveMessage("HttpService:StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    //Vincent._Log.SaveMessage("HttpService:StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                throw new Exception(e.ToString());
            }
            catch (Exception e)
            {
                //Vincent._Log.SaveMessage("HttpService" + e.ToString());
                throw new Exception(e.ToString());
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        /// <summary>
        /// 处理http GET请求，返回数据
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <returns>http GET成功后返回的数据，失败抛WebException异常</returns>
        public static string Get(string url)
        {
            System.GC.Collect();
            string result = "";

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            //请求url以获取数据
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";

                //设置代理
                WebProxy proxy = new WebProxy();
                proxy.Address = new Uri(PROXY_URL);
                request.Proxy = proxy;

                //获取服务器返回
                response = (HttpWebResponse)request.GetResponse();

                //获取HTTP返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                //Vincent._Log.SaveMessage("HttpService:Thread - caught ThreadAbortException - resetting.");
                //Vincent._Log.SaveMessage("Exception message: " + e.Message);
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                //Vincent._Log.SaveMessage("HttpService" + e.ToString());
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    //Vincent._Log.SaveMessage("HttpService:StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                   // Vincent._Log.SaveMessage("HttpService:StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                throw new Exception(e.ToString());
            }
            catch (Exception e)
            {
                //Vincent._Log.SaveMessage("HttpService" + e.ToString());
                throw new Exception(e.ToString());
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        /// <summary>
        /// 把XML数据转换为SortedDictionary<string, string>集合
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static SortedDictionary<string, string> GetInfoFromXml(string xmlstring)
        {
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;
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

    }
}