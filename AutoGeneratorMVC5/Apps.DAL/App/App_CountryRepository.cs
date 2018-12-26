using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.DAL.App
{
    public partial class App_CountryRepository
    {
        #region 获取国家名称
        /// <summary>
        /// 获取国家名称
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetName(string ID)
        {
            var Name = string.Empty;
            var _App_CountryModel = GetById(ID);
            if (null != _App_CountryModel)
            {
                Name = _App_CountryModel.Name;
            }
            return Name;
        }
        #endregion

        #region 获取国家名称
        /// <summary>
        /// 获取国家名称
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public string GetNames(string Ids)
        {
            var Names = string.Empty;
            if (!string.IsNullOrEmpty(Ids))
            {
                var app_Countries = FindList(EF => Ids.Contains(EF.Id)).ToList();
                if (null != app_Countries)
                {
                    Names = string.Join(",", app_Countries.Select(EF => EF.Name).ToArray());
                }
            }
            return Names;
        }
        #endregion
    }
}
