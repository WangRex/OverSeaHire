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
	public class Virtual_QRTZ_FIRED_TRIGGERSBLL
	{
        [Dependency]
        public IQRTZ_FIRED_TRIGGERSRepository m_Rep { get; set; }

		public virtual List<QRTZ_FIRED_TRIGGERSModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<QRTZ_FIRED_TRIGGERS> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.SCHED_NAME.Contains(queryStr)
								|| a.ENTRY_ID.Contains(queryStr)
								|| a.TRIGGER_NAME.Contains(queryStr)
								|| a.TRIGGER_GROUP.Contains(queryStr)
								|| a.INSTANCE_NAME.Contains(queryStr)
								
								
								
								|| a.STATE.Contains(queryStr)
								|| a.JOB_NAME.Contains(queryStr)
								|| a.JOB_GROUP.Contains(queryStr)
								
								
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
        public virtual List<QRTZ_FIRED_TRIGGERSModel> CreateModelList(ref IQueryable<QRTZ_FIRED_TRIGGERS> queryData)
        {

            List<QRTZ_FIRED_TRIGGERSModel> modelList = (from r in queryData
                                              select new QRTZ_FIRED_TRIGGERSModel
                                              {
													SCHED_NAME = r.SCHED_NAME,
													ENTRY_ID = r.ENTRY_ID,
													TRIGGER_NAME = r.TRIGGER_NAME,
													TRIGGER_GROUP = r.TRIGGER_GROUP,
													INSTANCE_NAME = r.INSTANCE_NAME,
													FIRED_TIME = r.FIRED_TIME,
													SCHED_TIME = r.SCHED_TIME,
													PRIORITY = r.PRIORITY,
													STATE = r.STATE,
													JOB_NAME = r.JOB_NAME,
													JOB_GROUP = r.JOB_GROUP,
													IS_NONCONCURRENT = r.IS_NONCONCURRENT,
													REQUESTS_RECOVERY = r.REQUESTS_RECOVERY,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, QRTZ_FIRED_TRIGGERSModel model)
        {
            try
            {
                QRTZ_FIRED_TRIGGERS entity = m_Rep.GetById(model.SCHED_NAME);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new QRTZ_FIRED_TRIGGERS();
               				entity.SCHED_NAME = model.SCHED_NAME;
				entity.ENTRY_ID = model.ENTRY_ID;
				entity.TRIGGER_NAME = model.TRIGGER_NAME;
				entity.TRIGGER_GROUP = model.TRIGGER_GROUP;
				entity.INSTANCE_NAME = model.INSTANCE_NAME;
				entity.FIRED_TIME = model.FIRED_TIME;
				entity.SCHED_TIME = model.SCHED_TIME;
				entity.PRIORITY = model.PRIORITY;
				entity.STATE = model.STATE;
				entity.JOB_NAME = model.JOB_NAME;
				entity.JOB_GROUP = model.JOB_GROUP;
				entity.IS_NONCONCURRENT = model.IS_NONCONCURRENT;
				entity.REQUESTS_RECOVERY = model.REQUESTS_RECOVERY;
  

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

		
       

        public virtual bool Edit(ref ValidationErrors errors, QRTZ_FIRED_TRIGGERSModel model)
        {
            try
            {
                QRTZ_FIRED_TRIGGERS entity = m_Rep.GetById(model.SCHED_NAME);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.SCHED_NAME = model.SCHED_NAME;
				entity.ENTRY_ID = model.ENTRY_ID;
				entity.TRIGGER_NAME = model.TRIGGER_NAME;
				entity.TRIGGER_GROUP = model.TRIGGER_GROUP;
				entity.INSTANCE_NAME = model.INSTANCE_NAME;
				entity.FIRED_TIME = model.FIRED_TIME;
				entity.SCHED_TIME = model.SCHED_TIME;
				entity.PRIORITY = model.PRIORITY;
				entity.STATE = model.STATE;
				entity.JOB_NAME = model.JOB_NAME;
				entity.JOB_GROUP = model.JOB_GROUP;
				entity.IS_NONCONCURRENT = model.IS_NONCONCURRENT;
				entity.REQUESTS_RECOVERY = model.REQUESTS_RECOVERY;
 


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

      

        public virtual QRTZ_FIRED_TRIGGERSModel GetById(string id)
        {
            if (IsExists(id))
            {
                QRTZ_FIRED_TRIGGERS entity = m_Rep.GetById(id);
                QRTZ_FIRED_TRIGGERSModel model = new QRTZ_FIRED_TRIGGERSModel();
                              				model.SCHED_NAME = entity.SCHED_NAME;
				model.ENTRY_ID = entity.ENTRY_ID;
				model.TRIGGER_NAME = entity.TRIGGER_NAME;
				model.TRIGGER_GROUP = entity.TRIGGER_GROUP;
				model.INSTANCE_NAME = entity.INSTANCE_NAME;
				model.FIRED_TIME = entity.FIRED_TIME;
				model.SCHED_TIME = entity.SCHED_TIME;
				model.PRIORITY = entity.PRIORITY;
				model.STATE = entity.STATE;
				model.JOB_NAME = entity.JOB_NAME;
				model.JOB_GROUP = entity.JOB_GROUP;
				model.IS_NONCONCURRENT = entity.IS_NONCONCURRENT;
				model.REQUESTS_RECOVERY = entity.REQUESTS_RECOVERY;
 
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
