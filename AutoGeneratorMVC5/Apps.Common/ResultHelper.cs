using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace Apps.Common
{
    public class ResultHelper
    {
        #region 创建一个全球唯一的32位ID
        /// <summary>
        /// 创建一个全球唯一的32位ID
        /// </summary>
        /// <returns>ID串</returns>
        public static string NewId
        {
            get
            {
                string id = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
                string guid = Guid.NewGuid().ToString().Replace("-", "");
                id += guid.Substring(0, 10);
                return id;
            }
        }
        #endregion

        #region 获取时间字符串
        /// <summary>
        /// 获取时间字符串
        /// </summary>
        public static string NewTimeId
        {
            get
            {
                string id = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
                return id.Substring(11,9);
            }
        }
        #endregion

        #region 截取字符串
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="length">剩下长度</param>
        /// <returns>指定字符串并加...</returns>
        public static string SubValue(string value, int length)
        {
            if (value.Length > length)
            {
                value = value.Substring(0, length); value = value + "..."; return NoHtml(value);
            }
            else { return NoHtml(value); }
        }
        #endregion

        #region 还原的时候
        /// <summary>
        /// 还原的时候
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string InputText(string inputString)
        {

            if ((inputString != null) && (inputString != String.Empty))
            {
                inputString = inputString.Trim();
                //if (inputString.Length > maxLength) 
                //inputString = inputString.Substring(0, maxLength); 
                inputString = inputString.Replace("<br>", "\n");
                inputString = inputString.Replace("&", "&amp");
                inputString = inputString.Replace("'", "''");
                inputString = inputString.Replace("<", "&lt");
                inputString = inputString.Replace(">", "&gt");
                inputString = inputString.Replace("chr(60)", "&lt");
                inputString = inputString.Replace("chr(37)", "&gt");
                inputString = inputString.Replace("\"", "&quot");
                inputString = inputString.Replace(";", ";");

                return inputString;
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region 添加的时候
        /// <summary>
        /// 添加的时候
        /// </summary>
        /// <param name="outputString"></param>
        /// <returns></returns>
        public static string OutputText(string outputString)
        {

            if ((outputString != null) && (outputString != String.Empty))
            {
                outputString = outputString.Trim();
                outputString = outputString.Replace("&amp", "&");
                outputString = outputString.Replace("''", "'");
                outputString = outputString.Replace("&lt", "<");
                outputString = outputString.Replace("&gt", ">");
                outputString = outputString.Replace("&lt", "chr(60)");
                outputString = outputString.Replace("&gt", "chr(37)");
                outputString = outputString.Replace("&quot", "\"");
                outputString = outputString.Replace(";", ";");
                outputString = outputString.Replace("\n", "<br>");
                return outputString;
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region 去除HTML标记
        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="NoHTML">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string NoHtml(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&hellip;", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&mdash;", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&ldquo;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring = Regex.Replace(Htmlstring, @"&rdquo;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;

        }
        #endregion

        #region 格式化文本（防止SQL注入）
        /// <summary>
        /// 格式化文本（防止SQL注入）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Formatstr(string html)
        {
            Regex regex1 = new Regex(@"<script[\s\S]+</script *>", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex(@" href *= *[\s\S]*script *:", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(@" on[\s\S]*=", RegexOptions.IgnoreCase);
            Regex regex4 = new Regex(@"<iframe[\s\S]+</iframe *>", RegexOptions.IgnoreCase);
            Regex regex5 = new Regex(@"<frameset[\s\S]+</frameset *>", RegexOptions.IgnoreCase);
            Regex regex10 = new Regex(@"select", RegexOptions.IgnoreCase);
            Regex regex11 = new Regex(@"update", RegexOptions.IgnoreCase);
            Regex regex12 = new Regex(@"delete", RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件
            html = regex4.Replace(html, ""); //过滤iframe
            html = regex10.Replace(html, "s_elect");
            html = regex11.Replace(html, "u_pudate");
            html = regex12.Replace(html, "d_elete");
            html = html.Replace("'", "’");
            html = html.Replace("&nbsp;", " ");
            return html;
        }
        #endregion

        #region 检查SQL语句合法性
        /// <summary>
        /// 检查SQL语句合法性
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ValidateSQL(string sql, ref string msg)
        {
            if (sql.ToLower().IndexOf("delete") > 0)
            {
                msg = "查询参数中含有非法语句DELETE";
                return false;
            }
            if (sql.ToLower().IndexOf("update") > 0)
            {
                msg = "查询参数中含有非法语句UPDATE";
                return false;
            }

            if (sql.ToLower().IndexOf("insert") > 0)
            {
                msg = "查询参数中含有非法语句INSERT";
                return false;
            }
            return true;
        }
        #endregion

        #region 获取当前时间
        /// <summary>
        /// 获取当前时间
        /// </summary>
        public static DateTime NowTime
        {
            get 
            {
                return DateTime.Now;
            }
        }
        #endregion

        #region 将日期转换成字符串
        /// <summary>
        /// 将日期转换成字符串
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>字符串</returns>
        public static string DateTimeConvertString(DateTime? dt)
        {
            if (dt == null)
            {
                return "";
            }
            else
            {
                return Convert.ToDateTime(dt.ToString()).ToShortDateString();
            }
        }
        #endregion

        #region 将字符串转换成日期
        /// <summary>
        /// 将字符串转换成日期
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>日期</returns>
        public static DateTime? StringConvertDatetime(string str)
        {
            if (str == null)
            {
                return null ;
            }
            else
            {
                try
                {
                    return Convert.ToDateTime(str);
                }
                catch {
                    return null;
                }
            }
        }
        #endregion

        #region 获取用户IP
        /// <summary>
        /// 获取用户IP
        /// </summary>
        /// <returns></returns>
        public static string GetUserIP()
        {
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
            else
                return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }
        #endregion

        #region 获取日期长字符
        /// <summary>
        /// 获取日期长字符
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime GetTimeByLong(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = new TimeSpan(timeStamp);
            return dtStart.Add(toNow);
        }
        #endregion

        #region 获取错误信息
        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static IEnumerable<ShowError> AllModelStateErrors(ModelStateDictionary modelState)
        {
            var result = new List<ShowError>();
            //找到出错的字段以及出错信息
            var errorFieldsAndMsgs = modelState.Where(m => m.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });
            foreach (var item in errorFieldsAndMsgs)
            {
                //获取键
                var fieldKey = item.Key;
                //获取键对应的错误信息
                var fieldErrors = item.Errors
                    .Select(e => new ShowError(fieldKey, e.ErrorMessage));
                result.AddRange(fieldErrors);
            }
            return result;
        }
        #endregion
    }
}
