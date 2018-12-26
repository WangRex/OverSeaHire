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
	public class Virtual_DEF_TestJobsDetailRelationBLL
	{
        [Dependency]
        public IDEF_TestJobsDetailRelationRepository m_Rep { get; set; }

		public virtual List<DEF_TestJobsDetailRelationModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<DEF_TestJobsDetailRelation> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.VerCode.Contains(queryStr)
								|| a.PCode.Contains(queryStr)
								|| a.CCode.Contains(queryStr)
								|| a.Name.Contains(queryStr)
								|| a.Description.Contains(queryStr)
								
								
								
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
        public virtual List<DEF_TestJobsDetailRelationModel> CreateModelList(ref IQueryable<DEF_TestJobsDetailRelation> queryData)
        {

            List<DEF_TestJobsDetailRelationModel> modelList = (from r in queryData
                                              select new DEF_TestJobsDetailRelationModel
                                              {
													VerCode = r.VerCode,
													PCode = r.PCode,
													CCode = r.CCode,
													Name = r.Name,
													Description = r.Description,
													Result = r.Result,
													Sort = r.Sort,
													ExSort = r.ExSort,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, DEF_TestJobsDetailRelationModel model)
        {
            try
            {
                DEF_TestJobsDetailRelation entity = m_Rep.GetById(model.VerCode);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new DEF_TestJobsDetailRelation();
               				entity.VerCode = model.VerCode;
				entity.PCode = model.PCode;
				entity.CCode = model.CCode;
				entity.Name = model.Name;
				entity.Description = model.Description;
				entity.Result = model.Result;
				entity.Sort = model.Sort;
				entity.ExSort = model.ExSort;
  

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

		
       

        public virtual bool Edit(ref ValidationErrors errors, DEF_TestJobsDetailRelationModel model)
        {
            try
            {
                DEF_TestJobsDetailRelation entity = m_Rep.GetById(model.VerCode);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.VerCode = model.VerCode;
				entity.PCode = model.PCode;
				entity.CCode = model.CCode;
				entity.Name = model.Name;
				entity.Description = model.Description;
				entity.Result = model.Result;
				entity.Sort = model.Sort;
				entity.ExSort = model.ExSort;
 


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

      

        public virtual DEF_TestJobsDetailRelationModel GetById(string id)
        {
            if (IsExists(id))
            {
                DEF_TestJobsDetailRelation entity = m_Rep.GetById(id);
                DEF_TestJobsDetailRelationModel model = new DEF_TestJobsDetailRelationModel();
                              				model.VerCode = entity.VerCode;
				model.PCode = entity.PCode;
				model.CCode = entity.CCode;
				model.Name = entity.Name;
				model.Description = entity.Description;
				model.Result = entity.Result;
				model.Sort = entity.Sort;
				model.ExSort = entity.ExSort;
 
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
