using Apps.BLL;
using Apps.BLL.Sys;
using Apps.Common;
using Apps.Core;
using Apps.DAL;
using Apps.DAL.Sys;
using Apps.Models;
using Apps.Models.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Security;

namespace Apps.WebApi.Core
{
    public class SupportFilter : AuthorizeAttribute
    {
        //重写基类的验证方式，加入我们自定义的Ticket验证
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //url获取token
            var content = actionContext.Request.Properties[ConfigPara.MS_HttpContext] as HttpContextBase;

            var token = content.Request.QueryString[ConfigPara.Token];
            if (!string.IsNullOrEmpty(token))
            {

                //读取请求上下文中的Controller,Action,Id
                var routes = new RouteCollection();
                RouteConfig.RegisterRoutes(routes);
                RouteData routeData = routes.GetRouteData(content);
                //取出区域的控制器Action,id
                string controller = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                string action = actionContext.ActionDescriptor.ActionName;
                //URL路径
                WebApiOnlineUser _WebApiOnlineUser = LoginUserManage.GetOnlineAccountToken(token);
                if (_WebApiOnlineUser != null && ValiddatePermission(_WebApiOnlineUser.AccountId, controller, action))
                {
                    //已经登录，有权限，且没有单机登录限制
                    base.IsAuthorized(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            //如果取不到身份验证信息，并且不允许匿名访问，则返回未验证401
            else
            {
                var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                if (isAnonymous) base.OnAuthorization(actionContext);
                else HandleUnauthorizedRequest(actionContext);
            }
        }

        //TODO 追加缓存机制
        public bool ValiddatePermission(string AccountId, string controller, string action)
        {
            bool bResult = false;
            List<permModel> perm = null;
            SysUserBLL userBLL = new SysUserBLL()
            {
                m_Rep = new SysUserRepository(new DBContainer()),
                sysRightRep = new SysRightRepository(new DBContainer())
            };
            perm = userBLL.GetPermission(AccountId, controller);//获取当前用户的权限列表
                                                               //查询当前Action 是否有操作权限，大于0表示有，否则没有
            int count = perm.Where(a => a.KeyCode.ToLower() == action.ToLower()).Count();
                if (count > 0)
                {
                    bResult = true;
                }
                else
                {
                    bResult = false;
                    LoginUserManage.RedirectUrlFor401();
                }
            return bResult;
        }
    }
}