using System.Web.Mvc;

namespace Apps.Web.Areas.APP
{
    public class APPAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "APP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "appGlobalization", // 路由名称
                "{lang}/app/{controller}/{action}/{id}", // 带有参数的 URL
                new { lang = "zh", controller = "Home", action = "Index", id = UrlParameter.Optional }, // 参数默认值
                new { lang = "^[a-zA-Z]{2}(-[a-zA-Z]{2})?$" }    //参数约束
            );
            context.MapRoute(
                "app_default",
                "app/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
