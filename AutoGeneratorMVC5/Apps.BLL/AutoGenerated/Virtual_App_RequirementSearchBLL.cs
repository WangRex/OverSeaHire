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
using Apps.IDAL.App;
using Apps.Models.App;
namespace Apps.BLL.App
{
	public class Virtual_App_RequirementSearchBLL
	{
        [Dependency]
        public IApp_RequirementSearchRepository m_Rep { get; set; }

		public virtual List<App_RequirementSearchModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<App_RequirementSearch> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.Id.Contains(queryStr)
								
								
								|| a.CreateUserName.Contains(queryStr)
								|| a.ModificationUserName.Contains(queryStr)
								
								|| a.ParentId.Contains(queryStr)
								|| a.Content.Contains(queryStr)
								|| a.PK_App_Customer_CustomerName.Contains(queryStr)
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
        public virtual List<App_RequirementSearchModel> CreateModelList(ref IQueryable<App_RequirementSearch> queryData)
        {

            List<App_RequirementSearchModel> modelList = (from r in queryData
                                              select new App_RequirementSearchModel
                                              {
													Id = r.Id,
													CreateTime = r.CreateTime,
													ModificationTime = r.ModificationTime,
													CreateUserName = r.CreateUserName,
													ModificationUserName = r.ModificationUserName,
													SortCode = r.SortCode,
													ParentId = r.ParentId,
													Content = r.Content,
													PK_App_Customer_CustomerName = r.PK_App_Customer_CustomerName,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, App_RequirementSearchModel model)
        {
            try
            {
                App_RequirementSearch entity = m_Rep.GetById(model.Id);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new App_RequirementSearch();
               				entity.Id = model.Id;
				entity.CreateTime = model.CreateTime;
				entity.ModificationTime = model.ModificationTime;
				entity.CreateUserName = model.CreateUserName;
				entity.ModificationUserName = model.ModificationUserName;
				entity.SortCode = model.SortCode;
				entity.ParentId = model.ParentId;
				entity.Content = model.Content;
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

		
       

        public virtual bool Edit(ref ValidationErrors errors, App_RequirementSearchModel model)
        {
            try
            {
                App_RequirementSearch entity = m_Rep.GetById(model.Id);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.Id = model.Id;
				entity.CreateTime = model.CreateTime;
				entity.ModificationTime = model.ModificationTime;
				entity.CreateUserName = model.CreateUserName;
				entity.ModificationUserName = model.ModificationUserName;
				entity.SortCode = model.SortCode;
				entity.ParentId = model.ParentId;
				entity.Content = model.Content;
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

      

        public virtual App_RequirementSearchModel GetById(string id)
        {
            if (IsExists(id))
            {
                App_RequirementSearch entity = m_Rep.GetById(id);
                App_RequirementSearchModel model = new App_RequirementSearchModel();
                              				model.Id = entity.Id;
				model.CreateTime = entity.CreateTime;
				model.ModificationTime = entity.ModificationTime;
				model.CreateUserName = entity.CreateUserName;
				model.ModificationUserName = entity.ModificationUserName;
				model.SortCode = entity.SortCode;
				model.ParentId = entity.ParentId;
				model.Content = entity.Content;
				model.PK_App_Customer_CustomerName = entity.PK_App_Customer_CustomerName;
 
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
