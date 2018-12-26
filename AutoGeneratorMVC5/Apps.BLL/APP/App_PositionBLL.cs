using Apps.Common;
using Apps.Models;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using Apps.Models.App;

namespace Apps.BLL.App
{
    public partial class App_PositionBLL
    {

        #region 获取职位名称
        /// <summary>
        /// 获取职位名称
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetName(string ID)
        {
            var Name = string.Empty;
            var _App_PositionModel = GetById(ID);
            if (null != _App_PositionModel)
            {
                Name = _App_PositionModel.Name;
            }
            return Name;
        }
        #endregion

        #region 获取职位树列表
        /// <summary>
        /// 获取职位树列表
        /// </summary>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public List<PositionTreeVm> GetAppPositions(string PosIds, ref string ErrorMsg)
        {
            var positionTreeVms = new List<PositionTreeVm>();
            var app_Positions = m_Rep.GetList(EF => EF.ParentId == "0").OrderBy(EF => EF.SortCode).ThenByDescending(EF => EF.CreateTime).ToList();
            if (!string.IsNullOrEmpty(PosIds))
            {
                app_Positions = app_Positions.Where(EF => PosIds.Contains(EF.Id)).ToList();
            }
            foreach (var item in app_Positions)
            {
                var positionTreeVm = new PositionTreeVm
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    positionTreeVms = new List<PositionTreeVm>()
                };
                positionTreeVms.Add(positionTreeVm);
            }
            foreach (var item in positionTreeVms)
            {
                item.positionTreeVms.AddRange(GetChildren(item, PosIds));
            }
            if (positionTreeVms.Count == 0)
            {
                ErrorMsg = "职位列表为空";
                return null;
            }
            else
            {
                return positionTreeVms;
            }
        }
        #endregion

