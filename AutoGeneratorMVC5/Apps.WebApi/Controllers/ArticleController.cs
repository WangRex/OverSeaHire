/**
* 命名空间: Apps.WebApi.Controllers
*
* 功 能： N/A
* 类 名： ArticleController
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017-9-11 20:17:51 王仁禧 初版
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
using System.Linq;
using Apps.WebApi.Core;
using Apps.Models;
using Microsoft.Practices.Unity;
using Apps.BLL.Sys;
using System.Web.Http;
using Apps.BLL.MIS;
using System.Web.Mvc;

namespace Apps.WebApi.Controllers
{
    /// <summary>
    /// 文章管理
    /// </summary>
    public class ArticleController : BaseApiController
    {
        #region BLLs
        /// <summary>
        /// 系统用户
        /// </summary>
        [Dependency]
        public SysUserBLL _SysUserBLL { get; set; }
        /// <summary>
        /// 文章
        /// </summary>
        [Dependency]
        public MIS_ArticleBLL _MIS_ArticleBLL { get; set; }
        /// <summary>
        /// 文章栏目
        /// </summary>
        [Dependency]
        public MIS_Article_CategoryBLL _MIS_Article_CategoryBLL { get; set; }
        #endregion

        #region 获取文章列表
        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="CategoryID">栏目ID(轮播图:001001,帮助信息:001002)</param>
        /// <param name="IsType">类型(轮播图:0,帮助信息:[注意事项:1,办理费用:2,办理流程:3,准备材料:4,签订合同:5])</param>
        /// <param name="PageNum">页数</param>
        /// <param name="RecordNum">条数</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public object GetArticleList(string UserID, string CategoryID, string IsType, int PageNum, int RecordNum)
        {
            LogHandler.WriteServiceLog(UserID, "CategoryID:" + CategoryID + ",IsType:" + IsType + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum, "成功", "进入方法", "ArticleController.GetArticleList()");
            Response response = new Response();
            List<string> CategoryIDList = null;
            var CategoryList = _MIS_Article_CategoryBLL.m_Rep.FindList().ToList();
            if (!string.IsNullOrEmpty(CategoryID))
            {
                CategoryList = CategoryList.Where(EF => EF.ParentId == CategoryID || EF.Id == CategoryID).ToList();
                CategoryIDList = CategoryList.Select(EF => EF.Id).ToList();
            }
            List<MIS_Article> MIS_ArticleList = _MIS_ArticleBLL.m_Rep.FindList(EF => EF.CheckFlag == 1).OrderBy(EF => EF.Sort).ToList();
            if (null != CategoryIDList)
            {
                MIS_ArticleList = MIS_ArticleList.Where(EF => CategoryIDList.Contains(EF.CategoryId)).ToList();
            }
            if (null != IsType)
            {
                MIS_ArticleList = MIS_ArticleList.Where(EF => EF.IsType == IsType).ToList();
            }
            if (PageNum > 0)
            {
                MIS_ArticleList = MIS_ArticleList.Skip((PageNum - 1) * RecordNum).Take(RecordNum).ToList();
            }
            if (MIS_ArticleList.Count == 0)
            {
                LogHandler.WriteServiceLog(UserID, "返回文章集合为空", "成功", "获取文章", "ArticleController.GetArticleList()");
                response.Code = 1;
                response.Message = "文章列表为空";
                response.Data = null;
                return Json(response);
            }
            LogHandler.WriteServiceLog(UserID, "返回文章集合长度为 = " + MIS_ArticleList.Count, "成功", "获取文章", "ArticleController.GetArticleList()");
            response.Code = 0;
            response.Message = "文章列表成功";
            response.Data = new
            {
                L_ArticleList = MIS_ArticleList.Select(a => new
                {
                    Id = a.Id,
                    ImgUrl = a.ImgUrl,
                    Title = a.Title,
                    BodyContent = a.BodyContent,
                    Category = a.MIS_Article_Category.Name,
                })
            };
            return Json(response);
        }
        #endregion

        #region 获取文章栏目
        /// <summary>
        /// 获取文章栏目
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="CategoryID">栏目ID</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public object GetCategory(string UserID, string CategoryID)
        {
            LogHandler.WriteServiceLog(UserID, "UserID = " + UserID + ", CategoryID = " + CategoryID, "成功", "进入方法", "ArticleController.GetCategory()");
            Response response = new Response();
            if (string.IsNullOrEmpty(CategoryID))
            {
                CategoryID = "0";
            }
            var RefList = new SelectList(_MIS_Article_CategoryBLL.GetList(CategoryID), "Id", "Name");
            if (RefList.Count() == 0)
            {
                LogHandler.WriteServiceLog(UserID, "文章栏目为空", "成功", "获取列表", "ArticleController.GetCategory()");
                response.Code = 1;
                response.Message = "文章栏目为空";
                response.Data = null;
                return Json(response);
            }
            LogHandler.WriteServiceLog(UserID, "获取文章栏目列表长度为 = " + RefList.Count(), "成功", "获取列表", "ArticleController.GetCategory()");
            response.Code = 0;
            response.Message = "获取文章栏目列表成功";
            response.Data = new
            {
                list = RefList
            };
            return Json(response);
        }
        #endregion

        #region 获取文章
        /// <summary>
        /// 获取文章
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="ArticleID">文章ID</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public object GetArticle(string UserID, string ArticleID)
        {
            LogHandler.WriteServiceLog(UserID, "UserID = " + UserID + ", ArticleID = " + ArticleID, "成功", "进入方法", "ArticleController.GetArticle()");
            Response response = new Response();

            var Article = _MIS_ArticleBLL.m_Rep.Find(a => a.Id.Equals(ArticleID));
            if (null == Article)
            {
                LogHandler.WriteServiceLog(UserID, "结果为空", "失败", "获取文章", "ArticleController.GetArticle()");
                response.Code = 1;
                response.Message = "获取文章为空";
                response.Data = null;
                return Json(response);
            }

            //累加文章阅读次数
            if (null == Article.Click)
            {
                Article.Click = 1;
            }
            else
            {
                Article.Click++;
            }
            _MIS_ArticleBLL.m_Rep.Edit(Article);

            LogHandler.WriteServiceLog(UserID, "返回文章成功", "成功", "获取文章", "ArticleController.GetArticle()");
            response.Code = 0;
            response.Message = "获取文章成功";
            response.Data = new
            {
                S_Article = new
                {
                    Id = Article.Id,
                    ImgUrl = Article.ImgUrl,
                    Title = Article.Title,
                    BodyContent = Article.BodyContent,
                    Category = Article.MIS_Article_Category.Name,
                }
            };
            return Json(response);
        }
        #endregion
    }
}