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
using Apps.IDAL.Sys;
using Apps.Models.Sys;
namespace Apps.BLL.Sys
{
	public class Virtual_SysUserBLL
	{
        [Dependency]
        public ISysUserRepository m_Rep { get; set; }

		public virtual List<SysUserModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<SysUser> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.Id.Contains(queryStr)
								|| a.UserName.Contains(queryStr)
								|| a.Password.Contains(queryStr)
								|| a.TrueName.Contains(queryStr)
								|| a.Card.Contains(queryStr)
								|| a.MobileNumber.Contains(queryStr)
								|| a.PhoneNumber.Contains(queryStr)
								|| a.QQ.Contains(queryStr)
								|| a.EmailAddress.Contains(queryStr)
								|| a.OtherContact.Contains(queryStr)
								|| a.Province.Contains(queryStr)
								|| a.City.Contains(queryStr)
								|| a.Village.Contains(queryStr)
								|| a.Address.Contains(queryStr)
								
								
								|| a.CreatePerson.Contains(queryStr)
								|| a.Sex.Contains(queryStr)
								
								
								|| a.Marital.Contains(queryStr)
								|| a.Political.Contains(queryStr)
								|| a.Nationality.Contains(queryStr)
								|| a.Native.Contains(queryStr)
								|| a.School.Contains(queryStr)
								|| a.Professional.Contains(queryStr)
								|| a.Degree.Contains(queryStr)
								|| a.DepId.Contains(queryStr)
								|| a.PosId.Contains(queryStr)
								|| a.Expertise.Contains(queryStr)
								
								|| a.Photo.Contains(queryStr)
								|| a.Attach.Contains(queryStr)
								|| a.Lead.Contains(queryStr)
								|| a.LeadName.Contains(queryStr)
								
								
								
								|| a.OpenID.Contains(queryStr)
								
