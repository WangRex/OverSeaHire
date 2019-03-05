using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Common;
using Apps.Models;
using Microsoft.Practices.Unity;
using Apps.Models.Sys;
using System;
using Apps.Web.Core;
using Apps.Locale;
using Apps.IBLL.Sys;
using Apps.BLL.Sys;
using System.Text;
using Newtonsoft.Json;
using Apps.BLL.App;

namespace Apps.Web.Controllers
{
    public class SysUserController : BaseController
    {

        #region BLLs
        [Dependency]
        public SysRoleBLL _SysRoleBLL { get; set; }
        [Dependency]
        public SysPositionBLL _SysPositionBLL { get; set; }
        [Dependency]
        public SysUserBLL m_BLL { get; set; }
        [Dependency]
        public ISysStructBLL structBLL { get; set; }
        [Dependency]
        public ISysPositionBLL posBLL { get; set; }
        [Dependency]
        public ISysAreasBLL areasBLL { get; set; }
        [Dependency]
        public App_CustomerBLL customerBLL { get; set; }
        [Dependency]
        public EnumDictionaryBLL enumDictionaryBLL { get; set; }
        #endregion

        ValidationErrors errors = new ValidationErrors();

        #region 首页
        [SupportFilter]
        public ActionResult Index()
        {
            //获取当前登录用户，如果是指定的人事管理用户账号，则需要按照部门显示用户
            var _Account = GetAccount();
            ViewBag.DepId = structBLL.GetCompanyId(_Account.DepId);
            return View();
        }
        [SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager, string queryStr, string sexs, string depId, string roles)
        {
            //获取当前登录用户，如果是指定的人事管理用户账号，则需要按照部门显示用户
            var _Account = GetAccount();
            if (!_SysRoleBLL.ToBeCheckAuthorityRole(_Account.RoleId, "超级管理员") && !_SysRoleBLL.ToBeCheckAuthorityRole(_Account.RoleId, "总账号"))
            {
                if (string.IsNullOrEmpty(depId))
                {
                    depId = structBLL.GetCompanyId(_Account.DepId);
                }
            }
            List<SysUserModel> list = m_BLL.GetList(ref pager, queryStr, sexs, depId, roles);
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new
                        {
                            Id = r.Id,
                            UserName = r.UserName,
                            Password = r.Password,
                            TrueName = r.TrueName,
                            Card = r.Card,
                            MobileNumber = r.MobileNumber,
                            PhoneNumber = r.PhoneNumber,
                            QQ = r.QQ,
                            EmailAddress = r.EmailAddress,
                            OtherContact = r.OtherContact,
                            Province = r.Province,
                            City = r.City,
                            Village = r.Village,
                            Address = r.Address,
                            State = r.State,
                            CreateTime = r.CreateTime,
                            CreatePerson = r.CreatePerson,
                            Sex = r.Sex,
                            Birthday = r.Birthday,
                            JoinDate = r.JoinDate,
                            Marital = r.Marital,
                            Political = r.Political,
                            Nationality = r.Nationality,
                            Native = r.Native,
                            School = r.School,
                            Professional = r.Professional,
                            Degree = r.Degree,
                            DepId = r.DepId,
                            PosId = r.PosId,
                            Expertise = r.Expertise,
                            JobState = r.JobState,
                            Photo = r.Photo,
                            Attach = r.Attach,
                            Lead = r.Lead,
                            LeadName = r.LeadName,
                            IsSelLead = r.IsSelLead,
                            IsReportCalendar = r.IsReportCalendar,
                            IsSecretary = r.IsSecretary,
                            RoleName = _SysRoleBLL.GetRefSysRole(r.Id),
                            CompanyName = structBLL.GetCompanyName(r.DepId),
                            DepName = structBLL.GetFullDepName(r.DepId),
                            PosName = _SysPositionBLL.GetById(r.PosId).Name,
                            OpenID = r.OpenID,
                            SwitchBtnLead = r.SwitchBtnLead,
                        }).ToArray()
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 获取部门名称
        /// <summary>
        /// 获取部门名称
        /// </summary>
        /// <param name="depId"></param>
        /// <returns></returns>
        public string GetDepName(string depId)
        {
            return structBLL.GetById(depId).Name;
        }
        #endregion

