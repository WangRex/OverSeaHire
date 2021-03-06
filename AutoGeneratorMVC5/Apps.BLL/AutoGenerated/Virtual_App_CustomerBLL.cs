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
	public class Virtual_App_CustomerBLL
	{
        [Dependency]
        public IApp_CustomerRepository m_Rep { get; set; }

		public virtual List<App_CustomerModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<App_Customer> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.Id.Contains(queryStr)
								
								
								|| a.CreateUserName.Contains(queryStr)
								|| a.ModificationUserName.Contains(queryStr)
								
								|| a.ParentId.Contains(queryStr)
								
								|| a.Info.Contains(queryStr)
								|| a.EnumCustomerLevel.Contains(queryStr)
								|| a.CustomerName.Contains(queryStr)
								|| a.Password.Contains(queryStr)
								|| a.Sex.Contains(queryStr)
								|| a.Phone.Contains(queryStr)
								|| a.NickName.Contains(queryStr)
								
								|| a.Height.Contains(queryStr)
								|| a.Weight.Contains(queryStr)
								|| a.Nation.Contains(queryStr)
								|| a.Introduction.Contains(queryStr)
								|| a.CustomerPhoto.Contains(queryStr)
								|| a.OpenID.Contains(queryStr)
								|| a.BirthDay.Contains(queryStr)
								|| a.BirthPlace.Contains(queryStr)
								|| a.CurrentPlace.Contains(queryStr)
								|| a.WeChatNumber.Contains(queryStr)
								|| a.Cultural.Contains(queryStr)
								|| a.EnumCustomerType.Contains(queryStr)
								|| a.EnumForeignLangGrade.Contains(queryStr)
								|| a.SwitchBtnPassport.Contains(queryStr)
								|| a.AbroadExp.Contains(queryStr)
								|| a.EnumDriverLicence.Contains(queryStr)
								|| a.SwitchBtnRecommend.Contains(queryStr)
								|| a.VideoPath.Contains(queryStr)
								|| a.WordPath.Contains(queryStr)
								|| a.WordName.Contains(queryStr)
								|| a.WordExt.Contains(queryStr)
								|| a.JobIntension.Contains(queryStr)
								|| a.EnglishName.Contains(queryStr)
								|| a.MaritalStatus.Contains(queryStr)
								|| a.PassportNo.Contains(queryStr)
								|| a.Religion.Contains(queryStr)
								|| a.ExpectCountry.Contains(queryStr)
								|| a.SwitchBtnInterview.Contains(queryStr)
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
        public virtual List<App_CustomerModel> CreateModelList(ref IQueryable<App_Customer> queryData)
        {

            List<App_CustomerModel> modelList = (from r in queryData
                                              select new App_CustomerModel
                                              {
													Id = r.Id,
													CreateTime = r.CreateTime,
													ModificationTime = r.ModificationTime,
													CreateUserName = r.CreateUserName,
													ModificationUserName = r.ModificationUserName,
													SortCode = r.SortCode,
													ParentId = r.ParentId,
													Record = r.Record,
													Info = r.Info,
													EnumCustomerLevel = r.EnumCustomerLevel,
													CustomerName = r.CustomerName,
													Password = r.Password,
													Sex = r.Sex,
													Phone = r.Phone,
													NickName = r.NickName,
													Age = r.Age,
													Height = r.Height,
													Weight = r.Weight,
													Nation = r.Nation,
													Introduction = r.Introduction,
													CustomerPhoto = r.CustomerPhoto,
													OpenID = r.OpenID,
													BirthDay = r.BirthDay,
													BirthPlace = r.BirthPlace,
													CurrentPlace = r.CurrentPlace,
													WeChatNumber = r.WeChatNumber,
													Cultural = r.Cultural,
													EnumCustomerType = r.EnumCustomerType,
													EnumForeignLangGrade = r.EnumForeignLangGrade,
													SwitchBtnPassport = r.SwitchBtnPassport,
													AbroadExp = r.AbroadExp,
													EnumDriverLicence = r.EnumDriverLicence,
													SwitchBtnRecommend = r.SwitchBtnRecommend,
													VideoPath = r.VideoPath,
													WordPath = r.WordPath,
													WordName = r.WordName,
													WordExt = r.WordExt,
													JobIntension = r.JobIntension,
													EnglishName = r.EnglishName,
													MaritalStatus = r.MaritalStatus,
													PassportNo = r.PassportNo,
													Religion = r.Religion,
													ExpectCountry = r.ExpectCountry,
													SwitchBtnInterview = r.SwitchBtnInterview,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, App_CustomerModel model)
        {
            try
            {
                App_Customer entity = m_Rep.GetById(model.Id);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new App_Customer();
               				entity.Id = model.Id;
				entity.CreateTime = model.CreateTime;
				entity.ModificationTime = model.ModificationTime;
				entity.CreateUserName = model.CreateUserName;
				entity.ModificationUserName = model.ModificationUserName;
				entity.SortCode = model.SortCode;
				entity.ParentId = model.ParentId;
				entity.Record = model.Record;
				entity.Info = model.Info;
				entity.EnumCustomerLevel = model.EnumCustomerLevel;
				entity.CustomerName = model.CustomerName;
				entity.Password = model.Password;
				entity.Sex = model.Sex;
				entity.Phone = model.Phone;
				entity.NickName = model.NickName;
				entity.Age = model.Age;
				entity.Height = model.Height;
				entity.Weight = model.Weight;
				entity.Nation = model.Nation;
				entity.Introduction = model.Introduction;
				entity.CustomerPhoto = model.CustomerPhoto;
				entity.OpenID = model.OpenID;
				entity.BirthDay = model.BirthDay;
				entity.BirthPlace = model.BirthPlace;
				entity.CurrentPlace = model.CurrentPlace;
				entity.WeChatNumber = model.WeChatNumber;
				entity.Cultural = model.Cultural;
				entity.EnumCustomerType = model.EnumCustomerType;
				entity.EnumForeignLangGrade = model.EnumForeignLangGrade;
				entity.SwitchBtnPassport = model.SwitchBtnPassport;
				entity.AbroadExp = model.AbroadExp;
				entity.EnumDriverLicence = model.EnumDriverLicence;
				entity.SwitchBtnRecommend = model.SwitchBtnRecommend;
				entity.VideoPath = model.VideoPath;
				entity.WordPath = model.WordPath;
				entity.WordName = model.WordName;
				entity.WordExt = model.WordExt;
				entity.JobIntension = model.JobIntension;
				entity.EnglishName = model.EnglishName;
				entity.MaritalStatus = model.MaritalStatus;
				entity.PassportNo = model.PassportNo;
				entity.Religion = model.Religion;
				entity.ExpectCountry = model.ExpectCountry;
				entity.SwitchBtnInterview = model.SwitchBtnInterview;
  

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

		
       

        public virtual bool Edit(ref ValidationErrors errors, App_CustomerModel model)
        {
            try
            {
                App_Customer entity = m_Rep.GetById(model.Id);
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
				entity.Record = model.Record;
				entity.Info = model.Info;
				entity.EnumCustomerLevel = model.EnumCustomerLevel;
				entity.CustomerName = model.CustomerName;
				entity.Password = model.Password;
				entity.Sex = model.Sex;
				entity.Phone = model.Phone;
				entity.NickName = model.NickName;
				entity.Age = model.Age;
				entity.Height = model.Height;
				entity.Weight = model.Weight;
				entity.Nation = model.Nation;
				entity.Introduction = model.Introduction;
				entity.CustomerPhoto = model.CustomerPhoto;
				entity.OpenID = model.OpenID;
				entity.BirthDay = model.BirthDay;
				entity.BirthPlace = model.BirthPlace;
				entity.CurrentPlace = model.CurrentPlace;
				entity.WeChatNumber = model.WeChatNumber;
				entity.Cultural = model.Cultural;
				entity.EnumCustomerType = model.EnumCustomerType;
				entity.EnumForeignLangGrade = model.EnumForeignLangGrade;
				entity.SwitchBtnPassport = model.SwitchBtnPassport;
				entity.AbroadExp = model.AbroadExp;
				entity.EnumDriverLicence = model.EnumDriverLicence;
				entity.SwitchBtnRecommend = model.SwitchBtnRecommend;
				entity.VideoPath = model.VideoPath;
				entity.WordPath = model.WordPath;
				entity.WordName = model.WordName;
				entity.WordExt = model.WordExt;
				entity.JobIntension = model.JobIntension;
				entity.EnglishName = model.EnglishName;
				entity.MaritalStatus = model.MaritalStatus;
				entity.PassportNo = model.PassportNo;
				entity.Religion = model.Religion;
				entity.ExpectCountry = model.ExpectCountry;
				entity.SwitchBtnInterview = model.SwitchBtnInterview;
 


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

      

        public virtual App_CustomerModel GetById(string id)
        {
            if (IsExists(id))
            {
                App_Customer entity = m_Rep.GetById(id);
                App_CustomerModel model = new App_CustomerModel();
                              				model.Id = entity.Id;
				model.CreateTime = entity.CreateTime;
				model.ModificationTime = entity.ModificationTime;
				model.CreateUserName = entity.CreateUserName;
				model.ModificationUserName = entity.ModificationUserName;
				model.SortCode = entity.SortCode;
				model.ParentId = entity.ParentId;
				model.Record = entity.Record;
				model.Info = entity.Info;
				model.EnumCustomerLevel = entity.EnumCustomerLevel;
				model.CustomerName = entity.CustomerName;
				model.Password = entity.Password;
				model.Sex = entity.Sex;
				model.Phone = entity.Phone;
				model.NickName = entity.NickName;
				model.Age = entity.Age;
				model.Height = entity.Height;
				model.Weight = entity.Weight;
				model.Nation = entity.Nation;
				model.Introduction = entity.Introduction;
				model.CustomerPhoto = entity.CustomerPhoto;
				model.OpenID = entity.OpenID;
				model.BirthDay = entity.BirthDay;
				model.BirthPlace = entity.BirthPlace;
				model.CurrentPlace = entity.CurrentPlace;
				model.WeChatNumber = entity.WeChatNumber;
				model.Cultural = entity.Cultural;
				model.EnumCustomerType = entity.EnumCustomerType;
				model.EnumForeignLangGrade = entity.EnumForeignLangGrade;
				model.SwitchBtnPassport = entity.SwitchBtnPassport;
				model.AbroadExp = entity.AbroadExp;
				model.EnumDriverLicence = entity.EnumDriverLicence;
				model.SwitchBtnRecommend = entity.SwitchBtnRecommend;
				model.VideoPath = entity.VideoPath;
				model.WordPath = entity.WordPath;
				model.WordName = entity.WordName;
				model.WordExt = entity.WordExt;
				model.JobIntension = entity.JobIntension;
				model.EnglishName = entity.EnglishName;
				model.MaritalStatus = entity.MaritalStatus;
				model.PassportNo = entity.PassportNo;
				model.Religion = entity.Religion;
				model.ExpectCountry = entity.ExpectCountry;
				model.SwitchBtnInterview = entity.SwitchBtnInterview;
 
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
