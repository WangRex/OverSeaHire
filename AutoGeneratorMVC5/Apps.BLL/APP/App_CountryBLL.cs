using Apps.Common;
using Apps.Models;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using Apps.Models.App;

namespace Apps.BLL.App
{
    public  partial class App_CountryBLL
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
             if(null != _App_CountryModel)
             {
                 Name = _App_CountryModel.Name;
             }
            return Name;
        }
        #endregion
    }
 }