								|| a.ModificationUser.Contains(queryStr)
								|| a.SwitchBtnLead.Contains(queryStr)
								|| a.EnumUserType.Contains(queryStr)
								|| a.PK_App_Customer_CustomerName.Contains(queryStr)
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
        public virtual List<SysUserModel> CreateModelList(ref IQueryable<SysUser> queryData)
        {

            List<SysUserModel> modelList = (from r in queryData
                                              select new SysUserModel
                                              {
													Id = r.Id,
													UserName = r.UserName,
													Password = r.Password,
													TrueName = r.TrueName,
													Card = r.Card,
													MobileNumber = r.MobileNumber,
													PhoneNumber = r.PhoneNumber,
													QQ = r.QQ,
													EmailAddress = r.EmailAddress,
													OtherContact = r.OtherContact,
													Province = r.Province,
													City = r.City,
													Village = r.Village,
													Address = r.Address,
													State = r.State,
													CreateTime = r.CreateTime,
													CreatePerson = r.CreatePerson,
													Sex = r.Sex,
													Birthday = r.Birthday,
													JoinDate = r.JoinDate,
													Marital = r.Marital,
													Political = r.Political,
													Nationality = r.Nationality,
													Native = r.Native,
													School = r.School,
													Professional = r.Professional,
													Degree = r.Degree,
													DepId = r.DepId,
													PosId = r.PosId,
													Expertise = r.Expertise,
													JobState = r.JobState,
													Photo = r.Photo,
													Attach = r.Attach,
													Lead = r.Lead,
													LeadName = r.LeadName,
													IsSelLead = r.IsSelLead,
													IsReportCalendar = r.IsReportCalendar,
													IsSecretary = r.IsSecretary,
													OpenID = r.OpenID,
													ModificationTime = r.ModificationTime,
													ModificationUser = r.ModificationUser,
													SwitchBtnLead = r.SwitchBtnLead,
													EnumUserType = r.EnumUserType,
													PK_App_Customer_CustomerName = r.PK_App_Customer_CustomerName,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, SysUserModel model)
        {
            try
            {
                SysUser entity = m_Rep.GetById(model.Id);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new SysUser();
               				entity.Id = model.Id;
				entity.UserName = model.UserName;
				entity.Password = model.Password;
				entity.TrueName = model.TrueName;
				entity.Card = model.Card;
				entity.MobileNumber = model.MobileNumber;
				entity.PhoneNumber = model.PhoneNumber;
				entity.QQ = model.QQ;
				entity.EmailAddress = model.EmailAddress;
				entity.OtherContact = model.OtherContact;
				entity.Province = model.Province;
				entity.City = model.City;
				entity.Village = model.Village;
				entity.Address = model.Address;
				entity.State = model.State;
				entity.CreateTime = model.CreateTime;
				entity.CreatePerson = model.CreatePerson;
				entity.Sex = model.Sex;
				entity.Birthday = model.Birthday;
				entity.JoinDate = model.JoinDate;
				entity.Marital = model.Marital;
				entity.Political = model.Political;
				entity.Nationality = model.Nationality;
				entity.Native = model.Native;
				entity.School = model.School;
				entity.Professional = model.Professional;
				entity.Degree = model.Degree;
				entity.DepId = model.DepId;
				entity.PosId = model.PosId;
				entity.Expertise = model.Expertise;
				entity.JobState = model.JobState;
				entity.Photo = model.Photo;
				entity.Attach = model.Attach;
				entity.Lead = model.Lead;
				entity.LeadName = model.LeadName;
				entity.IsSelLead = model.IsSelLead;
				entity.IsReportCalendar = model.IsReportCalendar;
				entity.IsSecretary = model.IsSecretary;
				entity.OpenID = model.OpenID;
				entity.ModificationTime = model.ModificationTime;
				entity.ModificationUser = model.ModificationUser;
				entity.SwitchBtnLead = model.SwitchBtnLead;
				entity.EnumUserType = model.EnumUserType;
				entity.PK_App_Customer_CustomerName = model.PK_App_Customer_CustomerName;
  

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

		
       

        public virtual bool Edit(ref ValidationErrors errors, SysUserModel model)
        {
            try
            {
                SysUser entity = m_Rep.GetById(model.Id);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.Id = model.Id;
				entity.UserName = model.UserName;
				entity.Password = model.Password;
				entity.TrueName = model.TrueName;
				entity.Card = model.Card;
				entity.MobileNumber = model.MobileNumber;
				entity.PhoneNumber = model.PhoneNumber;
				entity.QQ = model.QQ;
				entity.EmailAddress = model.EmailAddress;
				entity.OtherContact = model.OtherContact;
				entity.Province = model.Province;
				entity.City = model.City;
				entity.Village = model.Village;
				entity.Address = model.Address;
				entity.State = model.State;
				entity.CreateTime = model.CreateTime;
				entity.CreatePerson = model.CreatePerson;
				entity.Sex = model.Sex;
				entity.Birthday = model.Birthday;
				entity.JoinDate = model.JoinDate;
				entity.Marital = model.Marital;
				entity.Political = model.Political;
				entity.Nationality = model.Nationality;
				entity.Native = model.Native;
				entity.School = model.School;
				entity.Professional = model.Professional;
				entity.Degree = model.Degree;
				entity.DepId = model.DepId;
				entity.PosId = model.PosId;
				entity.Expertise = model.Expertise;
				entity.JobState = model.JobState;
				entity.Photo = model.Photo;
				entity.Attach = model.Attach;
				entity.Lead = model.Lead;
				entity.LeadName = model.LeadName;
				entity.IsSelLead = model.IsSelLead;
				entity.IsReportCalendar = model.IsReportCalendar;
				entity.IsSecretary = model.IsSecretary;
				entity.OpenID = model.OpenID;
				entity.ModificationTime = model.ModificationTime;
				entity.ModificationUser = model.ModificationUser;
				entity.SwitchBtnLead = model.SwitchBtnLead;
				entity.EnumUserType = model.EnumUserType;
				entity.PK_App_Customer_CustomerName = model.PK_App_Customer_CustomerName;
 


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

      

        public virtual SysUserModel GetById(string id)
        {
            if (IsExists(id))
            {
                SysUser entity = m_Rep.GetById(id);
                SysUserModel model = new SysUserModel();
                              				model.Id = entity.Id;
				model.UserName = entity.UserName;
				model.Password = entity.Password;
				model.TrueName = entity.TrueName;
				model.Card = entity.Card;
				model.MobileNumber = entity.MobileNumber;
				model.PhoneNumber = entity.PhoneNumber;
				model.QQ = entity.QQ;
				model.EmailAddress = entity.EmailAddress;
				model.OtherContact = entity.OtherContact;
				model.Province = entity.Province;
				model.City = entity.City;
				model.Village = entity.Village;
				model.Address = entity.Address;
				model.State = entity.State;
				model.CreateTime = entity.CreateTime;
				model.CreatePerson = entity.CreatePerson;
				model.Sex = entity.Sex;
				model.Birthday = entity.Birthday;
				model.JoinDate = entity.JoinDate;
				model.Marital = entity.Marital;
				model.Political = entity.Political;
				model.Nationality = entity.Nationality;
				model.Native = entity.Native;
				model.School = entity.School;
				model.Professional = entity.Professional;
				model.Degree = entity.Degree;
				model.DepId = entity.DepId;
				model.PosId = entity.PosId;
				model.Expertise = entity.Expertise;
				model.JobState = entity.JobState;
				model.Photo = entity.Photo;
				model.Attach = entity.Attach;
				model.Lead = entity.Lead;
				model.LeadName = entity.LeadName;
				model.IsSelLead = entity.IsSelLead;
				model.IsReportCalendar = entity.IsReportCalendar;
				model.IsSecretary = entity.IsSecretary;
				model.OpenID = entity.OpenID;
				model.ModificationTime = entity.ModificationTime;
				model.ModificationUser = entity.ModificationUser;
				model.SwitchBtnLead = entity.SwitchBtnLead;
				model.EnumUserType = entity.EnumUserType;
				model.PK_App_Customer_CustomerName = entity.PK_App_Customer_CustomerName;
 
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
