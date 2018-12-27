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
	public class Virtual_Flow_FormContentBLL
	{
        [Dependency]
        public IFlow_FormContentRepository m_Rep { get; set; }

		public virtual List<Flow_FormContentModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<Flow_FormContent> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.Id.Contains(queryStr)
								|| a.Title.Contains(queryStr)
								|| a.UserId.Contains(queryStr)
								|| a.FormId.Contains(queryStr)
								|| a.FormLevel.Contains(queryStr)
								
								|| a.AttrA.Contains(queryStr)
								|| a.AttrB.Contains(queryStr)
								|| a.AttrC.Contains(queryStr)
								|| a.AttrD.Contains(queryStr)
								|| a.AttrE.Contains(queryStr)
								|| a.AttrF.Contains(queryStr)
								|| a.AttrG.Contains(queryStr)
								|| a.AttrH.Contains(queryStr)
								|| a.AttrI.Contains(queryStr)
								|| a.AttrJ.Contains(queryStr)
								|| a.AttrK.Contains(queryStr)
								|| a.AttrL.Contains(queryStr)
								|| a.AttrM.Contains(queryStr)
								|| a.AttrN.Contains(queryStr)
								|| a.AttrO.Contains(queryStr)
								|| a.AttrP.Contains(queryStr)
								|| a.AttrQ.Contains(queryStr)
								|| a.AttrR.Contains(queryStr)
								|| a.AttrS.Contains(queryStr)
								|| a.AttrT.Contains(queryStr)
								|| a.AttrU.Contains(queryStr)
								|| a.AttrV.Contains(queryStr)
								|| a.AttrW.Contains(queryStr)
								|| a.AttrX.Contains(queryStr)
								|| a.AttrY.Contains(queryStr)
								|| a.AttrZ.Contains(queryStr)
								|| a.CustomMember.Contains(queryStr)
								
								
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
        public virtual List<Flow_FormContentModel> CreateModelList(ref IQueryable<Flow_FormContent> queryData)
        {

            List<Flow_FormContentModel> modelList = (from r in queryData
                                              select new Flow_FormContentModel
                                              {
													Id = r.Id,
													Title = r.Title,
													UserId = r.UserId,
													FormId = r.FormId,
													FormLevel = r.FormLevel,
													CreateTime = r.CreateTime,
													AttrA = r.AttrA,
													AttrB = r.AttrB,
													AttrC = r.AttrC,
													AttrD = r.AttrD,
													AttrE = r.AttrE,
													AttrF = r.AttrF,
													AttrG = r.AttrG,
													AttrH = r.AttrH,
													AttrI = r.AttrI,
													AttrJ = r.AttrJ,
													AttrK = r.AttrK,
													AttrL = r.AttrL,
													AttrM = r.AttrM,
													AttrN = r.AttrN,
													AttrO = r.AttrO,
													AttrP = r.AttrP,
													AttrQ = r.AttrQ,
													AttrR = r.AttrR,
													AttrS = r.AttrS,
													AttrT = r.AttrT,
													AttrU = r.AttrU,
													AttrV = r.AttrV,
													AttrW = r.AttrW,
													AttrX = r.AttrX,
													AttrY = r.AttrY,
													AttrZ = r.AttrZ,
													CustomMember = r.CustomMember,
													TimeOut = r.TimeOut,
													IsDelete = r.IsDelete,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, Flow_FormContentModel model)
        {
            try
            {
                Flow_FormContent entity = m_Rep.GetById(model.Id);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new Flow_FormContent();
               				entity.Id = model.Id;
				entity.Title = model.Title;
				entity.UserId = model.UserId;
				entity.FormId = model.FormId;
				entity.FormLevel = model.FormLevel;
				entity.CreateTime = model.CreateTime;
				entity.AttrA = model.AttrA;
				entity.AttrB = model.AttrB;
				entity.AttrC = model.AttrC;
				entity.AttrD = model.AttrD;
				entity.AttrE = model.AttrE;
				entity.AttrF = model.AttrF;
				entity.AttrG = model.AttrG;
				entity.AttrH = model.AttrH;
				entity.AttrI = model.AttrI;
				entity.AttrJ = model.AttrJ;
				entity.AttrK = model.AttrK;
				entity.AttrL = model.AttrL;
				entity.AttrM = model.AttrM;
				entity.AttrN = model.AttrN;
				entity.AttrO = model.AttrO;
				entity.AttrP = model.AttrP;
				entity.AttrQ = model.AttrQ;
				entity.AttrR = model.AttrR;
				entity.AttrS = model.AttrS;
				entity.AttrT = model.AttrT;
				entity.AttrU = model.AttrU;
				entity.AttrV = model.AttrV;
				entity.AttrW = model.AttrW;
				entity.AttrX = model.AttrX;
				entity.AttrY = model.AttrY;
				entity.AttrZ = model.AttrZ;
				entity.CustomMember = model.CustomMember;
				entity.TimeOut = model.TimeOut;
				entity.IsDelete = model.IsDelete;
  

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

		
       

        public virtual bool Edit(ref ValidationErrors errors, Flow_FormContentModel model)
        {
            try
            {
                Flow_FormContent entity = m_Rep.GetById(model.Id);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.Id = model.Id;
				entity.Title = model.Title;
				entity.UserId = model.UserId;
				entity.FormId = model.FormId;
				entity.FormLevel = model.FormLevel;
				entity.CreateTime = model.CreateTime;
				entity.AttrA = model.AttrA;
				entity.AttrB = model.AttrB;
				entity.AttrC = model.AttrC;
				entity.AttrD = model.AttrD;
				entity.AttrE = model.AttrE;
				entity.AttrF = model.AttrF;
				entity.AttrG = model.AttrG;
				entity.AttrH = model.AttrH;
				entity.AttrI = model.AttrI;
				entity.AttrJ = model.AttrJ;
				entity.AttrK = model.AttrK;
				entity.AttrL = model.AttrL;
				entity.AttrM = model.AttrM;
				entity.AttrN = model.AttrN;
				entity.AttrO = model.AttrO;
				entity.AttrP = model.AttrP;
				entity.AttrQ = model.AttrQ;
				entity.AttrR = model.AttrR;
				entity.AttrS = model.AttrS;
				entity.AttrT = model.AttrT;
				entity.AttrU = model.AttrU;
				entity.AttrV = model.AttrV;
				entity.AttrW = model.AttrW;
				entity.AttrX = model.AttrX;
				entity.AttrY = model.AttrY;
				entity.AttrZ = model.AttrZ;
				entity.CustomMember = model.CustomMember;
				entity.TimeOut = model.TimeOut;
				entity.IsDelete = model.IsDelete;
 


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

      

        public virtual Flow_FormContentModel GetById(string id)
        {
            if (IsExists(id))
            {
                Flow_FormContent entity = m_Rep.GetById(id);
                Flow_FormContentModel model = new Flow_FormContentModel();
                              				model.Id = entity.Id;
				model.Title = entity.Title;
				model.UserId = entity.UserId;
				model.FormId = entity.FormId;
				model.FormLevel = entity.FormLevel;
				model.CreateTime = entity.CreateTime;
				model.AttrA = entity.AttrA;
				model.AttrB = entity.AttrB;
				model.AttrC = entity.AttrC;
				model.AttrD = entity.AttrD;
				model.AttrE = entity.AttrE;
				model.AttrF = entity.AttrF;
				model.AttrG = entity.AttrG;
				model.AttrH = entity.AttrH;
				model.AttrI = entity.AttrI;
				model.AttrJ = entity.AttrJ;
				model.AttrK = entity.AttrK;
				model.AttrL = entity.AttrL;
				model.AttrM = entity.AttrM;
				model.AttrN = entity.AttrN;
				model.AttrO = entity.AttrO;
				model.AttrP = entity.AttrP;
				model.AttrQ = entity.AttrQ;
				model.AttrR = entity.AttrR;
				model.AttrS = entity.AttrS;
				model.AttrT = entity.AttrT;
				model.AttrU = entity.AttrU;
				model.AttrV = entity.AttrV;
				model.AttrW = entity.AttrW;
				model.AttrX = entity.AttrX;
				model.AttrY = entity.AttrY;
				model.AttrZ = entity.AttrZ;
				model.CustomMember = entity.CustomMember;
				model.TimeOut = entity.TimeOut;
				model.IsDelete = entity.IsDelete;
 
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