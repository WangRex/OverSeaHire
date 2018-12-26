using System;
using System.Collections.Generic;

namespace Apps.Core
{
    /// <summary>
    /// WebApi在线用户缓存信息
    /// </summary>
    public class WebApiOnlineUser
    {
        public string AccountId { get; set; }
        public string UserName { get; set; }
        public string TrueName { get; set; }
        public string Photo { get; set; }
        public string RoleId { get; set; }
        public string DepId { get; set; }
        public string TokenKey { get; set; }
        public string CustomerAccount { get; set; } //0:系统管理用户 1：前端客户


        #region 类构造器
        /// <summary>
        /// 类默认构造器
        /// </summary>
        public WebApiOnlineUser()
        {
        }

        /// <summary>
        /// 类参数构造器
        /// </summary>
        /// <param name="uniqueID">用户 ID</param>
        /// <param name="userName">用户名称</param>
        public WebApiOnlineUser(string AccountId, string UserName, string TrueName, string Photo, string RoleId,string DepId, string CustomerAccount)
        {
            this.AccountId = AccountId;
            this.UserName = UserName;
            this.TrueName = TrueName;
            this.Photo = Photo;
            this.RoleId = RoleId;
            this.DepId = DepId;
            this.CustomerAccount = CustomerAccount;
        }
        #endregion

    }
}