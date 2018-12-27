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
	public class Virtual_SysUserConfigBLL
	{
        [Dependency]
        public ISysUserConfigRepository m_Rep { get; set; }

		public virtual List<SysUserConfigModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<SysUserConfig> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.Id.Contains(queryStr)
								|| a.Name.Contains(queryStr)
								|| a.Value.Contains(queryStr)
								|| a.Type.Contains(queryStr)
								
								|| a.UserId.Contains(queryStr)
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
        public virtual List<SysUserConfigModel> CreateModelList(ref IQueryable<SysUserConfig> queryData)
        {

            List<SysUserConfigModel> modelList = (from r in queryData
                                              select new SysUserConfigModel
                                              {
													Id = r.Id,
													Name = r.Name,
													Value = r.Value,
													Type = r.Type,
													State = r.State,
													UserId = r.UserId,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, SysUserConfigModel model)
        {
            try
            {
                SysUserConfig entity = m_Rep.GetById(model.Id);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new SysUserConfig();
               				entity.Id = model.Id;
				entity.Name = model.Name;
				entity.Value = model.Value;
				entity.Type = model.Type;
				entity.State = model.State;
				entity.UserId = model.UserId;
  

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

		
       

        public virtual bool Edit(ref ValidationErrors errors, SysUserConfigModel model)
        {
            try
            {
                SysUserConfig entity = m_Rep.GetById(model.Id);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.Id = model.Id;
				entity.Name = model.Name;
				entity.Value = model.Value;
				entity.Type = model.Type;
				entity.State = model.State;
				entity.UserId = model.UserId;
 


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

      

        public virtual SysUserConfigModel GetById(string id)
        {
            if (IsExists(id))
            {
                SysUserConfig entity = m_Rep.GetById(id);
                SysUserConfigModel model = new SysUserConfigModel();
                              				model.Id = entity.Id;
				model.Name = entity.Name;
				model.Value = entity.Value;
				model.Type = entity.Type;
				model.State = entity.State;
				model.UserId = entity.UserId;
 
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