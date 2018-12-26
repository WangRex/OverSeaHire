using System;
using System.Linq;
using Apps.IDAL;
using Apps.Models;
using System.Data;

namespace Apps.DAL.Sys
{
    public partial class SysAreasRepository
    {
        #region 获取地区翻译
        /// <summary>
        /// 获取地区翻译
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string GetAreasName(string Id)
        {
            string areaName = "";
            var area = GetById(Id);
            if (null != area)
            {
                areaName = area.Name;
            }
            return areaName;
        } 
        #endregion
    }
}
