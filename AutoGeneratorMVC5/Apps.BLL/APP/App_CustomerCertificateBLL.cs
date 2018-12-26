using Apps.Common;
using Apps.Models;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using Apps.Models.App;

namespace Apps.BLL.App
{
    public  partial class App_CustomerCertificateBLL
    {

        #region 获取奖项名称
        /// <summary>
        /// 获取奖项名称
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetName(string ID)
        {
             var Name = string.Empty;
             var _App_CustomerCertificateModel = GetById(ID);
             if(null != _App_CustomerCertificateModel)
             {
                 Name = _App_CustomerCertificateModel.Name;
             }
            return Name;
        }
        #endregion
    }
 }

