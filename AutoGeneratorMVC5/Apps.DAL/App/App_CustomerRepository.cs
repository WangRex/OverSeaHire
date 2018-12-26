using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.DAL.App
{
    public partial class App_CustomerRepository
    {
        #region 获取用户姓名
        /// <summary>
        /// 获取用户姓名
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public string GetCustomerName(string CustomerID)
        {
            var CustomerName = string.Empty;
            var _APP_CustomerModel = GetById(CustomerID);
            if (null != _APP_CustomerModel)
            {
                CustomerName = _APP_CustomerModel.CustomerName;
            }
            return CustomerName;
        }
        #endregion
    }
}
