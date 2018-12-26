using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.Common;
using Apps.Models.Sys;
namespace Apps.BLL.Sys
{
    public partial class SysStructBLL
    {
        [Dependency]
        public SysRoleBLL _SysRoleBLL { get; set; }

        public List<SysStructModel> GetList(string parentId)
        {
            IQueryable<SysStruct> queryData = null;
            queryData = m_Rep.GetList(a => a.ParentId == parentId).OrderBy(a => a.Sort);
            return CreateModelList(ref queryData);
        }

        public List<SysStructModel> GetListById(string id, string DepId, string RoleId)
        {
            IQueryable<SysStruct> queryData = null;
            queryData = m_Rep.GetList(a => a.ParentId == id).OrderBy(a => a.Sort);
            if (!_SysRoleBLL.ToBeCheckAuthorityRole(RoleId, "超级管理员") && !_SysRoleBLL.ToBeCheckAuthorityRole(RoleId, "总账号"))
            {
                //获取当前用户的公司
                var CompanyId = GetCompanyId(DepId);
                List<SysStruct> list = m_Rep.FindList().ToList();
                string DepIds = "root";
                GetDepIds(list, CompanyId, ref DepIds);
                queryData = queryData.Where(EF => DepIds.Contains(EF.Id));
            }
            return CreateModelList(ref queryData);
        }

        public override List<SysStructModel> CreateModelList(ref IQueryable<SysStruct> queryData)
        {

            List<SysStructModel> modelList = (from r in queryData
                                              select new SysStructModel
                                              {
                                                  Id = r.Id,
                                                  Name = r.Name,
                                                  ParentId = r.ParentId,
                                                  Sort = r.Sort,
                                                  Enable = r.Enable,
                                                  CreateTime = r.CreateTime,
                                                  Higher = r.Higher,
                                                  Remark = r.Remark,
                                                  Type = "group"
                                              }).ToList();
            return modelList;
        }

        #region 判断传入部门ID是否是名称对应的部门
        /// <summary>
        /// 判断传入部门ID是否是名称对应的部门
        /// </summary>
        /// <param name="depId">部门ID</param>
        /// <param name="depName">部门名称</param>
        /// <returns></returns>
        public bool ToBeCheckAuthority(string depId, string depName)
        {
            //获取传入部门名称的部门ID
            string strDepId = m_Rep.Find(d => d.Name == depName).Id.Clone().ToString();
            if (depId.Equals(strDepId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 拼接部门名字
        /// <summary>
        /// 拼接部门名字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        public string GetFullDepName(string id)
        {
            string _DepName = "";
            var dept = GetById(id);
            if (null == dept)
            {
                return _DepName;
            }
            string ParentId = dept.ParentId;
            if ("root".Equals(ParentId))
            {
                _DepName = "";
            }
            else
            {
                _DepName = dept.Name;
                while (!"root".Equals(ParentId))
                {
                    dept = GetById(dept.ParentId);
                    if (null != dept)
                    {
                        if (!dept.ParentId.Equals("root"))
                        {
                            _DepName = dept.Name + "-" + _DepName;
                        }
                        ParentId = dept.ParentId;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return _DepName;
        }
        #endregion

        #region 获取公司名称
        /// <summary>
        /// 获取公司名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        public string GetCompanyName(string id)
        {
            var dept = GetById(id);
            string _DepName = "", ParentId = dept.ParentId;
            if ("root".Equals(ParentId))
            {
                _DepName = dept.Name;
            }
            else
            {
                while (!"root".Equals(ParentId))
                {
                    dept = GetById(dept.ParentId);
                    if (null != dept)
                    {
                        //如果当前部门的parentID是root，就说明是公司级别了
                        if (dept.ParentId.Equals("root"))
                        {
                            _DepName = dept.Name;
                        }
                        ParentId = dept.ParentId;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return _DepName;
        }
        #endregion

        #region 获取公司主键
        /// <summary>
        /// 获取公司主键
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        public string GetCompanyId(string id)
        {
            var dept = GetById(id);
            string _CompanyId = "", ParentId = dept.ParentId;
            if ("root".Equals(ParentId))
            {
                _CompanyId = dept.Id;
            }
            else
            {
                while (!"root".Equals(ParentId))
                {
                    dept = GetById(dept.ParentId);
                    if (null != dept)
                    {
                        //如果当前部门的parentID是root，就说明是公司级别了
                        if (dept.ParentId.Equals("root"))
                        {
                            _CompanyId = dept.Id;
                        }
                        ParentId = dept.ParentId;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return _CompanyId;
        }
        #endregion

        #region 获取公司名和部门名，逗号分隔
        /// <summary>
        /// 获取公司名和部门名，逗号分隔
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        public string GetCompanyDepName(string id)
        {
            string CompanyDepName = "", CompanyName = "", DepName = "";
            var dept = GetById(id);
            string ParentId = dept.ParentId;
            if ("root".Equals(ParentId))
            {
                //如果当前部门父ID是root，则直接是公司，返回即可
                CompanyName = dept.Name;
            }
            else
            {
                while (!"root".Equals(ParentId))
                {
                    DepName = dept.Name;
                    dept = GetById(dept.ParentId);
                    if (null != dept)
                    {
                        //如果当前部门的parentID是root，就说明是公司级别了
                        if (dept.ParentId.Equals("root"))
                        {
                            CompanyName = dept.Name;
                        }
                        else
                        {
                            DepName = dept.Name + "-" + DepName;
                        }
                        ParentId = dept.ParentId;
                    }
                    else
                    {
                        break;
                    }
                }

            }
            CompanyDepName = CompanyName + "," + DepName;
            return CompanyDepName;
        }
        #endregion

        #region 获取部门名称
        /// <summary>
        /// 获取部门名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        public string GetDeptName(string id)
        {
            string DepName = "";
            var dept = GetById(id);
            if (null != dept)
            {
                DepName = dept.Name;
            }
            return DepName;
        }
        #endregion

        #region 获取部门ID下属所有部门ID
        /// <summary>
        /// 获取部门ID下属所有部门ID
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id"></param>
        /// <param name="DepIds"></param>
        /// <returns></returns>
        public void GetDepIds(List<SysStruct> list, string id, ref string DepIds)
        {
            if (list == null)
                return;
            List<SysStruct> sublist;
            DepIds += "," + id;
            if (!string.IsNullOrWhiteSpace(id))
            {
                sublist = list.Where(t => t.ParentId == id).ToList();
            }
            else
            {
                sublist = list.Where(t => string.IsNullOrWhiteSpace(t.ParentId)).ToList();
            }
            if (!sublist.Any())
                return;
            foreach (var item in sublist)
            {
                DepIds += "," + item.Id;
                GetDepIds(list, item.Id, ref DepIds);
            }
        }
        #endregion

        #region 判断是否是公司
        /// <summary>
        /// 判断是否是公司
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckIsCom(string id)
        {
            bool flag = false;
            var dept = GetById(id);
            string ParentId = dept.ParentId;
            if ("root".Equals(ParentId))
            {
                flag = true;
            }
            return flag;
        }
        #endregion
    }
}