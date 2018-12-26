using Apps.Common;
using Apps.Core;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Apps.WebApi.Core
{
    //[SupportFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BaseApiController : ApiController
    {
        protected virtual HttpRequestBase httpRequestBase
        {
            get {
                var context = (HttpContextBase)Request.Properties[ConfigPara.MS_HttpContext];
                return context.Request;
            }
        }
        protected virtual WebApiOnlineUser GetCurrentAccountInfo
        {
            get
            {
                var context = (HttpContextBase)Request.Properties[ConfigPara.MS_HttpContext];
                var token = context.Request.QueryString[ConfigPara.Token];
                return LoginUserManage.GetOnlineAccountToken(token);
            }
        }
    }
}