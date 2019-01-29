using Apps.BLL.App;
using Apps.BLL.Sys;
using Apps.Common;
using Apps.Models.App;
using Apps.Web.Core;
using Microsoft.Practices.Unity;
using NPOI.XWPF.UserModel;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Xceed.Words.NET;

namespace Apps.Web.Areas.APP.Controllers
{
    public class ExportResumeController : BaseController
    {
        #region BLLs
        [Dependency]
        public App_CustomerBLL app_CustomerBLL { get; set; }
        [Dependency]
        public EnumDictionaryBLL enumDictionaryBLL { get; set; }
        [Dependency]
        public App_PositionBLL app_PositionBLL { get; set; }
        [Dependency]
        public App_CountryBLL app_CountryBLL { get; set; }
        #endregion
        public ActionResult GenerateDocument()
        {
            return View();
        }
        [HttpGet]
        public void GenerateDocument(string ResumeId)
        {
            App_CustomerModel entity = app_CustomerBLL.GetById(ResumeId);
            entity.EnumCustomerLevel = enumDictionaryBLL.GetDicName("APP_Customer.EnumCustomerLevel", entity.EnumCustomerLevel);
            entity.EnumCustomerType = enumDictionaryBLL.GetDicName("APP_Customer.EnumCustomerType", entity.EnumCustomerType);
            entity.EnumForeignLangGrade = enumDictionaryBLL.GetDicName("App_Customer.EnumForeignLangGrade", entity.EnumForeignLangGrade);
            entity.EnumDriverLicence = enumDictionaryBLL.GetDicName("App_Customer.EnumDriverLicence", entity.EnumDriverLicence);
            entity.AbroadExp = enumDictionaryBLL.GetDicName("App_CustomerJobIntension.AbroadExp", entity.AbroadExp);
            entity.SwitchBtnPassport = entity.SwitchBtnPassport == "1" ? "有" : "无";
            entity.SwitchBtnRecommend = entity.SwitchBtnRecommend == "1" ? "推荐" : "无";
            entity.JobIntension = app_PositionBLL.GetNames(entity.JobIntension);
            entity.ExpectCountry = app_CountryBLL.GetName(entity.ExpectCountry);

            string filepath = Server.MapPath("/blank.docx");
            using (FileStream stream = System.IO.File.OpenRead(filepath))
            {
                XWPFDocument doc = new XWPFDocument(stream);
                //遍历段落
                foreach (var para in doc.Paragraphs)
                {
                    ReplaceKey(para, entity);
                }
                var tables = doc.Tables;
                StringBuilder sb = new StringBuilder();
                foreach (var table in tables)    //遍历表格
                {
                    var picCell = table.Rows[0].GetCell(1);
                    foreach (var item in picCell.Paragraphs)
                    {
                        string textpic = item.ParagraphText;
                        var runspic = item.Runs;
                        for (int i = 0; i < runspic.Count; i++)
                        {
                            var run = runspic[i];
                            textpic = run.ToString();
                            runspic[i].SetText("", 0);
                            using (FileStream fsImg = new FileStream(Server.MapPath("~/upload/201812/31/201812311823543276.jpg"), FileMode.Open, FileAccess.Read))
                            {
                                runspic[i].AddPicture(fsImg, (int)PictureType.JPEG, "10.jpg", (int)(120.0 * 9525), (int)(160.0 * 9525));
                            }
                        }
                    }
                    foreach (var row in table.Rows)    //遍历行
                    {
                        var c0 = row.GetCell(0);        //获得单元格0
                        var tableCells = row.GetTableCells();
                        foreach (var item in tableCells)
                        {
                            foreach (var para in item.Paragraphs)
                            {
                                string text = para.ParagraphText;
                                var runs = para.Runs;
                                for (int i = 0; i < runs.Count; i++)
                                {
                                    var run = runs[i];
                                    text = run.ToString();
                                    Type t = entity.GetType();
                                    PropertyInfo[] pi = t.GetProperties();
                                    foreach (PropertyInfo p in pi)
                                    {
                                        if (text.Contains(p.Name.ToLower()))
                                        {
                                            text = text.Replace(p.Name.ToLower(), Utils.ObjToStr(p.GetValue(entity, null)));
                                        }
                                    }
                                    runs[i].SetText(text, 0);
                                }
                            }

                        }
                    }
                }
                sb.ToString();
                MemoryStream ms = new MemoryStream();
                doc.Write(ms);
                Response.ContentType = "application/vnd.ms-word";
                Response.ContentEncoding = Encoding.UTF8;
                Response.Charset = "";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(entity.CustomerName + "简历.doc", Encoding.UTF8));
                Response.BinaryWrite(ms.GetBuffer());
                Response.End();
            }
        }
        private void ReplaceKey(XWPFParagraph para, App_CustomerModel entity)
        {

            string text = para.ParagraphText;
            var runs = para.Runs;
            string styleid = para.Style;
            for (int i = 0; i < runs.Count; i++)
            {
                var run = runs[i];
                text = run.ToString();
                Type t = entity.GetType();
                PropertyInfo[] pi = t.GetProperties();
                foreach (PropertyInfo p in pi)
                {
                    if (text.Contains(p.Name.ToLower()))
                    {
                        text = text.Replace(p.Name.ToLower(), Utils.ObjToStr(p.GetValue(entity, null)));
                    }
                }
                runs[i].SetText(text, 0);
            }
        }
    }
}