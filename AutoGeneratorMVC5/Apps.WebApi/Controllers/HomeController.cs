using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Models.Sys;
using Apps.Models;
using Microsoft.Practices.Unity;
using Apps.IBLL;
using Apps.Common;
using System.Globalization;
using System.Threading;
using System.Text;
using System;
using Apps.Locale;
using Apps.Core.OnlineStat;
using Apps.Models.MIS;
using Apps.IBLL.MIS;
using Apps.Models.Flow;
using Apps.IBLL.Flow;
using Apps.IBLL.Sys;
using Apps.BLL.Sys;
using Apps.WebApi.Core;

namespace Apps.Web.Controllers
{
    public class HomeController : BaseApiController
    {
        #region UI框架
        [Dependency]
        public IHomeBLL homeBLL { get; set; }
        [Dependency]
        public ISysModuleBLL m_BLL { get; set; }
        private SysConfigModel siteConfig = new SysConfigBLL().loadConfig(Utils.GetXmlMapPath("Configpath"));
        ValidationErrors errors = new ValidationErrors();
        [Dependency]
        public ISysUserConfigBLL userConfigBLL { get; set; }

        //父ID=0的数据为顶级菜单
        public object GetTopMenu()
        {

            var currentAccount = GetCurrentAccountInfo;
            List<SysModuleModel> list = homeBLL.GetMenuByPersonId(currentAccount.AccountId, "0");
            var json = from r in list
                        select new
                        {
                            id = r.Id,
                            title = r.Name,
                            icon = r.Url,
                            link = r.Iconic
                        };

            return Json(json);
        }

        /// <summary>
        /// 获取导航菜单
        /// </summary>
        /// <param name="id">所属</param>
        /// <returns>树</returns>
        public object GetTreeByEasyui(string id)
        {
            var currentAccount = GetCurrentAccountInfo;
            List<SysModuleModel> list = homeBLL.GetMenuByPersonId(currentAccount.AccountId, id);
                var json = from r in list
                           select new SysModuleNavModel()
                           {
                               id = r.Id,
                               text = r.Name,
                               attributes = r.Url,
                               iconCls = r.Iconic,
                               state = (m_BLL.GetList(r.Id).Count > 0) ? "closed" : "open"
                           };


                return Json(json);
        }



        public object TopInfo()
        {
            var currentAccount = GetCurrentAccountInfo;
            return Json(currentAccount);
        }

        #endregion

        #region BLLs
        [Dependency]
        public ISysUserBLL userBLL { get; set; }
        [Dependency]
        public ISysStructBLL structBLL { get; set; }
        [Dependency]
        public ISysAreasBLL areasBLL { get; set; }
        [Dependency]
        public ISysUserBLL sysUserBLL { get; set; }
        [Dependency]
        public IAccountBLL accountBLL { get; set; }
        [Dependency]
        public IMIS_ArticleBLL atr_BLL { get; set; }
        [Dependency]
        public IFlow_FormContentBLL formContentBLL { get; set; }
        #endregion

        #region 我的资料
        public object Info()
        {
            var currentAccount = GetCurrentAccountInfo;
            SysUserModel entity = sysUserBLL.GetById(currentAccount.AccountId);
            //防止读取错误
           
            SysUserEditModel info = new SysUserEditModel()
            {
                Id = entity.Id,
                UserName = entity.UserName,
                TrueName = entity.TrueName,
                Card = entity.Card,
                MobileNumber = entity.MobileNumber,
                PhoneNumber = entity.PhoneNumber,
                QQ = entity.QQ,
                EmailAddress = entity.EmailAddress,
                OtherContact = entity.OtherContact,
                Province = entity.Province,
                City = entity.City,
                Village = entity.Village,
                Address = entity.Address,
                State = entity.State,
                CreateTime = entity.CreateTime,
                CreatePerson = entity.CreatePerson,
                Sex = entity.Sex,
                Birthday = ResultHelper.DateTimeConvertString(entity.Birthday),
                JoinDate = ResultHelper.DateTimeConvertString(entity.JoinDate),
                Marital = entity.Marital,
                Political = entity.Political,
                Nationality = entity.Nationality,
                Native = entity.Native,
                School = entity.School,
                Professional = entity.Professional,
                Degree = entity.Degree,
                DepId = entity.DepId,
                PosId = entity.PosId,
                Expertise = entity.Expertise,
                JobState = entity.JobState,
                Photo = entity.Photo,
                Attach = entity.Attach,
                RoleName = userBLL.GetRefSysRole(currentAccount.AccountId),
                CityName = entity.City,
                ProvinceName = entity.Province,
                VillageName = entity.Village,
                DepName = entity.DepName,
                PosName = entity.PosName
            };
            return Json(info);
        }
        #endregion
    }
}