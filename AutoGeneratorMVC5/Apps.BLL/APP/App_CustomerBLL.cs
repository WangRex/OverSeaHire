using Apps.Common;
using Apps.Models;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using Microsoft.Practices.Unity;
using Apps.DAL.Sys;
using Apps.BLL.Sys;
using System;
using Apps.Models.App;
using Apps.DAL.App;

namespace Apps.BLL.App
{
    public partial class App_CustomerBLL
    {
        #region Reps
        [Dependency]
        public SysLogRepository sysLog { get; set; }
        [Dependency]
        public EnumDictionaryBLL enumDictionary { get; set; }
        [Dependency]
        public App_CustomerWorkExpBLL customerWorkExpBLL { get; set; }
        [Dependency]
        public App_CustomerEduExpBLL customerEduExpBLL { get; set; }
        [Dependency]
        public App_CustomerFamilyBLL customerFamilyBLL { get; set; }
        [Dependency]
        public App_CustomerJobIntensionBLL customerJobIntensionBLL { get; set; }
        [Dependency]
        public App_CustomerCertificateBLL customerCertificateBLL { get; set; }
        [Dependency]
        public App_PositionBLL positionBLL { get; set; }
        [Dependency]
        public App_CountryRepository countryRepository { get; set; }
        [Dependency]
        public App_CustomerCollectRepository customerCollectRepository { get; set; }
        [Dependency]
        public App_CompanyBLL companyBLL { get; set; }
        #endregion

        #region 获取用户姓名
        /// <summary>
        /// 获取用户姓名
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public string GetCustomerName(string CustomerID)
        {
            var CustomerName = string.Empty;
            var _APP_CustomerModel = GetById(CustomerID);
            if (null != _APP_CustomerModel)
            {
                CustomerName = _APP_CustomerModel.CustomerName;
            }
            return CustomerName;
        }
        #endregion

        #region 获取昵称
        /// <summary>
        /// 获取昵称
        /// </summary>
        /// <param name="NickID"></param>
        /// <returns></returns>
        public string GetNickName(string NickID)
        {
            var NickName = string.Empty;
            var _APP_CustomerModel = GetById(NickID);
            if (null != _APP_CustomerModel)
            {
                NickName = _APP_CustomerModel.NickName;
            }
            return NickName;
        }
        #endregion

        #region 用户登录注册
        /// <summary>
        /// 用户登录注册
        /// </summary>
        /// <param name="customerLoginRegister"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public LoginVm CustomerLoginRegister(CustomerLoginRegister customerLoginRegister, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(customerLoginRegister.mobile, customerLoginRegister.ToString(), "开始", "CustomerLoginRegister", "App_CustomerBLL");
            App_Customer customer = new App_Customer();
            LoginVm loginVm = new LoginVm();
            var now = ResultHelper.NowTime;
            string code = customerLoginRegister.code, mobile = customerLoginRegister.mobile;
            var Code = HttpContext.Current.Cache[mobile];
            if ("8888".Equals(code) || Code.Equals(code))
            {
                sysLog.WriteServiceLog(customerLoginRegister.mobile, customerLoginRegister.ToString() + ",验证码验证成功", "验证", "CustomerLoginRegister", "App_CustomerBLL");
                customer = m_Rep.Find(EF => EF.Phone == customerLoginRegister.mobile && EF.EnumCustomerType == customerLoginRegister.identity);
                if (null == customer)
                {
                    sysLog.WriteServiceLog(customerLoginRegister.mobile, customerLoginRegister.ToString() + ",用户不存在，注册新用户", "注册", "CustomerLoginRegister", "App_CustomerBLL");
                    customer = new App_Customer();
                    customer.Id = ResultHelper.NewId;
                    customer.CreateTime = now;
                    customer.CreateUserName = mobile;
                    customer.ModificationTime = now;
                    customer.ModificationUserName = mobile;
                    customer.Phone = mobile;
                    customer.EnumCustomerType = customerLoginRegister.identity;
                    customer.CustomerName = mobile;
                    //默认记录值是1:APP注册用户,为了雇主端推荐功能      以有推荐标识的简历，排列在前面。之后是app上面注册的工人简历。最后是后台管理系统导入的数据
                    customer.EnumCustomerLevel = "1";
                    if (m_Rep.Create(customer))
                    {
                        loginVm.Id = customer.Id;
                        loginVm.CustomerName = customer.CustomerName;
                        loginVm.Photo = customer.CustomerPhoto;
                        loginVm.Phone = customer.Phone;
                        loginVm.IntroFlag = false;
                        loginVm.IntensionFlag = false;
                        ErrorMsg = "用户不存在，注册新用户成功";
                        sysLog.WriteServiceLog(customerLoginRegister.mobile, customerLoginRegister.ToString() + ",用户不存在，注册新用户", "成功", "CustomerLoginRegister", "App_CustomerBLL");
                        return loginVm;
                    }
                    else
                    {
                        ErrorMsg = "用户不存在，注册新用户失败";
                        sysLog.WriteServiceLog(customerLoginRegister.mobile, customerLoginRegister.ToString() + ",用户不存在，注册新用户", "失败", "CustomerLoginRegister", "App_CustomerBLL");
                        return null;
                    }
                }
                else
                {
                    loginVm.Id = customer.Id;
                    loginVm.CustomerName = customer.CustomerName;
                    loginVm.Photo = customer.CustomerPhoto;
                    loginVm.Phone = customer.Phone;
                    loginVm.IntroFlag = IsCheckCustomerIntro(customer.Id);
                    loginVm.IntensionFlag = IsCheckCustomerIntension(customer.Id);
                    ErrorMsg = "用户存在";
                    sysLog.WriteServiceLog(customerLoginRegister.mobile, customerLoginRegister.ToString() + ",用户存在", "成功", "CustomerLoginRegister", "App_CustomerBLL");
                    return loginVm;
                }
            }
            else
            {
                ErrorMsg = "验证码无效，请重新获取";
                sysLog.WriteServiceLog(customerLoginRegister.mobile, customerLoginRegister.ToString() + ",验证码无效，请重新获取", "失败", "CustomerLoginRegister", "App_CustomerBLL");
                return null;
            }
        }
        #endregion

