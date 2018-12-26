using Apps.Common;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Apps.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ImageUploadController : ApiController
    {

        #region 上传文件
        /// <summary>
        /// 通过multipart/form-data方式上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response> PostFile()
        {
            var strPath = "";
            string FileExt = string.Empty, FileName = string.Empty;
            Response response = new Response();
            try
            {
                // 是否请求包含multipart/form-data。
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string root = HttpContext.Current.Server.MapPath("/upload/");
                root = root.Replace("API", "");
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                var provider = new MultipartFormDataStreamProvider(root);

                StringBuilder sb = new StringBuilder();

                // 阅读表格数据并返回一个异步任务.
                await Request.Content.ReadAsMultipartAsync(provider);

                // 如何上传文件到文件名.
                foreach (var file in provider.FileData)
                {
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    if (fileinfo.Length <= 0)
                    {
                        response.Code = 2;
                        response.Message = "上传出错";
                        response.Data = null;
                    }
                    else
                    {
                        FileName = orfilename.Substring(0, orfilename.LastIndexOf('.'));
                        string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                        FileExt = fileExt;
                        String ymd = DateTime.Now.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                        String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                        if (!Directory.Exists(root + ymd))
                        {
                            Directory.CreateDirectory(root + ymd);
                        }
                        fileinfo.CopyTo(Path.Combine(root, ymd + "\\" + newFileName + fileExt), true);
                        sb.Append("/upload/" + ymd + "/" + newFileName + fileExt);
                        strPath = sb.ToString();
                    }
                    fileinfo.Delete();//删除原文件
                }
                response.Code = 0;
                response.Message = "上传成功";
                response.Data = new
                {
                    path = strPath,
                    FileName = FileName,
                    FileExt = FileExt
                };
            }
            catch (Exception e)
            {
                response.Code = 2;
                response.Message = "上传出错" + e.Message;
                response.Data = null;
            }
            return response;
        }
        #endregion 上传文件

        #region 单文件上传
        /// <summary>
        /// 单文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SingleFile()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            string _refilepath = ContextRequest.GetQueryString("ReFilePath"); //取得返回的对象名称
            string _upfilepath = ContextRequest.GetQueryString("UpFilePath"); //取得上传的对象名称
            string _delfile = ContextRequest.GetString(_refilepath);
            var postedFile = context.Request.Files[_upfilepath];
            Response response = new Response();
            if (postedFile == null)
            {
                response.Code = 1;
                response.Message = "请选择上传文件";
                response.Data = "";
                return Json(response);
            }

            string root = HttpContext.Current.Server.MapPath("/upload/");
            string fileExt = Utils.GetFileExt(postedFile.FileName); //文件扩展名，不含“.”
            string fileName = Utils.GetRamCode() + "." + fileExt; //随机文件名
            string ServerMapPath = root + "CustomerImg";
            if (!Directory.Exists(ServerMapPath))
            {
                Directory.CreateDirectory(ServerMapPath);
            }
            string filepath = ServerMapPath + "\\" + fileName;
            postedFile.SaveAs(filepath);
            response.Code = 0;
            response.Message = "上传文件成功";
            response.Data = "/upload/CustomerImg/" + fileName;
            return Json(response);
        }
        #endregion

        #region base64编码的图片文件上传
        /// <summary>
        /// base64编码的图片文件上传{file: base64}
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object UploadBase64(dynamic data)
        {
            //获取base64编码的图片
            Response response = new Response();
            string text = data["file"];
            text = text.Replace("\"", "");
            //LogHandler.WriteServiceLog("Test", text.Substring(0, 450), "参数验证", "UploadBase64", "UploadBase64");
            //获取文件储存路径
            string suffix = ".jpg"; //文件的后缀名根据实际情况
            string root = HttpContext.Current.Server.MapPath("/upload/");
            root = root.Replace("API", "");
            string ServerMapPath = root + "CustomerImg";
            string fileName = GetTimeStamp() + suffix;
            if (!Directory.Exists(ServerMapPath))
            {
                Directory.CreateDirectory(ServerMapPath);
            }
            //LogHandler.WriteServiceLog("Test", ServerMapPath, "建立文件路径", "UploadBase64", "UploadBase64");
            string filepath = ServerMapPath + "\\" + fileName;
            //获取图片并保存
            Base64ToImg(text).Save(filepath);
            //LogHandler.WriteServiceLog("Test", filepath, "保存成功", "UploadBase64", "UploadBase64");
            //获取文件储存路径
            response.Code = 0;
            response.Message = "上传文件成功";
            response.Data = "/upload/CustomerImg/" + fileName;
            return Json(response);

        }
        #endregion

        #region 解析base64编码获取图片
        /// <summary>
        /// 解析base64编码获取图片
        /// </summary>
        /// <param name="base64Code"></param>
        /// <returns></returns>
        private Bitmap Base64ToImg(string base64Code)
        {
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(base64Code));
            return new Bitmap(stream);
        }
        #endregion

        #region 获取当前时间段额时间戳
        /// <summary>
        /// 获取当前时间段额时间戳
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }
        #endregion
    }
}