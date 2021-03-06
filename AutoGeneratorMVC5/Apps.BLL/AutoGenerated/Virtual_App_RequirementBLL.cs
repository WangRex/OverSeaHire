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
using Apps.IDAL.App;
using Apps.Models.App;
namespace Apps.BLL.App
{
	public class Virtual_App_RequirementBLL
	{
        [Dependency]
        public IApp_RequirementRepository m_Rep { get; set; }

		public virtual List<App_RequirementModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<App_Requirement> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.Id.Contains(queryStr)
								
								
								|| a.CreateUserName.Contains(queryStr)
								|| a.ModificationUserName.Contains(queryStr)
								
								|| a.ParentId.Contains(queryStr)
								|| a.Title.Contains(queryStr)
								|| a.SubTitle.Contains(queryStr)
								|| a.PK_App_Position_Name.Contains(queryStr)
								|| a.WorkPlace.Contains(queryStr)
								|| a.WorkLimitSex.Contains(queryStr)
								
								
								|| a.EnumWorkLimitDegree.Contains(queryStr)
								|| a.SalaryLow.Contains(queryStr)
								|| a.SalaryHigh.Contains(queryStr)
								|| a.PromiseMoney.Contains(queryStr)
								|| a.ServiceMoney.Contains(queryStr)
								|| a.TotalServiceMoney.Contains(queryStr)
								|| a.PublishDate.Contains(queryStr)
								
								|| a.Tag.Contains(queryStr)
								|| a.PK_App_Customer_CustomerName.Contains(queryStr)
								|| a.Description.Contains(queryStr)
								|| a.SwitchBtnRecommend.Contains(queryStr)
								|| a.CompanyName.Contains(queryStr)
								|| a.PK_App_Country_Name.Contains(queryStr)
								
								|| a.TransactProvince.Contains(queryStr)
								
								|| a.ServiceTailMoney.Contains(queryStr)
								|| a.SwitchBtnOpen.Contains(queryStr)
								
