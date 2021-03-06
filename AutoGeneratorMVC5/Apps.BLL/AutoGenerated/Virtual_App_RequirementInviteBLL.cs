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
	public class Virtual_App_RequirementInviteBLL
	{
        [Dependency]
        public IApp_RequirementInviteRepository m_Rep { get; set; }

		public virtual List<App_RequirementInviteModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<App_RequirementInvite> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.Id.Contains(queryStr)
								
								
								|| a.CreateUserName.Contains(queryStr)
								|| a.ModificationUserName.Contains(queryStr)
								
								|| a.ParentId.Contains(queryStr)
								|| a.PK_App_Requirement_Title.Contains(queryStr)
								|| a.InitiatorId.Contains(queryStr)
								|| a.Inviter.Contains(queryStr)
								|| a.SwitchBtnAgree.Contains(queryStr)
								|| a.SwitchBtnContractorAgree.Contains(queryStr)
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
        public virtual List<App_RequirementInviteModel> CreateModelList(ref IQueryable<App_RequirementInvite> queryData)
        {

            List<App_RequirementInviteModel> modelList = (from r in queryData
                                              select new App_RequirementInviteModel
                                              {
													Id = r.Id,
													CreateTime = r.CreateTime,
													ModificationTime = r.ModificationTime,
													CreateUserName = r.CreateUserName,
													ModificationUserName = r.ModificationUserName,
													SortCode = r.SortCode,
													ParentId = r.ParentId,
													PK_App_Requirement_Title = r.PK_App_Requirement_Title,
													InitiatorId = r.InitiatorId,
													Inviter = r.Inviter,
													SwitchBtnAgree = r.SwitchBtnAgree,
													SwitchBtnContractorAgree = r.SwitchBtnContractorAgree,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, App_RequirementInviteModel model)
        {
            try
            {
                App_RequirementInvite entity = m_Rep.GetById(model.Id);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new App_RequirementInvite();
               				entity.Id = model.Id;
				entity.CreateTime = model.CreateTime;
				entity.ModificationTime = model.ModificationTime;
				entity.CreateUserName = model.CreateUserName;
				entity.ModificationUserName = model.ModificationUserName;
				entity.SortCode = model.SortCode;
				entity.ParentId = model.ParentId;
				entity.PK_App_Requirement_Title = model.PK_App_Requirement_Title;
				entity.InitiatorId = model.InitiatorId;
				entity.Inviter = model.Inviter;
				entity.SwitchBtnAgree = model.SwitchBtnAgree;
				entity.SwitchBtnContractorAgree = model.SwitchBtnContractorAgree;
  

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

		
       

        public virtual bool Edit(ref ValidationErrors errors, App_RequirementInviteModel model)
        {
            try
            {
                App_RequirementInvite entity = m_Rep.GetById(model.Id);
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
				entity.PK_App_Requirement_Title = model.PK_App_Requirement_Title;
				entity.InitiatorId = model.InitiatorId;
				entity.Inviter = model.Inviter;
				entity.SwitchBtnAgree = model.SwitchBtnAgree;
				entity.SwitchBtnContractorAgree = model.SwitchBtnContractorAgree;
 


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

      

        public virtual App_RequirementInviteModel GetById(string id)
        {
            if (IsExists(id))
            {
                App_RequirementInvite entity = m_Rep.GetById(id);
                App_RequirementInviteModel model = new App_RequirementInviteModel();
                              				model.Id = entity.Id;
				model.CreateTime = entity.CreateTime;
				model.ModificationTime = entity.ModificationTime;
				model.CreateUserName = entity.CreateUserName;
				model.ModificationUserName = entity.ModificationUserName;
				model.SortCode = entity.SortCode;
				model.ParentId = entity.ParentId;
				model.PK_App_Requirement_Title = entity.PK_App_Requirement_Title;
				model.InitiatorId = entity.InitiatorId;
				model.Inviter = entity.Inviter;
				model.SwitchBtnAgree = entity.SwitchBtnAgree;
				model.SwitchBtnContractorAgree = entity.SwitchBtnContractorAgree;
 
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
