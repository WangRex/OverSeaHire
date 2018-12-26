using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.Common;
using System.Transactions;
using Apps.Models.Sys;
using Apps.IBLL;
using Apps.IDAL;
using Apps.BLL.Core;
using Apps.Locale;

namespace Apps.BLL.Sys
{
    public partial class SysRoleBLL
    {
        #region 重写转换方法
        /// <summary>
        /// 重写转换方法
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        public override List<SysRoleModel> CreateModelList(ref IQueryable<SysRole> queryData)
        {
            List<SysRoleModel> modelList = new List<SysRoleModel>();
            foreach (var r in queryData)
            {
                modelList.Add(new SysRoleModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    CreateTime = r.CreateTime,
                    CreatePerson = r.CreatePerson,
                    UserName = GetRefSysUser(r.Id)
                });
            }
            return modelList;
        }
        #endregion

        #region 重写创建
        /// <summary>
        /// 重写创建
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public override bool Create(ref ValidationErrors errors, SysRoleModel model)
        {
            try
            {
                SysRole entity = m_Rep.GetById(model.Id);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new SysRole();
                entity.Id = model.Id;
                entity.Name = model.Name;
                entity.RoleCode = model.RoleCode;
                entity.Description = model.Description;
                entity.CreateTime = model.CreateTime;
                entity.CreatePerson = model.CreatePerson;
                entity.ModificationTime = model.ModificationTime;
                entity.ModificationPerson = model.ModificationPerson;
                if (m_Rep.Create(entity))
                {
                    //分配给角色
                    m_Rep.P_Sys_InsertSysRight();
                    //清理无用的项
                    m_Rep.P_Sys_ClearUnusedRightOperate();
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

        #region 获取角色对应的所有用户
        /// <summary>
        /// 获取角色对应的所有用户
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns></returns>
        public string GetRefSysUser(string roleId)
        {
            string UserName = "";
            var userList = m_Rep.GetRefSysUser(roleId);
            if (userList != null)
            {
                foreach (var user in userList)
                {
                    UserName += "[" + user.UserName + "] ";
                }
            }
            return UserName;
        }
        #endregion
        #region 获取角色对应的所有用户
        /// <summary>
        /// 获取角色对应的所有用户
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns></returns>
        public string GetRefSysRole(string UserId)
        {
            string RoleName = "";
            var userList = m_Rep.GetRefSysRole(UserId);
            if (userList != null)
            {
                foreach (var user in userList)
                {
                    RoleName += "[" + user.Name + "] ";
                }
            }
            return RoleName;
        }
        #endregion

        #region 获取角色名对应主键
        /// <summary>
        /// 获取角色名对应主键
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public string GetRoleIdByName(string roleName)
        {
            string _RoleId = string.Empty;
            var _SysRole = m_Rep.Find(SR => SR.Name.Equals(roleName));
            if (null != _SysRole)
            {
                _RoleId = _SysRole.Id;
            }
            return _RoleId;
        }
        #endregion

        #region 获取角色对应的所有用户
        /// <summary>
        /// 获取角色对应的所有用户完整信息
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns></returns>
        public IQueryable<SysUser> GetRefSysUserAll(string roleId)
        {
            var userList = m_Rep.GetRefSysUser(roleId);
            return userList;
        }
        #endregion

        #region 根据角色ID获取用户
        /// <summary>
        /// 根据角色ID获取用户
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="roleId"></param>
        /// <param name="depId"></param>
        /// <param name="queryStr"></param>
        /// <returns></returns>
        public IQueryable<P_Sys_GetUserByRoleId_Result> GetUserByRoleId(ref GridPager pager, string roleId, string depId, string queryStr)
        {
            IQueryable<P_Sys_GetUserByRoleId_Result> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetUserByRoleId(roleId, depId).Where(a => a.TrueName.Contains(queryStr));
                pager.totalRows = queryData.Count();
                queryData = m_Rep.GetUserByRoleId(roleId, depId).Where(a => a.TrueName.Contains(queryStr));
            }
            else
            {
                queryData = m_Rep.GetUserByRoleId(roleId, depId);
                pager.totalRows = queryData.Count();
                queryData = m_Rep.GetUserByRoleId(roleId, depId);
            }

            return queryData.Skip((pager.page - 1) * pager.rows).Take(pager.rows);
        }
        #endregion

        #region 更新用户角色
        /// <summary>
        /// 更新用户角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public bool UpdateSysRoleSysUser(string roleId, string[] userIds)
        {
            try
            {
                m_Rep.UpdateSysRoleSysUser(roleId, userIds);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                return false;
            }
        }
        #endregion

        #region 判断传入角色ID是否包含Code对应的角色
        /// <summary>
        /// 判断传入角色ID是否包含Code对应的角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="roleCode">角色名称</param>
        /// <returns></returns>
        public bool ToBeCheckAuthorityRoleCode(string roleId, string roleCode)
        {
            //获取传入角色名称的角色ID
            SysRole _SysRole = m_Rep.Find(d => d.RoleCode == roleCode);
            if (null == _SysRole)
            {
                return false;
            }
            string strRoleId = _SysRole.Id;
            if (roleId.Contains(strRoleId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 判断传入角色ID是否包含名称对应的角色
        /// <summary>
        /// 判断传入角色ID是否包含名称对应的角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public bool ToBeCheckAuthorityRole(string roleId, string roleName)
        {
            //获取传入角色名称的角色ID
            SysRole _SysRole = m_Rep.Find(d => d.Name == roleName);
            if (null == _SysRole)
            {
                return false;
            }
            string strRoleId = _SysRole.Id;
            if (roleId.Contains(strRoleId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

    }
}




