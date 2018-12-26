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
using Apps.IDAL.QRTZ;
using Apps.Models.QRTZ;
namespace Apps.BLL.QRTZ
{
	public class Virtual_QRTZ_JOB_DETAILSBLL
	{
        [Dependency]
        public IQRTZ_JOB_DETAILSRepository m_Rep { get; set; }

		public virtual List<QRTZ_JOB_DETAILSModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<QRTZ_JOB_DETAILS> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.SCHED_NAME.Contains(queryStr)
								|| a.JOB_NAME.Contains(queryStr)
								|| a.JOB_GROUP.Contains(queryStr)
								|| a.DESCRIPTION.Contains(queryStr)
								|| a.JOB_CLASS_NAME.Contains(queryStr)
								
								
								
								
								
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
        public virtual List<QRTZ_JOB_DETAILSModel> CreateModelList(ref IQueryable<QRTZ_JOB_DETAILS> queryData)
        {

            List<QRTZ_JOB_DETAILSModel> modelList = (from r in queryData
                                              select new QRTZ_JOB_DETAILSModel
                                              {
													SCHED_NAME = r.SCHED_NAME,
													JOB_NAME = r.JOB_NAME,
													JOB_GROUP = r.JOB_GROUP,
													DESCRIPTION = r.DESCRIPTION,
													JOB_CLASS_NAME = r.JOB_CLASS_NAME,
													IS_DURABLE = r.IS_DURABLE,
													IS_NONCONCURRENT = r.IS_NONCONCURRENT,
													IS_UPDATE_DATA = r.IS_UPDATE_DATA,
													REQUESTS_RECOVERY = r.REQUESTS_RECOVERY,
													JOB_DATA = r.JOB_DATA,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, QRTZ_JOB_DETAILSModel model)
        {
            try
            {
                QRTZ_JOB_DETAILS entity = m_Rep.GetById(model.SCHED_NAME);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new QRTZ_JOB_DETAILS();
               				entity.SCHED_NAME = model.SCHED_NAME;
				entity.JOB_NAME = model.JOB_NAME;
				entity.JOB_GROUP = model.JOB_GROUP;
				entity.DESCRIPTION = model.DESCRIPTION;
				entity.JOB_CLASS_NAME = model.JOB_CLASS_NAME;
				entity.IS_DURABLE = model.IS_DURABLE;
				entity.IS_NONCONCURRENT = model.IS_NONCONCURRENT;
				entity.IS_UPDATE_DATA = model.IS_UPDATE_DATA;
				entity.REQUESTS_RECOVERY = model.REQUESTS_RECOVERY;
				entity.JOB_DATA = model.JOB_DATA;
  

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

		
       

        public virtual bool Edit(ref ValidationErrors errors, QRTZ_JOB_DETAILSModel model)
        {
            try
            {
                QRTZ_JOB_DETAILS entity = m_Rep.GetById(model.SCHED_NAME);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.SCHED_NAME = model.SCHED_NAME;
				entity.JOB_NAME = model.JOB_NAME;
				entity.JOB_GROUP = model.JOB_GROUP;
				entity.DESCRIPTION = model.DESCRIPTION;
				entity.JOB_CLASS_NAME = model.JOB_CLASS_NAME;
				entity.IS_DURABLE = model.IS_DURABLE;
				entity.IS_NONCONCURRENT = model.IS_NONCONCURRENT;
				entity.IS_UPDATE_DATA = model.IS_UPDATE_DATA;
				entity.REQUESTS_RECOVERY = model.REQUESTS_RECOVERY;
				entity.JOB_DATA = model.JOB_DATA;
 


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

      

        public virtual QRTZ_JOB_DETAILSModel GetById(string id)
        {
            if (IsExists(id))
            {
                QRTZ_JOB_DETAILS entity = m_Rep.GetById(id);
                QRTZ_JOB_DETAILSModel model = new QRTZ_JOB_DETAILSModel();
                              				model.SCHED_NAME = entity.SCHED_NAME;
				model.JOB_NAME = entity.JOB_NAME;
				model.JOB_GROUP = entity.JOB_GROUP;
				model.DESCRIPTION = entity.DESCRIPTION;
				model.JOB_CLASS_NAME = entity.JOB_CLASS_NAME;
				model.IS_DURABLE = entity.IS_DURABLE;
				model.IS_NONCONCURRENT = entity.IS_NONCONCURRENT;
				model.IS_UPDATE_DATA = entity.IS_UPDATE_DATA;
				model.REQUESTS_RECOVERY = entity.REQUESTS_RECOVERY;
				model.JOB_DATA = entity.JOB_DATA;
 
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
