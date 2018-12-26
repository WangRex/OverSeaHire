using System.Collections.Generic;
using System.Linq;
using Apps.Models;
using Apps.Common;
using System;
using Apps.BLL.Core;

namespace Apps.BLL.Sys
{
    /// <summary>
    /// 字段翻译
    /// </summary>
    public partial class EnumDictionaryBLL
    {
        #region 根据表名获取字典数据列表
        /// <summary>
        /// 根据表名获取字典数据列表
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        public List<EnumDictionary> GetDropDownList(string TableName)
        {
            IQueryable<EnumDictionary> queryData = null;
            queryData = m_Rep.GetList(EF => EF.TableName == TableName).OrderBy(EF => EF.SortCode).ThenByDescending(EF => EF.ModificationTime);
            return queryData.ToList();
        }
        #endregion

        #region 根据表名，key获取字典翻译
        /// <summary>
        /// 根据表名，key获取字典翻译
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string GetDicName(string TableName, string key)
        {
            string str = string.Empty;
            var _EnumDictionary = m_Rep.Find(ed => ed.TableName.Equals(TableName) && ed.ItemValue.Equals(key));
            if (null != _EnumDictionary)
            {
                str = _EnumDictionary.ItemName;
            }
            return str;
        }
        #endregion

        #region 根据列表，key获取字典翻译
        /// <summary>
        /// 根据列表，key获取字典翻译
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string GetDicName(IQueryable<EnumDictionary> list, string key)
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(key))
            {
                var _EnumDictionary = list.ToList().Find(ed => ed.ItemValue.Equals(key));
                if (null != _EnumDictionary)
                {
                    str = _EnumDictionary.ItemName;
                }
            }
            return str;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public bool Delete(ref ValidationErrors errors, int id)
        {
            try
            {
                if (m_Rep.Delete(id) == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }
        #endregion
    }
}