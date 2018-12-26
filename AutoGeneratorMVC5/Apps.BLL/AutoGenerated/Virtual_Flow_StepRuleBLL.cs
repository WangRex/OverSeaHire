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
	public class Virtual_Flow_StepRuleBLL
	{
        [Dependency]
        public IFlow_StepRuleRepository m_Rep { get; set; }

		public virtual List<Flow_StepRuleModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<Flow_StepRule> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.Id.Contains(queryStr)
								|| a.StepId.Contains(queryStr)
								|| a.AttrId.Contains(queryStr)
								|| a.Operator.Contains(queryStr)
								|| a.Result.Contains(queryStr)
								|| a.NextStep.Contains(queryStr)
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
        public virtual List<Flow_StepRuleModel> CreateModelList(ref IQueryable<Flow_StepRule> queryData)
        {

            List<Flow_StepRuleModel> modelList = (from r in queryData
                                              select new Flow_StepRuleModel
                                              {
													Id = r.Id,
													StepId = r.StepId,
													AttrId = r.AttrId,
													Operator = r.Operator,
													Result = r.Result,
													NextStep = r.NextStep,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, Flow_StepRuleModel model)
        {
            try
            {
                Flow_StepRule entity = m_Rep.GetById(model.Id);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new Flow_StepRule();
               				entity.Id = model.Id;
				entity.StepId = model.StepId;
				entity.AttrId = model.AttrId;
				entity.Operator = model.Operator;
				entity.Result = model.Result;
				entity.NextStep = model.NextStep;
  

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

		
       

        public virtual bool Edit(ref ValidationErrors errors, Flow_StepRuleModel model)
        {
            try
            {
                Flow_StepRule entity = m_Rep.GetById(model.Id);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.Id = model.Id;
				entity.StepId = model.StepId;
				entity.AttrId = model.AttrId;
				entity.Operator = model.Operator;
				entity.Result = model.Result;
				entity.NextStep = model.NextStep;
 


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

      

        public virtual Flow_StepRuleModel GetById(string id)
        {
            if (IsExists(id))
            {
                Flow_StepRule entity = m_Rep.GetById(id);
                Flow_StepRuleModel model = new Flow_StepRuleModel();
                              				model.Id = entity.Id;
				model.StepId = entity.StepId;
				model.AttrId = entity.AttrId;
				model.Operator = entity.Operator;
				model.Result = entity.Result;
				model.NextStep = entity.NextStep;
 
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
