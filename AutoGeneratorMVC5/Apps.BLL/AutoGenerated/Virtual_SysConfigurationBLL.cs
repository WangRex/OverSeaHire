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
	public class Virtual_SysConfigurationBLL
	{
        [Dependency]
        public ISysConfigurationRepository m_Rep { get; set; }

		public virtual List<SysConfigurationModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<SysConfiguration> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.Id.Contains(queryStr)
								
								
								|| a.CreateUserName.Contains(queryStr)
								|| a.ModificationUserName.Contains(queryStr)
								
								|| a.ParentId.Contains(queryStr)
								|| a.Tel.Contains(queryStr)
								|| a.WebName.Contains(queryStr)
								|| a.ComName.Contains(queryStr)
								|| a.CompanyName.Contains(queryStr)
								|| a.ContactName.Contains(queryStr)
								|| a.ContactPhone.Contains(queryStr)
								|| a.ContactWeChat.Contains(queryStr)
								|| a.ContactMail.Contains(queryStr)
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
        public virtual List<SysConfigurationModel> CreateModelList(ref IQueryable<SysConfiguration> queryData)
        {

            List<SysConfigurationModel> modelList = (from r in queryData
                                              select new SysConfigurationModel
                                              {
													Id = r.Id,
													CreateTime = r.CreateTime,
													ModificationTime = r.ModificationTime,
													CreateUserName = r.CreateUserName,
													ModificationUserName = r.ModificationUserName,
													SortCode = r.SortCode,
													ParentId = r.ParentId,
													Tel = r.Tel,
													WebName = r.WebName,
													ComName = r.ComName,
													CompanyName = r.CompanyName,
													ContactName = r.ContactName,
													ContactPhone = r.ContactPhone,
													ContactWeChat = r.ContactWeChat,
													ContactMail = r.ContactMail,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, SysConfigurationModel model)
        {
            try
            {
                SysConfiguration entity = m_Rep.GetById(model.Id);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new SysConfiguration();
               				entity.Id = model.Id;
				entity.CreateTime = model.CreateTime;
				entity.ModificationTime = model.ModificationTime;
				entity.CreateUserName = model.CreateUserName;
				entity.ModificationUserName = model.ModificationUserName;
				entity.SortCode = model.SortCode;
				entity.ParentId = model.ParentId;
				entity.Tel = model.Tel;
				entity.WebName = model.WebName;
				entity.ComName = model.ComName;
				entity.CompanyName = model.CompanyName;
				entity.ContactName = model.ContactName;
				entity.ContactPhone = model.ContactPhone;
				entity.ContactWeChat = model.ContactWeChat;
				entity.ContactMail = model.ContactMail;
  

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

		
       

        public virtual bool Edit(ref ValidationErrors errors, SysConfigurationModel model)
        {
            try
            {
                SysConfiguration entity = m_Rep.GetById(model.Id);
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
				entity.Tel = model.Tel;
				entity.WebName = model.WebName;
				entity.ComName = model.ComName;
				entity.CompanyName = model.CompanyName;
				entity.ContactName = model.ContactName;
				entity.ContactPhone = model.ContactPhone;
				entity.ContactWeChat = model.ContactWeChat;
				entity.ContactMail = model.ContactMail;
 


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

      

        public virtual SysConfigurationModel GetById(string id)
        {
            if (IsExists(id))
            {
                SysConfiguration entity = m_Rep.GetById(id);
                SysConfigurationModel model = new SysConfigurationModel();
                              				model.Id = entity.Id;
				model.CreateTime = entity.CreateTime;
				model.ModificationTime = entity.ModificationTime;
				model.CreateUserName = entity.CreateUserName;
				model.ModificationUserName = entity.ModificationUserName;
				model.SortCode = entity.SortCode;
				model.ParentId = entity.ParentId;
				model.Tel = entity.Tel;
				model.WebName = entity.WebName;
				model.ComName = entity.ComName;
				model.CompanyName = entity.CompanyName;
				model.ContactName = entity.ContactName;
				model.ContactPhone = entity.ContactPhone;
				model.ContactWeChat = entity.ContactWeChat;
				model.ContactMail = entity.ContactMail;
 
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
