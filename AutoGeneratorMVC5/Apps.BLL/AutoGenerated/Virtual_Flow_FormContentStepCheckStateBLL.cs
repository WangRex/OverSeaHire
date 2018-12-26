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
using Apps.IDAL.Flow;
using Apps.Models.Flow;
namespace Apps.BLL.Flow
{
	public class Virtual_Flow_FormContentStepCheckStateBLL
	{
        [Dependency]
        public IFlow_FormContentStepCheckStateRepository m_Rep { get; set; }

		public virtual List<Flow_FormContentStepCheckStateModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<Flow_FormContentStepCheckState> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.Id.Contains(queryStr)
								|| a.StepCheckId.Contains(queryStr)
								|| a.UserId.Contains(queryStr)
								
								|| a.Reamrk.Contains(queryStr)
								|| a.TheSeal.Contains(queryStr)
								
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
        public virtual List<Flow_FormContentStepCheckStateModel> CreateModelList(ref IQueryable<Flow_FormContentStepCheckState> queryData)
        {

            List<Flow_FormContentStepCheckStateModel> modelList = (from r in queryData
                                              select new Flow_FormContentStepCheckStateModel
                                              {
													Id = r.Id,
													StepCheckId = r.StepCheckId,
													UserId = r.UserId,
													CheckFlag = r.CheckFlag,
													Reamrk = r.Reamrk,
													TheSeal = r.TheSeal,
													CreateTime = r.CreateTime,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, Flow_FormContentStepCheckStateModel model)
        {
            try
            {
                Flow_FormContentStepCheckState entity = m_Rep.GetById(model.Id);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new Flow_FormContentStepCheckState();
               				entity.Id = model.Id;
				entity.StepCheckId = model.StepCheckId;
				entity.UserId = model.UserId;
				entity.CheckFlag = model.CheckFlag;
				entity.Reamrk = model.Reamrk;
				entity.TheSeal = model.TheSeal;
				entity.CreateTime = model.CreateTime;
  

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

		
       

        public virtual bool Edit(ref ValidationErrors errors, Flow_FormContentStepCheckStateModel model)
        {
            try
            {
                Flow_FormContentStepCheckState entity = m_Rep.GetById(model.Id);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.Id = model.Id;
				entity.StepCheckId = model.StepCheckId;
				entity.UserId = model.UserId;
				entity.CheckFlag = model.CheckFlag;
				entity.Reamrk = model.Reamrk;
				entity.TheSeal = model.TheSeal;
				entity.CreateTime = model.CreateTime;
 


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

      

        public virtual Flow_FormContentStepCheckStateModel GetById(string id)
        {
            if (IsExists(id))
            {
                Flow_FormContentStepCheckState entity = m_Rep.GetById(id);
                Flow_FormContentStepCheckStateModel model = new Flow_FormContentStepCheckStateModel();
                              				model.Id = entity.Id;
				model.StepCheckId = entity.StepCheckId;
				model.UserId = entity.UserId;
				model.CheckFlag = entity.CheckFlag;
				model.Reamrk = entity.Reamrk;
				model.TheSeal = entity.TheSeal;
				model.CreateTime = entity.CreateTime;
 
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
