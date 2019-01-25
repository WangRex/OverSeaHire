using Apps.Common;
using Apps.Models;
using System.Linq;
using System.Collections.Generic;
using Apps.Models.App;
using Microsoft.Practices.Unity;
using Apps.DAL.Sys;
using Apps.BLL.Sys;
using Apps.DAL.App;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Data.Entity.Infrastructure;

namespace Apps.BLL.App
{
    public partial class App_RequirementBLL
    {
        #region Reps
        [Dependency]
        public SysLogRepository sysLog { get; set; }
        [Dependency]
        public EnumDictionaryBLL enumDictionary { get; set; }
        [Dependency]
        public App_RequirementSearchRepository requirementSearchRepository { get; set; }
        [Dependency]
        public App_RequirementCollectRepository requirementCollectRepository { get; set; }
        [Dependency]
        public App_CustomerRepository customerRepository { get; set; }
        [Dependency]
        public App_CountryRepository countryRepository { get; set; }
        [Dependency]
        public App_ApplyJobRepository applyJobRepository { get; set; }
        [Dependency]
        public App_ApplyJobRecordRepository applyJobRecordRepository { get; set; }
        [Dependency]
        public SysAreasRepository sysAreasRepository { get; set; }
        [Dependency]
        public App_PositionBLL app_PositionBLL { get; set; }
        [Dependency]
        public App_CustomerJobIntensionRepository customerJobIntensionRepository { get; set; }
        [Dependency]
        public SysMessageRepository sysMessageRepository { get; set; }
        [Dependency]
        public App_CustomerCollectRepository customerCollectRepository { get; set; }
        [Dependency]
        public App_CustomerPosSearchRepository customerPosSearchRepository { get; set; }
        [Dependency]
        public App_RequirementInviteRepository requirementInviteRepository { get; set; }
        #endregion

        #region 获取首页需求列表
        /// <summary>
        /// 获取首页需求列表
        /// </summary>
        /// <param name="requireSearchForm"></param>
        /// <param name="DataCount"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public RequirementHomePage GetApp_Requirements(RequireSearchForm requireSearchForm, ref int DataCount, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(requireSearchForm.UserId, requireSearchForm.ToString(), "开始", "GetApp_Requirements", "App_RequirementBLL");
            int PageNum = requireSearchForm.PageNum, RecordNum = requireSearchForm.RecordNum;
            RequirementHomePage requirementHomePage = new RequirementHomePage();
            List<RequirementVm> requirementVms = new List<RequirementVm>();
            List<PositionTreeVm> positionTreeVms = new List<PositionTreeVm>();
            string PosIds = "";
            var now = ResultHelper.NowTime;
            var app_Requirements = m_Rep.FindList().OrderByDescending(EF => EF.ModificationTime).ToList();
            if (!string.IsNullOrEmpty(requireSearchForm.QueryStr))
            {
                //如果输入的内容不为空，则增加搜索历史
                App_RequirementSearch app_RequirementSearch = new App_RequirementSearch();
                app_RequirementSearch.Id = ResultHelper.NewId;
                app_RequirementSearch.CreateTime = now;
                app_RequirementSearch.CreateUserName = requireSearchForm.UserId;
                app_RequirementSearch.ModificationTime = now;
                app_RequirementSearch.ModificationUserName = requireSearchForm.UserId;
                app_RequirementSearch.PK_App_Customer_CustomerName = requireSearchForm.UserId;
                app_RequirementSearch.Content = requireSearchForm.QueryStr;
                requirementSearchRepository.Create(app_RequirementSearch);
                //获取国家主键
                var countries = countryRepository.FindList(EF => EF.Name != null && EF.Name.Contains(requireSearchForm.QueryStr));
                var countryIds = countries.Select(EF => EF.Id);
                //获取职位主键
                var positions = app_PositionBLL.m_Rep.FindList(EF => EF.Name != null && EF.Name.Contains(requireSearchForm.QueryStr));
                var positionIds = positions.Select(EF => EF.Id);
                app_Requirements = app_Requirements.Where(EF => positionIds.Contains(EF.PK_App_Position_Name) || countryIds.Contains(EF.PK_App_Country_Name)).ToList();
            }
            if (!string.IsNullOrEmpty(requireSearchForm.PublishUserId))
            {
                //按照发布人获取需求列表
                app_Requirements = app_Requirements.Where(EF => EF.PK_App_Customer_CustomerName == requireSearchForm.PublishUserId).ToList();
            }
            app_Requirements = GetRequirements(requireSearchForm, app_Requirements);
            DataCount = app_Requirements.Count;
            if (DataCount == 0)
            {
                ErrorMsg = "获取需求列表为空";
                sysLog.WriteServiceLog(requireSearchForm.UserId, requireSearchForm.ToString() + ErrorMsg, "结束", "GetApp_Requirements", "App_RequirementBLL");
                return requirementHomePage;
            }
            if (PageNum > 0)
            {
                app_Requirements = app_Requirements.Skip((PageNum - 1) * RecordNum).Take(RecordNum).ToList();
            }
            foreach (var item in app_Requirements)
            {
                var requirementVm = new RequirementVm();
                requirementVm.Id = item.Id;
                requirementVm.Title = item.Title;
                var country = countryRepository.GetById(item.PK_App_Country_Name);
                if (null != country)
                {
                    requirementVm.CountryName = country.Name;
                    requirementVm.CountryImg = country.CountryImg;
                }
                requirementVm.SalaryLow = GetSalary(item.SalaryLow);
                requirementVm.SalaryHigh = GetSalary(item.SalaryHigh);
                requirementVm.PublishDate = item.PublishDate;
                requirementVm.Tag = item.Tag.Split(',').ToList();
                requirementVm.CompanyName = string.IsNullOrEmpty(item.CompanyName) ? "雇主直招" : item.CompanyName;
                requirementVm.ApplyCount = applyJobRepository.GetList(EF => EF.PK_App_Requirement_Title == item.Id).Count().ToString(); ;
                requirementVm.TransactProvinceCode = item.TransactProvince;
                requirementVm.TransactProvinceName = sysAreasRepository.GetAreasName(item.TransactProvince);
                requirementVm.PK_App_Position_Name = item.PK_App_Position_Name;
                requirementVm.PositionName = app_PositionBLL.GetName(item.PK_App_Position_Name);
                requirementVm.SwitchBtnOpen = item.SwitchBtnOpen;
                requirementVm.ClickNumber = item.ClickNumber;
                requirementVm.InterviewNumber = applyJobRepository.FindList(EF => EF.PK_App_Requirement_Title == item.Id).ToList().Where(EF => Utils.ObjToInt(EF.CurrentStep, 0) > 3).Count();
                requirementVm.CompleteNumber = applyJobRepository.FindList(EF => EF.PK_App_Requirement_Title == item.Id && EF.EnumApplyStatus == "0").Count();
                requirementVms.Add(requirementVm);
            }
            requirementHomePage.requirementVms = requirementVms;
            PosIds = string.Join(",", requirementVms.Select(EF => EF.PK_App_Position_Name).Distinct().ToArray());
            var PosIdList = app_PositionBLL.GetParentIds(PosIds);
            positionTreeVms = app_PositionBLL.GetAppPositions(string.Join(",", PosIdList), ref ErrorMsg);
            requirementHomePage.positionTreeVms = positionTreeVms;
            ErrorMsg = "获取需求列表成功";
            sysLog.WriteServiceLog(requireSearchForm.UserId, requireSearchForm.ToString() + ",DataCount:" + DataCount + ErrorMsg, "结束", "GetApp_Requirements", "App_RequirementBLL");
            return requirementHomePage;
        }

        private List<App_Requirement> GetRequirements(RequireSearchForm requireSearchForm, List<App_Requirement> app_Requirements)
        {
            if (!string.IsNullOrEmpty(requireSearchForm.SwitchBtnOpen))
            {
                app_Requirements = app_Requirements.Where(EF => EF.SwitchBtnOpen == requireSearchForm.SwitchBtnOpen).ToList();
            }
            if (!string.IsNullOrEmpty(requireSearchForm.PK_App_Position_Name))
            {
                //工种是多选的
                app_Requirements = app_Requirements.Where(EF => requireSearchForm.PK_App_Position_Name.Contains(EF.PK_App_Position_Name)).ToList();
            }
            if (!string.IsNullOrEmpty(requireSearchForm.Country))
            {
                var countries = countryRepository.FindList(EF => EF.Name != null && EF.Name.Contains(requireSearchForm.Country));
                var countryIds = countries.Select(EF => EF.Id);
                //国家是多选的
                app_Requirements = app_Requirements.Where(EF => countryIds.Contains(EF.PK_App_Country_Name)).ToList();
            }
            if (!string.IsNullOrEmpty(requireSearchForm.TransactProvince))
            {
                //办理省是多选的
                var ProList = requireSearchForm.TransactProvince.Split(',');
                app_Requirements = app_Requirements.Where(EF => ProList.Contains(EF.TransactProvince)).ToList();
            }
            if (requireSearchForm.SalaryMin != 0)
            {
                app_Requirements = app_Requirements.Where(EF => Utils.ObjToInt(EF.SalaryLow, 0) >= requireSearchForm.SalaryMin).ToList();
            }
            if (requireSearchForm.SalaryMax != 0)
            {
                app_Requirements = app_Requirements.Where(EF => Utils.ObjToInt(EF.SalaryHigh, 0) <= requireSearchForm.SalaryMax).ToList();
            }
            if (!string.IsNullOrEmpty(requireSearchForm.TotalYear))
            {
                var TotalYear = requireSearchForm.TotalYear;
                switch (TotalYear)
                {
                    case "0":
                        break;
                    case "1":
                        app_Requirements = app_Requirements.Where(EF => EF.TotalYear == 1).ToList();
                        break;
                    case "2":
                        app_Requirements = app_Requirements.Where(EF => EF.TotalYear == 2).ToList();
                        break;
                    case "3":
                        app_Requirements = app_Requirements.Where(EF => EF.TotalYear >= 3).ToList();
                        break;
                    default:
                        break;
                }
            }
            if ("1".Equals(requireSearchForm.IsRecommend))
            {
                app_Requirements = app_Requirements.Where(EF => EF.SwitchBtnRecommend == "1").ToList();
            }
            if ("1".Equals(requireSearchForm.IsLatest))
            {
                app_Requirements = app_Requirements.OrderByDescending(EF => EF.ModificationTime).ToList();
            }
            if ("1".Equals(requireSearchForm.IsHighSalary))
            {
                app_Requirements = app_Requirements.OrderByDescending(EF => EF.SalaryHigh).ToList();
            }
            if ("1".Equals(requireSearchForm.IsHot))
            {
                app_Requirements = app_Requirements.OrderByDescending(EF => EF.ApplyCount).ToList();
            }

            return app_Requirements;
        }
        #endregion

