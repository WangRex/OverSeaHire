using Apps.Common;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Apps.BLL.Sys;

namespace Apps.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class AddressListController : ApiController
    {
        [Dependency]
        public SysAreasBLL areasBLL { get; set; }

        /// <summary>
        /// 获取省市区
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public object GetAddress(string id)
        {
            //List<SysAreasModel> list = areasBLL.GetList(id);
            //StringBuilder sb = new StringBuilder("");
            //foreach (var i in list)
            //{
            //    sb.AppendFormat("<option value='{0}'>{1}</option>", i.Id, i.Name);
            //}
            if (string.IsNullOrEmpty(id))
            {
                id = "0";
            }
            var RefList = new SelectList(areasBLL.GetList(id), "Id", "Name");
            Response response = new Response();
            response.Code = 0;
            response.Message = "获取省市区列表成功";
            response.Data = new
            {
                list = RefList
            };
            return Json(response);
        }
    }
}