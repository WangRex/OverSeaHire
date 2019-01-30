using Apps.BLL.App;
using Apps.BLL.Sys;
using Apps.Common;
using Apps.Models.App;
using Apps.Web.Core;
using Microsoft.Practices.Unity;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using System;
using System.IO;
using System.Linq;
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
        [Dependency]
        public App_CustomerWorkExpBLL customerWorkExpBLL { get; set; }
        [Dependency]
        public App_CustomerEduExpBLL customerEduExpBLL { get; set; }
        [Dependency]
        public App_CustomerFamilyBLL customerFamilyBLL { get; set; }
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

            string filepath = Server.MapPath("/template.docx");
            using (FileStream stream = System.IO.File.OpenRead(filepath))
            {
                XWPFDocument doc = new XWPFDocument(stream);
                //遍历段落
                foreach (var para in doc.Paragraphs)
                {
                    ReplaceKey(para, entity);
                }
                ReplaceTableKey(doc, entity);
                string outputdoc = Server.MapPath("/" + ResultHelper.NewId + ".docx");
                FileStream sw = System.IO.File.Create(outputdoc);
                doc.Write(sw);
                sw.Close();
                FileStream streamData = System.IO.File.OpenRead(outputdoc);
                XWPFDocument docData = new XWPFDocument(streamData);
                MemoryStream msData = new MemoryStream();
                docData = delEmptyRow(docData);
                docData.Write(msData);
                //删除临时文件
                FileInfo fileDel = new FileInfo(outputdoc);
                fileDel.Delete();
                Response.ContentType = "application/vnd.ms-word";
                Response.ContentEncoding = Encoding.UTF8;
                Response.Charset = "";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(entity.CustomerName + "简历.docx", Encoding.UTF8));
                Response.BinaryWrite(msData.GetBuffer());
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
                if (text.Contains("expectcountry"))
                {
                    text = text.Replace("expectcountry", entity.ExpectCountry);
                }
                runs[i].SetText(text, 0);
            }
        }
        private void ReplaceTableKey(XWPFDocument doc, App_CustomerModel entity)
        {
            var table = doc.Tables[0];
            //获取图片位置单元格（下角标从0开始）
            var picCell = table.Rows[0].GetCell(1);
            foreach (var item in picCell.Paragraphs)
            {
                var runspic = item.Runs;
                for (int i = 0; i < runspic.Count; i++)
                {
                    var run = runspic[i];
                    runspic[i].SetText("", 0);
                    //var curFile = Server.MapPath("~/upload/201812/31/201812311823543276.jpg");
                    var curFile = Server.MapPath("~" + entity.CustomerPhoto);
                    if (System.IO.File.Exists(curFile))
                    {
                        //如果文件存在，则插入图片。
                        using (FileStream fsImg = new FileStream(curFile, FileMode.Open, FileAccess.Read))
                        {
                            runspic[i].AddPicture(fsImg, (int)PictureType.JPEG, "10.jpg", (int)(120.0 * 9525), (int)(160.0 * 9525));
                        }
                    }
                }
            }
            var rows = doc.Tables[0].Rows;
            //获取求职意向单元格
            var cellJobIntension = doc.Tables[0].Rows[0].GetCell(0);
            cellJobIntension.SetText("申请职位：" + entity.JobIntension);
            //获取中文名称单元格
            var cellCustomerName = doc.Tables[0].Rows[1].GetCell(2);
            cellCustomerName.SetText(entity.CustomerName);
            //获取性别单元格
            var cellSex = doc.Tables[0].Rows[1].GetCell(4);
            cellSex.SetText(entity.Sex);
            //获取英文名称单元格
            var cellEnglishName = doc.Tables[0].Rows[2].GetCell(2);
            cellEnglishName.SetText(entity.EnglishName);
            //获取婚姻单元格
            var cellMaritalStatus = doc.Tables[0].Rows[2].GetCell(4);
            cellMaritalStatus.SetText(entity.MaritalStatus);
            //获取电话单元格
            var cellPhone = doc.Tables[0].Rows[3].GetCell(1);
            cellPhone.SetText(entity.Phone);
            //获取年龄单元格
            var cellAge = doc.Tables[0].Rows[3].GetCell(3);
            cellAge.SetText(entity.Age.ToString());
            //获取护照号码单元格
            var cellPassportNo = doc.Tables[0].Rows[4].GetCell(1);
            cellPassportNo.SetText(entity.PassportNo);
            //获取身高单元格
            var cellHeight = doc.Tables[0].Rows[4].GetCell(3);
            cellHeight.SetText(entity.Height);
            //获取籍贯单元格
            var cellBirthPlace = doc.Tables[0].Rows[5].GetCell(1);
            cellBirthPlace.SetText(entity.BirthPlace);
            //获取体重单元格
            var cellWeight = doc.Tables[0].Rows[5].GetCell(3);
            cellWeight.SetText(entity.Weight);
            //获取民族单元格
            var cellNation = doc.Tables[0].Rows[5].GetCell(5);
            cellNation.SetText(entity.Nation);
            //获取出国经历单元格
            var cellAbroadExp = doc.Tables[0].Rows[6].GetCell(1);
            cellAbroadExp.SetText(entity.AbroadExp);
            //获取英语单元格
            var cellEnumForeignLangGrade = doc.Tables[0].Rows[6].GetCell(3);
            cellEnumForeignLangGrade.SetText(entity.EnumForeignLangGrade);
            //获取宗教信仰单元格
            var cellReligion = doc.Tables[0].Rows[6].GetCell(5);
            cellReligion.SetText(entity.Religion);
            //获取期望职位单元格
            var cellExpectPosition = doc.Tables[0].Rows[7].GetCell(1);
            cellExpectPosition.SetText(entity.JobIntension);
            //获取期望国家单元格
            var cellExpectCountry = doc.Tables[0].Rows[7].GetCell(3);
            cellExpectCountry.SetText(entity.ExpectCountry);
            //获取驾照单元格
            var cellEnumDriverLicence = doc.Tables[0].Rows[7].GetCell(5);
            cellEnumDriverLicence.SetText(entity.EnumDriverLicence);
            //获取教育经历
            var workExps = customerWorkExpBLL.m_Rep.FindList(EF => EF.PK_App_Customer_CustomerName == entity.Id).ToList();
            var eduExps = customerEduExpBLL.m_Rep.FindList(EF => EF.PK_App_Customer_CustomerName == entity.Id).ToList();
            var families = customerFamilyBLL.m_Rep.FindList(EF => EF.PK_App_Customer_CustomerName == entity.Id).ToList();
            int eduCount = eduExps.Count, workCount = workExps.Count, familyCount = families.Count;
            if (eduCount > 0)
            {
                for (int i = 0; i < eduCount; i++)
                {
                    var item = eduExps[i];
                    var cellEduDate = doc.Tables[0].Rows[10 + i].GetCell(0);
                    cellEduDate.SetText(item.StartDate + "~" + item.EndDate);
                    var cellEduSchool = doc.Tables[0].Rows[10 + i].GetCell(1);
                    cellEduSchool.SetText(item.School);
                    var cellEduMajor = doc.Tables[0].Rows[10 + i].GetCell(2);
                    cellEduMajor.SetText(item.Major);
                    var cellEduDegree = doc.Tables[0].Rows[10 + i].GetCell(3);
                    cellEduDegree.SetText(item.Degree);
                    var cellEduCertificate = doc.Tables[0].Rows[10 + i].GetCell(4);
                    cellEduCertificate.SetText(item.Certificate);
                }
            }
            if (workCount > 0)
            {
                for (int i = 0; i < workCount; i++)
                {
                    var item = workExps[i];
                    var cellWorkDate = doc.Tables[0].Rows[17 + i].GetCell(0);
                    cellWorkDate.SetText(item.StartDate + "~" + item.EndDate);
                    var cellWorkCompany = doc.Tables[0].Rows[17 + i].GetCell(1);
                    cellWorkCompany.SetText(item.Company);
                    var cellWorkPosition = doc.Tables[0].Rows[17 + i].GetCell(2);
                    cellWorkPosition.SetText(item.Position);
                    var cellWorkJobDescription = doc.Tables[0].Rows[17 + i].GetCell(3);
                    cellWorkJobDescription.SetText(item.JobDescription);
                }
            }
            if (familyCount > 0)
            {
                for (int i = 0; i < familyCount; i++)
                {
                    var item = families[i];
                    var cellFamilyName = doc.Tables[0].Rows[24 + i].GetCell(0);
                    cellFamilyName.SetText(item.Name);
                    var cellFamilyRelation = doc.Tables[0].Rows[24 + i].GetCell(1);
                    cellFamilyRelation.SetText(item.Relation);
                    var cellFamilyAge = doc.Tables[0].Rows[24 + i].GetCell(2);
                    cellFamilyAge.SetText(item.Age.ToString());
                    var cellFamilyOccupation = doc.Tables[0].Rows[24 + i].GetCell(3);
                    cellFamilyOccupation.SetText(item.Occupation);
                }
            }
            //获取自我评价单元格
            var cellIntroduction = doc.Tables[0].Rows[rows.Count - 1].GetCell(1);
            cellIntroduction.SetText(entity.Introduction);
        }

        private XWPFDocument delEmptyRow(XWPFDocument doc)
        {
            for (int i = 0; i < doc.Tables[0].Rows.Count; i++)
            {
                if (doc.Tables[0].Rows[i].GetCell(0).GetText() == "")
                {
                    doc.Tables[0].RemoveRow(i);
                    doc = delEmptyRow(doc);
                }
            }
            return doc;
        }
    }
}