        #region 递归获取子节点(内部方法)
        /// <summary>
        /// 递归获取子节点
        /// </summary>
        /// <param name="sysStructTree"></param>
        /// <returns></returns>
        private List<PositionTreeVm> GetChildren(PositionTreeVm positionTreeVm, string PosIds)
        {
            List<PositionTreeVm> childrens = new List<PositionTreeVm>();
            var app_Positions = m_Rep.GetList(EF => EF.ParentId == positionTreeVm.Id).OrderBy(EF => EF.SortCode).ThenByDescending(EF => EF.CreateTime).ToList();
            if (!string.IsNullOrEmpty(PosIds))
            {
                app_Positions = app_Positions.Where(EF => PosIds.Contains(EF.Id)).ToList();
            }
            foreach (var item in app_Positions)
            {
                var positionTree = new PositionTreeVm()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    positionTreeVms = new List<PositionTreeVm>()
                };
                childrens.Add(positionTree);
            }
            foreach (PositionTreeVm item in childrens)
            {
                item.positionTreeVms.AddRange(GetChildren(item, PosIds));
            }
            return childrens;
        }
        #endregion

        #region 根据父ID获取树
        /// <summary>
        /// 根据父ID获取树
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<App_PositionModel> GetList(string parentId)
        {
            IQueryable<App_Position> queryData = null;
            queryData = m_Rep.GetList(a => a.ParentId == parentId).OrderBy(a => a.SortCode);
            return CreateModelList(ref queryData);
        }
        #endregion

        #region 获取父亲节点主键集合
        /// <summary>
        /// 获取父亲节点主键集合
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public List<string> GetParentIds(string Ids)
        {
            List<string> AllIds = new List<string>();
            var positions = m_Rep.FindList().ToList();
            var positionChildren = positions.Where(EF => Ids.Contains(EF.Id)).ToList();
            foreach (var item in positionChildren)
            {
                AllIds.Add(item.Id);
                var parents = positions.Where(EF => EF.Id == item.ParentId).ToList();
                foreach (var itemparent in parents)
                {
                    AllIds.Add(itemparent.Id);
                    var pps = positions.Where(EF => EF.Id == itemparent.ParentId).ToList();
                    foreach (var itemp in pps)
                    {
                        AllIds.Add(itemp.Id);
                    }
                }
            }
            return AllIds;
        }
        #endregion

        #region 获取职位列表
        public List<App_PositionVm> GetListByParentId(string id)
        {
            List<App_PositionVm> positionVms = new List<App_PositionVm>();
            var app_Positions = m_Rep.GetList(EF => EF.ParentId == id).OrderBy(EF => EF.SortCode).ThenByDescending(EF => EF.CreateTime).ToList();
            foreach (var item in app_Positions)
            {
                var positionTreeVm = new App_PositionVm
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                };
                positionVms.Add(positionTreeVm);
            }
            return positionVms;
        }
        #endregion

        #region 获取多个职位名称
        /// <summary>
        /// 获取多个职位名称
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public string GetNames(string Ids)
        {
            var Names = string.Empty;
            if (!string.IsNullOrEmpty(Ids))
            {
                var app_Positions = m_Rep.FindList(EF => Ids.Contains(EF.Id)).ToList();
                if (null != app_Positions)
                {
                    Names = string.Join(",", app_Positions.Select(EF => EF.Name).ToArray());
                }
            }
            return Names;
        }
        #endregion

        #region 获取常用职位树
        /// <summary>
        /// 获取常用职位树
        /// </summary>
        /// <param name="PosIds"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public List<PositionTreeVm> GetCommonUseAppPositions(string PosIds, ref string ErrorMsg)
        {
            var positionTreeVms = new List<PositionTreeVm>();
            var app_Positions = m_Rep.GetList(EF => EF.ParentId == "0" && EF.SwitchBtnCommonUse == "1").OrderBy(EF => EF.SortCode).ThenByDescending(EF => EF.CreateTime).ToList();
            if (!string.IsNullOrEmpty(PosIds))
            {
                app_Positions = app_Positions.Where(EF => PosIds.Contains(EF.Id)).ToList();
            }
            foreach (var item in app_Positions)
            {
                var positionTreeVm = new PositionTreeVm
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    positionTreeVms = new List<PositionTreeVm>()
                };
                positionTreeVms.Add(positionTreeVm);
            }
            foreach (var item in positionTreeVms)
            {
                item.positionTreeVms.AddRange(GetChildren(item, PosIds));
            }
            if (positionTreeVms.Count == 0)
            {
                ErrorMsg = "职位列表为空";
                return null;
            }
            else
            {
                return positionTreeVms;
            }
        }
        #endregion

        #region 获取职位树ComboTree
        /// <summary>
        /// 获取职位树ComboTree
        /// </summary>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public List<PositionComboTreeVm> GetAppPositionComboTree()
        {
            var positionComboTrees = new List<PositionComboTreeVm>();
            var app_Positions = m_Rep.GetList(EF => EF.ParentId == "0").OrderBy(EF => EF.SortCode).ThenByDescending(EF => EF.CreateTime).ToList();
            foreach (var item in app_Positions)
            {
                var positionTreeVm = new PositionComboTreeVm
                {
                    id = item.Id,
                    text = item.Name,
                    children = new List<PositionComboTreeVm>()
                };
                positionComboTrees.Add(positionTreeVm);
            }
            foreach (var item in positionComboTrees)
            {
                item.children.AddRange(GetComboTreeChildren(item));
            }
            if (positionComboTrees.Count == 0)
            {
                return null;
            }
            else
            {
                return positionComboTrees;
            }
        }
        #endregion

        #region 递归获取子节点(内部方法)
        /// <summary>
        /// 递归获取子节点
        /// </summary>
        /// <param name="sysStructTree"></param>
        /// <returns></returns>
        private List<PositionComboTreeVm> GetComboTreeChildren(PositionComboTreeVm positionComboTreeVm)
        {
            List<PositionComboTreeVm> children = new List<PositionComboTreeVm>();
            var app_Positions = m_Rep.GetList(EF => EF.ParentId == positionComboTreeVm.id).OrderBy(EF => EF.SortCode).ThenByDescending(EF => EF.CreateTime).ToList();
            foreach (var item in app_Positions)
            {
                var positionTree = new PositionComboTreeVm()
                {
                    id = item.Id,
                    text = item.Name,
                    children = new List<PositionComboTreeVm>()
                };
                children.Add(positionTree);
            }
            foreach (PositionComboTreeVm item in children)
            {
                item.children.AddRange(GetComboTreeChildren(item));
            }
            return children;
        }
        #endregion
    }
}

