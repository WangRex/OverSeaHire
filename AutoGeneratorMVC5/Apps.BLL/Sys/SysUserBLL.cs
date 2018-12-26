using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apps.BLL.Core;
using Apps.IBLL;
using Microsoft.Practices.Unity;
using Apps.IDAL;
using Apps.Models.Sys;
using Apps.Common;
using Apps.Models;
using System.Transactions;
using Apps.Locale;
using Apps.IDAL.Sys;
using System.IO;
using LinqToExcel;

namespace Apps.BLL.Sys
{
    public partial class SysUserBLL
    {
        #region BLLs
        [Dependency]
        public SysStructBLL _SysStructBLL { get; set; }
        [Dependency]
        public SysPositionBLL _SysPositionBLL { get; set; }
        #endregion

        #region GetUserName
        /// <summary>
        /// GetUserName
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetUserName(string ID)
        {
            var UserName = string.Empty;
            var _refModel = GetById(ID);
            if (null != _refModel)
            {
                UserName = _refModel.UserName;
            }
            return UserName;
        }
        #endregion

        #region Reps
        [Dependency]
        public ISysRightRepository sysRightRep { get; set; }
        [Dependency]
        public ISysStructRepository structRep { get; set; }
        [Dependency]
        public ISysPositionRepository posRep { get; set; }
        [Dependency]
        public ISysRoleRepository roleRep { get; set; }
        [Dependency]
        public ISysLogRepository logRep { get; set; }
        #endregion

        #region 获取菜单权限
        /// <summary>
        /// 获取菜单权限
        /// </summary>
        /// <param name="accountid"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public List<permModel> GetPermission(string accountid, string controller)
        {
            return sysRightRep.GetPermission(accountid, controller);
        }
        #endregion

        #region 按照部门获取列表
        /// <summary>
        /// 按照部门获取列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="queryStr"></param>
        /// <param name="depId"></param>
        /// <returns></returns>
        public List<SysUserModel> GetList(ref GridPager pager, string queryStr, string depId)
        {

            List<SysUser> query = null;
            IQueryable<SysUser> list = m_Rep.GetList();
            pager.totalRows = list.Count();
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                list = list.Where(a => a.UserName.Contains(queryStr) || a.TrueName.Contains(queryStr));
            }
            //根据部门来查询
            if (!string.IsNullOrWhiteSpace(depId) && depId != "root")
            {
                list = list.Where(a => a.DepId == depId);

            }
            if (pager.order == "desc")
            {
                if (pager.order == "UserName")
                {
                    query = list.OrderBy(c => c.UserName).Skip((pager.page - 1) * pager.rows).Take(pager.rows).ToList();
                }
                else//createtime
                {
                    query = list.OrderBy(c => c.CreateTime).Skip((pager.page - 1) * pager.rows).Take(pager.rows).ToList();
                }
            }
            else
            {
                if (pager.order == "UserName")
                {
                    query = list.OrderByDescending(c => c.UserName).Skip((pager.page - 1) * pager.rows).Take(pager.rows).ToList();
                }
                else//createtime
                {
                    query = list.OrderByDescending(c => c.CreateTime).Skip((pager.page - 1) * pager.rows).Take(pager.rows).ToList();
                }
            }

            List<SysUserModel> userInfoList = new List<SysUserModel>();
            List<SysUser> dataList = query.ToList();
            foreach (var user in dataList)
            {
                SysUserModel userModel = new SysUserModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Password = user.Password,
                    TrueName = user.TrueName,
                    MobileNumber = user.MobileNumber,
                    PhoneNumber = user.PhoneNumber,
                    QQ = user.QQ,
                    EmailAddress = user.EmailAddress,
                    OtherContact = user.OtherContact,
                    Province = user.Province,
                    City = user.City,
                    Village = user.Village,
                    Address = user.Address,
                    State = user.State,
                    CreateTime = user.CreateTime,
                    CreatePerson = user.CreatePerson,
                    RoleName = GetRefSysRole(user.Id),
                    PosName = user.SysPosition.Name,
                    DepName = user.SysStruct.Name
                };
                userInfoList.Add(userModel);
            }

