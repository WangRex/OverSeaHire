
using System.Collections.Generic;
using Apps.Models.Sys;

namespace Apps.IBLL.Sys
{
    public partial interface ISysStructBLL
    {

        List<SysStructModel> GetList(string parentId);

        /// <summary>
        /// 根据部门ID获取部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<SysStructModel> GetListById(string id, string DepId, string RoleId);

        /// <summary>
        /// 根据部门ID获取部门全名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetFullDepName(string id);

        /// <summary>
        /// 获取公司名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetCompanyName(string id);

        /// <summary>
        /// 获取公司主键
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetCompanyId(string id);

        /// <summary>
        /// 获取公司名和部门名，逗号分隔
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetCompanyDepName(string id);

        /// <summary>
        /// 获取部门名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetDeptName(string id);

        /// <summary>
        /// 判断是否是公司级别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool CheckIsCom(string id);

    }
}