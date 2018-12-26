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
	public class Virtual_QRTZ_SIMPLE_TRIGGERSBLL
	{
        [Dependency]
        public IQRTZ_SIMPLE_TRIGGERSRepository m_Rep { get; set; }

		public virtual List<QRTZ_SIMPLE_TRIGGERSModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<QRTZ_SIMPLE_TRIGGERS> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.SCHED_NAME.Contains(queryStr)
								|| a.TRIGGER_NAME.Contains(queryStr)
								|| a.TRIGGER_GROUP.Contains(queryStr)
								
								
								
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
        public virtual List<QRTZ_SIMPLE_TRIGGERSModel> CreateModelList(ref IQueryable<QRTZ_SIMPLE_TRIGGERS> queryData)
        {

            List<QRTZ_SIMPLE_TRIGGERSModel> modelList = (from r in queryData
                                              select new QRTZ_SIMPLE_TRIGGERSModel
                                              {
													SCHED_NAME = r.SCHED_NAME,
													TRIGGER_NAME = r.TRIGGER_NAME,
													TRIGGER_GROUP = r.TRIGGER_GROUP,
													REPEAT_COUNT = r.REPEAT_COUNT,
													REPEAT_INTERVAL = r.REPEAT_INTERVAL,
													TIMES_TRIGGERED = r.TIMES_TRIGGERED,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, QRTZ_SIMPLE_TRIGGERSModel model)
        {
            try
            {
                QRTZ_SIMPLE_TRIGGERS entity = m_Rep.GetById(model.SCHED_NAME);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new QRTZ_SIMPLE_TRIGGERS();
               				entity.SCHED_NAME = model.SCHED_NAME;
				entity.TRIGGER_NAME = model.TRIGGER_NAME;
				entity.TRIGGER_GROUP = model.TRIGGER_GROUP;
				entity.REPEAT_COUNT = model.REPEAT_COUNT;
				entity.REPEAT_INTERVAL = model.REPEAT_INTERVAL;
				entity.TIMES_TRIGGERED = model.TIMES_TRIGGERED;
  

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

		
       

        public virtual bool Edit(ref ValidationErrors errors, QRTZ_SIMPLE_TRIGGERSModel model)
        {
            try
            {
                QRTZ_SIMPLE_TRIGGERS entity = m_Rep.GetById(model.SCHED_NAME);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.SCHED_NAME = model.SCHED_NAME;
				entity.TRIGGER_NAME = model.TRIGGER_NAME;
				entity.TRIGGER_GROUP = model.TRIGGER_GROUP;
				entity.REPEAT_COUNT = model.REPEAT_COUNT;
				entity.REPEAT_INTERVAL = model.REPEAT_INTERVAL;
				entity.TIMES_TRIGGERED = model.TIMES_TRIGGERED;
 


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

      

        public virtual QRTZ_SIMPLE_TRIGGERSModel GetById(string id)
        {
            if (IsExists(id))
            {
                QRTZ_SIMPLE_TRIGGERS entity = m_Rep.GetById(id);
                QRTZ_SIMPLE_TRIGGERSModel model = new QRTZ_SIMPLE_TRIGGERSModel();
                              				model.SCHED_NAME = entity.SCHED_NAME;
				model.TRIGGER_NAME = entity.TRIGGER_NAME;
				model.TRIGGER_GROUP = entity.TRIGGER_GROUP;
				model.REPEAT_COUNT = entity.REPEAT_COUNT;
				model.REPEAT_INTERVAL = entity.REPEAT_INTERVAL;
				model.TIMES_TRIGGERED = entity.TIMES_TRIGGERED;
 
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
