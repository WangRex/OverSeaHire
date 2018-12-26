using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tenpay
{
    /// <summary>
    /// 微信订单明细实体对象
    /// </summary>
    [Serializable]
    public class AccessTokenModel
    {

        /// <summary>
        /// 接口调用凭证
        /// </summary>
        public string access_token = "";

        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public string expires_in = "";

        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string refresh_token = "";

        /// <summary>
        /// 授权用户唯一标识
        /// </summary>
        public string openid = "";

        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public string scope = "";

        /// <summary>
        /// 当且仅当该移动应用已获得该用户的userinfo授权时，才会出现该字段
        /// </summary>
        public string unionid = "";

        /// <summary>
        /// 错误代码
        /// </summary>
        public string errcode = "";
        /// <summary>
        /// 错误代码描述
        /// </summary>
        public string err_code_des = "";

    }
}