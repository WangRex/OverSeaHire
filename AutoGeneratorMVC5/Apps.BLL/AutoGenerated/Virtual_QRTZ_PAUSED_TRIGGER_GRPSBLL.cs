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
	public class Virtual_QRTZ_PAUSED_TRIGGER_GRPSBLL
	{
        [Dependency]
        public IQRTZ_PAUSED_TRIGGER_GRPSRepository m_Rep { get; set; }

		public virtual List<QRTZ_PAUSED_TRIGGER_GRPSModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<QRTZ_PAUSED_TRIGGER_GRPS> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.SCHED_NAME.Contains(queryStr)
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
        public virtual List<QRTZ_PAUSED_TRIGGER_GRPSModel> CreateModelList(ref IQueryable<QRTZ_PAUSED_TRIGGER_GRPS> queryData)
        {

            List<QRTZ_PAUSED_TRIGGER_GRPSModel> modelList = (from r in queryData
                                              select new QRTZ_PAUSED_TRIGGER_GRPSModel
                                              {
													SCHED_NAME = r.SCHED_NAME,
													TRIGGER_GROUP = r.TRIGGER_GROUP,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, QRTZ_PAUSED_TRIGGER_GRPSModel model)
        {
            try
            {
                QRTZ_PAUSED_TRIGGER_GRPS entity = m_Rep.GetById(model.SCHED_NAME);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new QRTZ_PAUSED_TRIGGER_GRPS();
               				entity.SCHED_NAME = model.SCHED_NAME;
				entity.TRIGGER_GROUP = model.TRIGGER_GROUP;
  

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

		
       

        public virtual bool Edit(ref ValidationErrors errors, QRTZ_PAUSED_TRIGGER_GRPSModel model)
        {
            try
            {
                QRTZ_PAUSED_TRIGGER_GRPS entity = m_Rep.GetById(model.SCHED_NAME);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.SCHED_NAME = model.SCHED_NAME;
				entity.TRIGGER_GROUP = model.TRIGGER_GROUP;
 


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

      

        public virtual QRTZ_PAUSED_TRIGGER_GRPSModel GetById(string id)
        {
            if (IsExists(id))
            {
                QRTZ_PAUSED_TRIGGER_GRPS entity = m_Rep.GetById(id);
                QRTZ_PAUSED_TRIGGER_GRPSModel model = new QRTZ_PAUSED_TRIGGER_GRPSModel();
                              				model.SCHED_NAME = entity.SCHED_NAME;
				model.TRIGGER_GROUP = entity.TRIGGER_GROUP;
 
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
