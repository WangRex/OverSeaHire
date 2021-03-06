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
	public class Virtual_QRTZ_SIMPROP_TRIGGERSBLL
	{
        [Dependency]
        public IQRTZ_SIMPROP_TRIGGERSRepository m_Rep { get; set; }

		public virtual List<QRTZ_SIMPROP_TRIGGERSModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<QRTZ_SIMPROP_TRIGGERS> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.SCHED_NAME.Contains(queryStr)
								|| a.TRIGGER_NAME.Contains(queryStr)
								|| a.TRIGGER_GROUP.Contains(queryStr)
								|| a.STR_PROP_1.Contains(queryStr)
								|| a.STR_PROP_2.Contains(queryStr)
								|| a.STR_PROP_3.Contains(queryStr)
								
								
								
								
								
								
								
								
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
        public virtual List<QRTZ_SIMPROP_TRIGGERSModel> CreateModelList(ref IQueryable<QRTZ_SIMPROP_TRIGGERS> queryData)
        {

            List<QRTZ_SIMPROP_TRIGGERSModel> modelList = (from r in queryData
                                              select new QRTZ_SIMPROP_TRIGGERSModel
                                              {
													SCHED_NAME = r.SCHED_NAME,
													TRIGGER_NAME = r.TRIGGER_NAME,
													TRIGGER_GROUP = r.TRIGGER_GROUP,
													STR_PROP_1 = r.STR_PROP_1,
													STR_PROP_2 = r.STR_PROP_2,
													STR_PROP_3 = r.STR_PROP_3,
													INT_PROP_1 = r.INT_PROP_1,
													INT_PROP_2 = r.INT_PROP_2,
													LONG_PROP_1 = r.LONG_PROP_1,
													LONG_PROP_2 = r.LONG_PROP_2,
													DEC_PROP_1 = r.DEC_PROP_1,
													DEC_PROP_2 = r.DEC_PROP_2,
													BOOL_PROP_1 = r.BOOL_PROP_1,
													BOOL_PROP_2 = r.BOOL_PROP_2,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, QRTZ_SIMPROP_TRIGGERSModel model)
        {
            try
            {
                QRTZ_SIMPROP_TRIGGERS entity = m_Rep.GetById(model.SCHED_NAME);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new QRTZ_SIMPROP_TRIGGERS();
               				entity.SCHED_NAME = model.SCHED_NAME;
				entity.TRIGGER_NAME = model.TRIGGER_NAME;
				entity.TRIGGER_GROUP = model.TRIGGER_GROUP;
				entity.STR_PROP_1 = model.STR_PROP_1;
				entity.STR_PROP_2 = model.STR_PROP_2;
				entity.STR_PROP_3 = model.STR_PROP_3;
				entity.INT_PROP_1 = model.INT_PROP_1;
				entity.INT_PROP_2 = model.INT_PROP_2;
				entity.LONG_PROP_1 = model.LONG_PROP_1;
				entity.LONG_PROP_2 = model.LONG_PROP_2;
				entity.DEC_PROP_1 = model.DEC_PROP_1;
				entity.DEC_PROP_2 = model.DEC_PROP_2;
				entity.BOOL_PROP_1 = model.BOOL_PROP_1;
				entity.BOOL_PROP_2 = model.BOOL_PROP_2;
  

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

		
       

        public virtual bool Edit(ref ValidationErrors errors, QRTZ_SIMPROP_TRIGGERSModel model)
        {
            try
            {
                QRTZ_SIMPROP_TRIGGERS entity = m_Rep.GetById(model.SCHED_NAME);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.SCHED_NAME = model.SCHED_NAME;
				entity.TRIGGER_NAME = model.TRIGGER_NAME;
				entity.TRIGGER_GROUP = model.TRIGGER_GROUP;
				entity.STR_PROP_1 = model.STR_PROP_1;
				entity.STR_PROP_2 = model.STR_PROP_2;
				entity.STR_PROP_3 = model.STR_PROP_3;
				entity.INT_PROP_1 = model.INT_PROP_1;
				entity.INT_PROP_2 = model.INT_PROP_2;
				entity.LONG_PROP_1 = model.LONG_PROP_1;
				entity.LONG_PROP_2 = model.LONG_PROP_2;
				entity.DEC_PROP_1 = model.DEC_PROP_1;
				entity.DEC_PROP_2 = model.DEC_PROP_2;
				entity.BOOL_PROP_1 = model.BOOL_PROP_1;
				entity.BOOL_PROP_2 = model.BOOL_PROP_2;
 


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

      

        public virtual QRTZ_SIMPROP_TRIGGERSModel GetById(string id)
        {
            if (IsExists(id))
            {
                QRTZ_SIMPROP_TRIGGERS entity = m_Rep.GetById(id);
                QRTZ_SIMPROP_TRIGGERSModel model = new QRTZ_SIMPROP_TRIGGERSModel();
                              				model.SCHED_NAME = entity.SCHED_NAME;
				model.TRIGGER_NAME = entity.TRIGGER_NAME;
				model.TRIGGER_GROUP = entity.TRIGGER_GROUP;
				model.STR_PROP_1 = entity.STR_PROP_1;
				model.STR_PROP_2 = entity.STR_PROP_2;
				model.STR_PROP_3 = entity.STR_PROP_3;
				model.INT_PROP_1 = entity.INT_PROP_1;
				model.INT_PROP_2 = entity.INT_PROP_2;
				model.LONG_PROP_1 = entity.LONG_PROP_1;
				model.LONG_PROP_2 = entity.LONG_PROP_2;
				model.DEC_PROP_1 = entity.DEC_PROP_1;
				model.DEC_PROP_2 = entity.DEC_PROP_2;
				model.BOOL_PROP_1 = entity.BOOL_PROP_1;
				model.BOOL_PROP_2 = entity.BOOL_PROP_2;
 
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