        #region 获取用户资料
        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public CustomerModel GetCustomer(string customerId, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(customerId, "customerId:" + customerId, "开始", "GetCustomer", "App_CustomerBLL");
            App_Customer customer = new App_Customer();
            CustomerModel customerModel = new CustomerModel();
            customer = m_Rep.GetById(customerId);
            if (null != customer)
            {
                ErrorMsg = "获取用户资料成功";
                sysLog.WriteServiceLog(customerId, "customerId:" + customerId + ",获取用户资料", "成功", "GetCustomer", "App_CustomerBLL");
                customerModel.Id = customer.Id;
                customerModel.CustomerName = customer.CustomerName;
                customerModel.Phone = customer.Phone;
                customerModel.EnumCustomerType = customer.EnumCustomerType;
                customerModel.CustomerType = enumDictionary.GetDicName("APP_Customer.EnumCustomerType", customer.EnumCustomerType);
                customerModel.Photo = customer.CustomerPhoto;
                customerModel.Sex = customer.Sex;
                customerModel.Age = customer.Age;
                customerModel.Height = customer.Height;
                customerModel.Weight = customer.Weight;
                customerModel.Nation = customer.Nation;
                customerModel.BirthPlace = customer.BirthPlace;
                customerModel.WordPath = customer.WordPath;
                customerModel.WordName = customer.WordName;
                customerModel.WordExt = customer.WordExt;
                customerModel.VideoPath = customer.VideoPath;
                customerModel.Introduction = customer.Introduction;
                customerModel.workExpPosts = new List<WorkExpPost>();
                customerModel.workExpPosts = customerWorkExpBLL.m_Rep.FindList(EF => EF.PK_App_Customer_CustomerName == customerId).Select(EF => new WorkExpPost
                {
                    PK_App_Customer_CustomerName = EF.PK_App_Customer_CustomerName,
                    StartDate = EF.StartDate,
                    EndDate = EF.EndDate,
                    Company = EF.Company,
                    Position = EF.Position,
                }).ToList();
                customerModel.eduExpPosts = new List<EduExpPost>();
                customerModel.eduExpPosts = customerEduExpBLL.m_Rep.FindList(EF => EF.PK_App_Customer_CustomerName == customerId).Select(EF => new EduExpPost
                {
                    PK_App_Customer_CustomerName = EF.PK_App_Customer_CustomerName,
                    StartDate = EF.StartDate,
                    EndDate = EF.EndDate,
                    School = EF.School,
                    Degree = EF.Degree,
                }).ToList();
                customerModel.familyPosts = new List<FamilyPost>();
                customerModel.familyPosts = customerFamilyBLL.m_Rep.FindList(EF => EF.PK_App_Customer_CustomerName == customerId).Select(EF => new FamilyPost
                {
                    PK_App_Customer_CustomerName = EF.PK_App_Customer_CustomerName,
                    Name = EF.Name,
                    Age = EF.Age,
                    Relation = EF.Relation,
                }).ToList();
                customerModel.IntroFlag = IsCheckCustomerIntro(customer.Id);
                customerModel.IntensionFlag = IsCheckCustomerIntension(customer.Id);
                return customerModel;
            }
            else
            {
                ErrorMsg = "获取用户资料为空";
                sysLog.WriteServiceLog(customerId, "customerId:" + customerId + ",获取用户资料", "失败", "GetCustomer", "App_CustomerBLL");
                return null;
            }
        }
        #endregion