								|| a.PreTaxSalary.Contains(queryStr)
								|| a.WorkHourPerWeek.Contains(queryStr)
								|| a.EnumWorkPermit.Contains(queryStr)
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
        public virtual List<App_RequirementModel> CreateModelList(ref IQueryable<App_Requirement> queryData)
        {

            List<App_RequirementModel> modelList = (from r in queryData
                                              select new App_RequirementModel
                                              {
													Id = r.Id,
													CreateTime = r.CreateTime,
													ModificationTime = r.ModificationTime,
													CreateUserName = r.CreateUserName,
													ModificationUserName = r.ModificationUserName,
													SortCode = r.SortCode,
													ParentId = r.ParentId,
													Title = r.Title,
													SubTitle = r.SubTitle,
													PK_App_Position_Name = r.PK_App_Position_Name,
													WorkPlace = r.WorkPlace,
													WorkLimitSex = r.WorkLimitSex,
													WorkLimitAgeLow = r.WorkLimitAgeLow,
													WorkLimitAgeHigh = r.WorkLimitAgeHigh,
													EnumWorkLimitDegree = r.EnumWorkLimitDegree,
													SalaryLow = r.SalaryLow,
													SalaryHigh = r.SalaryHigh,
													PromiseMoney = r.PromiseMoney,
													ServiceMoney = r.ServiceMoney,
													TotalServiceMoney = r.TotalServiceMoney,
													PublishDate = r.PublishDate,
													TotalHire = r.TotalHire,
													Tag = r.Tag,
													PK_App_Customer_CustomerName = r.PK_App_Customer_CustomerName,
													Description = r.Description,
													SwitchBtnRecommend = r.SwitchBtnRecommend,
													CompanyName = r.CompanyName,
													PK_App_Country_Name = r.PK_App_Country_Name,
													TotalYear = r.TotalYear,
													TransactProvince = r.TransactProvince,
													ApplyCount = r.ApplyCount,
													ServiceTailMoney = r.ServiceTailMoney,
													SwitchBtnOpen = r.SwitchBtnOpen,
													ClickNumber = r.ClickNumber,
													PreTaxSalary = r.PreTaxSalary,
													WorkHourPerWeek = r.WorkHourPerWeek,
													EnumWorkPermit = r.EnumWorkPermit,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, App_RequirementModel model)
        {
            try
            {
                App_Requirement entity = m_Rep.GetById(model.Id);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new App_Requirement();
               				entity.Id = model.Id;
				entity.CreateTime = model.CreateTime;
				entity.ModificationTime = model.ModificationTime;
				entity.CreateUserName = model.CreateUserName;
				entity.ModificationUserName = model.ModificationUserName;
				entity.SortCode = model.SortCode;
				entity.ParentId = model.ParentId;
				entity.Title = model.Title;
				entity.SubTitle = model.SubTitle;
				entity.PK_App_Position_Name = model.PK_App_Position_Name;
				entity.WorkPlace = model.WorkPlace;
				entity.WorkLimitSex = model.WorkLimitSex;
				entity.WorkLimitAgeLow = model.WorkLimitAgeLow;
				entity.WorkLimitAgeHigh = model.WorkLimitAgeHigh;
				entity.EnumWorkLimitDegree = model.EnumWorkLimitDegree;
				entity.SalaryLow = model.SalaryLow;
				entity.SalaryHigh = model.SalaryHigh;
				entity.PromiseMoney = model.PromiseMoney;
				entity.ServiceMoney = model.ServiceMoney;
				entity.TotalServiceMoney = model.TotalServiceMoney;
				entity.PublishDate = model.PublishDate;
				entity.TotalHire = model.TotalHire;
				entity.Tag = model.Tag;
				entity.PK_App_Customer_CustomerName = model.PK_App_Customer_CustomerName;
				entity.Description = model.Description;
				entity.SwitchBtnRecommend = model.SwitchBtnRecommend;
				entity.CompanyName = model.CompanyName;
				entity.PK_App_Country_Name = model.PK_App_Country_Name;
				entity.TotalYear = model.TotalYear;
				entity.TransactProvince = model.TransactProvince;
				entity.ApplyCount = model.ApplyCount;
				entity.ServiceTailMoney = model.ServiceTailMoney;
				entity.SwitchBtnOpen = model.SwitchBtnOpen;
				entity.ClickNumber = model.ClickNumber;
				entity.PreTaxSalary = model.PreTaxSalary;
				entity.WorkHourPerWeek = model.WorkHourPerWeek;
				entity.EnumWorkPermit = model.EnumWorkPermit;
  

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

		
       

        public virtual bool Edit(ref ValidationErrors errors, App_RequirementModel model)
        {
            try
            {
                App_Requirement entity = m_Rep.GetById(model.Id);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.Id = model.Id;
				entity.CreateTime = model.CreateTime;
				entity.ModificationTime = model.ModificationTime;
				entity.CreateUserName = model.CreateUserName;
				entity.ModificationUserName = model.ModificationUserName;
				entity.SortCode = model.SortCode;
				entity.ParentId = model.ParentId;
				entity.Title = model.Title;
				entity.SubTitle = model.SubTitle;
				entity.PK_App_Position_Name = model.PK_App_Position_Name;
				entity.WorkPlace = model.WorkPlace;
				entity.WorkLimitSex = model.WorkLimitSex;
				entity.WorkLimitAgeLow = model.WorkLimitAgeLow;
				entity.WorkLimitAgeHigh = model.WorkLimitAgeHigh;
				entity.EnumWorkLimitDegree = model.EnumWorkLimitDegree;
				entity.SalaryLow = model.SalaryLow;
				entity.SalaryHigh = model.SalaryHigh;
				entity.PromiseMoney = model.PromiseMoney;
				entity.ServiceMoney = model.ServiceMoney;
				entity.TotalServiceMoney = model.TotalServiceMoney;
				entity.PublishDate = model.PublishDate;
				entity.TotalHire = model.TotalHire;
				entity.Tag = model.Tag;
				entity.PK_App_Customer_CustomerName = model.PK_App_Customer_CustomerName;
				entity.Description = model.Description;
				entity.SwitchBtnRecommend = model.SwitchBtnRecommend;
				entity.CompanyName = model.CompanyName;
				entity.PK_App_Country_Name = model.PK_App_Country_Name;
				entity.TotalYear = model.TotalYear;
				entity.TransactProvince = model.TransactProvince;
				entity.ApplyCount = model.ApplyCount;
				entity.ServiceTailMoney = model.ServiceTailMoney;
				entity.SwitchBtnOpen = model.SwitchBtnOpen;
				entity.ClickNumber = model.ClickNumber;
				entity.PreTaxSalary = model.PreTaxSalary;
				entity.WorkHourPerWeek = model.WorkHourPerWeek;
				entity.EnumWorkPermit = model.EnumWorkPermit;
 


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

      

        public virtual App_RequirementModel GetById(string id)
        {
            if (IsExists(id))
            {
                App_Requirement entity = m_Rep.GetById(id);
                App_RequirementModel model = new App_RequirementModel();
                              				model.Id = entity.Id;
				model.CreateTime = entity.CreateTime;
				model.ModificationTime = entity.ModificationTime;
				model.CreateUserName = entity.CreateUserName;
				model.ModificationUserName = entity.ModificationUserName;
				model.SortCode = entity.SortCode;
				model.ParentId = entity.ParentId;
				model.Title = entity.Title;
				model.SubTitle = entity.SubTitle;
				model.PK_App_Position_Name = entity.PK_App_Position_Name;
				model.WorkPlace = entity.WorkPlace;
				model.WorkLimitSex = entity.WorkLimitSex;
				model.WorkLimitAgeLow = entity.WorkLimitAgeLow;
				model.WorkLimitAgeHigh = entity.WorkLimitAgeHigh;
				model.EnumWorkLimitDegree = entity.EnumWorkLimitDegree;
				model.SalaryLow = entity.SalaryLow;
				model.SalaryHigh = entity.SalaryHigh;
				model.PromiseMoney = entity.PromiseMoney;
				model.ServiceMoney = entity.ServiceMoney;
				model.TotalServiceMoney = entity.TotalServiceMoney;
				model.PublishDate = entity.PublishDate;
				model.TotalHire = entity.TotalHire;
				model.Tag = entity.Tag;
				model.PK_App_Customer_CustomerName = entity.PK_App_Customer_CustomerName;
				model.Description = entity.Description;
				model.SwitchBtnRecommend = entity.SwitchBtnRecommend;
				model.CompanyName = entity.CompanyName;
				model.PK_App_Country_Name = entity.PK_App_Country_Name;
				model.TotalYear = entity.TotalYear;
				model.TransactProvince = entity.TransactProvince;
				model.ApplyCount = entity.ApplyCount;
				model.ServiceTailMoney = entity.ServiceTailMoney;
				model.SwitchBtnOpen = entity.SwitchBtnOpen;
				model.ClickNumber = entity.ClickNumber;
				model.PreTaxSalary = entity.PreTaxSalary;
				model.WorkHourPerWeek = entity.WorkHourPerWeek;
				model.EnumWorkPermit = entity.EnumWorkPermit;
 
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