        #region 获取职位名称
        /// <summary>
        /// 获取职位名称
        /// </summary>
        /// <param name="posId"></param>
        /// <returns></returns>
        public string GetPosName(string posId)
        {
            return posBLL.GetById(posId).Name;
        }
        #endregion

        #region Lookup
        /// <summary>
        /// Lookup
        /// </summary>
        /// <param name="posId"></param>
        /// <returns></returns>
        public ActionResult LookUp(string owner)
        {
            if (string.IsNullOrEmpty(owner))
            {
                ViewBag.owner = "1";
            }
            else
            {
                ViewBag.owner = owner;
            }
            return View();
        }
        #endregion

        #region 设置用户角色
        [SupportFilter(ActionName = "Allot")]
        public ActionResult GetRoleByUser(string userIds)
        {
            ViewBag.UserId = userIds;
            return View();
        }
        [SupportFilter(ActionName = "Allot")]
        public JsonResult GetRoleListByUser(GridPager pager, string userIds, string queryStr)
        {
            if (string.IsNullOrWhiteSpace(userIds))
                return Json(0);
            string[] UserIds = userIds.Split(',');
            var userList = m_BLL.GetRoleByUserId(ref pager, UserIds[0], queryStr);
            var list = userList;
            var jsonData = new
            {
                total = pager.totalRows,
                rows = (
                    from r in list
                    select new SysRoleModel()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Description = r.Description,
                        Flag = r.flag == "0" ? "0" : "1",
                    }
                ).ToArray()
            };
            return Json(jsonData);
        }
        [SupportFilter(ActionName = "Save")]
        public JsonResult UpdateUserRoleByUserId(string userIds, string roleIds)
        {
            string[] arr = roleIds.Split(',');
            string[] UserIds = userIds.Split(',');
            for (int i = 0; i < UserIds.Length; i++)
            {
                if (m_BLL.UpdateSysRoleSysUser(UserIds[i], arr))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Ids:" + roleIds + "userId:" + UserIds[i], "成功", "分配角色", "用户设置");
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Ids:" + roleIds, "失败", "分配角色", "用户设置");
                    return Json(JsonHandler.CreateMessage(0, Resource.SetFail), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(JsonHandler.CreateMessage(1, Resource.SetSucceed), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 创建
        [SupportFilter]
        public ActionResult Create()
        {
            ViewBag.Struct = new SelectList(structBLL.GetList("0"), "Id", "Name");
            ViewBag.Areas = new SelectList(areasBLL.GetList("0"), "Id", "Name");
            ViewBag.EnumUserType = new SelectList(enumDictionaryBLL.GetDropDownList("SysUser.EnumUserType"), "ItemValue", "ItemName");
            SysUserModel model = new SysUserModel()
            {
                Id = ResultHelper.NewId,
                JoinDate = ResultHelper.NowTime

            };
            return View(model);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Create(SysUserModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var _NowDT = ResultHelper.NowTime;
                var userId = GetUserId();
                model.CreateTime = _NowDT;
                model.CreatePerson = userId;
                model.ModificationTime = _NowDT;
                model.ModificationUser = userId;
                if (model.SwitchBtnLead == null)
                {
                    model.SwitchBtnLead = "0";
                }
                else
                {
                    model.SwitchBtnLead = "1";
                }
                if (m_BLL.Create(ref errors, model))
                {
                    #region 如果系统用户创建成功，则创建一个AppCustomer
                    //创建用户
                    var customer = customerBLL.m_Rep.Find(EF => EF.Phone == model.MobileNumber && EF.EnumCustomerType == model.EnumUserType);
                    if (null == customer)
                    {
                        customer = new App_Customer()
                        {
                            Id = ResultHelper.NewId,
                            CreateTime = _NowDT,
                            CreateUserName = userId,
                            ModificationTime = _NowDT,
                            ModificationUserName = userId,
                            CustomerName = model.UserName,
                            Phone = model.MobileNumber,
                            Sex = model.Sex,
                            EnumCustomerType = model.EnumUserType,
                        };
                        customerBLL.m_Rep.Create(customer);
                    }
                    else
                    {
                        customer.ModificationTime = _NowDT;
                        customer.ModificationUserName = userId;
                        customer.EnumCustomerType = model.EnumUserType;
                        customerBLL.m_Rep.Edit(customer);
                    }
                    //更新用户关联信息
                    model.PK_App_Customer_CustomerName = customer.Id;
                    m_BLL.Edit(ref errors, model);
                    #endregion
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + model.Id + ",Name:" + model.UserName, "成功", "创建", "用户设置");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + model.Id + ",Name:" + model.UserName + "," + ErrorCol, "失败", "创建", "用户设置");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail), JsonRequestBehavior.AllowGet);
            }
        }
        //判断是否用户重复
        [HttpPost]
        public JsonResult JudgeUserName(string userName)
        {
            return Json("用户名已经存在！", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 修改
        [SupportFilter]
        public ActionResult Edit(string id)
        {
            ViewBag.Areas = new SelectList(areasBLL.GetList("0"), "Id", "Name");
            ViewBag.EnumUserType = new SelectList(enumDictionaryBLL.GetDropDownList("SysUser.EnumUserType"), "ItemValue", "ItemName");
            SysUserModel entity = m_BLL.GetById(id);
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Edit(SysUserEditModel info)
        {
            var dtNow = ResultHelper.NowTime;
            string strUserId = GetUserId();
            info.ModificationTime = dtNow;
            info.ModificationUser = GetUserTrueName();
            if (info != null && ModelState.IsValid)
            {
                if (info.SwitchBtnLead == null)
                {
                    info.SwitchBtnLead = "0";
                }
                else
                {
                    info.SwitchBtnLead = "1";
                }
                if (m_BLL.Edit(ref errors, info))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Name:" + info.UserName, "成功", "修改", "用户设置");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Name:" + info.UserName + "," + ErrorCol, "失败", "修改", "用户设置");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":" + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [SupportFilter(ActionName = "Edit")]
        public JsonResult ReSet(string Id, string Pwd)
        {
            SysUserEditModel editModel = new SysUserEditModel();
            editModel.Id = Id;
            editModel.Password = Pwd;
            if (m_BLL.EditPwd(ref errors, editModel))
            {
                LogHandler.WriteServiceLog(GetUserId(), "Id:" + Id + ",密码:" + Pwd, "成功", "初始化密码", "用户设置");
                return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ErrorCol = errors.Error;
                LogHandler.WriteServiceLog(GetUserId(), "Id:" + Id + ",,密码:" + Pwd + ErrorCol, "失败", "初始化密码", "用户设置");
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":" + ErrorCol), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 详细
        [SupportFilter]
        public ActionResult Details(string id)
        {

            SysUserModel entity = m_BLL.GetById(id);
            //防止读取错误
            string CityName, ProvinceName, VillageName, DepName, PosName;
            try
            {
                CityName = !string.IsNullOrEmpty(entity.City) ? areasBLL.GetById(entity.City).Name : "";
                ProvinceName = !string.IsNullOrEmpty(entity.Province) ? areasBLL.GetById(entity.Province).Name : "";
                VillageName = !string.IsNullOrEmpty(entity.Village) ? areasBLL.GetById(entity.Village).Name : "";
                DepName = !string.IsNullOrEmpty(entity.DepId) ? structBLL.GetById(entity.DepId).Name : "";
                PosName = !string.IsNullOrEmpty(entity.PosId) ? structBLL.GetById(entity.PosId).Name : "";
            }
            catch
            {
                CityName = "";
                ProvinceName = "";
                VillageName = "";
                DepName = "";
                PosName = "";
            }
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
                RoleName = m_BLL.GetRefSysRole(id),
                CityName = CityName,
                ProvinceName = ProvinceName,
                VillageName = VillageName,
                DepName = DepName,
                PosName = PosName,
                OpenID = entity.OpenID
            };
            return View(info);
        }

        #endregion

        #region 删除
        [HttpPost]
        [SupportFilter]
        public JsonResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                string[] Ids = id.Split(',');
                for (int i = 0; i < Ids.Length; i++)
                {
                    //保护管理员不能被删除
                    if (Ids[i] == "admin")
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "尝试删除管理员", "失败", "删除", "用户设置");
                        return Json(JsonHandler.CreateMessage(0, "管理员不能被删除！"), JsonRequestBehavior.AllowGet);
                    }
                    if (m_BLL.Delete(ref errors, Ids[i]))
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + Ids[i], "成功", "删除", "用户设置");
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + Ids[i] + "," + ErrorCol, "失败", "删除", "用户设置");
                        return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                    }
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail), JsonRequestBehavior.AllowGet);
            }
            return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 获取系统角色列表
        /// <summary>
        /// 获取系统角色列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRoleList()
        {
            var list = _SysRoleBLL.m_Rep.FindList().ToList();
            StringBuilder sb = new StringBuilder("");
            sb.AppendFormat("<option value='{0}'>{1}</option>", "", "--请选择--");
            foreach (var i in list)
            {
                sb.AppendFormat("<option value='{0}'>{1}</option>", i.Id, i.Name);
            }
            return Json(sb.ToString());
        }
        #endregion

        #region 获取性别
        /// <summary>
        /// 获取性别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSexList()
        {
            var list = _SysRoleBLL.m_Rep.FindList().ToList();
            return Json("<option value=''>--请选择--</option><option value='男'>男</option><option value='女'>女</option>");
        }
        #endregion

        #region 获取用户
        /// <summary>
        /// 获取用户模块
        /// </summary>
        /// <param name="UserIDs"></param>
        /// <returns></returns>
        public ActionResult IndexAppModule(string UserIDs)
        {
            ViewBag.UserIDs = UserIDs;
            return View();
        }
        /// <summary>
        /// 获取用户模块
        /// </summary>
        /// <param name="UserIDs"></param>
        /// <returns></returns>
        public JsonResult IndexAppModuleList(GridPager pager, string UserIDs)
        {
            List<SysUser> list = m_BLL.GetUserListByUserIds(pager, UserIDs).ToList();
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new UserAppModuleModel()
                        {
                            Id = r.Id,
                            UserName = r.UserName,
                            TrueName = r.TrueName,
                            MobileNumber = r.MobileNumber,
                        }).ToArray()
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateAppModuleByUserId()
        {
            string result = Request.Form[0];
            //后台拿到字符串时直接反序列化。根据需要自己处理
            List<UserAppModuleModel> datagridList = new List<UserAppModuleModel>();
            try
            {
                datagridList = JsonConvert.DeserializeObject<List<UserAppModuleModel>>(result);
            }
            catch (Exception)
            {
                Response refData = new Response();
                refData.Code = 0;
                refData.Message = "输入数据类型错误，请点撤销后重新输入";
                return Json(refData);
            }
            if (m_BLL.UpdateSysUserModel(datagridList))
            {
                return Json(JsonHandler.CreateMessage(1, Resource.SetSucceed), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ErrorCol = errors.Error;
                return Json(JsonHandler.CreateMessage(0, Resource.SetFail), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 用户导入
        [HttpPost]
        public JsonResult ImportUserExcel(string filePath)
        {
            AccountModel account = GetAccount();
            LogHandler.WriteServiceLog(account.Id, "导入路径:" + filePath, "成功", "进入", "SysUserController.ImportUserExcel");
            var sysUserList = new List<SysUser>();
            bool checkResult = false;
            //检查导入数据是否正常
            checkResult = m_BLL.CheckImportUser(filePath, sysUserList, ref errors);
            if (checkResult)
            {
                bool status = true;
                foreach (SysUser Item in sysUserList)
                {
                    var _NowDT = ResultHelper.NowTime;
                    Item.CreateTime = _NowDT;
                    Item.ModificationTime = _NowDT;
                    Item.CreatePerson = GetUserId();
                    Item.JoinDate = _NowDT;
                    if (!m_BLL.m_Rep.Create(Item))
                    {
                        status = false;
                    }
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                if (status)
                {
                    dic.Add("status", "success");
                    dic.Add("message", "导入成功");
                    LogHandler.WriteServiceLog(GetUserId(), "", "成功导入", "导入", "ImportUserExcel");
                    return Json(dic);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), ErrorCol, "失败", "导入", "ImportUserExcel");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
                }
            }
            else
            {
                string ErrorCol = errors.Error;
                LogHandler.WriteServiceLog(GetUserId(), ErrorCol, "失败", "导入", "ImportUserExcel");
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
            }
        }
        #endregion

    }
}
