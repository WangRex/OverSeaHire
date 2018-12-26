using Apps.Common;
using Apps.Models;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using Apps.Models.App;

namespace Apps.BLL.App
{
    public  partial class App_CustomerFamilyBLL
    {

        #region 获取姓名
        /// <summary>
        /// 获取姓名
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetName(string ID)
        {
             var Name = string.Empty;
             var _App_CustomerFamilyModel = GetById(ID);
             if(null != _App_CustomerFamilyModel)
             {
                 Name = _App_CustomerFamilyModel.Name;
             }
            return Name;
        }
        #endregion
    }
 }

