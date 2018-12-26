using Apps.IBLL;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.Models.Sys;
using Apps.DAL;
using Apps.DAL.Sys;

namespace Apps.BLL
{
    public class AccountBLL : IAccountBLL
    {
        #region Reps
        [Dependency]
        public AccountRepository accountRepository { get; set; }
        [Dependency]
        public SysUserRepository sysUserRepository { get; set; }
        #endregion

        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public SysUser Login(string username, string pwd, ref string ErrorMsg)
        {
            var user = accountRepository.Login(username, pwd);
            if (null == user)
            {
                ErrorMsg = "用户名或者密码错误";
                return null;
            }
            return user;
        }
        #endregion
    }
}
