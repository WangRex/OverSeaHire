//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Apps.Models;
using Apps.Common;
using Microsoft.Practices.Unity;
using System.Transactions;
using Apps.IBLL;
using Apps.IDAL;
using Apps.BLL.Core;
using Apps.Locale;
using Apps.IDAL.Sys;
using Apps.Models.Sys;
namespace Apps.BLL.Sys
{
	public class Virtual_SysModuleOperateBLL
	{
        [Dependency]
        public ISysModuleOperateRepository m_Rep { get; set; }

		public virtual List<SysModuleOperateModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<SysModuleOperate> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.Id.Contains(queryStr)
								|| a.Name.Contains(queryStr)
								|| a.KeyCode.Contains(queryStr)
								|| a.ModuleId.Contains(queryStr)
								
								
								);
            }
            else
            {
                queryData = m_Rep.GetList();
            }
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
        public virtual List<SysModuleOperateModel> CreateModelList(ref IQueryable<SysModuleOperate> queryData)
        {

            List<SysModuleOperateModel> modelList = (from r in queryData
                                              select new SysModuleOperateModel
                                              {
													Id = r.Id,
													Name = r.Name,
													KeyCode = r.KeyCode,
													ModuleId = r.ModuleId,
													IsValid = r.IsValid,
													Sort = r.Sort,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, SysModuleOperateModel model)
        {
            try
            {
                SysModuleOperate entity = m_Rep.GetById(model.Id);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new SysModuleOperate();
               				entity.Id = model.Id;
				entity.Name = model.Name;
				entity.KeyCode = model.KeyCode;
				entity.ModuleId = model.ModuleId;
				entity.IsValid = model.IsValid;
				entity.Sort = model.Sort;
  

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



         public virtual bool Delete(ref ValidationErrors errors, string id)
        {
            try
            {
                if (m_Rep.Delete(id) == 1)
                {
                    return true;
                }
                else
                {
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

        public virtual bool Delete(ref ValidationErrors errors, string[] deleteCollection)
        {
            try
            {
                if (deleteCollection != null)
                {
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        if (m_Rep.Delete(deleteCollection) == deleteCollection.Length)
                        {
                            transactionScope.Complete();
                            return true;
                        }
                        else
                        {
                            Transaction.Current.Rollback();
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }

		
       

        public virtual bool Edit(ref ValidationErrors errors, SysModuleOperateModel model)
        {
            try
            {
                SysModuleOperate entity = m_Rep.GetById(model.Id);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.Id = model.Id;
				entity.Name = model.Name;
				entity.KeyCode = model.KeyCode;
				entity.ModuleId = model.ModuleId;
				entity.IsValid = model.IsValid;
				entity.Sort = model.Sort;
 


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

      

        public virtual SysModuleOperateModel GetById(string id)
        {
            if (IsExists(id))
            {
                SysModuleOperate entity = m_Rep.GetById(id);
                SysModuleOperateModel model = new SysModuleOperateModel();
                              				model.Id = entity.Id;
				model.Name = entity.Name;
				model.KeyCode = entity.KeyCode;
				model.ModuleId = entity.ModuleId;
				model.IsValid = entity.IsValid;
				model.Sort = entity.Sort;
 
                return model;
            }
            else
            {
                return null;
            }
        }

        public virtual bool IsExists(string id)
        {
            return m_Rep.IsExist(id);
        }
		  public void Dispose()
        { 
            
        }

	}
}