            return userInfoList;
        }
        #endregion

        #region 按照职位获取用户
        /// <summary>
        /// 按照职位获取用户
        /// </summary>
        /// <param name="posId"></param>
        /// <returns></returns>
        public List<SysUserModel> GetListByPosId(string posId)
        {
            IQueryable<SysUser> list = m_Rep.GetListByPosId(posId);
            List<SysUserModel> modelList = (from r in list
                                            select new SysUserModel
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
                                                IsSecretary = r.IsSecretary
                                            }).ToList();
            return modelList;
        }
        #endregion

        #region 获取用户角色
        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetRefSysRole(string userId)
        {
            string RoleName = "";
            var roleList = m_Rep.GetRefSysRole(userId);
            if (roleList != null)
            {
                foreach (var role in roleList)
                {
                    RoleName += "[" + role.Name + "] ";
                }
            }
            return RoleName;
        }
        #endregion

        #region 获取角色
        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<P_Sys_GetRoleByUserId_Result> GetRoleByUserId(ref GridPager pager, string userId, string queryStr)
        {
            List<P_Sys_GetRoleByUserId_Result> queryData = m_Rep.GetRoleByUserId(userId).OrderByDescending(EF => EF.CreateTime).ToList();
            if (!string.IsNullOrEmpty(queryStr))
            {
                queryData = queryData.Where(EF => EF.Name.Contains(queryStr)).ToList();
            }
            pager.totalRows = queryData.Count;
            return queryData.Skip((pager.page - 1) * pager.rows).Take(pager.rows).ToList();
        }
        #endregion

        #region 根据部门ID获取用户列表
        /// <summary>
        /// 根据部门ID获取用户列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="depId"></param>
        /// <param name="queryStr"></param>
        /// <returns></returns>
        public List<SysUserModel> GetUserByDepId(ref GridPager pager, string depId, string queryStr)
        {
            IQueryable<P_Sys_GetUserByDepId_Result> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetUserByDepId(depId).Where(a => a.TrueName.Contains(queryStr));
                pager.totalRows = queryData.Count();
                queryData = m_Rep.GetUserByDepId(depId).Where(a => a.TrueName.Contains(queryStr));
            }
            else
            {
                queryData = m_Rep.GetUserByDepId(depId);
                pager.totalRows = queryData.Count();
                queryData = m_Rep.GetUserByDepId(depId);
            }

            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);

        }
        #endregion

        #region 转换用户列表
        /// <summary>
        /// 转换用户列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        private List<SysUserModel> CreateModelList(ref IQueryable<P_Sys_GetUserByDepId_Result> queryData)
        {
            List<SysUserModel> modelList = (from r in queryData
                                            select new SysUserModel
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
                                                IsSecretary = r.IsSecretary
                                            }).ToList();
            foreach (var v in modelList)
            {
                v.DepName = structRep.GetById(v.DepId).Name;
                v.PosName = posRep.GetById(v.PosId).Name;
            }
            return modelList;
        }
        #endregion

        #region 更新用户角色
        /// <summary>
        /// 更新用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public bool UpdateSysRoleSysUser(string userId, string[] roleIds)
        {
            try
            {
                m_Rep.UpdateSysRoleSysUser(userId, roleIds);
                return true;

            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                return false;
            }

        }
        #endregion

        #region 重写创建方法
        /// <summary>
        /// 重写创建方法
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public override bool Create(ref ValidationErrors errors, SysUserModel model)
        {
            try
            {
                if (model.State)
                {
                    if (m_Rep.GetList(a => a.UserName == model.UserName).Count() > 0)
                    {
                        errors.Add("用户名已经存在！");
                        return false;
                    }
                }
                if (m_Rep.GetList(a => a.MobileNumber.Equals(model.MobileNumber)).Count() > 0)
                {
                    errors.Add("手机号已注册！");
                    return false;
                }
                SysUser entity = new SysUser();
                entity.Id = model.Id;
                entity.UserName = model.UserName;
                entity.Password = model.Password;
                entity.TrueName = model.TrueName;
                entity.Card = model.Card;
                entity.MobileNumber = model.MobileNumber;
                entity.PhoneNumber = model.PhoneNumber;
                entity.QQ = model.QQ;
                entity.EmailAddress = model.EmailAddress;
                entity.OtherContact = model.OtherContact;
                entity.Province = model.Province;
                entity.City = model.City;
                entity.Village = model.Village;
                entity.Address = model.Address;
                entity.State = model.State;
                entity.CreateTime = model.CreateTime;
                entity.CreatePerson = model.CreatePerson;
                entity.ModificationTime = model.ModificationTime;
                entity.ModificationUser = model.ModificationUser;
                entity.Sex = model.Sex;
                entity.Birthday = model.Birthday;
                entity.JoinDate = model.JoinDate;
                entity.Marital = model.Marital;
                entity.Political = model.Political;
                entity.Nationality = model.Nationality;
                entity.Native = model.Native;
                entity.School = model.School;
                entity.Professional = model.Professional;
                entity.Degree = model.Degree;
                entity.DepId = model.DepId;
                entity.PosId = model.PosId;
                entity.Expertise = model.Expertise;
                entity.JobState = model.JobState;
                entity.Photo = model.Photo;
                entity.Attach = model.Attach;
                entity.Lead = model.Lead;
                entity.LeadName = model.LeadName;
                entity.IsSelLead = model.IsSelLead;
                entity.IsReportCalendar = model.IsReportCalendar;
                entity.IsSecretary = model.IsSecretary;
                entity.SwitchBtnLead = model.SwitchBtnLead;
                entity.EnumUserType = model.EnumUserType;
                entity.PK_App_Customer_CustomerName = model.PK_App_Customer_CustomerName;

                if (m_Rep.Create(entity))
                {
                    return true;
                }
                else
                {
                    errors.Add(Resource.InsertFail);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }
        #endregion

        #region 编辑用户
        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Edit(ref ValidationErrors errors, SysUserEditModel model)
        {
            try
            {
                if (model.State)
                {
                    if (string.IsNullOrEmpty(model.UserName))
                    {
                        errors.Add("用户名不能为空！");
                        return false;
                    }
                    var list = m_Rep.GetList(a => a.UserName == model.UserName).ToList();
                    if (list.Count() > 0 && !list.First().Id.Equals(model.Id))
                    {
                        errors.Add("用户名已经存在！");
                        return false;
                    }
                }
                var listPh = m_Rep.GetList(a => a.MobileNumber.Equals(model.MobileNumber)).ToList();
                if (listPh.Count() > 0 && !listPh.First().Id.Equals(model.Id))
                {
                    errors.Add("手机号已注册！");
                    return false;
                }
                SysUser entity = m_Rep.GetById(model.Id);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                entity.TrueName = model.TrueName;
                entity.UserName = model.UserName;
                entity.Password = model.Password;
                entity.Card = model.Card;
                entity.MobileNumber = model.MobileNumber;
                entity.PhoneNumber = model.PhoneNumber;
                entity.QQ = model.QQ;
                entity.EmailAddress = model.EmailAddress;
                entity.OtherContact = model.OtherContact;
                entity.Province = model.Province;
                entity.City = model.City;
                entity.Village = model.Village;
                entity.Address = model.Address;
                entity.State = model.State;
                entity.CreateTime = model.CreateTime;
                entity.CreatePerson = model.CreatePerson;
                entity.Sex = model.Sex;
                entity.Birthday = ResultHelper.StringConvertDatetime(model.Birthday);
                entity.JoinDate = ResultHelper.StringConvertDatetime(model.JoinDate);
                entity.Marital = model.Marital;
                entity.Political = model.Political;
                entity.Nationality = model.Nationality;
                entity.Native = model.Native;
                entity.School = model.School;
                entity.Professional = model.Professional;
                entity.Degree = model.Degree;
                entity.DepId = model.DepId;
                entity.PosId = model.PosId;
                entity.Expertise = model.Expertise;
                entity.JobState = model.JobState;
                entity.Photo = model.Photo;
                entity.Attach = model.Attach;
                entity.Lead = model.Lead;
                entity.LeadName = model.LeadName;
                entity.IsSelLead = model.IsSelLead;
                entity.IsReportCalendar = model.IsReportCalendar;
                entity.IsSecretary = model.IsSecretary;
                entity.ModificationTime = model.ModificationTime;
                entity.ModificationUser = model.ModificationUser;
                entity.SwitchBtnLead = model.SwitchBtnLead;
                entity.EnumUserType = model.EnumUserType;
                entity.PK_App_Customer_CustomerName = model.PK_App_Customer_CustomerName;

                if (m_Rep.Edit(entity))
                {
                    return true;
                }
                else
                {
                    errors.Add(Resource.NoDataChange);
                    return false;
                }

            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool EditPwd(ref ValidationErrors errors, SysUserEditModel model)
        {
            try
            {
                SysUser entity = m_Rep.GetById(model.Id);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }

                entity.Password = model.Password;

                if (m_Rep.Edit(entity))
                {
                    return true;
                }
                else
                {
                    errors.Add(Resource.NoDataChange);
                    return false;
                }

            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }
        #endregion

        #region 重写根据ID获取用户
        /// <summary>
        /// 重写根据ID获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override SysUserModel GetById(string id)
        {
            SysUser entity = m_Rep.GetById(id);
            if (null != entity)
            {
                SysUserModel model = new SysUserModel();
                model.Id = entity.Id;
                model.UserName = entity.UserName;
                model.Password = entity.Password;
                model.TrueName = entity.TrueName;
                model.Card = entity.Card;
                model.MobileNumber = entity.MobileNumber;
                model.PhoneNumber = entity.PhoneNumber;
                model.QQ = entity.QQ;
                model.EmailAddress = entity.EmailAddress;
                model.OtherContact = entity.OtherContact;
                model.Province = entity.Province;
                model.City = entity.City;
                model.Village = entity.Village;
                model.Address = entity.Address;
                model.State = entity.State;
                model.CreateTime = entity.CreateTime;
                model.CreatePerson = entity.CreatePerson;
                model.Sex = entity.Sex;
                model.Birthday = entity.Birthday;
                model.JoinDate = entity.JoinDate;
                model.Marital = entity.Marital;
                model.Political = entity.Political;
                model.Nationality = entity.Nationality;
                model.Native = entity.Native;
                model.School = entity.School;
                model.Professional = entity.Professional;
                model.Degree = entity.Degree;
                model.DepId = entity.DepId;
                model.DepName = entity.SysStruct.Name;
                model.PosId = entity.PosId;
                model.PosName = entity.SysPosition.Name;
                model.Expertise = entity.Expertise;
                model.JobState = entity.JobState;
                model.Photo = entity.Photo;
                model.Attach = entity.Attach;
                model.Lead = entity.Lead;
                model.LeadName = entity.LeadName;
                model.IsSelLead = entity.IsSelLead;
                model.IsReportCalendar = entity.IsReportCalendar;
                model.IsSecretary = entity.IsSecretary;
                model.OpenID = entity.OpenID;
                model.ModificationTime = entity.ModificationTime;
                model.ModificationUser = entity.ModificationUser;
                model.SwitchBtnLead = entity.SwitchBtnLead;
                model.EnumUserType = entity.EnumUserType;
                model.PK_App_Customer_CustomerName = entity.PK_App_Customer_CustomerName;
                return model;
            }
            else
            {
                return null;
            }

        }
        #endregion

        #region 获取真实名字
        /// <summary>
        /// 获取真实名字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTrueName(string id)
        {
            var TrueName = string.Empty;
            var _refModel = GetById(id);
            if (null != _refModel)
            {
                TrueName = _refModel.TrueName;
            }
            return TrueName;
        }
        #endregion

        #region 模糊搜索name
        /// <summary>
        /// 模糊搜索name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<SysUser> GetListBySelName(string name)
        {
            return m_Rep.GetList(a => a.TrueName.Contains(name)).ToList();
        }
        #endregion

        #region 按照部门ID获取用户列表
        /// <summary>
        /// 按照部门ID获取用户列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SysUser> GetListByDepId(string id)
        {
            return m_Rep.GetList(a => a.DepId == id).ToList();
        }
        #endregion

        #region 获取二级架构下的用户
        /// <summary>
        /// 获取二级架构下的用户
        /// </summary>
        /// <returns></returns>
        public List<SysOnlineUserModel> GetAllUsers()
        {
            IQueryable<P_Sys_GetAllUsers_Result> queryData = m_Rep.GetAllUsers();
            List<SysOnlineUserModel> modelList = (from r in queryData
                                                  select new SysOnlineUserModel
                                                  {
                                                      UserId = r.UserId,
                                                      TrueName = r.TrueName,
                                                      Email = r.EmailAddress,
                                                      PhoneNumber = r.PhoneNumber,
                                                      Photo = r.Photo,
                                                      PosName = r.PosName,
                                                      Sort = r.Sort,
                                                      StructId = r.StructId,
                                                      StructName = r.StructName,
                                                      ContextId = "",
                                                      Status = 0,//0离线状态1在线2忙碌3离开
                                                  }).ToList();
            return modelList;
        }
        #endregion

        #region 根据角色名获取用户列表
        /// <summary>
        /// 根据角色名获取用户列表
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public List<SysUser> GetUserListByRoleName(string roleName)
        {
            List<SysUser> _SysUserList = new List<SysUser>();
            //获取传入角色名称的角色ID
            SysRole _SysRole = roleRep.Find(d => d.Name == roleName);
            if (null != _SysRole)
            {
                _SysUserList = roleRep.GetRefSysUser(_SysRole.Id).ToList();
            }
            return _SysUserList;

        }
        #endregion

        #region 根据角色名获取用户列表
        /// <summary>
        /// 根据角色名获取用户列表
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public List<SysUser> GetUserListByRoleNameAndTrueName(string roleName, string TrueName)
        {
            List<SysUser> _SysUserList = new List<SysUser>();
            //获取传入角色名称的角色ID
            SysRole _SysRole = roleRep.Find(d => d.Name == roleName);
            if (null != _SysRole)
            {
                _SysUserList = roleRep.GetRefSysUser(_SysRole.Id).ToList();
                if (!string.IsNullOrEmpty(TrueName))
                {
                    _SysUserList = _SysUserList.Where(EF => EF.TrueName.Contains(TrueName)).ToList();
                }
            }
            return _SysUserList;

        }
        #endregion

        #region 获取座机号
        /// <summary>
        /// 获取座机号
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetPhoneNumber(string ID)
        {
            var PhoneNumber = string.Empty;
            var _refModel = GetById(ID);
            if (null != _refModel)
            {
                PhoneNumber = _refModel.PhoneNumber;
            }
            return PhoneNumber;
        }
        #endregion

        #region 获取手机号
        /// <summary>
        /// 获取手机号
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetMobileNumber(string ID)
        {
            var MobileNumber = string.Empty;
            var _refModel = GetById(ID);
            if (null != _refModel)
            {
                MobileNumber = _refModel.MobileNumber;
            }
            return MobileNumber;
        }
        #endregion

        #region 获取简介
        /// <summary>
        /// 获取简介
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetExpertise(string ID)
        {
            var Expertise = string.Empty;
            var _refModel = GetById(ID);
            if (null != _refModel)
            {
                Expertise = _refModel.Expertise;
            }
            return Expertise;
        }
        #endregion

        #region 按照部门角色性别获取列表
        /// <summary>
        /// 按照部门角色性别获取列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="sexs"></param>
        /// <param name="roles"></param>
        /// <param name="depId"></param>
        /// <returns></returns>
        public List<SysUserModel> GetDSRList(ref GridPager pager, string sexs, string depId, string roles)
        {

            List<SysUser> query = null;
            IQueryable<SysUser> list = m_Rep.GetList();
            pager.totalRows = list.Count();
            if (!string.IsNullOrWhiteSpace(sexs))
            {
                list = list.Where(a => a.Sex == sexs);
            }
            //根据部门来查询
            if (!string.IsNullOrWhiteSpace(depId) && depId != "root")
            {
                list = list.Where(a => a.DepId.Equals(depId));

            }
            if (!string.IsNullOrWhiteSpace(roles))
            {
                var role = m_Rep.Find(O => O.Id.Equals(roles));
                //list = list.Where(a => a.SysRole.Contains(""));

            }
            if (pager.order == "desc")
            {
                if (pager.order == "UserName")
                {
                    query = list.OrderBy(c => c.UserName).Skip((pager.page - 1) * pager.rows).Take(pager.rows).ToList();
                }
                else//createtime
                {
                    query = list.OrderBy(c => c.CreateTime).Skip((pager.page - 1) * pager.rows).Take(pager.rows).ToList();
                }
            }
            else
            {
                if (pager.order == "UserName")
                {
                    query = list.OrderByDescending(c => c.UserName).Skip((pager.page - 1) * pager.rows).Take(pager.rows).ToList();
                }
                else//createtime
                {
                    query = list.OrderByDescending(c => c.CreateTime).Skip((pager.page - 1) * pager.rows).Take(pager.rows).ToList();
                }
            }

            List<SysUserModel> userInfoList = new List<SysUserModel>();
            List<SysUser> dataList = query.ToList();
            foreach (var user in dataList)
            {
                SysUserModel userModel = new SysUserModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Password = user.Password,
                    TrueName = user.TrueName,
                    MobileNumber = user.MobileNumber,
                    PhoneNumber = user.PhoneNumber,
                    QQ = user.QQ,
                    EmailAddress = user.EmailAddress,
                    OtherContact = user.OtherContact,
                    Province = user.Province,
                    City = user.City,
                    Village = user.Village,
                    Address = user.Address,
                    State = user.State,
                    CreateTime = user.CreateTime,
                    CreatePerson = user.CreatePerson,
                    RoleName = GetRefSysRole(user.Id),
                    PosName = user.SysPosition.Name,
                    DepName = user.SysStruct.Name
                };
                userInfoList.Add(userModel);
            }

            return userInfoList;
        }
        #endregion

        #region 根据角色id获取用户列表
        /// <summary>
        /// 根据角色名获取用户列表
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public IQueryable<SysUser> GetUserListByRoleId(string roleid)
        {
            IQueryable<SysUser> _SysUserList = null;
            //获取传入角色名称的角色ID

            if (null != roleid)
            {
                _SysUserList = roleRep.GetRefSysUser(roleid);
            }
            return _SysUserList;

        }
        #endregion

        #region 根据部门性别角色获取用户列表
        /// <summary>
        /// 根据部门性别角色获取用户列表
        /// </summary>
        /// <param name="sexs"></param>
        /// <param name="depId"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        /// 
        public List<SysUserModel> GetList(ref GridPager pager, string queryStr, string sexs, string depId, string roles)
        {
            IQueryable<SysUser> queryData = null;
            if (!string.IsNullOrWhiteSpace(roles))
            {
                queryData = GetUserListByRoleId(roles);
            }
            else
            {
                queryData = m_Rep.GetList();
            }
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = queryData.Where(a => a.UserName.Contains(queryStr)
                                || a.TrueName.Contains(queryStr)
                                || a.MobileNumber.Contains(queryStr)
                                || a.PhoneNumber.Contains(queryStr));
            }

            if (!string.IsNullOrWhiteSpace(depId))
            {
                //求所有子节点ID 包括节点本身
                var DepIds = "";
                List<SysStruct> _SysStructList = _SysStructBLL.m_Rep.FindList().ToList();
                _SysStructBLL.GetDepIds(_SysStructList, depId, ref DepIds);
                queryData = queryData.Where(que => DepIds.Contains(que.DepId));
            }
            if (!string.IsNullOrWhiteSpace(sexs))
            {
                queryData = queryData.Where(que => que.Sex.Equals(sexs));
            }
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
        #endregion

        #region 根据用户ID集合获取用户列表
        /// <summary>
        /// 根据用户ID集合获取用户列表
        /// </summary>
        /// <param name=""></param>
        /// <param name="UserIDs"></param>
        /// <returns></returns>
        public IQueryable<SysUser> GetUserListByUserIds(GridPager pager, string UserIDs)
        {
            IQueryable<SysUser> _SysUserList = null;
            {
                _SysUserList = m_Rep.FindPageList(ref pager, EF => UserIDs.Contains(EF.Id));
            }
            return _SysUserList;
        }
        #endregion

        #region 更新用户模块
        /// <summary>
        /// 更新用户模块
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public bool UpdateSysUserModel(List<UserAppModuleModel> list)
        {
            var flag = true;
            try
            {
                foreach (UserAppModuleModel _UserAppModuleModel in list)
                {
                    var _SysUser = m_Rep.GetById(_UserAppModuleModel.Id);
                    _SysUser.ModificationTime = ResultHelper.NowTime;
                    //_SysUser.Hair = _UserAppModuleModel.Hair;
                    //_SysUser.Food = _UserAppModuleModel.Food;
                    //_SysUser.Maintain = _UserAppModuleModel.Maintain;
                    if (!m_Rep.Edit(_SysUser))
                    {
                        flag = false;
                    }
                }
                return flag;

            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                return false;
            }
        }
        #endregion

        #region 校验用户导入excel表
        /// <summary>
        /// 校验用户导入excel表
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="sysUserList"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public bool CheckImportUser(string filePath, List<SysUser> sysUserList, ref ValidationErrors errors)
        {
            var Structs = _SysStructBLL.m_Rep.FindList().ToList();
            var Poss = _SysPositionBLL.m_Rep.FindList().ToList();
            var Users = m_Rep.FindList().ToList();
            filePath = Utils.GetMapPath(filePath);
            var targetFile = new FileInfo(filePath);
            if (!targetFile.Exists)
            {
                errors.Add("导入的数据文件不存在");
                return false;
            }
            int rowIndex = 1;
            var excelFile = new ExcelQueryFactory(filePath);
            //获取所有的worksheet
            var sheetList = excelFile.GetWorksheetNames();
            //检查数据正确性
            foreach (var sheet in sheetList)
            {
                var errorMessage = new StringBuilder();
                //获得sheet对应的数据
                var data = excelFile.WorksheetNoHeader(sheet).ToList();
                //遍历sheet1下面的数据
                for (int i = 1; i < data.Count; i++)
                {
                    SysStruct Company = new SysStruct();
                    var sysUser = new SysUser();
                    sysUser.Id = ResultHelper.NewId;
                    for (int j = 0; j < data[i].Count; j++)
                    {
                        var dataValue = data[i][j].Value.ToString();
                        switch (j)
                        {
                            case 0:
                                if (string.IsNullOrEmpty(dataValue))
                                {
                                    break;
                                }
                                var _SysUser = Users.Find(EF => EF.UserName != null && EF.UserName.Trim().Equals(dataValue));
                                if (null != _SysUser)
                                {
                                    errorMessage.Append("第" + (i + 1) + "行，用户名已经存在<br/>");
                                    break;
                                }
                                sysUser.UserName = dataValue;
                                break;
                            case 1:
                                sysUser.TrueName = dataValue;
                                break;
                            case 2:
                                Company = Structs.Find(S => S.Name.Trim().Equals(dataValue));
                                if (Company == null)
                                {
                                    errorMessage.Append("第" + (i + 1) + "行，包含系统不存在的公司，请核对<br/>");
                                    break;
                                }
                                break;
                            case 3:
                                //获取公司下的部门，部门允许空
                                if (string.IsNullOrEmpty(dataValue))
                                {
                                    sysUser.DepId = Company.Id;
                                }
                                else
                                {
                                    var Dept = Structs.Find(S => S.Name.Trim() == dataValue && S.ParentId == Company.Id);
                                    if (Dept == null)
                                    {
                                        errorMessage.Append("第" + (i + 1) + "行，包含系统不存在的部门，请核对<br/>");
                                        break;
                                    }
                                    sysUser.DepId = Dept.Id;
                                }
                                break;
                            case 4:
                                var PosId = Poss.Find(S => S.Name.Trim().Equals(dataValue));
                                if (PosId == null)
                                {
                                    errorMessage.Append("第" + (i + 1) + "行，包含系统不存在的职位，请核对<br/>");
                                    break;
                                }
                                sysUser.PosId = PosId.Id;
                                break;
                            case 5:
                                sysUser.Sex = dataValue;
                                break;
                            case 6:
                                //用手机号来校验是否重复用户
                                if (string.IsNullOrEmpty(dataValue))
                                {
                                    errorMessage.Append("第" + (i + 1) + "行，手机号为空<br/>");
                                    break;
                                }
                                _SysUser = Users.Find(EF => EF.MobileNumber != null && EF.MobileNumber.Trim().Equals(dataValue));
                                if (null != _SysUser)
                                {
                                    errorMessage.Append("第" + (i + 1) + "行，手机号已经存在<br/>");
                                    break;
                                }
                                sysUser.MobileNumber = dataValue;
                                break;
                            case 7:
                                sysUser.PhoneNumber = dataValue;
                                break;
                            case 8:
                                DateTime dtBirth = ResultHelper.NowTime;
                                if (string.IsNullOrEmpty(dataValue))
                                {
                                    sysUser.Birthday = null;
                                }
                                else
                                {
                                    try
                                    {
                                        var boolFlag = DateTime.TryParse(dataValue, out dtBirth);
                                        if (boolFlag)
                                        {
                                            sysUser.Birthday = dtBirth;
                                        }
                                        else
                                        {
                                            sysUser.Birthday = null;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sysUser.Birthday = null;
                                        errorMessage.Append("第" + (i + 1) + "行，日期格式不正确，请核对<br/>");
                                    }
                                }
                                break;
                            case 9:
                                sysUser.State = dataValue.Equals("1") ? true : false;
                                break;
                            case 10:
                                sysUser.Password = dataValue;
                                break;
                            case 11:
                                sysUser.JobState = dataValue.Equals("1") ? true : false;
                                break;
                            case 12:
                                sysUser.EmailAddress = dataValue;
                                break;
                            case 13:
                                sysUser.Expertise = dataValue;
                                break;
                                break;
                        }
                    }
                    sysUserList.Add(sysUser);
                }
                //集合错误
                if (errorMessage.Length > 0)
                {
                    errors.Add(string.Format(
                        "在Sheet {0} 发现错误：<br/>{1}{2}",
                        sheet,
                        errorMessage,
                        "<br/>"));
                }
                rowIndex += 1;
            }
            if (errors.Count > 0)
            {
                return false;
            }
            return true;
        }
        #endregion

    }
}
