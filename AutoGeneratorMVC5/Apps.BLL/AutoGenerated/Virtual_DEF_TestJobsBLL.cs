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
using Apps.IDAL.DEF;
using Apps.Models.DEF;
namespace Apps.BLL.DEF
{
	public class Virtual_DEF_TestJobsBLL
	{
        [Dependency]
        public IDEF_TestJobsRepository m_Rep { get; set; }

		public virtual List<DEF_TestJobsModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<DEF_TestJobs> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.VerCode.Contains(queryStr)
								|| a.Name.Contains(queryStr)
								
								|| a.Description.Contains(queryStr)
								|| a.Creator.Contains(queryStr)
								
								
								|| a.Closer.Contains(queryStr)
								
								
								
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
        public virtual List<DEF_TestJobsModel> CreateModelList(ref IQueryable<DEF_TestJobs> queryData)
        {

            List<DEF_TestJobsModel> modelList = (from r in queryData
                                              select new DEF_TestJobsModel
                                              {
													VerCode = r.VerCode,
													Name = r.Name,
													Result = r.Result,
													Description = r.Description,
													Creator = r.Creator,
													CrtDt = r.CrtDt,
													CloseState = r.CloseState,
													Closer = r.Closer,
													CloseDt = r.CloseDt,
													Def = r.Def,
													CheckFlag = r.CheckFlag,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, DEF_TestJobsModel model)
        {
            try
            {
                DEF_TestJobs entity = m_Rep.GetById(model.VerCode);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new DEF_TestJobs();
               				entity.VerCode = model.VerCode;
				entity.Name = model.Name;
				entity.Result = model.Result;
				entity.Description = model.Description;
				entity.Creator = model.Creator;
				entity.CrtDt = model.CrtDt;
				entity.CloseState = model.CloseState;
				entity.Closer = model.Closer;
				entity.CloseDt = model.CloseDt;
				entity.Def = model.Def;
				entity.CheckFlag = model.CheckFlag;
  

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

		
       

        public virtual bool Edit(ref ValidationErrors errors, DEF_TestJobsModel model)
        {
            try
            {
                DEF_TestJobs entity = m_Rep.GetById(model.VerCode);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.VerCode = model.VerCode;
				entity.Name = model.Name;
				entity.Result = model.Result;
				entity.Description = model.Description;
				entity.Creator = model.Creator;
				entity.CrtDt = model.CrtDt;
				entity.CloseState = model.CloseState;
				entity.Closer = model.Closer;
				entity.CloseDt = model.CloseDt;
				entity.Def = model.Def;
				entity.CheckFlag = model.CheckFlag;
 


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

      

        public virtual DEF_TestJobsModel GetById(string id)
        {
            if (IsExists(id))
            {
                DEF_TestJobs entity = m_Rep.GetById(id);
                DEF_TestJobsModel model = new DEF_TestJobsModel();
                              				model.VerCode = entity.VerCode;
				model.Name = entity.Name;
				model.Result = entity.Result;
				model.Description = entity.Description;
				model.Creator = entity.Creator;
				model.CrtDt = entity.CrtDt;
				model.CloseState = entity.CloseState;
				model.Closer = entity.Closer;
				model.CloseDt = entity.CloseDt;
				model.Def = entity.Def;
				model.CheckFlag = entity.CheckFlag;
 
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