        #region 获取标题
        /// <summary>
        /// 获取标题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTitle(string id)
        {
            string name = "";
            if (null != id)
            {
                var model = GetById(id);
                if (null != model)
                {
                    name = model.Title;
                }
            }
            return name;
        }
        #endregion

        #region 获取薪资万
        /// <summary>
        /// 获取薪资万
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetSalary(string salary)
        {
            string salaryName = "";
            if (!string.IsNullOrEmpty(salary))
            {
                var salaryD = Utils.ObjToDecimal(salary, 0) / 10000;
                salaryName = salaryD.ToString() + "万";
            }
            return salaryName;
        }
        #endregion

        #region 获取需求详情
        /// <summary>
        /// 获取需求详情
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RequirementId"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public RequirementDetailVm GetApp_Requirement(string UserId, string RequirementId, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "RequirementId:" + RequirementId, "开始", "GetApp_Requirement", "App_RequirementBLL");
            RequirementDetailVm requirementDetailVm = new RequirementDetailVm();
            var app_Requirement = m_Rep.GetById(RequirementId);
            if (null == app_Requirement)
            {
                ErrorMsg = "获取需求详情为空";
                sysLog.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",ErrorMsg:" + ErrorMsg, "结束", "GetApp_Requirement", "App_RequirementBLL");
                return null;
            }
            //累加需求点击量
            app_Requirement.ClickNumber++;
            m_Rep.Edit(app_Requirement);
            LinqHelper.ModelTrans(app_Requirement, requirementDetailVm);
            requirementDetailVm.Id = app_Requirement.Id;
            requirementDetailVm.Title = app_Requirement.Title;
            requirementDetailVm.PK_App_Position_Name = app_Requirement.PK_App_Position_Name;
            requirementDetailVm.PositionName = app_PositionBLL.GetName(app_Requirement.PK_App_Position_Name);
            requirementDetailVm.WorkPlace = app_Requirement.WorkPlace;
            requirementDetailVm.WorkLimitSex = app_Requirement.WorkLimitSex;
            requirementDetailVm.WorkLimitAgeLow = app_Requirement.WorkLimitAgeLow.ToString();
            requirementDetailVm.WorkLimitAgeHigh = app_Requirement.WorkLimitAgeHigh.ToString();
            requirementDetailVm.EnumWorkLimitDegree = app_Requirement.EnumWorkLimitDegree;
            requirementDetailVm.WorkLimitDegree = enumDictionary.GetDicName("App_Requirement.EnumWorkLimitDegree", app_Requirement.EnumWorkLimitDegree);
            requirementDetailVm.SalaryLow = GetSalary(app_Requirement.SalaryLow);
            requirementDetailVm.SalaryHigh = GetSalary(app_Requirement.SalaryHigh);
            requirementDetailVm.PromiseMoney = app_Requirement.PromiseMoney;
            requirementDetailVm.ServiceMoney = app_Requirement.ServiceMoney;
            requirementDetailVm.ServiceTailMoney = app_Requirement.ServiceTailMoney;
            requirementDetailVm.TotalServiceMoney = app_Requirement.TotalServiceMoney;
            requirementDetailVm.PublishDate = app_Requirement.PublishDate;
            requirementDetailVm.TotalHire = app_Requirement.TotalHire.ToString();
            requirementDetailVm.Tag = app_Requirement.Tag.Split(',').ToList();
            requirementDetailVm.Description = app_Requirement.Description;
            requirementDetailVm.ClickNumber = app_Requirement.ClickNumber;
            requirementDetailVm.SwitchBtnRecommend = app_Requirement.SwitchBtnRecommend;
            requirementDetailVm.SwitchBtnOpen = app_Requirement.SwitchBtnOpen;
            requirementDetailVm.TotalYear = app_Requirement.TotalYear.ToString();
            requirementDetailVm.ApplyCount = applyJobRepository.GetList(EF => EF.PK_App_Requirement_Title == app_Requirement.Id).Count().ToString();
            requirementDetailVm.CompanyName = string.IsNullOrEmpty(requirementDetailVm.CompanyName) ? "雇主直招" : requirementDetailVm.CompanyName;
            requirementDetailVm.SwitchBtnOpen = app_Requirement.SwitchBtnOpen;
            var customer = customerRepository.GetById(app_Requirement.PK_App_Customer_CustomerName);
            if (null != customer)
            {
                requirementDetailVm.PublishUserId = customer.Id;
                requirementDetailVm.PublishUserName = customer.CustomerName;
                requirementDetailVm.PublishUserPhoto = customer.CustomerPhoto;
                requirementDetailVm.PublisherPhone = customer.Phone;
                requirementDetailVm.PublisherWeChatNumber = customer.WeChatNumber;
                requirementDetailVm.PublishUserCompany = "";
            }
            var country = countryRepository.GetById(app_Requirement.PK_App_Country_Name);
            if (null != country)
            {
                requirementDetailVm.CountryName = country.Name;
                requirementDetailVm.CountryImg = country.CountryImg;
            }
            requirementDetailVm.RequirementCollId = "";
            //增加获取本需求是否收藏
            var requirementCollect = requirementCollectRepository.Find(EF => EF.PK_App_Customer_CustomerName == UserId && EF.PK_App_Requirement_Title == RequirementId);
            if (null != requirementCollect)
            {
                requirementDetailVm.RequirementCollId = requirementCollect.Id;
            }
            //获取推荐用户
            requirementDetailVm.RecommendUsers = GetRecommenUsers(app_Requirement);
            var applyJobs = applyJobRepository.FindList(EF => EF.PK_App_Requirement_Title == RequirementId && EF.EnumApplyStatus == "0").ToList();
            //获取申请面试的人数
            var applyJobCount = applyJobs.Where(EF => EF.CurrentStep == "2" && EF.EnumApplyStatus == "0").Count();
            requirementDetailVm.ApplyingCount = applyJobCount;
            //获取面试中的人数
            var InterviewCount = applyJobs.Where(EF => EF.EnumApplyStatus == "0" && (Utils.ObjToInt(EF.CurrentStep, 0) >= 3 && Utils.ObjToInt(EF.CurrentStep, 0) < 9)).Count();
            requirementDetailVm.InterviewCount = InterviewCount;
            ErrorMsg = "获取需求成功";
            sysLog.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",ErrorMsg:" + ErrorMsg, "结束", "GetApp_Requirement", "App_RequirementBLL");
            return requirementDetailVm;
        }
        #endregion

        #region 获取推荐人列表
        /// <summary>
        /// 获取推荐人列表
        /// </summary>
        /// <param name="app_Requirement"></param>
        /// <returns></returns>
        private List<ApplyJobUserVm> GetRecommenUsers(App_Requirement app_Requirement)
        {
            List<ApplyJobUserVm> applyJobUserVms = new List<ApplyJobUserVm>();
            var workMates = customerRepository.FindList(EF => EF.EnumDriverLicence != null ||
                                                                (EF.JobIntension.Contains(app_Requirement.PK_App_Position_Name)
                                                                && EF.Sex == app_Requirement.WorkLimitSex
                                                                && EF.Age >= app_Requirement.WorkLimitAgeLow
                                                                && EF.Age <= app_Requirement.WorkLimitAgeHigh
                                                                )).ToList();
            //继续判断国家
            foreach (var customerWorkmate in workMates)
            {
                //var customerJobIntension = customerJobIntensionRepository.Find(EF => EF.PK_App_Customer_CustomerName == item.Id);
                //if (customerJobIntension != null && customerJobIntension.ExpectCountry.Contains(app_Requirement.PK_App_Country_Name))
                //{
                //}
                ApplyJobUserVm applyJobUserVm = new ApplyJobUserVm();
                applyJobUserVm.CustomerId = customerWorkmate.Id;
                applyJobUserVm.CustomerName = customerWorkmate.CustomerName;
                applyJobUserVm.Photo = customerWorkmate.CustomerPhoto;
                applyJobUserVm.CreateTime = customerWorkmate.CreateTime;
                applyJobUserVm.Age = customerWorkmate.Age;
                applyJobUserVm.BirthPlace = customerWorkmate.BirthPlace;
                applyJobUserVm.Sex = customerWorkmate.Sex;
                applyJobUserVm.AbroadExp = enumDictionary.GetDicName("App_CustomerJobIntension.AbroadExp", customerWorkmate.AbroadExp);
                applyJobUserVm.EnumDriverLicence = customerWorkmate.EnumDriverLicence;
                applyJobUserVm.DriverLicence = enumDictionary.GetDicName("App_CustomerWorkmate.EnumDriverLicence", customerWorkmate.EnumDriverLicence);
                applyJobUserVm.JobIntension = customerWorkmate.JobIntension;
                applyJobUserVm.JobIntensionName = app_PositionBLL.GetNames(customerWorkmate.JobIntension);
                applyJobUserVm.SwitchBtnRecommend = customerWorkmate.SwitchBtnRecommend;
                applyJobUserVm.VideoPath = customerWorkmate.VideoPath;
                applyJobUserVm.EnumCustomerLevel = customerWorkmate.EnumCustomerLevel;
                applyJobUserVms.Add(applyJobUserVm);
            }
            return applyJobUserVms;
        }
        #endregion

        #region 获取热搜列表
        /// <summary>
        /// 获取热搜列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="PageNum"></param>
        /// <param name="RecordNum"></param>
        /// <param name="DataCount"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public List<string> GetHotSearches(string UserId, int PageNum, int RecordNum, ref int DataCount, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetHotSearches", "App_RequirementBLL");
            List<string> hotSearchs = new List<string>();
            var app_RequirementSearches = requirementSearchRepository.FindList().OrderByDescending(EF => EF.ModificationTime).ToList();
            if (app_RequirementSearches.Count == 0)
            {
                ErrorMsg = "获取热搜列表为空";
                sysLog.WriteServiceLog(UserId, "PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",ErrorMsg:" + ErrorMsg, "结束", "GetHotSearches", "App_RequirementBLL");
                return hotSearchs;
            }
            var requirementSearchesOrder = app_RequirementSearches.GroupBy(EF => EF.Content).OrderByDescending(EF => EF.Count());
            foreach (var item in requirementSearchesOrder)
            {
                var x = item.Count();
                hotSearchs.Add(item.Key);
            }
            DataCount = hotSearchs.Count;
            if (PageNum > 0)
            {
                hotSearchs = hotSearchs.Skip((PageNum - 1) * RecordNum).Take(RecordNum).ToList();
            }
            ErrorMsg = "获取热搜列表成功";
            sysLog.WriteServiceLog(UserId, "PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetHotSearchs", "App_RequirementBLL");
            return hotSearchs;
        }
        #endregion

        #region 获取国家列表
        /// <summary>
        /// 获取国家列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="PageNum"></param>
        /// <param name="RecordNum"></param>
        /// <param name="DataCount"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public List<CountryVm> GetCountries(string UserId, int PageNum, int RecordNum, ref int DataCount, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetCountries", "App_RequirementBLL");
            List<CountryVm> countryVms = new List<CountryVm>();
            var app_Countries = countryRepository.FindList().OrderByDescending(EF => EF.ModificationTime).ToList();
            if (app_Countries.Count == 0)
            {
                ErrorMsg = "获取国家列表为空";
                sysLog.WriteServiceLog(UserId, "PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",ErrorMsg:" + ErrorMsg, "结束", "GetCountries", "App_RequirementBLL");
                return countryVms;
            }
            DataCount = app_Countries.Count;
            if (PageNum > 0)
            {
                app_Countries = app_Countries.Skip((PageNum - 1) * RecordNum).Take(RecordNum).ToList();
            }
            countryVms = app_Countries.Select(EF => new CountryVm
            {
                Id = EF.Id,
                Name = EF.Name,
                CountryImg = EF.CountryImg
            }).ToList();
            ErrorMsg = "获取国家列表成功";
            sysLog.WriteServiceLog(UserId, "PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetCountries", "App_RequirementBLL");
            return countryVms;
        }
        #endregion

        #region 收藏需求
        /// <summary>
        /// 收藏需求
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RequirementId"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public string CollectRequirement(string UserId, string RequirementId, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "RequirementId:" + RequirementId, "开始", "CollectRequirement", "App_RequirementBLL");
            var now = ResultHelper.NowTime;
            var app_Requirement = m_Rep.GetById(RequirementId);
            if (null == app_Requirement)
            {
                ErrorMsg = "获取需求详情为空";
                sysLog.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",ErrorMsg:" + ErrorMsg, "结束", "CollectRequirement", "App_RequirementBLL");
                return null;
            }
            App_RequirementCollect app_RequirementCollect = new App_RequirementCollect();
            app_RequirementCollect.Id = ResultHelper.NewId;
            app_RequirementCollect.CreateUserName = UserId;
            app_RequirementCollect.CreateTime = now;
            app_RequirementCollect.ModificationTime = now;
            app_RequirementCollect.ModificationUserName = UserId;
            app_RequirementCollect.PK_App_Customer_CustomerName = UserId;
            app_RequirementCollect.PK_App_Requirement_Title = RequirementId;
            try
            {
                requirementCollectRepository.Create(app_RequirementCollect);
                ErrorMsg = "需求收藏成功";
                sysLog.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",ErrorMsg:" + ErrorMsg, "结束", "CollectRequirement", "App_RequirementBLL");
                return app_RequirementCollect.Id;
            }
            catch (Exception ex)
            {
                ErrorMsg = "需求收藏异常";
                sysLog.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",ErrorMsg:" + ErrorMsg + ex.Message, "结束", "CollectRequirement", "App_RequirementBLL");
                return null;
            }
        }
        #endregion

        #region 取消收藏需求
        /// <summary>
        /// 取消收藏需求
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RequirementCollectId"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public bool UnCollectRequirement(string UserId, string RequirementCollectId, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "RequirementCollectId:" + RequirementCollectId, "开始", "UnCollectRequirement", "App_RequirementBLL");
            var app_RequirementCollect = requirementCollectRepository.GetById(RequirementCollectId);
            if (null == app_RequirementCollect)
            {
                ErrorMsg = "获取需求收藏为空";
                sysLog.WriteServiceLog(UserId, "RequirementCollectId:" + RequirementCollectId + ",ErrorMsg:" + ErrorMsg, "结束", "UnCollectRequirement", "App_RequirementBLL");
                return false;
            }
            try
            {
                requirementCollectRepository.Delete(RequirementCollectId);
                ErrorMsg = "需求取消收藏成功";
                sysLog.WriteServiceLog(UserId, "RequirementCollectId:" + RequirementCollectId + ",ErrorMsg:" + ErrorMsg, "结束", "UnCollectRequirement", "App_RequirementBLL");
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = "需求取消收藏异常";
                sysLog.WriteServiceLog(UserId, "RequirementCollectId:" + RequirementCollectId + ",ErrorMsg:" + ErrorMsg + ex.Message, "结束", "UnCollectRequirement", "App_RequirementBLL");
                return false;
            }
        }
        #endregion

        #region 获取收藏需求列表
        /// <summary>
        /// 获取收藏需求列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="DataCount"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public List<RequirementVm> GetRequirementCollections(RequireSearchForm requireSearchForm, ref int DataCount, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(requireSearchForm.UserId, requireSearchForm.ToString(), "开始", "GetRequirementCollections", "App_RequirementBLL");
            int PageNum = requireSearchForm.PageNum, RecordNum = requireSearchForm.RecordNum;
            List<RequirementVm> requirementVms = new List<RequirementVm>();
            var app_RequirementCollections = requirementCollectRepository.FindList(EF => EF.PK_App_Customer_CustomerName == requireSearchForm.UserId);
            var RequirementIds = app_RequirementCollections.Select(EF => EF.PK_App_Requirement_Title).ToArray();
            var app_Requirements = m_Rep.FindList(EF => RequirementIds.Contains(EF.Id)).ToList();
            app_Requirements = GetRequirements(requireSearchForm, app_Requirements);
            DataCount = app_Requirements.Count;
            if (DataCount == 0)
            {
                ErrorMsg = "获取需求列表为空";
                sysLog.WriteServiceLog(requireSearchForm.UserId, requireSearchForm.ToString() + ErrorMsg, "结束", "GetRequirementCollections", "App_RequirementBLL");
                return requirementVms;
            }
            if (PageNum > 0)
            {
                app_Requirements = app_Requirements.Skip((PageNum - 1) * RecordNum).Take(RecordNum).ToList();
            }
            foreach (var item in app_Requirements)
            {
                var requirementVm = new RequirementVm();
                requirementVm.Id = item.Id;
                requirementVm.Title = item.Title;
                var country = countryRepository.GetById(item.PK_App_Country_Name);
                if (null != country)
                {
                    requirementVm.CountryName = country.Name;
                    requirementVm.CountryImg = country.CountryImg;
                }
                requirementVm.SalaryLow = GetSalary(item.SalaryLow);
                requirementVm.SalaryHigh = GetSalary(item.SalaryHigh);
                requirementVm.PublishDate = item.PublishDate;
                requirementVm.Tag = item.Tag.Split(',').ToList();
                requirementVm.CompanyName = string.IsNullOrEmpty(item.CompanyName) ? "雇主直招" : item.CompanyName;
                requirementVm.ApplyCount = applyJobRepository.GetList(EF => EF.PK_App_Requirement_Title == item.Id).Count().ToString(); ;
                requirementVm.TransactProvinceCode = item.TransactProvince;
                requirementVm.TransactProvinceName = sysAreasRepository.GetAreasName(item.TransactProvince);
                requirementVm.PK_App_Position_Name = item.PK_App_Position_Name;
                requirementVm.PositionName = app_PositionBLL.GetName(item.PK_App_Position_Name);
                requirementVms.Add(requirementVm);
            }
            ErrorMsg = "获取需求列表成功";
            sysLog.WriteServiceLog(requireSearchForm.UserId, requireSearchForm.ToString() + ",DataCount:" + DataCount + ErrorMsg, "结束", "GetRequirementCollections", "App_RequirementBLL");
            return requirementVms;
        }
        #endregion

        #region 获取应聘过需求列表
        /// <summary>
        /// 获取应聘过需求列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="DataCount"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public List<RequirementVm> GetRequirementApplieds(RequireSearchForm requireSearchForm, ref int DataCount, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(requireSearchForm.UserId, requireSearchForm.ToString(), "开始", "GetRequirementApplieds", "App_RequirementBLL");
            int PageNum = requireSearchForm.PageNum, RecordNum = requireSearchForm.RecordNum;
            List<RequirementVm> requirementVms = new List<RequirementVm>();
            var app_RequirementApplies = applyJobRepository.FindList(EF => EF.PK_App_Customer_CustomerName == requireSearchForm.UserId);
            var RequirementIds = app_RequirementApplies.Select(EF => EF.PK_App_Requirement_Title).ToArray();
            var app_Requirements = m_Rep.FindList(EF => RequirementIds.Contains(EF.Id)).ToList();
            app_Requirements = GetRequirements(requireSearchForm, app_Requirements);
            DataCount = app_Requirements.Count;
            if (DataCount == 0)
            {
                ErrorMsg = "获取需求列表为空";
                sysLog.WriteServiceLog(requireSearchForm.UserId, requireSearchForm.ToString() + ErrorMsg, "结束", "GetRequirementApplieds", "App_RequirementBLL");
                return requirementVms;
            }
            if (PageNum > 0)
            {
                app_Requirements = app_Requirements.Skip((PageNum - 1) * RecordNum).Take(RecordNum).ToList();
            }
            foreach (var item in app_Requirements)
            {
                var requirementVm = new RequirementVm();
                requirementVm.Id = item.Id;
                requirementVm.Title = item.Title;
                var country = countryRepository.GetById(item.PK_App_Country_Name);
                if (null != country)
                {
                    requirementVm.CountryName = country.Name;
                    requirementVm.CountryImg = country.CountryImg;
                }
                requirementVm.SalaryLow = GetSalary(item.SalaryLow);
                requirementVm.SalaryHigh = GetSalary(item.SalaryHigh);
                requirementVm.PublishDate = item.PublishDate;
                requirementVm.Tag = item.Tag.Split(',').ToList();
                requirementVm.CompanyName = string.IsNullOrEmpty(item.CompanyName) ? "雇主直招" : item.CompanyName;
                requirementVm.ApplyCount = applyJobRepository.GetList(EF => EF.PK_App_Requirement_Title == item.Id).Count().ToString(); ;
                requirementVm.TransactProvinceCode = item.TransactProvince;
                requirementVm.TransactProvinceName = sysAreasRepository.GetAreasName(item.TransactProvince);
                requirementVm.PK_App_Position_Name = item.PK_App_Position_Name;
                requirementVm.PositionName = app_PositionBLL.GetName(item.PK_App_Position_Name);
                requirementVms.Add(requirementVm);
            }
            ErrorMsg = "获取需求列表成功";
            sysLog.WriteServiceLog(requireSearchForm.UserId, requireSearchForm.ToString() + ",DataCount:" + DataCount + ErrorMsg, "结束", "GetRequirementApplieds", "App_RequirementBLL");
            return requirementVms;
        }
        #endregion

        #region 发布需求
        /// <summary>
        /// 发布需求
        /// </summary>
        /// <param name="requirementPost"></param>
        /// <returns></returns>
        public bool CreateRequirement(RequirementPost requirementPost, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(requirementPost.UserId, requirementPost.ToString(), "开始", "CreateRequirement", "App_RequirementBLL");
            var now = ResultHelper.NowTime;
            App_Requirement app_Requirement = new App_Requirement();
            app_Requirement.Id = ResultHelper.NewId;
            app_Requirement.CreateTime = now;
            app_Requirement.CreateUserName = requirementPost.UserId;
            app_Requirement.ModificationTime = now;
            app_Requirement.ModificationUserName = requirementPost.UserId;
            app_Requirement.PK_App_Customer_CustomerName = requirementPost.PublishUserId;
            app_Requirement.PK_App_Position_Name = requirementPost.PK_App_Position_Name;
            app_Requirement.Title = requirementPost.Title;
            app_Requirement.PK_App_Country_Name = requirementPost.PK_App_Country_Name;
            app_Requirement.PreTaxSalary = requirementPost.PreTaxSalary;
            app_Requirement.WorkHourPerWeek = requirementPost.WorkHourPerWeek;
            //计算薪资范围时薪x（45-60）每周工时x（50）每年工作周数x汇率
            //获取对应国家汇率
            var country = countryRepository.GetById(requirementPost.PK_App_Country_Name);
            decimal ExchangeRate = Utils.ObjToDecimal(country.ExchangeRate, 1),
                PreTaxSalary = Utils.ObjToDecimal(requirementPost.PreTaxSalary, 1);
            app_Requirement.SalaryLow = (PreTaxSalary * 45 * 50 * ExchangeRate).ToString();
            app_Requirement.SalaryHigh = (PreTaxSalary * 60 * 50 * ExchangeRate).ToString();
            app_Requirement.WorkLimitSex = requirementPost.WorkLimitSex;
            app_Requirement.WorkLimitAgeLow = requirementPost.WorkLimitAgeLow;
            app_Requirement.WorkLimitAgeHigh = requirementPost.WorkLimitAgeHigh;
            app_Requirement.EnumWorkLimitDegree = requirementPost.EnumWorkLimitDegree;
            app_Requirement.TotalHire = requirementPost.TotalHire;
            app_Requirement.Tag = requirementPost.Tag;
            app_Requirement.Description = requirementPost.Description;
            app_Requirement.SwitchBtnOpen = requirementPost.SwitchBtnOpen;
            app_Requirement.EnumWorkPermit = requirementPost.EnumWorkPermit;
            try
            {
                m_Rep.Create(app_Requirement);
                ErrorMsg = "职位发布成功";
                sysLog.WriteServiceLog(requirementPost.UserId, requirementPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "CreateRequirement", "App_RequirementBLL");
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = "职位发布失败";
                sysLog.WriteServiceLog(requirementPost.UserId, requirementPost.ToString() + ",ErrorMsg:" + ErrorMsg + ex.Message, "结束", "CreateRequirement", "App_RequirementBLL");
                return false;
            }
        }
        #endregion

        #region 修改发布需求
        /// <summary>
        /// 修改发布需求
        /// </summary>
        /// <param name="requirementPost"></param>
        /// <returns></returns>
        public bool EditRequirement(RequirementPost requirementPost, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(requirementPost.UserId, requirementPost.ToString(), "开始", "EditRequirement", "App_RequirementBLL");
            var now = ResultHelper.NowTime;
            App_Requirement app_Requirement = m_Rep.GetById(requirementPost.Id);
            if (null == app_Requirement)
            {
                ErrorMsg = "需求不存在";
                sysLog.WriteServiceLog(requirementPost.UserId, requirementPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "EditRequirement", "App_RequirementBLL");
                return false;
            }
            app_Requirement.ModificationTime = now;
            app_Requirement.ModificationUserName = requirementPost.UserId;
            if (!string.IsNullOrEmpty(requirementPost.Title))
            {
                app_Requirement.Title = requirementPost.Title;
            }
            if (!string.IsNullOrEmpty(requirementPost.PK_App_Country_Name))
            {
                app_Requirement.PK_App_Country_Name = requirementPost.PK_App_Country_Name;
            }
            if (!string.IsNullOrEmpty(requirementPost.PreTaxSalary))
            {
                app_Requirement.PreTaxSalary = requirementPost.PreTaxSalary;
            }
            if (!string.IsNullOrEmpty(requirementPost.WorkHourPerWeek))
            {
                app_Requirement.WorkHourPerWeek = requirementPost.WorkHourPerWeek;
            }
            if (!string.IsNullOrEmpty(requirementPost.WorkLimitSex))
            {
                app_Requirement.WorkLimitSex = requirementPost.WorkLimitSex;
            }
            if (requirementPost.WorkLimitAgeLow != 0)
            {
                app_Requirement.WorkLimitAgeLow = requirementPost.WorkLimitAgeLow;
            }
            if (requirementPost.WorkLimitAgeLow != 0)
            {
                app_Requirement.WorkLimitAgeHigh = requirementPost.WorkLimitAgeHigh;
            }
            if (!string.IsNullOrEmpty(requirementPost.EnumWorkLimitDegree))
            {
                app_Requirement.EnumWorkLimitDegree = requirementPost.EnumWorkLimitDegree;
            }
            if (requirementPost.TotalHire != 0)
            {
                app_Requirement.TotalHire = requirementPost.TotalHire;
            }
            if (!string.IsNullOrEmpty(requirementPost.Tag))
            {
                app_Requirement.Tag = requirementPost.Tag;
            }
            if (!string.IsNullOrEmpty(requirementPost.Description))
            {
                app_Requirement.Description = requirementPost.Description;
            }
            if (!string.IsNullOrEmpty(requirementPost.SwitchBtnOpen))
            {
                app_Requirement.SwitchBtnOpen = requirementPost.SwitchBtnOpen;
            }
            //计算薪资范围时薪x（45-60）每周工时x（50）每年工作周数x汇率
            //获取对应国家汇率
            var country = countryRepository.GetById(requirementPost.PK_App_Country_Name);
            decimal ExchangeRate = Utils.ObjToDecimal(country.ExchangeRate, 1),
                PreTaxSalary = Utils.ObjToDecimal(requirementPost.PreTaxSalary, 1);
            app_Requirement.SalaryLow = (PreTaxSalary * 45 * 50 * ExchangeRate).ToString();
            app_Requirement.SalaryHigh = (PreTaxSalary * 60 * 50 * ExchangeRate).ToString();
            try
            {
                m_Rep.Edit(app_Requirement);
                ErrorMsg = "需求修改成功";
                sysLog.WriteServiceLog(requirementPost.UserId, requirementPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "EditRequirement", "App_RequirementBLL");
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = "需求修改失败";
                sysLog.WriteServiceLog(requirementPost.UserId, requirementPost.ToString() + ",ErrorMsg:" + ErrorMsg + ex.Message, "结束", "EditRequirement", "App_RequirementBLL");
                return false;
            }
        }
        #endregion

        #region 删除发布需求
        /// <summary>
        /// 删除发布需求
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RequirementId"></param>
        /// <returns></returns>
        public bool DeleteRequirement(string UserId, string RequirementId, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "RequirementId:" + RequirementId, "开始", "DeleteRequirement", "App_RequirementBLL");
            var now = ResultHelper.NowTime;
            App_Requirement app_Requirement = m_Rep.GetById(RequirementId);
            if (null == app_Requirement)
            {
                ErrorMsg = "需求不存在";
                sysLog.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",ErrorMsg:" + ErrorMsg, "结束", "DeleteRequirement", "App_RequirementBLL");
                return false;
            }
            try
            {
                m_Rep.Delete(RequirementId);
                ErrorMsg = "需求删除成功";
                sysLog.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",ErrorMsg:" + ErrorMsg, "结束", "DeleteRequirement", "App_RequirementBLL");
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = "需求删除失败";
                sysLog.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",ErrorMsg:" + ErrorMsg + ex.Message, "结束", "DeleteRequirement", "App_RequirementBLL");
                return false;
            }
        }
        #endregion

        #region 获取职位树
        /// <summary>
        /// 获取职位树
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public List<PositionTreeVm> GetPositionTree(string UserId, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "", "开始", "GetPositionTree", "App_RequirementBLL");
            var positionTreeVms = app_PositionBLL.GetAppPositions(null, ref ErrorMsg);
            var posIds = string.Join(",", app_PositionBLL.m_Rep.FindList(EF => EF.SwitchBtnCommonUse == "1").Select(EF => EF.Id).ToArray());
            List<PositionTreeVm> commonUseList = new List<PositionTreeVm>();
            List<PositionTreeVm> commonUseOneList = new List<PositionTreeVm>();
            var positionTreeVm = new PositionTreeVm
            {
                Id = "0",
                Name = "常用/热门",
                Description = "常用/热门",
                positionTreeVms = new List<PositionTreeVm>()
            };
            commonUseList = app_PositionBLL.GetCommonUseAppPositions(posIds, ref ErrorMsg);
            var secondList = new List<PositionTreeVm>();
            foreach (var item in commonUseList)
            {
                secondList.AddRange(item.positionTreeVms);
            }
            positionTreeVm.positionTreeVms = secondList;
            commonUseOneList.Add(positionTreeVm);
            commonUseOneList.AddRange(positionTreeVms);
            ErrorMsg = "获取职位树成功";
            sysLog.WriteServiceLog(UserId, "ErrorMsg:" + ErrorMsg, "结束", "GetPositionTree", "App_RequirementBLL");
            return commonUseOneList;
        }
        #endregion

        #region 编辑初始化需求
        /// <summary>
        /// 编辑初始化需求
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RequirementId"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public RequirementInitPost EditRequirementInit(string UserId, string RequirementId, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "RequirementId:" + RequirementId, "开始", "EditRequirementInit", "App_RequirementBLL");
            RequirementInitPost requirementPost = new RequirementInitPost();
            var app_Requirement = m_Rep.GetById(RequirementId);
            if (null == app_Requirement)
            {
                ErrorMsg = "获取需求详情为空";
                sysLog.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",ErrorMsg:" + ErrorMsg, "结束", "EditRequirementInit", "App_RequirementBLL");
                return null;
            }
            requirementPost.Id = app_Requirement.Id;
            requirementPost.UserId = app_Requirement.PK_App_Customer_CustomerName;
            requirementPost.PK_App_Position_Name = app_Requirement.PK_App_Position_Name;
            requirementPost.App_Position_Name = app_PositionBLL.GetName(app_Requirement.PK_App_Position_Name);
            requirementPost.Title = app_Requirement.Title;
            requirementPost.PK_App_Country_Name = app_Requirement.PK_App_Country_Name;
            requirementPost.App_Country_Name = countryRepository.GetName(app_Requirement.PK_App_Country_Name);
            requirementPost.PreTaxSalary = app_Requirement.PreTaxSalary;
            requirementPost.WorkHourPerWeek = app_Requirement.WorkHourPerWeek;
            requirementPost.WorkLimitSex = app_Requirement.WorkLimitSex;
            requirementPost.WorkLimitAgeLow = app_Requirement.WorkLimitAgeLow;
            requirementPost.WorkLimitAgeHigh = app_Requirement.WorkLimitAgeHigh;
            requirementPost.EnumWorkLimitDegree = app_Requirement.EnumWorkLimitDegree;
            requirementPost.TotalHire = app_Requirement.TotalHire;
            requirementPost.Tag = app_Requirement.Tag;
            requirementPost.Description = app_Requirement.Description;
            requirementPost.SwitchBtnOpen = app_Requirement.SwitchBtnOpen;
            requirementPost.EnumWorkPermit = app_Requirement.EnumWorkPermit;
            ErrorMsg = "获取需求成功";
            sysLog.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",ErrorMsg:" + ErrorMsg, "结束", "EditRequirementInit", "App_RequirementBLL");
            return requirementPost;
        }
        #endregion

        #region 获取首页推荐人列表
        /// <summary>
        /// 获取首页推荐人列表
        /// </summary>
        /// <param name="recommendUserSearchForm"></param>
        /// <param name="DataCount"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public EmployerHomePage GetRecommendUserList(RecommendUserSearchForm recommendUserSearchForm, ref int DataCount, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(recommendUserSearchForm.UserId, recommendUserSearchForm.ToString(), "开始", "GetRecommendUserList", "App_RequirementBLL");
            int PageNum = recommendUserSearchForm.PageNum, RecordNum = recommendUserSearchForm.RecordNum;
            EmployerHomePage employerHomePage = new EmployerHomePage();
            List<ApplyJobUserVm> applyJobUserVms = new List<ApplyJobUserVm>();
            var now = ResultHelper.NowTime;
            //如果未登录，或者登陆了，没有发布过职位，则需要显示所有人
            if (string.IsNullOrEmpty(recommendUserSearchForm.EmployerId))
            {
                var workMates = customerRepository.FindList().ToList();
                foreach (var customerWorkmate in workMates)
                {
                    ApplyJobUserVm applyJobUserVm = new ApplyJobUserVm();
                    applyJobUserVm.CustomerId = customerWorkmate.Id;
                    applyJobUserVm.CustomerName = customerWorkmate.CustomerName;
                    applyJobUserVm.Photo = customerWorkmate.CustomerPhoto;
                    applyJobUserVm.CreateTime = customerWorkmate.CreateTime;
                    applyJobUserVm.Age = customerWorkmate.Age;
                    applyJobUserVm.BirthPlace = customerWorkmate.BirthPlace;
                    applyJobUserVm.Sex = customerWorkmate.Sex;
                    applyJobUserVm.AbroadExp = enumDictionary.GetDicName("App_CustomerJobIntension.AbroadExp", customerWorkmate.AbroadExp);
                    applyJobUserVm.EnumDriverLicence = customerWorkmate.EnumDriverLicence;
                    applyJobUserVm.DriverLicence = enumDictionary.GetDicName("App_CustomerWorkmate.EnumDriverLicence", customerWorkmate.EnumDriverLicence);
                    applyJobUserVm.JobIntension = customerWorkmate.JobIntension;
                    applyJobUserVm.JobIntensionName = app_PositionBLL.GetNames(customerWorkmate.JobIntension);
                    applyJobUserVm.SwitchBtnRecommend = customerWorkmate.SwitchBtnRecommend;
                    applyJobUserVm.VideoPath = customerWorkmate.VideoPath;
                    applyJobUserVm.EnumCustomerLevel = customerWorkmate.EnumCustomerLevel;
                    applyJobUserVms.Add(applyJobUserVm);
                }
            }
            else
            {
                //获取当前账号发布的所有需求
                var app_Requirements = m_Rep.FindList(EF => EF.PK_App_Customer_CustomerName == recommendUserSearchForm.EmployerId && EF.SwitchBtnOpen == "1").ToList();
                string PosIds = string.Join(",", app_Requirements.Select(EF => EF.PK_App_Position_Name).Distinct().ToArray());
                var PosIdList = app_PositionBLL.GetParentIds(PosIds);
                var positionTreeVms = app_PositionBLL.GetAppPositions(string.Join(",", PosIdList), ref ErrorMsg);
                employerHomePage.positionTreeVms = positionTreeVms;
                foreach (var item in app_Requirements)
                {
                    applyJobUserVms.AddRange(GetRecommenUsers(item));
                }
            }
            //判断输入框是否为空
            if (!string.IsNullOrEmpty(recommendUserSearchForm.QueryStr))
            {
                //如果输入框不为空，则增加简历热搜
                App_CustomerPosSearch app_CustomerPosSearch = new App_CustomerPosSearch();
                app_CustomerPosSearch.Id = ResultHelper.NewId;
                app_CustomerPosSearch.CreateTime = now;
                app_CustomerPosSearch.CreateUserName = recommendUserSearchForm.UserId;
                app_CustomerPosSearch.ModificationTime = now;
                app_CustomerPosSearch.ModificationUserName = recommendUserSearchForm.UserId;
                app_CustomerPosSearch.PK_App_Customer_CustomerName = recommendUserSearchForm.UserId;
                app_CustomerPosSearch.Content = recommendUserSearchForm.QueryStr;
                customerPosSearchRepository.Create(app_CustomerPosSearch);
                var app_Position = app_PositionBLL.m_Rep.Find(EF => EF.Name == recommendUserSearchForm.QueryStr);
                if (app_Position != null)
                {
                    applyJobUserVms = applyJobUserVms.Where(EF => EF.JobIntension.Contains(app_Position.Id)).ToList();
                }
            }
            //判断工种是否为空
            if (!string.IsNullOrEmpty(recommendUserSearchForm.PK_App_Position_Name))
            {
                applyJobUserVms = applyJobUserVms.Where(EF => EF.JobIntension.Contains(recommendUserSearchForm.PK_App_Position_Name)).ToList();
            }
            //判断系统推荐是否为空
            if (recommendUserSearchForm.IsRecommend == "1")
            {
                applyJobUserVms = applyJobUserVms.Where(EF => EF.SwitchBtnRecommend == "1").OrderBy(EF => EF.EnumCustomerLevel).ToList();
            }
            //判断视频是否为空
            if (recommendUserSearchForm.HaveVideo == "1")
            {
                applyJobUserVms = applyJobUserVms.Where(EF => EF.VideoPath != null && EF.VideoPath != "").ToList();
            }
            //判断最新是否为空
            if (recommendUserSearchForm.IsLatest == "1")
            {
                applyJobUserVms = applyJobUserVms.OrderByDescending(EF => EF.CreateTime).ToList();
            }
            //判断工种是否为空
            if (!string.IsNullOrEmpty(recommendUserSearchForm.IntelligenceSort))
            {
                if ("Age" == recommendUserSearchForm.IntelligenceSort)
                {
                    //这里的年龄优先应该是年纪越小的越靠前排序
                    applyJobUserVms = applyJobUserVms.OrderBy(EF => EF.Age).ToList();
                }
                if ("DriverLicence" == recommendUserSearchForm.IntelligenceSort)
                {
                    applyJobUserVms = applyJobUserVms.OrderByDescending(EF => EF.EnumDriverLicence).ToList();
                }
            }
            DataCount = applyJobUserVms.Count;
            if (DataCount == 0)
            {
                ErrorMsg = "获取推荐人列表为空";
                sysLog.WriteServiceLog(recommendUserSearchForm.UserId, recommendUserSearchForm.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "GetRecommendUserList", "App_RequirementBLL");
                return employerHomePage;
            }
            if (PageNum > 0)
            {
                applyJobUserVms = applyJobUserVms.Skip((PageNum - 1) * RecordNum).Take(RecordNum).ToList();
            }
            employerHomePage.applyJobUserVms = applyJobUserVms;
            ErrorMsg = "获取推荐人列表成功";
            sysLog.WriteServiceLog(recommendUserSearchForm.UserId, recommendUserSearchForm.ToString() + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetRecommendUserList", "App_RequirementBLL");
            return employerHomePage;
        }
        #endregion

        #region 邀请工人
        /// <summary>
        /// 邀请工人
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="RequirementId"></param>
        /// <param name="WorkerId"></param>
        /// <returns></returns>
        public bool InviteWorker(string CustomerId, string RequirementId, string WorkerId, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(CustomerId, "RequirementId:" + RequirementId + ",WorkerId:" + WorkerId, "开始", "InviteWorker", "App_RequirementBLL");
            string PK_App_Customer_CustomerName = WorkerId;
            var now = ResultHelper.NowTime;
            //发起邀请，往邀请表里增加数据
            App_RequirementInvite app_RequirementInvite = new App_RequirementInvite();
            app_RequirementInvite.Id = ResultHelper.NewId;
            app_RequirementInvite.CreateTime = now;
            app_RequirementInvite.CreateUserName = CustomerId;
            app_RequirementInvite.ModificationTime = now;
            app_RequirementInvite.ModificationUserName = CustomerId;
            app_RequirementInvite.InitiatorId = CustomerId;
            app_RequirementInvite.Inviter = WorkerId;
            app_RequirementInvite.PK_App_Requirement_Title = RequirementId;
            try
            {
                requirementInviteRepository.Create(app_RequirementInvite);
                sysMessageRepository.CrtSysMessage(CustomerId, PK_App_Customer_CustomerName, RequirementId, "面试邀请", "恭喜，您收到一份雇主面试邀请，点击查看详情", "1", "1", "");
                ErrorMsg = "邀请成功";
                sysLog.WriteServiceLog(CustomerId, "RequirementId:" + RequirementId + ",WorkerId:" + WorkerId + ",ErrorMsg:" + ErrorMsg, "结束", "InviteWorker", "App_RequirementBLL");
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = "发送邀请失败";
                sysLog.WriteServiceLog(CustomerId, "RequirementId:" + RequirementId + ",WorkerId:" + WorkerId + ",ErrorMsg:" + ErrorMsg + ex.Message, "结束", "InviteWorker", "App_RequirementBLL");
                return false;
            }
        }
        #endregion

        #region 获取面试中的工友列表
        /// <summary>
        /// 获取面试中的工友列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RequirementId"></param>
        /// <param name="PageNum"></param>
        /// <param name="RecordNum"></param>
        /// <param name="DataCount"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public List<ApplyJobUserVm> GetInterviewUsers(string UserId, string RequirementId, string Flag, int PageNum, int RecordNum, ref int DataCount, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",Flag:" + Flag + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetInterviewUsers", "App_RequirementBLL");
            List<ApplyJobUserVm> applyJobUserVms = new List<ApplyJobUserVm>();
            var now = ResultHelper.NowTime;
            //获取需求
            var applyJobs = applyJobRepository.FindList(EF => EF.PK_App_Requirement_Title == RequirementId && EF.EnumApplyStatus == "0").ToList();
            var customerIds = applyJobs.Select(EF => EF.PK_App_Customer_CustomerName).ToArray();
            var customers = customerRepository.FindList(EF => customerIds.Contains(EF.Id)).ToList();
            if ("Apply".Equals(Flag))
            {
                applyJobs = applyJobs.Where(EF => EF.CurrentStep == "2").ToList();
            }
            if ("Interview".Equals(Flag))
            {
                applyJobs = applyJobs.Where(EF => Utils.ObjToInt(EF.CurrentStep, 0) >= 3 && Utils.ObjToInt(EF.CurrentStep, 0) < 9).ToList();
            }
            foreach (var item in applyJobs)
            {
                var customerWorkmate = customers.Find(EF => EF.Id == item.PK_App_Customer_CustomerName);
                ApplyJobUserVm applyJobUserVm = new ApplyJobUserVm();
                applyJobUserVm.CustomerId = customerWorkmate.Id;
                applyJobUserVm.CustomerName = customerWorkmate.CustomerName;
                applyJobUserVm.Photo = customerWorkmate.CustomerPhoto;
                applyJobUserVm.CreateTime = customerWorkmate.CreateTime;
                applyJobUserVm.Age = customerWorkmate.Age;
                applyJobUserVm.BirthPlace = customerWorkmate.BirthPlace;
                applyJobUserVm.Sex = customerWorkmate.Sex;
                applyJobUserVm.AbroadExp = enumDictionary.GetDicName("App_CustomerJobIntension.AbroadExp", customerWorkmate.AbroadExp);
                applyJobUserVm.EnumDriverLicence = customerWorkmate.EnumDriverLicence;
                applyJobUserVm.DriverLicence = enumDictionary.GetDicName("App_CustomerWorkmate.EnumDriverLicence", customerWorkmate.EnumDriverLicence);
                applyJobUserVm.JobIntension = customerWorkmate.JobIntension;
                applyJobUserVm.JobIntensionName = app_PositionBLL.GetNames(customerWorkmate.JobIntension);
                applyJobUserVm.SwitchBtnRecommend = customerWorkmate.SwitchBtnRecommend;
                applyJobUserVm.VideoPath = customerWorkmate.VideoPath;
                applyJobUserVm.ApplyJobId = item.Id;
                applyJobUserVms.Add(applyJobUserVm);
            }
            DataCount = applyJobUserVms.Count;
            if (DataCount == 0)
            {
                ErrorMsg = "获取面试中人列表为空";
                sysLog.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",Flag:" + Flag + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",ErrorMsg:" + ErrorMsg, "结束", "GetInterviewUsers", "App_RequirementBLL");
                return applyJobUserVms;
            }
            if (PageNum > 0)
            {
                applyJobUserVms = applyJobUserVms.Skip((PageNum - 1) * RecordNum).Take(RecordNum).ToList();
            }
            ErrorMsg = "获取面试中人列表为空";
            sysLog.WriteServiceLog(UserId, "RequirementId:" + RequirementId + ",Flag:" + Flag + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetInterviewUsers", "App_RequirementBLL");
            return applyJobUserVms;
        }
        #endregion

        #region 获取简历热搜列表
        /// <summary>
        /// 获取简历热搜列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="PageNum"></param>
        /// <param name="RecordNum"></param>
        /// <param name="DataCount"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public List<string> GetPosHotSearches(string UserId, int PageNum, int RecordNum, ref int DataCount, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetPosHotSearches", "App_RequirementBLL");
            List<string> hotSearchs = new List<string>();
            var app_CustomerPosSearches = customerPosSearchRepository.FindList().OrderByDescending(EF => EF.ModificationTime).ToList();
            if (app_CustomerPosSearches.Count == 0)
            {
                ErrorMsg = "获取热搜列表为空";
                sysLog.WriteServiceLog(UserId, "PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",ErrorMsg:" + ErrorMsg, "结束", "GetPosHotSearches", "App_RequirementBLL");
                return hotSearchs;
            }
            var customerPosSearches = app_CustomerPosSearches.GroupBy(EF => EF.Content).OrderByDescending(EF => EF.Count());
            foreach (var item in customerPosSearches)
            {
                var x = item.Count();
                hotSearchs.Add(item.Key);
            }
            DataCount = hotSearchs.Count;
            if (PageNum > 0)
            {
                hotSearchs = hotSearchs.Skip((PageNum - 1) * RecordNum).Take(RecordNum).ToList();
            }
            ErrorMsg = "获取热搜列表成功";
            sysLog.WriteServiceLog(UserId, "PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetPosHotSearches", "App_RequirementBLL");
            return hotSearchs;
        }
        #endregion

        #region 重写获取列表
        /// <summary>
        /// 重写获取列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="queryStr"></param>
        /// <returns></returns>
        public List<App_RequirementModel> GetRequirementList(ref GridPager pager, RequirementQuery requirementQuery)
        {
            IQueryable<App_Requirement> queryData = m_Rep.GetList();
            string strReqIds = null;
            //获取当前登录人所辖属的所有工人列表
            IQueryable<App_Customer> queryDataCustomer = customerRepository.GetList(EF => EF.ParentId != null);
            if (!requirementQuery.AdminFlag)
            {
                queryData = queryData.Where(EF => EF.SwitchBtnOpen == "1");
                queryDataCustomer = queryDataCustomer.Where(a => a.ParentId == requirementQuery.CustomerId);
            }
            var listApplyJob = applyJobRepository.FindList().ToList();
            string strCustomerIds = string.Join(",", queryDataCustomer.Select(EF => EF.Id).ToArray());
            //根据传入的不同状态获取职位列表
            if ("Applyed".Equals(requirementQuery.QueryFlag))
            {
                //获取所有的发起申请的工人主键集合
                listApplyJob = listApplyJob.Where(EF => strCustomerIds.Contains(EF.PK_App_Customer_CustomerName) && EF.EnumApplyStatus == "0" && EF.CurrentStep == "1").ToList();
                strReqIds = string.Join(",", listApplyJob.Select(EF => EF.PK_App_Requirement_Title).ToArray());
                queryData = queryData.Where(EF => strReqIds.Contains(EF.Id));
            }
            if ("Proceeding".Equals(requirementQuery.QueryFlag))
            {
                //获取所有的发起申请的工人主键集合
                listApplyJob = listApplyJob.Where(EF => strCustomerIds.Contains(EF.PK_App_Customer_CustomerName) && EF.EnumApplyStatus == "0" && EF.CurrentStep != "1").ToList();
                strReqIds = string.Join(",", listApplyJob.Select(EF => EF.PK_App_Requirement_Title).ToArray());
                queryData = queryData.Where(EF => strReqIds.Contains(EF.Id));
            }
            if (!string.IsNullOrWhiteSpace(requirementQuery.Title))
            {
                queryData = queryData.Where(EF => EF.Title != null && EF.Title.Contains(requirementQuery.Title));
            }
            if (!string.IsNullOrWhiteSpace(requirementQuery.PositionId))
            {
                var arrPositionId = requirementQuery.PositionId.Split(',').ToList();
                queryData = queryData.ToList().Where(EF => EF.PK_App_Position_Name != null && EF.PK_App_Position_Name.Split(',').Intersect(arrPositionId).Count() > 0).AsQueryable();
            }
            if (!string.IsNullOrWhiteSpace(requirementQuery.Sex))
            {
                queryData = queryData.Where(EF => EF.WorkLimitSex == requirementQuery.Sex);
            }
            if (requirementQuery.AgeLow != 0)
            {
                queryData = queryData.Where(EF => EF.WorkLimitAgeLow <= requirementQuery.AgeLow && EF.WorkLimitAgeHigh >= requirementQuery.AgeLow);
            }
            if (requirementQuery.AgeHigh != 0)
            {
                queryData = queryData.Where(EF => EF.WorkLimitAgeLow <= requirementQuery.AgeHigh && EF.WorkLimitAgeHigh >= requirementQuery.AgeHigh);
            }
            if (requirementQuery.SallaryLow != 0)
            {
                queryData = queryData.ToList().Where(EF => EF.SalaryLow != null && Utils.ObjToDecimal(EF.SalaryLow, 0) >= requirementQuery.SallaryLow).AsQueryable();
            }
            if (requirementQuery.SallaryHigh != 0)
            {
                queryData = queryData.ToList().Where(EF => EF.SalaryHigh != null && Utils.ObjToDecimal(EF.SalaryHigh, 0) >= requirementQuery.SallaryHigh).AsQueryable();
            }
            if (!string.IsNullOrWhiteSpace(requirementQuery.Country))
            {
                queryData = queryData.Where(EF => EF.PK_App_Country_Name == requirementQuery.Country);
            }
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
        #endregion

        #region 【后台】获取推荐人列表
        /// <summary>
        /// 【后台】获取推荐人列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="customerResumeQuery"></param>
        /// <returns></returns>
        public IQueryable<App_Customer> GetResumeList(ref GridPager pager, CustomerResumeQuery customerResumeQuery)
        {
            //获取当前登录人所辖属的所有工人列表
            IQueryable<App_Customer> queryData = customerRepository.GetList(EF => EF.ParentId != null);
            if (!customerResumeQuery.AdminFlag)
            {
                queryData = queryData.Where(EF => EF.ParentId == customerResumeQuery.CustomerId);
            }
            //获取需求信息
            var req = m_Rep.GetById(customerResumeQuery.RequirementId);
            //排除已经在面试中的用户
            var listApplyJob = applyJobRepository.FindList(EF => EF.PK_App_Requirement_Title == req.Id && EF.EnumApplyStatus == "0");
            string strCustomerIds = string.Join(",", listApplyJob.Select(EF => EF.PK_App_Customer_CustomerName).ToArray());
            queryData = queryData.Where(EF => !strCustomerIds.Contains(EF.Id));
            #region 如果入参为空，则默认使用需求信息
            if (string.IsNullOrEmpty(customerResumeQuery.JobIntension))
            {
                customerResumeQuery.JobIntension = req.PK_App_Position_Name;
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(customerResumeQuery.CustomerName))
            {
                queryData = queryData.Where(a => a.CustomerName != null && a.CustomerName.Contains(customerResumeQuery.CustomerName));
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
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return queryData;
        }
        #endregion

        #region 【后台】根据需求和状态获取工人列表
        /// <summary>
        /// 【后台】根据需求和状态获取工人列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="customerResumeQuery"></param>
        /// <returns></returns>
        public IQueryable<App_Customer> GetReqResumeList(ref GridPager pager, CustomerResumeQuery customerResumeQuery)
        {
            //获取当前登录人所辖属的所有工人列表
            IQueryable<App_Customer> queryData = customerRepository.GetList(EF => EF.ParentId != null);
            if (!customerResumeQuery.AdminFlag)
            {
                queryData = queryData.Where(a => a.ParentId == customerResumeQuery.CustomerId);
            }
            //获取需求信息
            var req = m_Rep.GetById(customerResumeQuery.RequirementId);
            #region 如果入参为空，则默认使用需求信息
            if (string.IsNullOrEmpty(customerResumeQuery.JobIntension))
            {
                customerResumeQuery.JobIntension = req.PK_App_Position_Name;
            }
            #endregion
            //查询应聘的人员信息
            if ("Applyed" == customerResumeQuery.QueryFlag)
            {
                //获取当前需求对应的工人列表
                var listApplyJob = applyJobRepository.FindList(EF => EF.PK_App_Requirement_Title == req.Id && EF.EnumApplyStatus == "0" && EF.CurrentStep == "1");
                string strCustomerIds = string.Join(",", listApplyJob.Select(EF => EF.PK_App_Customer_CustomerName).ToArray());
                queryData = queryData.Where(EF => strCustomerIds.Contains(EF.Id));
            }
            //查询推荐的工人列表
            if ("Recommend" == customerResumeQuery.QueryFlag)
            {
                //获取当前需求对应的工人列表
                queryData = queryData.Where(EF => EF.Sex == req.WorkLimitSex && EF.Age >= req.WorkLimitAgeLow && EF.Age <= req.WorkLimitAgeHigh);
                if (!string.IsNullOrWhiteSpace(req.PK_App_Position_Name))
                {
                    var arrJobIntension = req.PK_App_Position_Name.Split(',').ToList();
                    queryData = queryData.ToList().Where(EF => EF.JobIntension != null && EF.JobIntension.Split(',').Intersect(arrJobIntension).Count() > 0).AsQueryable();
                }
                //排除已经在面试中的用户
                var listApplyJob = applyJobRepository.FindList(EF => EF.PK_App_Requirement_Title == req.Id && EF.EnumApplyStatus == "0");
                string strCustomerIds = string.Join(",", listApplyJob.Select(EF => EF.PK_App_Customer_CustomerName).ToArray());
                queryData = queryData.Where(EF => !strCustomerIds.Contains(EF.Id));
            }
            //查询办理中的工人列表
            if ("Proceeding" == customerResumeQuery.QueryFlag)
            {
                //获取当前需求对应的处理中的工人列表
                var listApplyJob = applyJobRepository.FindList(EF => EF.PK_App_Requirement_Title == req.Id && EF.EnumApplyStatus == "0");
                string strCustomerIds = string.Join(",", listApplyJob.Select(EF => EF.PK_App_Customer_CustomerName).ToArray());
                queryData = queryData.Where(EF => strCustomerIds.Contains(EF.Id));
            }
            if (!string.IsNullOrWhiteSpace(customerResumeQuery.CustomerName))
            {
                queryData = queryData.Where(a => a.CustomerName != null && a.CustomerName.Contains(customerResumeQuery.CustomerName));
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
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return queryData;
        }
        #endregion

        #region 【后台】获取推荐职位列表
        /// <summary>
        /// 【后台】获取推荐职位列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="requirementQuery"></param>
        /// <returns></returns>
        public IQueryable<App_Requirement> GetRecommendRequirementList(ref GridPager pager, RequirementQuery requirementQuery)
        {
            //获取当前登录人所辖属的所有工人列表
            IQueryable<App_Customer> queryDataCustomer = customerRepository.GetList(EF => EF.ParentId != null);
            IQueryable<App_Requirement> queryDataRequirement = m_Rep.GetList(EF => EF.SwitchBtnOpen == "1");
            if (!requirementQuery.AdminFlag)
            {
                queryDataCustomer = queryDataCustomer.Where(EF => EF.ParentId == requirementQuery.CustomerId);
            }
            string strCustomerIds = string.Join(",", queryDataCustomer.Select(EF => EF.Id).ToArray());
            //排除已经在面试中的用户
            var listApplyJob = applyJobRepository.FindList(EF => EF.EnumApplyStatus == "0" && strCustomerIds.Contains(EF.PK_App_Customer_CustomerName));
            strCustomerIds = string.Join(",", listApplyJob.Select(EF => EF.PK_App_Customer_CustomerName).ToArray());
            queryDataCustomer = queryDataCustomer.Where(EF => !strCustomerIds.Contains(EF.Id));
            var listCustomer = queryDataCustomer.ToList();
            //获取按照性别和年龄满足条件的需求列表
            var strSex = string.Join(",", listCustomer.Select(EF => EF.Sex).ToArray());
            int iAgeHigh = listCustomer.OrderByDescending(EF => EF.Age).FirstOrDefault().Age;
            int iAgeLow = listCustomer.OrderBy(EF => EF.Age).FirstOrDefault().Age;
            var app_Requirements = queryDataRequirement.ToList().Where(EF => strSex.Contains(EF.WorkLimitSex) && ((iAgeLow >= EF.WorkLimitAgeLow && iAgeLow <= EF.WorkLimitAgeHigh) || (iAgeHigh >= EF.WorkLimitAgeLow && iAgeHigh <= EF.WorkLimitAgeHigh))).ToList();
            //获取所有的工人的求职意向，做推荐用
            var arrJobIntension = listCustomer.Select(EF => EF.JobIntension).ToList();
            app_Requirements = app_Requirements.Where(EF => EF.PK_App_Position_Name != null && EF.PK_App_Position_Name.Split(',').Intersect(arrJobIntension).Count() > 0).ToList();
            if (!string.IsNullOrEmpty(requirementQuery.Title))
            {
                app_Requirements = app_Requirements.Where(EF => EF.Title != null && EF.Title.Contains(requirementQuery.Title)).ToList();
            }
            if (!string.IsNullOrEmpty(requirementQuery.Country))
            {
                app_Requirements = app_Requirements.Where(EF => EF.PK_App_Country_Name == requirementQuery.Country).ToList();
            }
            if (!string.IsNullOrEmpty(requirementQuery.Sex))
            {
                app_Requirements = app_Requirements.Where(EF => EF.WorkLimitSex == requirementQuery.Sex).ToList();
            }
            if (requirementQuery.AgeLow != 0)
            {
                app_Requirements = app_Requirements.Where(EF => EF.WorkLimitAgeLow <= requirementQuery.AgeLow && EF.WorkLimitAgeHigh >= requirementQuery.AgeLow).ToList();
            }
            if (requirementQuery.AgeHigh != 0)
            {
                app_Requirements = app_Requirements.Where(EF => EF.WorkLimitAgeLow <= requirementQuery.AgeHigh && EF.WorkLimitAgeHigh >= requirementQuery.AgeHigh).ToList();
            }
            if (requirementQuery.SallaryLow != 0)
            {
                app_Requirements = app_Requirements.Where(EF => Utils.ObjToDecimal(EF.SalaryLow, 0) <= requirementQuery.SallaryLow && Utils.ObjToDecimal(EF.SalaryHigh, 0) >= requirementQuery.SallaryLow).ToList();
            }
            if (requirementQuery.SallaryHigh != 0)
            {
                app_Requirements = app_Requirements.Where(EF => Utils.ObjToDecimal(EF.SalaryLow, 0) <= requirementQuery.SallaryHigh && Utils.ObjToDecimal(EF.SalaryHigh, 0) >= requirementQuery.SallaryHigh).ToList();
            }
            pager.totalRows = app_Requirements.Count;
            //排序
            var queryData = LinqHelper.SortingAndPaging(app_Requirements.AsQueryable(), pager.sort, pager.order, pager.page, pager.rows);
            return queryData;
        }
        #endregion

        #region 【后台】获取雇主发起邀请职位列表
        /// <summary>
        /// 【后台】获取雇主发起邀请职位列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="requirementQuery"></param>
        /// <returns></returns>
        public IQueryable<App_Requirement> GetInviteRequirementList(ref GridPager pager, RequirementQuery requirementQuery)
        {
            //获取当前登录人所辖属的所有工人列表
            IQueryable<App_Customer> queryDataCustomer = customerRepository.GetList(EF => EF.ParentId != null);
            IQueryable<App_Requirement> queryDataRequirement = m_Rep.GetList(EF => EF.SwitchBtnOpen == "1");
            if (!requirementQuery.AdminFlag)
            {
                queryDataCustomer = queryDataCustomer.Where(EF => EF.ParentId == requirementQuery.CustomerId);
            }
            string strCustomerIds = string.Join(",", queryDataCustomer.Select(EF => EF.Id).ToArray());
            //获取所有的当前工人下的邀请数据
            var app_RequirementInvites = requirementInviteRepository.FindList(EF => strCustomerIds.Contains(EF.Inviter));
            string reqIds = string.Join(",", app_RequirementInvites.Select(EF => EF.PK_App_Requirement_Title).ToArray());
            var app_Requirements = queryDataRequirement.ToList().Where(EF => reqIds.Contains(EF.Id)).ToList();
            if (!string.IsNullOrEmpty(requirementQuery.Title))
            {
                app_Requirements = app_Requirements.Where(EF => EF.Title != null && EF.Title.Contains(requirementQuery.Title)).ToList();
            }
            if (!string.IsNullOrEmpty(requirementQuery.Country))
            {
                app_Requirements = app_Requirements.Where(EF => EF.PK_App_Country_Name == requirementQuery.Country).ToList();
            }
            if (!string.IsNullOrEmpty(requirementQuery.Sex))
            {
                app_Requirements = app_Requirements.Where(EF => EF.WorkLimitSex == requirementQuery.Sex).ToList();
            }
            if (requirementQuery.AgeLow != 0)
            {
                app_Requirements = app_Requirements.Where(EF => EF.WorkLimitAgeLow <= requirementQuery.AgeLow && EF.WorkLimitAgeHigh >= requirementQuery.AgeLow).ToList();
            }
            if (requirementQuery.AgeHigh != 0)
            {
                app_Requirements = app_Requirements.Where(EF => EF.WorkLimitAgeLow <= requirementQuery.AgeHigh && EF.WorkLimitAgeHigh >= requirementQuery.AgeHigh).ToList();
            }
            if (requirementQuery.SallaryLow != 0)
            {
                app_Requirements = app_Requirements.Where(EF => Utils.ObjToDecimal(EF.SalaryLow, 0) <= requirementQuery.SallaryLow && Utils.ObjToDecimal(EF.SalaryHigh, 0) >= requirementQuery.SallaryLow).ToList();
            }
            if (requirementQuery.SallaryHigh != 0)
            {
                app_Requirements = app_Requirements.Where(EF => Utils.ObjToDecimal(EF.SalaryLow, 0) <= requirementQuery.SallaryHigh && Utils.ObjToDecimal(EF.SalaryHigh, 0) >= requirementQuery.SallaryHigh).ToList();
            }
            pager.totalRows = app_Requirements.Count;
            //排序
            var queryData = LinqHelper.SortingAndPaging(app_Requirements.AsQueryable(), pager.sort, pager.order, pager.page, pager.rows);
            return queryData;
        }
        #endregion

        #region 【后台】获取相关职位
        /// <summary>
        /// 【后台】获取相关职位
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="requirementQuery"></param>
        /// <returns></returns>
        public List<App_RequirementModel> GetRelateJobs(ref GridPager pager, RequirementQuery requirementQuery)
        {
            List<string> listReqId = new List<string>();
            List<App_RequirementModel> listReq = new List<App_RequirementModel>();
            string strCustomerId = requirementQuery.CustomerId;
            var requirements = m_Rep.FindList(EF => EF.SwitchBtnOpen == "1").ToList();
            var customerResume = customerRepository.GetById(strCustomerId);
            var arrJobIntension = new List<string>();
            if (!string.IsNullOrEmpty(customerResume.JobIntension))
            {
                arrJobIntension = customerResume.JobIntension.Split(',').ToList();
            }
            if (!requirementQuery.AdminFlag)
            {
                requirements = requirements.Where(EF => EF.PK_App_Customer_CustomerName == requirementQuery.PublisherId).ToList();
            }
            if ("Recommend".Equals(requirementQuery.QueryFlag))
            {
                //系统推荐的
                var recommendRequirementList = requirements.Where(EF =>
                            EF.WorkLimitAgeLow <= customerResume.Age
                            && EF.WorkLimitAgeHigh >= customerResume.Age
                            && EF.WorkLimitSex == customerResume.Sex
                            && EF.PK_App_Position_Name != null && EF.PK_App_Position_Name.Split(',').Intersect(arrJobIntension).Count() > 0);
                foreach (var recommendReq in recommendRequirementList)
                {
                    App_RequirementModel app_RequirementModel = new App_RequirementModel();
                    LinqHelper.ModelTrans(recommendReq, app_RequirementModel);
                    app_RequirementModel.ReqType = "2";
                    listReq.Add(app_RequirementModel);
                }
            }
            else
            {
                var applyJobList = applyJobRepository.FindList(EF => EF.PK_App_Customer_CustomerName == strCustomerId && EF.EnumApplyStatus == "0");
                //这里包括了这个简历，所关联的所有职位，包括应聘的，系统推荐的，雇主面试邀请 ，办理中的职位
                //获取应聘的需求列表（就是发起申请的，申请里状态是1的那些）
                var applyingJobList = applyJobList.Where(EF => EF.EnumApplyStatus == "0" && EF.CurrentStep == "1");
                string strReqIds = string.Join(",", applyJobList.Select(EF => EF.PK_App_Requirement_Title).ToArray());
                var applyingJobReqList = requirements.Where(EF => strReqIds.Contains(EF.Id)).ToList();
                foreach (var applyingJobReq in applyingJobReqList)
                {
                    App_RequirementModel app_RequirementModel = new App_RequirementModel();
                    LinqHelper.ModelTrans(applyingJobReq, app_RequirementModel);
                    app_RequirementModel.ReqType = "1";
                    listReq.Add(app_RequirementModel);
                }
                //系统推荐的
                var recommendReqList = requirements.Where(EF =>
                            EF.WorkLimitAgeLow <= customerResume.Age
                            && EF.WorkLimitAgeHigh >= customerResume.Age
                            && EF.WorkLimitSex == customerResume.Sex
                            && EF.PK_App_Position_Name != null && EF.PK_App_Position_Name.Split(',').Intersect(arrJobIntension).Count() > 0);
                foreach (var recommendReq in recommendReqList)
                {
                    App_RequirementModel app_RequirementModel = new App_RequirementModel();
                    LinqHelper.ModelTrans(recommendReq, app_RequirementModel);
                    app_RequirementModel.ReqType = "2";
                    listReq.Add(app_RequirementModel);
                }
                //雇主面试邀请
                var inviteList = requirementInviteRepository.FindList(EF => EF.Inviter == strCustomerId);
                strReqIds = string.Join(",", inviteList.Select(EF => EF.PK_App_Requirement_Title).ToArray());
                var inviteReqList = requirements.Where(EF => strReqIds.Contains(EF.Id)).ToList();
                foreach (var inviteReq in inviteReqList)
                {
                    App_RequirementModel app_RequirementModel = new App_RequirementModel();
                    LinqHelper.ModelTrans(inviteReq, app_RequirementModel);
                    app_RequirementModel.ReqType = "3";
                    listReq.Add(app_RequirementModel);
                }
                //办理中的职位
                strReqIds = string.Join(",", applyJobList.Select(EF => EF.PK_App_Requirement_Title).ToArray());
                var applyJobReqList = requirements.Where(EF => strReqIds.Contains(EF.Id)).ToList();
                foreach (var applyJobReq in applyJobReqList)
                {
                    App_RequirementModel app_RequirementModel = new App_RequirementModel();
                    LinqHelper.ModelTrans(applyJobReq, app_RequirementModel);
                    app_RequirementModel.ReqType = "4";
                    listReq.Add(app_RequirementModel);
                }
            }
            pager.totalRows = listReq.Count();
            if (pager.page > 0)
            {
                listReq = listReq.OrderBy(EF => EF.ReqType).ThenByDescending(EF => EF.ModificationTime).Skip((pager.page - 1) * pager.rows).Take(pager.rows).ToList();
            }
            return listReq;
        }
        #endregion

        #region 【后台】给符合条件工人推送消息
        /// <summary>
        /// 【后台】给符合条件工人推送消息
        /// </summary>
        /// <param name="ReqId"></param>
        /// <returns></returns>
        public void SendMessageToWorker(string UserId, string ReqId)
        {
            //获取当前登录人所辖属的所有工人列表
            IQueryable<App_Customer> queryData = customerRepository.GetList();
            //获取需求信息
            var req = m_Rep.GetById(ReqId);
            var list = queryData.Where(EF => EF.Sex == req.WorkLimitSex
                                            && EF.Age >= req.WorkLimitAgeLow
                                            && EF.Age <= req.WorkLimitAgeHigh).ToList();
            var arrJobIntension = req.PK_App_Position_Name.Split(',');
            list = list.Where(EF => EF.JobIntension != null && EF.JobIntension.Split(',').Intersect(arrJobIntension).Count() > 0).ToList();
            foreach (var item in list)
            {
                //推送消息给工人，工人可以同意或者拒绝
                sysMessageRepository.CrtSysMessage(UserId, item.Id, req.Id, "职位匹配提醒", "根据您的求职意向，平台为您推荐了以下职位，点击查看详情", "1", "1", "");
            }
        }
        #endregion

        #region 【后台】获取职位列表
        /// <summary>
        /// 【后台】获取职位列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="requirementQuery"></param>
        /// <returns></returns>
        public List<App_RequirementModel> GetContractorRequirementList(ref GridPager pager, RequirementQuery requirementQuery)
        {
            IQueryable<App_Requirement> queryData = m_Rep.GetList();
            List<string> listReqIds = new List<string>();
            List<App_Requirement> app_Requirements = new List<App_Requirement>();
            //获取当前登录人所创建的职位列表
            if (!requirementQuery.AdminFlag)
            {
                queryData = queryData.Where(EF => EF.PK_App_Customer_CustomerName == requirementQuery.CustomerId);
            }
            var ReqIds = queryData.Select(EF => EF.Id).ToList();
            var listApplyJob = applyJobRepository.FindList().ToList().Where(EF => ReqIds.Contains(EF.PK_App_Requirement_Title)).ToList();
            if (requirementQuery.QueryFlag == "Applyed")
            {
                listApplyJob = listApplyJob.Where(EF => EF.EnumApplyStatus == "0" && EF.CurrentStep == "1").ToList();
                ReqIds = listApplyJob.Select(EF => EF.PK_App_Requirement_Title).ToList();
            }
            if (requirementQuery.QueryFlag == "Recommend")
            {
                foreach (var item in queryData.ToList())
                {
                    var customers = GetRecommenUsers(item);
                    if (customers.Count > 0)
                    {
                        listReqIds.Add(item.Id);
                    }
                }
                ReqIds = listReqIds;
            }
            if (requirementQuery.QueryFlag == "Invite")
            {
                var listInvite = requirementInviteRepository.FindList(EF => EF.InitiatorId == requirementQuery.CustomerId);
                ReqIds = listInvite.Select(EF => EF.PK_App_Requirement_Title).ToList();
            }
            if (requirementQuery.QueryFlag == "Proceeding")
            {
                listApplyJob = listApplyJob.Where(EF => EF.EnumApplyStatus == "0").ToList().Where(EF => Utils.ObjToInt(EF.CurrentStep, 0) > 3).ToList();
                ReqIds = listApplyJob.Select(EF => EF.PK_App_Requirement_Title).ToList();
            }
            if (requirementQuery.QueryFlag == "RelateJob")
            {
                listApplyJob = listApplyJob.Where(EF => EF.EnumApplyStatus == "0").ToList().Where(EF => Utils.ObjToInt(EF.CurrentStep, 0) > 3).ToList();
                ReqIds = listApplyJob.Select(EF => EF.PK_App_Requirement_Title).ToList();
            }
            queryData = queryData.Where(EF => ReqIds.Contains(EF.Id));
            if (!string.IsNullOrWhiteSpace(requirementQuery.Title))
            {
                queryData = queryData.Where(EF => EF.Title != null && EF.Title.Contains(requirementQuery.Title));
            }
            if (!string.IsNullOrWhiteSpace(requirementQuery.PositionId))
            {
                var arrPositionId = requirementQuery.PositionId.Split(',').ToList();
                queryData = queryData.ToList().Where(EF => EF.PK_App_Position_Name != null && EF.PK_App_Position_Name.Split(',').Intersect(arrPositionId).Count() > 0).AsQueryable();
            }
            if (!string.IsNullOrWhiteSpace(requirementQuery.Sex))
            {
                queryData = queryData.Where(EF => EF.WorkLimitSex == requirementQuery.Sex);
            }
            if (requirementQuery.AgeLow != 0)
            {
                queryData = queryData.Where(EF => EF.WorkLimitAgeLow <= requirementQuery.AgeLow && EF.WorkLimitAgeHigh >= requirementQuery.AgeLow);
            }
            if (requirementQuery.AgeHigh != 0)
            {
                queryData = queryData.Where(EF => EF.WorkLimitAgeLow <= requirementQuery.AgeHigh && EF.WorkLimitAgeHigh >= requirementQuery.AgeHigh);
            }
            if (requirementQuery.SallaryLow != 0)
            {
                queryData = queryData.ToList().Where(EF => EF.SalaryLow != null && Utils.ObjToDecimal(EF.SalaryLow, 0) >= requirementQuery.SallaryLow).AsQueryable();
            }
            if (requirementQuery.SallaryHigh != 0)
            {
                queryData = queryData.ToList().Where(EF => EF.SalaryHigh != null && Utils.ObjToDecimal(EF.SalaryHigh, 0) >= requirementQuery.SallaryHigh).AsQueryable();
            }
            if (!string.IsNullOrWhiteSpace(requirementQuery.Country))
            {
                queryData = queryData.Where(EF => EF.PK_App_Country_Name == requirementQuery.Country);
            }
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
        #endregion
    }
}

