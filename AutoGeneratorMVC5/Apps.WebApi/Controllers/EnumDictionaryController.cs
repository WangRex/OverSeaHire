/**
* 命名空间: Apps.WebApi.Controllers
*
* 功 能： N/A
* 类 名： EnumDictionaryController
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017-11-13 15:17:33 王仁禧 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：大连安琪科技有限公司 　　　　　　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using System.Collections.Generic;
using Apps.Common;
using System.Linq;
using Apps.WebApi.Core;
using Microsoft.Practices.Unity;
using Apps.BLL.Sys;
using System.Web.Http;
using Apps.Models.Sys;
using Apps.Models;

namespace Apps.WebApi.Controllers
{
    /// <summary>
    /// 字典
    /// </summary>
    public class EnumDictionaryController : BaseApiController
    {
        #region BLLs
        /// <summary>
        /// 数字字典
        /// </summary>
        [Dependency]
        public EnumDictionaryBLL _EnumDictionaryBLL { get; set; }
        #endregion

        #region 获取字典列表
        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <param name="CustomerID">用户ID</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        [HttpGet]
        public object GetDics(string CustomerID, string TableName)
        {
            LogHandler.WriteServiceLog(CustomerID, "用户ID : " + CustomerID + ", TableName = " + TableName, "成功", "进入方法", "EnumDictionaryController.GetDics()");
            Response response = new Response();
            List<EnumDictionaryViewModel> _EnumDictionaryViewModelList = new List<EnumDictionaryViewModel>();
            var EnumDictionaryList = _EnumDictionaryBLL.m_Rep.FindList(ED => ED.TableName.Equals(TableName)).OrderBy(ED => ED.SortCode).ThenByDescending(ED => ED.ModificationTime).ToList();
            if(EnumDictionaryList.Count == 0)
            {
                LogHandler.WriteServiceLog(CustomerID, "获取的字典集合为空", "成功", "获取字典集合", "EnumDictionaryController.GetDics()");
                response.Code = 1;
                response.Message = "获取的字典集合为空";
                response.Data = null;
            } else
            {
                LogHandler.WriteServiceLog(CustomerID, "获取的字典集合长度为 = " + EnumDictionaryList.Count, "成功", "获取字典集合", "EnumDictionaryController.GetDics()");
                response.Code = 0;
                response.Message = "获取的字典集合成功";
                foreach(EnumDictionary _EnumDictionary in EnumDictionaryList)
                {
                    EnumDictionaryViewModel _EnumDictionaryViewModel = new EnumDictionaryViewModel();
                    _EnumDictionaryViewModel.Id = _EnumDictionary.Id;
                    _EnumDictionaryViewModel.TableName = _EnumDictionary.TableName;
                    _EnumDictionaryViewModel.ItemName = _EnumDictionary.ItemName;
                    _EnumDictionaryViewModel.ItemValue = _EnumDictionary.ItemValue;
                    _EnumDictionaryViewModel.ItemPhoto = _EnumDictionary.ItemPhoto;
                    _EnumDictionaryViewModelList.Add(_EnumDictionaryViewModel);
                }
                response.Data = _EnumDictionaryViewModelList;
            }
            return Json(response);
        }
        #endregion
    }
}