        #region 更新用户资料
        /// <summary>
        /// 更新用户资料
        /// </summary>
        /// <param name="customerPost"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public bool UpdateCustomer(CustomerPost customerPost, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(customerPost.Id, customerPost.ToString(), "开始", "UpdateCustomer", "App_CustomerBLL");
            App_Customer customer = new App_Customer();
            var now = ResultHelper.NowTime;
            customer = m_Rep.GetById(customerPost.Id);
            if (null != customer)
            {
                if (!string.IsNullOrEmpty(customerPost.Photo))
                {
                    customer.CustomerPhoto = customerPost.Photo;
                }
                if (!string.IsNullOrEmpty(customerPost.CustomerName))
                {
                    customer.CustomerName = customerPost.CustomerName;
                }
                if (!string.IsNullOrEmpty(customerPost.Phone))
                {
                    customer.Phone = customerPost.Phone;
                }
                if (!string.IsNullOrEmpty(customerPost.Sex))
                {
                    customer.Sex = customerPost.Sex;
                }
                if (customerPost.Age != 0)
                {
                    customer.Age = customerPost.Age;
                }
                if (!string.IsNullOrEmpty(customerPost.Height))
                {
                    customer.Height = customerPost.Height;
                }
                if (!string.IsNullOrEmpty(customerPost.Weight))
                {
                    customer.Weight = customerPost.Weight;
                }
                if (!string.IsNullOrEmpty(customerPost.Nation))
                {
                    customer.Nation = customerPost.Nation;
                }
                if (!string.IsNullOrEmpty(customerPost.BirthPlace))
                {
                    customer.BirthPlace = customerPost.BirthPlace;
                }
                if (!string.IsNullOrEmpty(customerPost.Introduction))
                {
                    customer.Introduction = customerPost.Introduction;
                }
                customer.WordPath = customerPost.WordPath;
                customer.WordName = customerPost.WordName;
                customer.WordExt = customerPost.WordExt;
                customer.VideoPath = customerPost.VideoPath;
                //if (!string.IsNullOrEmpty(customerPost.WordPath))
                //{
                //    customer.WordPath = customerPost.WordPath;
                //}
                //if (!string.IsNullOrEmpty(customerPost.VideoPath))
                //{
                //    customer.VideoPath = customerPost.VideoPath;
                //}
                customer.ModificationTime = now;
                customer.ModificationUserName = customer.Id;
                try
                {
                    m_Rep.Edit(customer);
                    //修改主表成功后增加关联数据
                    if (null != customerPost.workExpPosts)
                    {
                        //先删掉之前保存的集合
                        var workExps = customerWorkExpBLL.m_Rep.FindList(EF => EF.PK_App_Customer_CustomerName == customerPost.Id);
                        var Ids = workExps.Select(EF => EF.Id).ToArray();
                        customerWorkExpBLL.m_Rep.Delete(Ids);
                        foreach (var item in customerPost.workExpPosts)
                        {
                            App_CustomerWorkExp app_CustomerWorkExp = new App_CustomerWorkExp();
                            app_CustomerWorkExp.Id = ResultHelper.NewId;
                            app_CustomerWorkExp.CreateTime = now;
                            app_CustomerWorkExp.CreateUserName = customerPost.Id;
                            app_CustomerWorkExp.ModificationTime = now;
                            app_CustomerWorkExp.ModificationUserName = customerPost.Id;
                            app_CustomerWorkExp.PK_App_Customer_CustomerName = customerPost.Id;
                            app_CustomerWorkExp.StartDate = item.StartDate;
                            app_CustomerWorkExp.EndDate = item.EndDate;
                            app_CustomerWorkExp.Company = item.Company;
                            app_CustomerWorkExp.Position = item.Position;
                            customerWorkExpBLL.m_Rep.Create(app_CustomerWorkExp);
                        }
                    }
                    if (null != customerPost.eduExpPosts)
                    {
                        //先删掉之前保存的集合
                        var eduExps = customerEduExpBLL.m_Rep.FindList(EF => EF.PK_App_Customer_CustomerName == customerPost.Id);
                        var Ids = eduExps.Select(EF => EF.Id).ToArray();
                        customerEduExpBLL.m_Rep.Delete(Ids);
                        foreach (var item in customerPost.eduExpPosts)
                        {
                            App_CustomerEduExp app_CustomerEduExp = new App_CustomerEduExp();
                            app_CustomerEduExp.Id = ResultHelper.NewId;
                            app_CustomerEduExp.CreateTime = now;
                            app_CustomerEduExp.CreateUserName = customerPost.Id;
                            app_CustomerEduExp.ModificationTime = now;
                            app_CustomerEduExp.ModificationUserName = customerPost.Id;
                            app_CustomerEduExp.PK_App_Customer_CustomerName = customerPost.Id;
                            app_CustomerEduExp.StartDate = item.StartDate;
                            app_CustomerEduExp.EndDate = item.EndDate;
                            app_CustomerEduExp.School = item.School;
                            app_CustomerEduExp.Degree = item.Degree;
                            customerEduExpBLL.m_Rep.Create(app_CustomerEduExp);
                        }
                    }
                    if (null != customerPost.familyPosts)
                    {
                        //先删掉之前保存的集合
                        var families = customerFamilyBLL.m_Rep.FindList(EF => EF.PK_App_Customer_CustomerName == customerPost.Id);
                        var Ids = families.Select(EF => EF.Id).ToArray();
                        customerFamilyBLL.m_Rep.Delete(Ids);
                        foreach (var item in customerPost.familyPosts)
                        {
                            App_CustomerFamily app_CustomerFamily = new App_CustomerFamily();
                            app_CustomerFamily.Id = ResultHelper.NewId;
                            app_CustomerFamily.CreateTime = now;
                            app_CustomerFamily.CreateUserName = customerPost.Id;
                            app_CustomerFamily.ModificationTime = now;
                            app_CustomerFamily.ModificationUserName = customerPost.Id;
                            app_CustomerFamily.PK_App_Customer_CustomerName = customerPost.Id;
                            app_CustomerFamily.Name = item.Name;
                            app_CustomerFamily.Age = item.Age;
                            app_CustomerFamily.Relation = item.Relation;
                            customerFamilyBLL.m_Rep.Create(app_CustomerFamily);
                        }
                    }
                    ErrorMsg = "用户信息更新成功";
                    sysLog.WriteServiceLog(customerPost.Id, customerPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "UpdateCustomer", "App_CustomerBLL");
                    return true;
                }
                catch (Exception ex)
                {
                    ErrorMsg = "用户信息更新失败";
                    sysLog.WriteServiceLog(customerPost.Id, customerPost.ToString() + ",ErrorMsg:" + ErrorMsg + ex.Message, "结束", "UpdateCustomer", "App_CustomerBLL");
                    return false;
                }
            }
            else
            {
                ErrorMsg = "获取用户资料为空";
                sysLog.WriteServiceLog(customerPost.Id, customerPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "UpdateCustomer", "App_CustomerBLL");
                return false;
            }
        }
        #endregion

        #region 更新用户求职意向
        /// <summary>
        /// 更新用户求职意向
        /// </summary>
        /// <param name="jobIntensionPost"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public bool UpdateCustomerJobInt(JobIntensionPost jobIntensionPost, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(jobIntensionPost.PK_App_Customer_CustomerName, jobIntensionPost.ToString(), "开始", "UpdateCustomerJobInt", "App_CustomerBLL");
            App_Customer customer = new App_Customer();
            App_CustomerJobIntension jobIntension = new App_CustomerJobIntension();
            var now = ResultHelper.NowTime;
            customer = m_Rep.GetById(jobIntensionPost.PK_App_Customer_CustomerName);
            if (null == customer)
            {
                ErrorMsg = "用户不存在";
                sysLog.WriteServiceLog(jobIntensionPost.PK_App_Customer_CustomerName, jobIntensionPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "UpdateCustomerJobInt", "App_CustomerBLL");
                return false;
            }
            var jobInt = customerJobIntensionBLL.m_Rep.Find(EF => EF.PK_App_Customer_CustomerName == customer.Id);
            //判断是否已经有求职意向了
            if (string.IsNullOrEmpty(jobIntensionPost.Id) && jobInt == null)
            {
                //如果没有求职意向，则创建
                jobIntension = new App_CustomerJobIntension();
                jobIntension.Id = ResultHelper.NewId;
                jobIntension.CreateTime = now;
                jobIntension.CreateUserName = jobIntensionPost.PK_App_Customer_CustomerName;
            }
            else
            {
                jobIntension = customerJobIntensionBLL.m_Rep.GetById(jobIntensionPost.Id);
                if (null == jobIntension)
                {
                    jobIntension = jobInt;
                }
            }
            jobIntension.ModificationTime = now;
            jobIntension.ModificationUserName = jobIntensionPost.PK_App_Customer_CustomerName;
            jobIntension.PK_App_Customer_CustomerName = jobIntensionPost.PK_App_Customer_CustomerName;
            if (!string.IsNullOrEmpty(jobIntensionPost.EnumPositionType))
            {
                //工种可以是多个
                jobIntension.EnumPositionType = jobIntensionPost.EnumPositionType;
            }
            if (!string.IsNullOrEmpty(jobIntensionPost.ExpectSalary))
            {
                //期望薪资是和搜索一样
                jobIntension.ExpectSalary = jobIntensionPost.ExpectSalary;
            }
            if (!string.IsNullOrEmpty(jobIntensionPost.ExpectCountry))
            {
                //国家可以是多个
                jobIntension.ExpectCountry = jobIntensionPost.ExpectCountry;
            }
            if (!string.IsNullOrEmpty(jobIntensionPost.Skills))
            {
                jobIntension.Skills = jobIntensionPost.Skills;
            }
            if (!string.IsNullOrEmpty(jobIntensionPost.Intention))
            {
                jobIntension.Intention = jobIntensionPost.Intention;
            }
            if (!string.IsNullOrEmpty(jobIntensionPost.EnumApplyWay))
            {
                jobIntension.EnumApplyWay = jobIntensionPost.EnumApplyWay;
            }
            if (!string.IsNullOrEmpty(jobIntensionPost.CurrentPlace))
            {
                jobIntension.CurrentPlace = jobIntensionPost.CurrentPlace;
            }
            if (!string.IsNullOrEmpty(jobIntensionPost.EnumForeignLangGrade))
            {
                jobIntension.EnumForeignLangGrade = jobIntensionPost.EnumForeignLangGrade;
            }
            if (!string.IsNullOrEmpty(jobIntensionPost.AbroadExp))
            {
                jobIntension.AbroadExp = jobIntensionPost.AbroadExp;
            }
            try
            {
                if (string.IsNullOrEmpty(jobIntensionPost.Id))
                {
                    customerJobIntensionBLL.m_Rep.Create(jobIntension);
                }
                else
                {
                    customerJobIntensionBLL.m_Rep.Edit(jobIntension);
                }
                //修改主表成功后增加关联数据
                if (null != jobIntensionPost.certificatePosts)
                {
                    //先删掉之前保存的集合
                    var certificates = customerCertificateBLL.m_Rep.FindList(EF => EF.PK_App_Customer_CustomerName == jobIntensionPost.PK_App_Customer_CustomerName);
                    var Ids = certificates.Select(EF => EF.Id).ToArray();
                    customerCertificateBLL.m_Rep.Delete(Ids);
                    foreach (var item in jobIntensionPost.certificatePosts)
                    {
                        App_CustomerCertificate app_CustomerCertificate = new App_CustomerCertificate();
                        app_CustomerCertificate.Id = ResultHelper.NewId;
                        app_CustomerCertificate.CreateTime = now;
                        app_CustomerCertificate.CreateUserName = jobIntensionPost.PK_App_Customer_CustomerName;
                        app_CustomerCertificate.ModificationTime = now;
                        app_CustomerCertificate.ModificationUserName = jobIntensionPost.PK_App_Customer_CustomerName;
                        app_CustomerCertificate.PK_App_Customer_CustomerName = jobIntensionPost.PK_App_Customer_CustomerName;
                        app_CustomerCertificate.StartDate = item.StartDate;
                        app_CustomerCertificate.EndDate = item.EndDate;
                        app_CustomerCertificate.Company = item.Company;
                        app_CustomerCertificate.Name = item.Name;
                        customerCertificateBLL.m_Rep.Create(app_CustomerCertificate);
                    }
                }
                //更新用户主表中求职意向字段
                customer.JobIntension = jobIntensionPost.EnumPositionType;
                customer.EnumForeignLangGrade = jobIntensionPost.EnumForeignLangGrade;
                customer.AbroadExp = jobIntensionPost.AbroadExp;
                customer.ModificationTime = now;
                customer.ModificationUserName = jobIntensionPost.PK_App_Customer_CustomerName;
                m_Rep.Edit(customer);
                ErrorMsg = "用户求职意向更新成功";
                sysLog.WriteServiceLog(jobIntensionPost.PK_App_Customer_CustomerName, jobIntensionPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "UpdateCustomerJobInt", "App_CustomerBLL");
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = "用户信息更新失败";
                sysLog.WriteServiceLog(jobIntensionPost.PK_App_Customer_CustomerName, jobIntensionPost.ToString() + ",ErrorMsg:" + ErrorMsg + ex.Message, "结束", "UpdateCustomerJobInt", "App_CustomerBLL");
                return false;
            }
        }
        #endregion

        #region 判断用户资料是否填写
        /// <summary>
        /// 判断用户资料是否填写
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool IsCheckCustomerIntro(string customerId)
        {
            var flag = true;
            var customer = m_Rep.GetById(customerId);
            if (null == customer)
            {
                flag = false;
                return flag;
            }
            if (string.IsNullOrEmpty(customer.CustomerName))
            {
                flag = false;
            }
            if (string.IsNullOrEmpty(customer.Phone))
            {
                flag = false;
            }
            if (string.IsNullOrEmpty(customer.Sex))
            {
                flag = false;
            }
            if (customer.Age == 0)
            {
                flag = false;
            }
            if (string.IsNullOrEmpty(customer.Height))
            {
                flag = false;
            }
            if (string.IsNullOrEmpty(customer.BirthPlace))
            {
                flag = false;
            }
            return flag;
        }
        #endregion

        #region 判断用户求职意向是否填写
        /// <summary>
        /// 判断用户求职意向是否填写
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool IsCheckCustomerIntension(string customerId)
        {
            var flag = true;
            var customer = m_Rep.GetById(customerId);
            if (null == customer)
            {
                flag = false;
                return flag;
            }
            var customerIntension = customerJobIntensionBLL.m_Rep.Find(EF => EF.PK_App_Customer_CustomerName == customerId);
            if (null == customerIntension)
            {
                flag = false;
                return flag;
            }
            if (string.IsNullOrEmpty(customerIntension.EnumPositionType))
            {
                flag = false;
            }
            if (string.IsNullOrEmpty(customerIntension.ExpectSalary))
            {
                flag = false;
            }
            if (string.IsNullOrEmpty(customerIntension.ExpectCountry))
            {
                flag = false;
            }
            //if (string.IsNullOrEmpty(customerIntension.Skills))
            //{
            //    flag = false;
            //}
            //if (string.IsNullOrEmpty(customerIntension.Intention))
            //{
            //    flag = false;
            //}
            //if (string.IsNullOrEmpty(customerIntension.EnumApplyWay))
            //{
            //    flag = false;
            //}
            //if (string.IsNullOrEmpty(customerIntension.CurrentPlace))
            //{
            //    flag = false;
            //}
            if (string.IsNullOrEmpty(customerIntension.EnumForeignLangGrade))
            {
                flag = false;
            }
            if (string.IsNullOrEmpty(customerIntension.AbroadExp))
            {
                flag = false;
            }
            //判断证书
            var certificates = customerCertificateBLL.m_Rep.FindList(EF => EF.PK_App_Customer_CustomerName == customerId).Count();
            if (certificates == 0)
            {
                flag = false;
            }
            return flag;
        }
        #endregion

        #region 获取用户工友列表
        /// <summary>
        /// 获取用户工友列表
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="PageNum"></param>
        /// <param name="RecordNum"></param>
        /// <param name="DataCount"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public List<CustomerWorkmateVm> GetCustomerWorkmates(string customerId, int PageNum, int RecordNum, ref int DataCount, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(customerId, "customerId:" + customerId + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetCustomerWorkmates", "App_CustomerBLL");
            App_Customer customer = new App_Customer();
            List<CustomerWorkmateVm> customerWorkmateVms = new List<CustomerWorkmateVm>();
            var customerWorkmates = m_Rep.FindList(EF => EF.ParentId == customerId).OrderByDescending(EF => EF.ModificationTime).ToList();
            DataCount = customerWorkmates.Count;
            if (PageNum > 0)
            {
                customerWorkmates = customerWorkmates.Skip((PageNum - 1) * RecordNum).Take(RecordNum).ToList();
            }
            customerWorkmateVms = customerWorkmates.Select(EF => new CustomerWorkmateVm
            {
                Id = EF.Id,
                PK_App_Customer_CustomerName = EF.ParentId,
                Photo = EF.CustomerPhoto,
                Name = EF.CustomerName,
                Phone = EF.Phone,
                Sex = EF.Sex,
                Age = EF.Age,
                BirthPlace = EF.BirthPlace,
                CurrentPlace = EF.CurrentPlace,
                Cultural = EF.Cultural,
                EnumForeignLangGrade = EF.EnumForeignLangGrade,
                SwitchBtnPassport = EF.SwitchBtnPassport,
                AbroadExp = EF.AbroadExp,
                AbroadExpName = enumDictionary.GetDicName("App_CustomerJobIntension.AbroadExp", EF.AbroadExp),
                Introduction = EF.Introduction,
                JobIntension = EF.JobIntension,
                JobIntensionNames = positionBLL.GetNames(EF.JobIntension),
                VideoPath = EF.VideoPath,
                WordPath = EF.WordPath,
                WordName = EF.WordName,
                WordExt = EF.WordExt,
                EnumDriverLicence = EF.EnumDriverLicence,
                DriverLicenceName = enumDictionary.GetDicName("App_CustomerWorkmate.EnumDriverLicence", EF.EnumDriverLicence)
            }).ToList();
            sysLog.WriteServiceLog(customerId, "customerId:" + customerId + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetCustomerWorkmates", "App_CustomerBLL");
            return customerWorkmateVms;
        }
        #endregion

        #region 创建或者修改工友
        /// <summary>
        /// 创建或者修改工友
        /// </summary>
        /// <param name="jobIntensionPost"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public bool CreateEditCustomerWorkmate(CustomerWorkmatePost customerWorkmatePost, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(customerWorkmatePost.PK_App_Customer_CustomerName, customerWorkmatePost.ToString(), "开始", "UpdateCustomerJobInt", "App_CustomerBLL");
            App_Customer customerOwner = new App_Customer();
            App_Customer customer = new App_Customer();
            var now = ResultHelper.NowTime;
            customerOwner = m_Rep.GetById(customerWorkmatePost.PK_App_Customer_CustomerName);
            if (null == customerOwner)
            {
                ErrorMsg = "用户不存在";
                sysLog.WriteServiceLog(customerWorkmatePost.PK_App_Customer_CustomerName, customerWorkmatePost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "UpdateCustomerJobInt", "App_CustomerBLL");
                return false;
            }
            //判断是否已经有工友了
            if (string.IsNullOrEmpty(customerWorkmatePost.Id))
            {
                //如果没有工友主键，则创建
                customer.Id = ResultHelper.NewId;
                customer.CreateTime = now;
                customer.CreateUserName = customerWorkmatePost.PK_App_Customer_CustomerName;
            }
            else
            {
                customer = m_Rep.GetById(customerWorkmatePost.Id);
            }
            customer.ModificationTime = now;
            customer.ModificationUserName = customerWorkmatePost.PK_App_Customer_CustomerName;
            if (!string.IsNullOrEmpty(customerWorkmatePost.PK_App_Customer_CustomerName))
            {
                customer.ParentId = customerWorkmatePost.PK_App_Customer_CustomerName;
            }
            if (!string.IsNullOrEmpty(customerWorkmatePost.Photo))
            {
                customer.CustomerPhoto = customerWorkmatePost.Photo;
            }
            if (!string.IsNullOrEmpty(customerWorkmatePost.Name))
            {
                customer.CustomerName = customerWorkmatePost.Name;
            }
            if (!string.IsNullOrEmpty(customerWorkmatePost.Phone))
            {
                customer.Phone = customerWorkmatePost.Phone;
            }
            if (!string.IsNullOrEmpty(customerWorkmatePost.Sex))
            {
                customer.Sex = customerWorkmatePost.Sex;
            }
            if (customerWorkmatePost.Age != 0)
            {
                customer.Age = customerWorkmatePost.Age;
            }
            if (!string.IsNullOrEmpty(customerWorkmatePost.BirthPlace))
            {
                customer.BirthPlace = customerWorkmatePost.BirthPlace;
            }
            if (!string.IsNullOrEmpty(customerWorkmatePost.CurrentPlace))
            {
                customer.CurrentPlace = customerWorkmatePost.CurrentPlace;
            }
            if (!string.IsNullOrEmpty(customerWorkmatePost.Cultural))
            {
                customer.Cultural = customerWorkmatePost.Cultural;
            }
            if (!string.IsNullOrEmpty(customerWorkmatePost.EnumForeignLangGrade))
            {
                customer.EnumForeignLangGrade = customerWorkmatePost.EnumForeignLangGrade;
            }
            if (!string.IsNullOrEmpty(customerWorkmatePost.SwitchBtnPassport))
            {
                customer.SwitchBtnPassport = customerWorkmatePost.SwitchBtnPassport;
            }
            if (!string.IsNullOrEmpty(customerWorkmatePost.AbroadExp))
            {
                customer.AbroadExp = customerWorkmatePost.AbroadExp;
            }
            if (!string.IsNullOrEmpty(customerWorkmatePost.Introduction))
            {
                customer.Introduction = customerWorkmatePost.Introduction;
            }
            customer.WordPath = customerWorkmatePost.WordPath;
            customer.WordName = customerWorkmatePost.WordName;
            customer.WordExt = customerWorkmatePost.WordExt;
            customer.VideoPath = customerWorkmatePost.VideoPath;
            //默认记录值是1:APP注册用户,为了雇主端推荐功能      以有推荐标识的简历，排列在前面。之后是app上面注册的工人简历。最后是后台管理系统导入的数据
            customer.EnumCustomerLevel = "1";
            if (!string.IsNullOrEmpty(customerWorkmatePost.JobIntension))
            {
                customer.JobIntension = customerWorkmatePost.JobIntension;
            }
            if (!string.IsNullOrEmpty(customerWorkmatePost.EnumDriverLicence))
            {
                customer.EnumDriverLicence = customerWorkmatePost.EnumDriverLicence;
            }
            try
            {
                if (string.IsNullOrEmpty(customerWorkmatePost.Id))
                {
                    m_Rep.Create(customer);
                }
                else
                {
                    m_Rep.Edit(customer);
                }
                ErrorMsg = "用户工友更新成功";
                sysLog.WriteServiceLog(customerWorkmatePost.PK_App_Customer_CustomerName, customerWorkmatePost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "UpdateCustomerJobInt", "App_CustomerBLL");
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = "用户工友更新失败";
                sysLog.WriteServiceLog(customerWorkmatePost.PK_App_Customer_CustomerName, customerWorkmatePost.ToString() + ",ErrorMsg:" + ErrorMsg + ex.Message, "结束", "UpdateCustomerJobInt", "App_CustomerBLL");
                return false;
            }
        }
        #endregion

        #region 获取用户求职意向
        /// <summary>
        /// 获取用户求职意向
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public CustomerJobIntensionVm GetCustomerIntension(string customerId, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(customerId, "customerId:" + customerId, "开始", "GetCustomerIntension", "App_CustomerBLL");
            CustomerJobIntensionVm customerJobIntensionVm = new CustomerJobIntensionVm();
            var customerJobIntension = customerJobIntensionBLL.m_Rep.Find(EF => EF.PK_App_Customer_CustomerName == customerId);
            if (null == customerJobIntension)
            {
                ErrorMsg = "获取用户求职意向为空";
                sysLog.WriteServiceLog(customerId, "customerId:" + customerId + ",ErrorMsg:" + ErrorMsg, "结束", "GetCustomerIntension", "App_CustomerBLL");
                return null;
            }
            customerJobIntensionVm.Id = customerJobIntension.Id;
            customerJobIntensionVm.PK_App_Customer_CustomerName = customerJobIntension.PK_App_Customer_CustomerName;
            customerJobIntensionVm.EnumPositionType = customerJobIntension.EnumPositionType;
            customerJobIntensionVm.PositionNames = positionBLL.GetNames(customerJobIntension.EnumPositionType);
            customerJobIntensionVm.ExpectSalary = customerJobIntension.ExpectSalary;
            customerJobIntensionVm.ExpectCountry = customerJobIntension.ExpectCountry;
            customerJobIntensionVm.Skills = customerJobIntension.Skills;
            customerJobIntensionVm.Intention = customerJobIntension.Intention;
            customerJobIntensionVm.EnumApplyWay = customerJobIntension.EnumApplyWay;
            customerJobIntensionVm.CurrentPlace = customerJobIntension.CurrentPlace;
            customerJobIntensionVm.EnumForeignLangGrade = customerJobIntension.EnumForeignLangGrade;
            customerJobIntensionVm.AbroadExp = customerJobIntension.AbroadExp;
            customerJobIntensionVm.certificatePosts = new List<CertificatePost>();
            customerJobIntensionVm.certificatePosts = customerCertificateBLL.m_Rep.FindList(EF => EF.PK_App_Customer_CustomerName == customerId).Select(EF => new CertificatePost
            {
                PK_App_Customer_CustomerName = EF.PK_App_Customer_CustomerName,
                StartDate = EF.StartDate,
                EndDate = EF.EndDate,
                Company = EF.Company,
                Name = EF.Name,
            }).ToList();
            ErrorMsg = "获取用户求职意向成功";
            sysLog.WriteServiceLog(customerId, "customerId:" + customerId + ",ErrorMsg:" + ErrorMsg, "结束", "GetCustomerIntension", "App_CustomerBLL");
            return customerJobIntensionVm;
        }
        #endregion

        #region 获取用户信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="WorkmateId"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public CustomerWorkmateVm GetCustomerWorkmate(string customerId, string WorkmateId, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(customerId, "WorkmateId:" + WorkmateId, "开始", "GetCustomerWorkmate", "App_CustomerBLL");
            CustomerWorkmateVm customerWorkmateVm = new CustomerWorkmateVm();
            var customerWorkmate = m_Rep.GetById(WorkmateId);
            if (null == customerWorkmate)
            {
                ErrorMsg = "获取用户工友为空";
                sysLog.WriteServiceLog(customerId, "WorkmateId:" + WorkmateId + ",ErrorMsg:" + ErrorMsg, "结束", "GetCustomerWorkmate", "App_CustomerBLL");
                return null;
            }
            customerWorkmateVm.Id = customerWorkmate.Id;
            customerWorkmateVm.PK_App_Customer_CustomerName = customerWorkmate.ParentId;
            customerWorkmateVm.Photo = customerWorkmate.CustomerPhoto;
            customerWorkmateVm.Name = customerWorkmate.CustomerName;
            customerWorkmateVm.Phone = customerWorkmate.Phone;
            customerWorkmateVm.Sex = customerWorkmate.Sex;
            customerWorkmateVm.Age = customerWorkmate.Age;
            customerWorkmateVm.BirthPlace = customerWorkmate.BirthPlace;
            customerWorkmateVm.CurrentPlace = customerWorkmate.CurrentPlace;
            customerWorkmateVm.Cultural = customerWorkmate.Cultural;
            customerWorkmateVm.EnumForeignLangGrade = customerWorkmate.EnumForeignLangGrade;
            customerWorkmateVm.ForeignLangGrade = enumDictionary.GetDicName("App_CustomerJobIntension.EnumForeignLangGrade", customerWorkmate.EnumForeignLangGrade);
            customerWorkmateVm.SwitchBtnPassport = customerWorkmate.SwitchBtnPassport;
            customerWorkmateVm.AbroadExp = customerWorkmate.AbroadExp;
            customerWorkmateVm.AbroadExpName = enumDictionary.GetDicName("App_CustomerJobIntension.AbroadExp", customerWorkmate.AbroadExp);
            customerWorkmateVm.Introduction = customerWorkmate.Introduction;
            customerWorkmateVm.VideoPath = customerWorkmate.VideoPath;
            customerWorkmateVm.WordPath = customerWorkmate.WordPath;
            customerWorkmateVm.WordName = customerWorkmate.WordName;
            customerWorkmateVm.WordExt = customerWorkmate.WordExt;
            customerWorkmateVm.JobIntension = customerWorkmate.JobIntension;
            customerWorkmateVm.JobIntensionNames = positionBLL.GetNames(customerWorkmate.JobIntension);
            customerWorkmateVm.EnumDriverLicence = customerWorkmate.EnumDriverLicence;
            customerWorkmateVm.DriverLicenceName = enumDictionary.GetDicName("App_CustomerWorkmate.EnumDriverLicence", customerWorkmate.EnumDriverLicence);
            //获取工人求职意向
            var customerJobIntensionVm = customerJobIntensionBLL.m_Rep.Find(EF => EF.PK_App_Customer_CustomerName == customerWorkmate.Id);
            if (null != customerJobIntensionVm)
            {
                customerWorkmateVm.customerJobIntension.ExpectSalary = customerJobIntensionVm.ExpectSalary;
                customerWorkmateVm.customerJobIntension.ExpectCountry = countryRepository.GetNames(customerJobIntensionVm.ExpectCountry);
            }
            //判断用户是否被收藏
            customerWorkmateVm.CustomerCollectId = "";
            var customerCollect = customerCollectRepository.Find(EF => EF.PK_App_Customer_CustomerName == customerId && EF.WorkerId == WorkmateId);
            if (null != customerCollect)
            {
                customerWorkmateVm.CustomerCollectId = customerCollect.Id;
            }
            ErrorMsg = "获取用户工友成功";
            sysLog.WriteServiceLog(customerId, "WorkmateId:" + WorkmateId + ",ErrorMsg:" + ErrorMsg, "结束", "GetCustomerWorkmate", "App_CustomerBLL");
            return customerWorkmateVm;
        }
        #endregion

        #region 收藏简历
        /// <summary>
        /// 收藏简历
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="WorkerId"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public string CollectCustomer(string UserId, string WorkerId, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "WorkerId:" + WorkerId, "开始", "CollectCustomer", "App_CustomerBLL");
            App_CustomerCollect customerCollect = new App_CustomerCollect();
            var now = ResultHelper.NowTime;
            customerCollect.Id = ResultHelper.NewId;
            customerCollect.CreateTime = now;
            customerCollect.CreateUserName = UserId;
            customerCollect.ModificationTime = now;
            customerCollect.ModificationUserName = UserId;
            customerCollect.PK_App_Customer_CustomerName = UserId;
            customerCollect.WorkerId = WorkerId;
            try
            {
                customerCollectRepository.Create(customerCollect);
                ErrorMsg = "简历收藏成功";
                sysLog.WriteServiceLog(UserId, "WorkerId:" + WorkerId + ",ErrorMsg:" + ErrorMsg, "结束", "CollectCustomer", "App_CustomerBLL");
                return customerCollect.Id;
            }
            catch (Exception ex)
            {
                ErrorMsg = "简历收藏失败";
                sysLog.WriteServiceLog(UserId, "WorkerId:" + WorkerId + ",ErrorMsg:" + ErrorMsg + ex.Message, "结束", "CollectCustomer", "App_CustomerBLL");
                return null;
            }
        }
        #endregion

        #region 获取用户收藏简历列表
        /// <summary>
        /// 获取用户收藏简历列表
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="PageNum"></param>
        /// <param name="RecordNum"></param>
        /// <param name="DataCount"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public List<ApplyJobUserVm> GetCollectCustomers(string customerId, int PageNum, int RecordNum, ref int DataCount, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(customerId, "customerId:" + customerId + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetCollectCustomers", "App_CustomerBLL");
            List<ApplyJobUserVm> applyJobUserVms = new List<ApplyJobUserVm>();
            var customerCollects = customerCollectRepository.FindList(EF => EF.PK_App_Customer_CustomerName == customerId).OrderByDescending(EF => EF.CreateTime).ToList();
            DataCount = customerCollects.Count;
            if (PageNum > 0)
            {
                customerCollects = customerCollects.Skip((PageNum - 1) * RecordNum).Take(RecordNum).ToList();
            }
            foreach (var item in customerCollects)
            {
                var customerWorkmate = m_Rep.GetById(item.WorkerId);
                ApplyJobUserVm applyJobUserVm = new ApplyJobUserVm();
                applyJobUserVm.CustomerId = customerWorkmate.Id;
                applyJobUserVm.CustomerName = customerWorkmate.CustomerName;
                applyJobUserVm.Photo = customerWorkmate.CustomerPhoto;
                applyJobUserVm.CreateTime = item.CreateTime;
                applyJobUserVm.Age = customerWorkmate.Age;
                applyJobUserVm.BirthPlace = customerWorkmate.BirthPlace;
                applyJobUserVm.Sex = customerWorkmate.Sex;
                applyJobUserVm.AbroadExp = enumDictionary.GetDicName("App_CustomerJobIntension.AbroadExp", customerWorkmate.AbroadExp);
                applyJobUserVm.EnumDriverLicence = customerWorkmate.EnumDriverLicence;
                applyJobUserVm.DriverLicence = enumDictionary.GetDicName("App_CustomerWorkmate.EnumDriverLicence", customerWorkmate.EnumDriverLicence);
                applyJobUserVm.JobIntension = customerWorkmate.JobIntension;
                applyJobUserVm.JobIntensionName = positionBLL.GetNames(customerWorkmate.JobIntension);
                applyJobUserVm.SwitchBtnRecommend = customerWorkmate.SwitchBtnRecommend;
                applyJobUserVm.VideoPath = customerWorkmate.VideoPath;
                applyJobUserVms.Add(applyJobUserVm);
            }
            sysLog.WriteServiceLog(customerId, "customerId:" + customerId + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetCustomerWorkmates", "App_CustomerBLL");
            return applyJobUserVms;
        }
        #endregion

        #region 取消收藏简历
        /// <summary>
        /// 取消收藏简历
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="WorkerId"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public bool UnCollectCustomer(string UserId, string CustomerCollectId, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "CustomerCollectId:" + CustomerCollectId, "开始", "UnCollectCustomer", "App_CustomerBLL");
            try
            {
                customerCollectRepository.Delete(CustomerCollectId);
                ErrorMsg = "简历取消收藏成功";
                sysLog.WriteServiceLog(UserId, "CustomerCollectId:" + CustomerCollectId + ",ErrorMsg:" + ErrorMsg, "结束", "UnCollectCustomer", "App_CustomerBLL");
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = "简历取消收藏失败";
                sysLog.WriteServiceLog(UserId, "CustomerCollectId:" + CustomerCollectId + ",ErrorMsg:" + ErrorMsg + ex.Message, "结束", "UnCollectCustomer", "App_CustomerBLL");
                return false;
            }
        }
        #endregion

        #region 企业认证
        /// <summary>
        /// 企业认证
        /// </summary>
        /// <param name="customerPost"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public bool UpdateCompany(CompanyPost companyPost, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(companyPost.UserId, companyPost.ToString(), "开始", "UpdateCompany", "App_CustomerBLL");
            var now = ResultHelper.NowTime;
            bool CreateFlag = true;
            App_Company app_Company = new App_Company();
            app_Company.Id = ResultHelper.NewId;
            app_Company.CreateTime = now;
            app_Company.CreateUserName = companyPost.UserId;
            if (!string.IsNullOrEmpty(companyPost.Id))
            {
                CreateFlag = false;
                app_Company = companyBLL.m_Rep.GetById(companyPost.Id);
            }
            app_Company.ModificationTime = now;
            app_Company.ModificationUserName = companyPost.UserId;
            app_Company.CompanyName = companyPost.CompanyName;
            app_Company.CompanyShortName = companyPost.CompanyShortName;
            app_Company.Industry = companyPost.Industry;
            app_Company.EnumCompanySize = companyPost.EnumCompanySize;
            app_Company.BusinessLicence = companyPost.BusinessLicence;
            app_Company.SwitchBtnApply = companyPost.SwitchBtnApply;
            app_Company.PK_App_Customer_CustomerName = companyPost.PK_App_Customer_CustomerName;
            try
            {
                if (CreateFlag)
                {
                    companyBLL.m_Rep.Create(app_Company);
                }
                else
                {
                    companyBLL.m_Rep.Edit(app_Company);
                }
                ErrorMsg = "用户企业认证信息更新成功";
                sysLog.WriteServiceLog(companyPost.UserId, companyPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "UpdateCompany", "App_CustomerBLL");
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = "用户企业认证信息更新失败";
                sysLog.WriteServiceLog(companyPost.UserId, companyPost.ToString() + ",ErrorMsg:" + ErrorMsg + ex.Message, "结束", "UpdateCompany", "App_CustomerBLL");
                return false;
            }
        }
        #endregion

        #region 获取企业认证
        /// <summary>
        /// 获取企业认证
        /// </summary>
        /// <param name="customerPost"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public CompanyVm GetCompany(string UserId, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "UserId:" + UserId, "开始", "GetCompany", "App_CustomerBLL");
            CompanyVm companyVm = new CompanyVm();
            var app_Company = companyBLL.m_Rep.Find(EF => EF.PK_App_Customer_CustomerName == UserId);
            if (null == app_Company)
            {
                ErrorMsg = "获取企业认证为空";
                sysLog.WriteServiceLog(UserId, "UserId:" + UserId + ",ErrorMsg:" + ErrorMsg, "结束", "GetCompany", "App_CustomerBLL");
                return null;
            }
            companyVm.Id = app_Company.Id;
            companyVm.CompanyName = app_Company.CompanyName;
            companyVm.CompanyShortName = app_Company.CompanyShortName;
            companyVm.Industry = app_Company.Industry;
            companyVm.EnumCompanySize = app_Company.EnumCompanySize;
            companyVm.CompanySize = enumDictionary.GetDicName("App_Company.EnumCompanySize", app_Company.EnumCompanySize);
            companyVm.BusinessLicence = app_Company.BusinessLicence;
            companyVm.SwitchBtnApply = app_Company.SwitchBtnApply;
            companyVm.PK_App_Customer_CustomerName = app_Company.PK_App_Customer_CustomerName;
            companyVm.App_Customer_CustomerName = GetCustomerName(app_Company.PK_App_Customer_CustomerName);
            ErrorMsg = "获取企业认证成功";
            sysLog.WriteServiceLog(UserId, "UserId:" + UserId + ",ErrorMsg:" + ErrorMsg, "结束", "GetCompany", "App_CustomerBLL");
            return companyVm;
        }
        #endregion

        #region 重写获取列表
        /// <summary>
        /// 重写获取列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="queryStr"></param>
        /// <returns></returns>
        public List<App_CustomerModel> GetResumeList(ref GridPager pager, CustomerResumeQuery customerResumeQuery)
        {
            IQueryable<App_Customer> queryData = m_Rep.GetList(EF => EF.ParentId != null);
            if (!customerResumeQuery.AdminFlag)
            {
                queryData = queryData.Where(a => a.ParentId == customerResumeQuery.CustomerId);
            }
            if (!string.IsNullOrWhiteSpace(customerResumeQuery.CustomerName))
            {
                queryData = queryData.Where(a => a.CustomerName != null && a.CustomerName.Contains(customerResumeQuery.CustomerName));
            }
            if (customerResumeQuery.Recommend)
            {
                queryData = queryData.Where(EF => EF.SwitchBtnRecommend == "1");
            }
            if (customerResumeQuery.Video)
            {
                queryData = queryData.Where(EF => EF.VideoPath != "" && EF.VideoPath != null);
            }
            if (customerResumeQuery.DriverLicence)
            {
                queryData = queryData.Where(EF => EF.EnumDriverLicence != "0" && EF.EnumDriverLicence != null);
            }
            if (customerResumeQuery.AbroadExp)
            {
                queryData = queryData.Where(EF => EF.EnumDriverLicence == "1");
            }
            if (!string.IsNullOrWhiteSpace(customerResumeQuery.Sex))
            {
                queryData = queryData.Where(a => a.Sex == customerResumeQuery.Sex);
            }
            if (!string.IsNullOrWhiteSpace(customerResumeQuery.CustomerPhone))
            {
                queryData = queryData.Where(a => a.Phone != null && a.Phone.Contains(customerResumeQuery.CustomerPhone));
            }
            if (customerResumeQuery.WorkLimitAgeHigh != 0)
            {
                queryData = queryData.Where(a => a.Age <= customerResumeQuery.WorkLimitAgeHigh);
            }
            if (customerResumeQuery.WorkLimitAgeLow != 0)
            {
                queryData = queryData.Where(a => a.Age >= customerResumeQuery.WorkLimitAgeLow);
            }
            if (!string.IsNullOrWhiteSpace(customerResumeQuery.JobIntension))
            {
                var arrJobIntension = customerResumeQuery.JobIntension.Split(',').ToList();
                queryData = queryData.ToList().Where(a => a.JobIntension != null && a.JobIntension.Split(',').Intersect(arrJobIntension).Count() > 0).AsQueryable();
            }
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
        #endregion
    }
}
