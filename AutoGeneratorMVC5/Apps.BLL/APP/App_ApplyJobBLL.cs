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

namespace Apps.BLL.App
{
    public partial class App_ApplyJobBLL
    {
        #region Reps
        [Dependency]
        public SysLogRepository sysLog { get; set; }
        [Dependency]
        public EnumDictionaryBLL enumDictionary { get; set; }
        [Dependency]
        public App_RequirementRepository requirementRepository { get; set; }
        [Dependency]
        public App_RequirementBLL requirementBLL { get; set; }
        [Dependency]
        public App_CustomerRepository customerRepository { get; set; }
        [Dependency]
        public App_CustomerJobIntensionRepository customerJobIntensionRepository { get; set; }
        [Dependency]
        public App_ApplyJobRecordRepository applyJobRecordRepository { get; set; }
        [Dependency]
        public App_ApplyJobStepRepository applyJobStepRepository { get; set; }
        [Dependency]
        public App_OfficeRepository officeRepository { get; set; }
        [Dependency]
        public SysMessageRepository sysMessageRepository { get; set; }
        #endregion

        #region 提交应聘申请
        /// <summary>
        /// 提交应聘申请
        /// </summary>
        /// <param name="applyJobPost"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public string CreateApplyJob(ApplyJobPost applyJobPost, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString(), "开始", "CreateApplyJob", "App_ApplyJobBLL");
            string ReqId = applyJobPost.RequirementId, customerId = applyJobPost.CustomerId;
            var now = ResultHelper.NowTime;
            var Req = requirementRepository.GetById(ReqId);
            if (null == Req)
            {
                ErrorMsg = "申请的需求不存在";
                sysLog.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString() + ErrorMsg, "结束", "CreateApplyJob", "App_ApplyJobBLL");
                return null;
            }
            string strApplyJobId = CrtStep2ApplyJob(applyJobPost, out ErrorMsg, customerId, Req);
            sysLog.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString() + ErrorMsg, "结束", "CreateApplyJob", "App_ApplyJobBLL");
            return strApplyJobId;
        }
        #endregion

        #region 修改应聘申请
        /// <summary>
        /// 修改应聘申请
        /// </summary>
        /// <param name="applyJobPost"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public ApplyJobVm EditApplyJob(EditApplyJobPost editApplyJobPost, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(editApplyJobPost.UserId, editApplyJobPost.ToString(), "开始", "EditApplyJob", "App_ApplyJobBLL");
            string ApplyJobId = editApplyJobPost.ApplyJobId, EnumApplyStatus = editApplyJobPost.EnumApplyStatus, CurrentStep = editApplyJobPost.CurrentStep;
            var now = ResultHelper.NowTime;
            var applyJob = m_Rep.GetById(ApplyJobId);
            if (null == applyJob)
            {
                ErrorMsg = "修改的申请不存在";
                sysLog.WriteServiceLog(editApplyJobPost.UserId, editApplyJobPost.ToString() + ErrorMsg, "结束", "EditApplyJob", "App_ApplyJobBLL");
                return null;
            }
            applyJob.ModificationTime = now;
            applyJob.ModificationUserName = editApplyJobPost.UserId;
            if (!string.IsNullOrEmpty(EnumApplyStatus))
            {
                applyJob.EnumApplyStatus = EnumApplyStatus;
            }
            if (!string.IsNullOrEmpty(CurrentStep))
            {
                applyJob.CurrentStep = CurrentStep;
            }
            if (!string.IsNullOrEmpty(editApplyJobPost.EnumPromisePayWay))
            {
                applyJob.EnumPromisePayWay = editApplyJobPost.EnumPromisePayWay;
            }
            if (!string.IsNullOrEmpty(editApplyJobPost.EnumServicePayWay))
            {
                applyJob.EnumServicePayWay = editApplyJobPost.EnumServicePayWay;
            }
            if (!string.IsNullOrEmpty(editApplyJobPost.EnumTailPayWay))
            {
                applyJob.EnumTailPayWay = editApplyJobPost.EnumTailPayWay;
            }
            try
            {
                m_Rep.Edit(applyJob);
                //如果步骤改成3，说明支付成功了，需要修改记录为已支付保证金，然后增加面试进行中记录
                if ("3".Equals(CurrentStep))
                {
                    //添加应聘记录--修改记录为已支付保证金
                    App_ApplyJobRecord applyJobRecord = new App_ApplyJobRecord();
                    //获取当前用户的当前应聘记录
                    applyJobRecord = applyJobRecordRepository.Find(EF => EF.PK_App_ApplyJob_Id == ApplyJobId && EF.PK_App_Customer_CustomerName == applyJob.PK_App_Customer_CustomerName && EF.Step == "2");
                    applyJobRecord.ModificationTime = now;
                    applyJobRecord.ModificationUserName = editApplyJobPost.UserId;
                    applyJobRecord.Result = "已完成";
                    applyJobRecord.Content = "保证金支付";
                    applyJobRecord.HappenDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                    applyJobRecordRepository.Edit(applyJobRecord);
                    //添加应聘记录--增加面试进行中记录
                    applyJobRecord = new App_ApplyJobRecord();
                    applyJobRecord.Id = ResultHelper.NewId;
                    applyJobRecord.CreateTime = now;
                    applyJobRecord.CreateUserName = editApplyJobPost.UserId;
                    applyJobRecord.ModificationTime = now;
                    applyJobRecord.ModificationUserName = editApplyJobPost.UserId;
                    applyJobRecord.PK_App_ApplyJob_Id = applyJob.Id;
                    applyJobRecord.PK_App_Customer_CustomerName = applyJob.PK_App_Customer_CustomerName;
                    applyJobRecord.Step = "3";
                    applyJobRecord.EnumApplyStatus = "3";
                    applyJobRecord.Result = "进行中";
                    applyJobRecord.Content = "面试";
                    applyJobRecord.HappenDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                    applyJobRecordRepository.Create(applyJobRecord);
                }
                //如果步骤改成4，说明面试完成了，需要修改记录面试结果，然后根据条件增加考试进行中记录
                if ("4".Equals(CurrentStep))
                {
                    //添加应聘记录--修改记录为已支付保证金
                    App_ApplyJobRecord applyJobRecord = new App_ApplyJobRecord();
                    //获取当前用户的当前应聘记录
                    applyJobRecord = applyJobRecordRepository.Find(EF => EF.PK_App_ApplyJob_Id == ApplyJobId && EF.PK_App_Customer_CustomerName == applyJob.PK_App_Customer_CustomerName && EF.Step == "3");
                    applyJobRecord.ModificationTime = now;
                    applyJobRecord.ModificationUserName = editApplyJobPost.UserId;
                    applyJobRecord.Result = editApplyJobPost.Result;
                    //如果面试通过，则增加考试记录
                    if ("已通过".Equals(editApplyJobPost.Result))
                    {
                        applyJobRecord.Content = "面试已通过";
                    }
                    else
                    {
                        applyJobRecord.Content = "面试未通过";
                    }
                    applyJobRecord.HappenDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                    applyJobRecordRepository.Edit(applyJobRecord);
                    //如果面试通过，则增加考试记录
                    if ("已通过".Equals(editApplyJobPost.Result))
                    {
                        //添加应聘记录--增加面试进行中记录
                        applyJobRecord = new App_ApplyJobRecord();
                        applyJobRecord.Id = ResultHelper.NewId;
                        applyJobRecord.CreateTime = now;
                        applyJobRecord.CreateUserName = editApplyJobPost.UserId;
                        applyJobRecord.ModificationTime = now;
                        applyJobRecord.ModificationUserName = editApplyJobPost.UserId;
                        applyJobRecord.PK_App_ApplyJob_Id = applyJob.Id;
                        applyJobRecord.PK_App_Customer_CustomerName = applyJob.PK_App_Customer_CustomerName;
                        applyJobRecord.Step = "4";
                        applyJobRecord.Result = "进行中";
                        applyJobRecord.Content = "考试";
                        applyJobRecord.ConfigDate = editApplyJobPost.ConfigDate;
                        applyJobRecord.ConfigPlace = editApplyJobPost.ConfigPlace;
                        applyJobRecordRepository.Create(applyJobRecord);
                    }
                    else
                    {
                        //如果面试未通过，则不更新步骤数，还改成3
                        applyJob.EnumApplyStatus = "3";
                        applyJob.CurrentStep = "3";
                        applyJob.ModificationTime = now;
                        applyJob.ModificationUserName = editApplyJobPost.UserId;
                        m_Rep.Edit(applyJob);
                    }
                }
                //如果步骤改成6，说明服务费支付成功了，需要修改记录为已支付服务费，然后增加审核材料进行中记录
                if ("6".Equals(EnumApplyStatus))
                {
                    var applyJobRecord = new App_ApplyJobRecord();
                    applyJobRecord.Id = ResultHelper.NewId;
                    applyJobRecord.CreateTime = now;
                    applyJobRecord.CreateUserName = editApplyJobPost.UserId;
                    applyJobRecord.ModificationTime = now;
                    applyJobRecord.ModificationUserName = editApplyJobPost.UserId;
                    applyJobRecord.PK_App_ApplyJob_Id = applyJob.Id;
                    applyJobRecord.PK_App_Customer_CustomerName = applyJob.PK_App_Customer_CustomerName;
                    applyJobRecord.Step = "6";
                    applyJobRecord.EnumApplyStatus = "0";
                    applyJobRecord.Result = "进行中";
                    applyJobRecord.Content = "审核材料";
                    applyJobRecord.HappenDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                    applyJobRecordRepository.Create(applyJobRecord);
                    string PreStep = "5";
                    App_ApplyJobRecord applyJobRecordPre = applyJobRecordRepository.Find(EF => EF.PK_App_ApplyJob_Id == ApplyJobId && EF.PK_App_Customer_CustomerName == applyJob.PK_App_Customer_CustomerName && EF.Step == PreStep);
                    applyJobRecord.Result = "已完成";
                    applyJobRecord.ModificationTime = now;
                    applyJobRecord.ModificationUserName = editApplyJobPost.UserId;
                    if (string.IsNullOrEmpty(applyJobRecord.HappenDate))
                    {
                        applyJobRecord.HappenDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    applyJobRecordRepository.Edit(applyJobRecord);
                }
                ApplyJobVm applyJobVm = GetApplyJob(editApplyJobPost.UserId, editApplyJobPost.ApplyJobId, editApplyJobPost.Longitude, editApplyJobPost.Latitude, ref ErrorMsg);
                ErrorMsg = "申请更新成功";
                sysLog.WriteServiceLog(editApplyJobPost.UserId, editApplyJobPost.ToString() + ErrorMsg, "结束", "EditApplyJob", "App_ApplyJobBLL");
                return applyJobVm;
            }
            catch (Exception ex)
            {
                ErrorMsg = "申请更新出现异常";
                sysLog.WriteServiceLog(editApplyJobPost.UserId, editApplyJobPost.ToString() + ErrorMsg + ex.Message, "结束", "EditApplyJob", "App_ApplyJobBLL");
                return null;
            }
        }
        #endregion

        #region 获取应聘申请
        /// <summary>
        /// 获取应聘申请
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="CustomerId"></param>
        /// <param name="PageNum"></param>
        /// <param name="RecordNum"></param>
        /// <param name="DataCount"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public List<ApplyJobVm> GetApplyJobs(string UserId, string CustomerId, int PageNum, int RecordNum, ref int DataCount, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "CustomerId:" + CustomerId + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetApplyJobs", "App_ApplyJobBLL");
            List<ApplyJobVm> applyJobVms = new List<ApplyJobVm>();
            var applyJobs = m_Rep.FindList(EF => EF.CreateUserName == CustomerId && EF.EnumApplyStatus != "2").OrderByDescending(EF => EF.ModificationTime).ToList();
            DataCount = applyJobs.Count;
            if (PageNum > 0)
            {
                applyJobs = applyJobs.Skip((PageNum - 1) * RecordNum).Take(RecordNum).ToList();
            }
            var requirements = requirementRepository.FindList().ToList();
            var offices = officeRepository.FindList().ToList();
            foreach (var item in applyJobs)
            {
                ApplyJobVm applyJobVm = new ApplyJobVm();
                applyJobVm.requirementDetailVm = new RequirementDetailVm();
                applyJobVm.applyJobUserVm = new ApplyJobUserVm();
                applyJobVm.applyJobRecordVm = new ApplyJobRecordVm();
                var app_Requirement = requirementBLL.GetApp_Requirement(item.PK_App_Customer_CustomerName, item.PK_App_Requirement_Title, ref ErrorMsg);
                LinqHelper.ModelTrans(app_Requirement, applyJobVm.requirementDetailVm);
                applyJobVm.ApplyJobId = item.Id;
                applyJobVm.PK_App_Requirement_Title = item.PK_App_Requirement_Title;
                applyJobVm.PK_App_Customer_CustomerName = item.PK_App_Customer_CustomerName;
                applyJobVm.ApplyJobPromiseMoney = item.PromiseMoney.ToString();
                applyJobVm.EnumApplyStatus = item.EnumApplyStatus;
                applyJobVm.ApplyStatus = enumDictionary.GetDicName("App_ApplyJob.EnumApplyStatus", item.EnumApplyStatus);
                applyJobVm.CurrentStep = item.CurrentStep;
                applyJobVm.PromiseMoney = item.PromiseMoney.ToString();
                applyJobVm.ServiceMoney = item.ServiceMoney.ToString();
                applyJobVm.ServiceTailMoney = item.TailMoney.ToString();
                applyJobVm.EnumPromisePayWay = item.EnumPromisePayWay;
                applyJobVm.PromisePayWayName = enumDictionary.GetDicName("App.EnumPayWay", item.EnumPromisePayWay);
                applyJobVm.EnumServicePayWay = item.EnumServicePayWay;
                applyJobVm.ServicePayWayName = enumDictionary.GetDicName("App.EnumPayWay", item.EnumServicePayWay);
                applyJobVm.EnumTailPayWay = item.EnumTailPayWay;
                applyJobVm.TailPayWayName = enumDictionary.GetDicName("App.EnumPayWay", item.EnumTailPayWay);
                //如果添加人和应聘人相同，则是自己报名
                var customer = customerRepository.GetById(item.PK_App_Customer_CustomerName);
                if (null != customer)
                {
                    applyJobVm.applyJobUserVm.CustomerName = customer.CustomerName;
                    applyJobVm.applyJobUserVm.Photo = customer.CustomerPhoto;
                    applyJobVm.applyJobUserVm.Age = customer.Age;
                    applyJobVm.applyJobUserVm.BirthPlace = customer.BirthPlace;
                    applyJobVm.applyJobUserVm.Sex = customer.Sex;
                }
                var customerJobIntension = customerJobIntensionRepository.Find(EF => EF.PK_App_Customer_CustomerName == item.PK_App_Customer_CustomerName);
                if (null != customerJobIntension)
                {
                    applyJobVm.applyJobUserVm.AbroadExp = enumDictionary.GetDicName("App_CustomerJobIntension.AbroadExp", customerJobIntension.AbroadExp);
                }
                var applyJobRecord = applyJobRecordRepository.Find(EF => EF.PK_App_ApplyJob_Id == item.Id && EF.Step == item.CurrentStep);
                LinqHelper.ModelTrans(applyJobRecord, applyJobVm.applyJobRecordVm);
                applyJobVms.Add(applyJobVm);
            }
            ErrorMsg = "获取申请列表成功";
            sysLog.WriteServiceLog(UserId, "CustomerId:" + CustomerId + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetApplyJobs", "App_ApplyJobBLL");
            return applyJobVms;
        }
        #endregion

        #region 获取应聘申请详情
        /// <summary>
        /// 获取应聘申请
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ApplyJobId"></param>
        /// <param name="Longitude"></param>
        /// <param name="Latitude"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public ApplyJobVm GetApplyJob(string UserId, string ApplyJobId, string Longitude, string Latitude, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "ApplyJobId:" + ApplyJobId + ",Longitude:" + Longitude + ",Latitude:" + Latitude, "开始", "GetApplyJob", "App_ApplyJobBLL");
            ApplyJobVm applyJobVm = new ApplyJobVm();
            var applyJob = m_Rep.GetById(ApplyJobId);
            if (null == applyJob)
            {
                ErrorMsg = "获取申请详情为空";
                sysLog.WriteServiceLog(UserId, "ApplyJobId:" + ApplyJobId + ",Longitude:" + Longitude + ",Latitude:" + Latitude + ",ErrorMsg:" + ErrorMsg, "结束", "GetApplyJob", "App_ApplyJobBLL");
                return null;
            }
            var offices = officeRepository.FindList().ToList();
            if (!string.IsNullOrEmpty(Longitude) && !string.IsNullOrEmpty(Latitude))
            {
                double dLat = Utils.StrToDouble(Latitude, 0);
                double dLng = Utils.StrToDouble(Longitude, 0);
                offices = offices.OrderBy(EF => GPSUtil.GetDistance(Utils.StrToDouble(EF.Latitude, 0), Utils.StrToDouble(EF.Longitude, 0), dLat, dLng)).ToList();
            }
            var officeNear = offices.FirstOrDefault();
            LinqHelper.ModelTrans(officeNear, applyJobVm.officeVm);
            var requirements = requirementRepository.FindList().ToList();
            var app_Requirement = requirementBLL.GetApp_Requirement(applyJob.PK_App_Customer_CustomerName, applyJob.PK_App_Requirement_Title, ref ErrorMsg);
            LinqHelper.ModelTrans(app_Requirement, applyJobVm.requirementDetailVm);
            applyJobVm.ApplyJobId = applyJob.Id;
            applyJobVm.PK_App_Requirement_Title = applyJob.PK_App_Requirement_Title;
            applyJobVm.PK_App_Customer_CustomerName = applyJob.PK_App_Customer_CustomerName;
            applyJobVm.ApplyJobPromiseMoney = applyJob.PromiseMoney.ToString();
            applyJobVm.EnumApplyStatus = applyJob.EnumApplyStatus;
            applyJobVm.CurrentStep = applyJob.CurrentStep;
            applyJobVm.PromiseMoney = applyJob.PromiseMoney.ToString();
            applyJobVm.ServiceMoney = applyJob.ServiceMoney.ToString();
            applyJobVm.ServiceTailMoney = applyJob.TailMoney.ToString();
            applyJobVm.EnumPromisePayWay = applyJob.EnumPromisePayWay;
            applyJobVm.EnumServicePayWay = applyJob.EnumServicePayWay;
            applyJobVm.EnumTailPayWay = applyJob.EnumTailPayWay;
            var customer = customerRepository.GetById(applyJob.PK_App_Customer_CustomerName);
            if (null != customer)
            {
                applyJobVm.applyJobUserVm.CustomerName = customer.CustomerName;
                applyJobVm.applyJobUserVm.Photo = customer.CustomerPhoto;
                applyJobVm.applyJobUserVm.Age = customer.Age;
                applyJobVm.applyJobUserVm.BirthPlace = customer.BirthPlace;
                applyJobVm.applyJobUserVm.Sex = customer.Sex;
            }
            var customerJobIntension = customerJobIntensionRepository.Find(EF => EF.PK_App_Customer_CustomerName == applyJob.PK_App_Customer_CustomerName);
            if (null != customerJobIntension)
            {
                applyJobVm.applyJobUserVm.AbroadExp = enumDictionary.GetDicName("App_CustomerJobIntension.AbroadExp", customerJobIntension.AbroadExp);
            }
            var applyJobRecord = applyJobRecordRepository.Find(EF => EF.PK_App_ApplyJob_Id == ApplyJobId && EF.Step == applyJob.CurrentStep);
            LinqHelper.ModelTrans(applyJobRecord, applyJobVm.applyJobRecordVm);

            var applyJobRecords = applyJobRecordRepository.FindList(EF => EF.PK_App_ApplyJob_Id == ApplyJobId).OrderBy(EF => EF.Step).ToList();
            foreach (var item in applyJobRecords)
            {
                var applyJobRecordVm = new ApplyJobRecordVm();
                LinqHelper.ModelTrans(item, applyJobRecordVm);
                applyJobVm.applyJobRecordVms.Add(applyJobRecordVm);
            }

            #region 获取支付明细
            ApplyJobPayVm applyJobPayVm = new ApplyJobPayVm();
            if ("2".Equals(applyJob.CurrentStep))
            {
                applyJobPayVm = new ApplyJobPayVm();
                applyJobPayVm.Name = "面试保证金";
                applyJobPayVm.Money = applyJobVm.PromiseMoney;
                applyJobPayVm.Status = "未支付";
                applyJobPayVm.PayTime = "";
                applyJobVm.applyJobPayVms.Add(applyJobPayVm);
            }
            if ("3".Equals(applyJob.CurrentStep) || "4".Equals(applyJob.CurrentStep))
            {
                applyJobPayVm = new ApplyJobPayVm();
                applyJobPayVm.Name = "面试保证金";
                applyJobPayVm.Money = applyJobVm.PromiseMoney;
                applyJobPayVm.Status = "已支付";
                //获取当时步骤记录的时间
                var applyJobRecord2 = applyJobRecordRepository.Find(EF => EF.PK_App_ApplyJob_Id == ApplyJobId && EF.Step == "2");
                applyJobPayVm.PayTime = applyJobRecord2.HappenDate;
                applyJobVm.applyJobPayVms.Add(applyJobPayVm);
            }
            if ("5".Equals(applyJob.CurrentStep))
            {
                applyJobPayVm = new ApplyJobPayVm();
                applyJobPayVm.Name = "面试保证金";
                applyJobPayVm.Money = applyJobVm.PromiseMoney;
                applyJobPayVm.Status = "已支付";
                //获取当时步骤记录的时间
                var applyJobRecord3 = applyJobRecordRepository.Find(EF => EF.PK_App_ApplyJob_Id == ApplyJobId && EF.Step == "3");
                applyJobPayVm.PayTime = applyJobRecord3.HappenDate;
                applyJobVm.applyJobPayVms.Add(applyJobPayVm);
                applyJobPayVm = new ApplyJobPayVm();
                applyJobPayVm.Name = "支付服务费";
                applyJobPayVm.Money = applyJobVm.ServiceMoney;
                applyJobPayVm.Status = "未支付";
                applyJobPayVm.PayTime = "";
                applyJobVm.applyJobPayVms.Add(applyJobPayVm);
            }
            if ("6".Equals(applyJob.CurrentStep) || "7".Equals(applyJob.CurrentStep))
            {
                applyJobPayVm = new ApplyJobPayVm();
                applyJobPayVm.Name = "面试保证金";
                applyJobPayVm.Money = applyJobVm.PromiseMoney;
                applyJobPayVm.Status = "已支付";
                //获取当时步骤记录的时间
                var applyJobRecord3 = applyJobRecordRepository.Find(EF => EF.PK_App_ApplyJob_Id == ApplyJobId && EF.Step == "3");
                applyJobPayVm.PayTime = applyJobRecord3.HappenDate;
                applyJobVm.applyJobPayVms.Add(applyJobPayVm);
                applyJobPayVm = new ApplyJobPayVm();
                applyJobPayVm.Name = "支付服务费";
                applyJobPayVm.Money = applyJobVm.ServiceMoney;
                applyJobPayVm.Status = "已支付";
                //获取当时步骤记录的时间
                var applyJobRecord5 = applyJobRecordRepository.Find(EF => EF.PK_App_ApplyJob_Id == ApplyJobId && EF.Step == "5");
                applyJobPayVm.PayTime = applyJobRecord5.HappenDate;
                applyJobVm.applyJobPayVms.Add(applyJobPayVm);
            }
            if ("8".Equals(applyJob.CurrentStep))
            {
                applyJobPayVm = new ApplyJobPayVm();
                applyJobPayVm.Name = "面试保证金";
                applyJobPayVm.Money = applyJobVm.PromiseMoney;
                applyJobPayVm.Status = "已支付";
                //获取当时步骤记录的时间
                var applyJobRecord3 = applyJobRecordRepository.Find(EF => EF.PK_App_ApplyJob_Id == ApplyJobId && EF.Step == "3");
                applyJobPayVm.PayTime = applyJobRecord3.HappenDate;
                applyJobVm.applyJobPayVms.Add(applyJobPayVm);
                applyJobPayVm = new ApplyJobPayVm();
                applyJobPayVm.Name = "支付服务费";
                applyJobPayVm.Money = applyJobVm.ServiceMoney;
                applyJobPayVm.Status = "已支付";
                //获取当时步骤记录的时间
                var applyJobRecord5 = applyJobRecordRepository.Find(EF => EF.PK_App_ApplyJob_Id == ApplyJobId && EF.Step == "5");
                applyJobPayVm.PayTime = applyJobRecord5.HappenDate;
                applyJobVm.applyJobPayVms.Add(applyJobPayVm);
                applyJobPayVm = new ApplyJobPayVm();
                applyJobPayVm.Name = "支付尾款";
                applyJobPayVm.Money = applyJobVm.ServiceTailMoney;
                applyJobPayVm.Status = "未支付";
                applyJobPayVm.PayTime = "";
                applyJobVm.applyJobPayVms.Add(applyJobPayVm);
            }
            if ("9".Equals(applyJob.CurrentStep))
            {
                applyJobPayVm = new ApplyJobPayVm();
                applyJobPayVm.Name = "面试保证金";
                applyJobPayVm.Money = applyJobVm.PromiseMoney;
                applyJobPayVm.Status = "已支付";
                //获取当时步骤记录的时间
                var applyJobRecord3 = applyJobRecordRepository.Find(EF => EF.PK_App_ApplyJob_Id == ApplyJobId && EF.Step == "3");
                applyJobPayVm.PayTime = applyJobRecord3.HappenDate;
                applyJobVm.applyJobPayVms.Add(applyJobPayVm);
                applyJobPayVm = new ApplyJobPayVm();
                applyJobPayVm.Name = "支付服务费";
                applyJobPayVm.Money = applyJobVm.ServiceMoney;
                applyJobPayVm.Status = "已支付";
                //获取当时步骤记录的时间
                var applyJobRecord5 = applyJobRecordRepository.Find(EF => EF.PK_App_ApplyJob_Id == ApplyJobId && EF.Step == "5");
                applyJobPayVm.PayTime = applyJobRecord5.HappenDate;
                applyJobVm.applyJobPayVms.Add(applyJobPayVm);
                applyJobPayVm = new ApplyJobPayVm();
                applyJobPayVm.Name = "支付尾款";
                applyJobPayVm.Money = applyJobVm.ServiceTailMoney;
                applyJobPayVm.Status = "已支付";
                //获取当时步骤记录的时间
                var applyJobRecord8 = applyJobRecordRepository.Find(EF => EF.PK_App_ApplyJob_Id == ApplyJobId && EF.Step == "8");
                applyJobPayVm.PayTime = applyJobRecord8.HappenDate;
                applyJobVm.applyJobPayVms.Add(applyJobPayVm);
            }
            #endregion

            ErrorMsg = "获取申请详情成功";
            sysLog.WriteServiceLog(UserId, "ApplyJobId:" + ApplyJobId + ",Longitude:" + Longitude + ",Latitude:" + Latitude + ",ErrorMsg:" + ErrorMsg, "结束", "GetApplyJob", "App_ApplyJobBLL");
            return applyJobVm;
        }
        #endregion

        #region 获取应聘申请记录列表
        /// <summary>
        /// 获取应聘申请记录列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ApplyJobId"></param>
        /// <param name="DataCount"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public List<ApplyJobRecordVm> GetApplyJobRecords(string UserId, string ApplyJobId, ref int DataCount, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "ApplyJobId:" + ApplyJobId, "开始", "GetApplyJobRecords", "App_ApplyJobBLL");
            List<ApplyJobRecordVm> applyJobRecordVms = new List<ApplyJobRecordVm>();
            var applyJobRecords = applyJobRecordRepository.FindList(EF => EF.PK_App_ApplyJob_Id == ApplyJobId).OrderByDescending(EF => EF.Step).ToList();
            DataCount = applyJobRecords.Count;
            var requirements = requirementRepository.FindList().ToList();
            foreach (var item in applyJobRecords)
            {
                ApplyJobRecordVm applyJobRecordVm = new ApplyJobRecordVm();
                LinqHelper.ModelTrans(item, applyJobRecordVm);
                applyJobRecordVms.Add(applyJobRecordVm);
            }
            ErrorMsg = "获取应聘记录列表成功";
            sysLog.WriteServiceLog(UserId, "ApplyJobId:" + ApplyJobId + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetApplyJobRecords", "App_ApplyJobBLL");
            return applyJobRecordVms;
        }
        #endregion

        #region 忽略职位
        /// <summary>
        /// 忽略职位
        /// </summary>
        /// <param name="applyJobPost"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public string IgnoreApplyJob(ApplyJobPost applyJobPost, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString(), "开始", "IgnoreApplyJob", "App_ApplyJobBLL");
            string ReqId = applyJobPost.RequirementId, customerId = applyJobPost.CustomerId;
            var now = ResultHelper.NowTime;
            var Req = requirementRepository.GetById(ReqId);
            if (null == Req)
            {
                ErrorMsg = "忽略的需求不存在";
                sysLog.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString() + ErrorMsg, "结束", "IgnoreApplyJob", "App_ApplyJobBLL");
                return null;
            }
            var sysMessage = sysMessageRepository.Find(EF => EF.PK_App_Customer_CustomerName == customerId && EF.BusinessID == ReqId);
            sysMessage.ModificationTime = now;
            sysMessage.ModificationUserName = applyJobPost.UserId;
            sysMessage.SwitchBtnButton = "0";
            sysMessage.ShowMessage = "已忽略";
            try
            {
                sysMessageRepository.Edit(sysMessage);
                ErrorMsg = "忽略职位成功";
                sysLog.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString() + ErrorMsg, "结束", "IgnoreApplyJob", "App_ApplyJobBLL");
                return "";
            }
            catch (Exception ex)
            {
                ErrorMsg = "忽略职位出现异常";
                sysLog.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString() + ErrorMsg + ex.Message, "结束", "IgnoreApplyJob", "App_ApplyJobBLL");
                return null;
            }
        }
        #endregion

        #region 个人中心--我的面试--应聘申请
        /// <summary>
        /// 个人中心--我的面试--应聘申请
        /// </summary>
        /// <param name="UserId">登录人主键</param>
        /// <param name="EmployerId">雇主主键</param>
        /// <param name="Flag">0:应聘申请, 1:面试中的</param>
        /// <param name="PageNum"></param>
        /// <param name="RecordNum"></param>
        /// <param name="DataCount"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public List<ApplyJobUserVm> GetApplyMyJobs(string UserId, string EmployerId, string Flag, int PageNum, int RecordNum, ref int DataCount, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(UserId, "EmployerId:" + EmployerId + ",Flag:" + Flag + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetApplyMyJobs", "App_ApplyJobBLL");
            List<ApplyJobUserVm> applyJobUserVms = new List<ApplyJobUserVm>();
            //先获取当前雇主的所有的发布职位
            var app_Requirements = requirementBLL.m_Rep.FindList(EF => EF.PK_App_Customer_CustomerName == EmployerId && EF.SwitchBtnOpen == "1").ToList();
            var ReqIds = app_Requirements.Select(EF => EF.Id).ToArray();
            var applyJobs = m_Rep.FindList(EF => ReqIds.Contains(EF.PK_App_Requirement_Title) && EF.EnumApplyStatus == "0").OrderByDescending(EF => EF.ModificationTime).ToList();
            if ("0".Equals(Flag))
            {
                applyJobs = applyJobs.Where(EF => EF.CurrentStep == "2").ToList();
            }
            if ("1".Equals(Flag))
            {
                applyJobs = applyJobs.Where(EF => Utils.ObjToInt(EF.CurrentStep, 0) >= 3 && Utils.ObjToInt(EF.CurrentStep, 0) < 9).ToList();
            }
            DataCount = applyJobs.Count;
            if (PageNum > 0)
            {
                applyJobs = applyJobs.Skip((PageNum - 1) * RecordNum).Take(RecordNum).ToList();
            }
            foreach (var item in applyJobs)
            {
                ApplyJobUserVm applyJobUserVm = new ApplyJobUserVm();
                applyJobUserVm.ApplyJobId = item.Id;
                applyJobUserVm.CustomerId = item.PK_App_Customer_CustomerName;
                var customer = customerRepository.GetById(item.PK_App_Customer_CustomerName);
                if (null != customer)
                {
                    applyJobUserVm.CustomerName = customer.CustomerName;
                    applyJobUserVm.Photo = customer.CustomerPhoto;
                    applyJobUserVm.Age = customer.Age;
                    applyJobUserVm.BirthPlace = customer.BirthPlace;
                    applyJobUserVm.Sex = customer.Sex;
                    applyJobUserVm.AbroadExp = enumDictionary.GetDicName("App_CustomerJobIntension.AbroadExp", customer.AbroadExp);
                }
                applyJobUserVms.Add(applyJobUserVm);
            }
            ErrorMsg = "获取申请人列表成功";
            sysLog.WriteServiceLog(UserId, "EmployerId:" + EmployerId + ",Flag:" + Flag + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",DataCount:" + DataCount + ",ErrorMsg:" + ErrorMsg, "结束", "GetApplyMyJobs", "App_ApplyJobBLL");
            return applyJobUserVms;
        }
        #endregion

        #region 【后台】获取当前选中用户的申请列表
        /// <summary>
        /// 【后台】获取当前选中用户的申请列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="queryStr"></param>
        /// <returns></returns>
        public List<App_ApplyJobModel> GetApplyJobList(ref GridPager pager, CustomerResumeQuery customerResumeQuery)
        {
            //先获取当前选中人所有的应聘记录
            IQueryable<App_ApplyJob> queryData = m_Rep.FindList(EF => EF.PK_App_Customer_CustomerName == customerResumeQuery.CustomerId);
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
        #endregion

        #region 提交应聘申请【后台批量】
        /// <summary>
        /// 提交应聘申请【后台批量】
        /// 目的是可以一次性给多人报名
        /// </summary>
        /// <param name="applyJobPost"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public int CreateApplyJobs(ApplyJobPost applyJobPost, string step, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString(), "开始", "CreateApplyJobs", "App_ApplyJobBLL");
            string ReqId = applyJobPost.RequirementId,
                //这里的顾客主键是逗号拼接的
                customerId = applyJobPost.CustomerId;
            int iCount = 0;
            var Req = requirementRepository.GetById(ReqId);
            if (null == Req)
            {
                ErrorMsg = "申请的需求不存在";
                sysLog.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString() + ErrorMsg, "结束", "CreateApplyJobs", "App_ApplyJobBLL");
                return 0;
            }
            var arrCustomerId = customerId.Split(',');
            foreach (var item in arrCustomerId)
            {
                if ("1".Equals(step))
                {
                    CrtApplyJob(applyJobPost, out ErrorMsg, item, Req);
                }
                if ("2".Equals(step))
                {
                    CrtStep2ApplyJob(applyJobPost, out ErrorMsg, item, Req);
                }
                if ("申请成功" == ErrorMsg)
                {
                    iCount++;
                }
            }
            ErrorMsg = "申请的需求成功";
            sysLog.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString() + ErrorMsg, "结束", "CreateApplyJobs", "App_ApplyJobBLL");
            return iCount;
        }

        private string CrtApplyJob(ApplyJobPost applyJobPost, out string ErrorMsg, string customerId, App_Requirement Req)
        {
            //先判断是否有未完成的应聘
            App_ApplyJob applyJob = m_Rep.Find(EF => EF.PK_App_Customer_CustomerName == customerId && EF.PK_App_Requirement_Title == Req.Id && EF.EnumApplyStatus == "0");
            if (null != applyJob)
            {
                ErrorMsg = "用户已经有未完成应聘,不可重复应聘";
                sysLog.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString() + ErrorMsg, "结束", "CreateApplyJob", "App_ApplyJobBLL");
                return null;
            }

            var customer = customerRepository.GetById(customerId);
            var now = ResultHelper.NowTime;
            applyJob = new App_ApplyJob();
            applyJob.Id = ResultHelper.NewId;
            applyJob.CreateTime = now;
            applyJob.CreateUserName = applyJobPost.UserId;
            applyJob.ModificationTime = now;
            applyJob.ModificationUserName = applyJobPost.UserId;
            applyJob.PK_App_Requirement_Title = Req.Id;
            applyJob.PK_App_Customer_CustomerName = customerId;
            applyJob.EnumApplyStatus = "0";
            //后台发起的应聘，直接步骤是1。
            applyJob.CurrentStep = "1";
            applyJob.PromiseMoney = Utils.ObjToDecimal(Req.PromiseMoney, 0);
            applyJob.ServiceMoney = Utils.ObjToDecimal(Req.ServiceMoney, 0);
            applyJob.TailMoney = Utils.ObjToDecimal(Req.ServiceTailMoney, 0);
            try
            {
                m_Rep.Create(applyJob);
                //添加应聘记录--发起应聘完成
                App_ApplyJobRecord applyJobRecord = new App_ApplyJobRecord();
                applyJobRecord.Id = ResultHelper.NewId;
                applyJobRecord.CreateTime = now;
                applyJobRecord.CreateUserName = applyJobPost.UserId;
                applyJobRecord.ModificationTime = now;
                applyJobRecord.ModificationUserName = applyJobPost.UserId;
                applyJobRecord.PK_App_ApplyJob_Id = applyJob.Id;
                applyJobRecord.PK_App_Customer_CustomerName = customerId;
                applyJobRecord.Step = "1";
                applyJobRecord.EnumApplyStatus = "1";
                applyJobRecord.Result = "进行中";
                applyJobRecord.Content = "发起应聘";
                applyJobRecord.HappenDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                applyJobRecordRepository.Create(applyJobRecord);
                Req.ApplyCount++;
                requirementRepository.Edit(Req);
                //应聘申请成功后，进行消息推送
                //工人消息推送
                sysMessageRepository.CrtSysMessage(applyJobPost.UserId, customerId, applyJob.Id, "应聘申请提醒", "你的应聘申请已提交成功", "1", "0", "待审批");
                sysMessageRepository.CrtSysMessage(applyJobPost.UserId, Req.PK_App_Customer_CustomerName, applyJob.Id, "待审批提醒", customer.CustomerName + "应聘了您的职位，点击查看详情", "1", "1", "待审批");
                ErrorMsg = "申请成功";
                sysLog.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString() + ErrorMsg, "结束", "CreateApplyJob", "App_ApplyJobBLL");
                return applyJob.Id;
            }
            catch (Exception ex)
            {
                ErrorMsg = "申请出现异常";
                sysLog.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString() + ErrorMsg + ex.Message, "结束", "CreateApplyJob", "App_ApplyJobBLL");
                return null;
            }
        }

        private string CrtStep2ApplyJob(ApplyJobPost applyJobPost, out string ErrorMsg, string customerId, App_Requirement Req)
        {
            //先判断是否有未完成的应聘
            App_ApplyJob applyJob = m_Rep.Find(EF => EF.PK_App_Customer_CustomerName == customerId && EF.PK_App_Requirement_Title == Req.Id && EF.EnumApplyStatus == "0");
            if (null != applyJob)
            {
                ErrorMsg = "用户已经有未完成应聘,不可重复应聘";
                sysLog.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString() + ErrorMsg, "结束", "CreateApplyJob", "App_ApplyJobBLL");
                return null;
            }

            var customer = customerRepository.GetById(customerId);
            var now = ResultHelper.NowTime;
            applyJob = new App_ApplyJob();
            applyJob.Id = ResultHelper.NewId;
            applyJob.CreateTime = now;
            applyJob.CreateUserName = applyJobPost.UserId;
            applyJob.ModificationTime = now;
            applyJob.ModificationUserName = applyJobPost.UserId;
            applyJob.PK_App_Requirement_Title = Req.Id;
            applyJob.PK_App_Customer_CustomerName = customerId;
            applyJob.EnumApplyStatus = "0";
            //后台发起的应聘，直接步骤是2。
            applyJob.CurrentStep = "2";
            applyJob.PromiseMoney = Utils.ObjToDecimal(Req.PromiseMoney, 0);
            applyJob.ServiceMoney = Utils.ObjToDecimal(Req.ServiceMoney, 0);
            applyJob.TailMoney = Utils.ObjToDecimal(Req.ServiceTailMoney, 0);
            applyJob.EnumApplyJobSource = applyJobPost.EnumApplyJobSource;
            try
            {
                m_Rep.Create(applyJob);
                //添加应聘记录--发起应聘完成
                App_ApplyJobRecord applyJobRecord = new App_ApplyJobRecord();
                applyJobRecord.Id = ResultHelper.NewId;
                applyJobRecord.CreateTime = now;
                applyJobRecord.CreateUserName = applyJobPost.UserId;
                applyJobRecord.ModificationTime = now;
                applyJobRecord.ModificationUserName = applyJobPost.UserId;
                applyJobRecord.PK_App_ApplyJob_Id = applyJob.Id;
                applyJobRecord.PK_App_Customer_CustomerName = customerId;
                applyJobRecord.Step = "1";
                applyJobRecord.EnumApplyStatus = "1";
                applyJobRecord.Result = "已完成";
                applyJobRecord.Content = "发起应聘";
                applyJobRecord.HappenDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                applyJobRecordRepository.Create(applyJobRecord);
                //添加应聘记录--待支付保证金进行中
                applyJobRecord = new App_ApplyJobRecord();
                applyJobRecord.Id = ResultHelper.NewId;
                applyJobRecord.CreateTime = now;
                applyJobRecord.CreateUserName = applyJobPost.UserId;
                applyJobRecord.ModificationTime = now;
                applyJobRecord.ModificationUserName = applyJobPost.UserId;
                applyJobRecord.PK_App_ApplyJob_Id = applyJob.Id;
                applyJobRecord.PK_App_Customer_CustomerName = customerId;
                applyJobRecord.Step = "2";
                applyJobRecord.EnumApplyStatus = "1";
                applyJobRecord.Result = "进行中";
                applyJobRecord.Content = applyJobStepRepository.GetStepName("2");
                applyJobRecord.HappenDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                applyJobRecordRepository.Create(applyJobRecord);
                Req.ApplyCount++;
                requirementRepository.Edit(Req);
                //应聘申请成功后，进行消息推送
                //工人消息推送
                sysMessageRepository.CrtSysMessage(applyJobPost.UserId, customerId, applyJob.Id, "应聘申请提醒", "你的应聘申请已提交成功", "1", "0", "待审批");
                sysMessageRepository.CrtSysMessage(applyJobPost.UserId, Req.PK_App_Customer_CustomerName, applyJob.Id, "待审批提醒", customer.CustomerName + "应聘了您的职位，点击查看详情", "1", "1", "待审批");
                ErrorMsg = "申请成功";
                sysLog.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString() + ErrorMsg, "结束", "CreateApplyJob", "App_ApplyJobBLL");
                return applyJob.Id;
            }
            catch (Exception ex)
            {
                ErrorMsg = "申请出现异常";
                sysLog.WriteServiceLog(applyJobPost.UserId, applyJobPost.ToString() + ErrorMsg + ex.Message, "结束", "CreateApplyJob", "App_ApplyJobBLL");
                return null;
            }
        }
        #endregion

        #region 下一步流程
        /// <summary>
        /// 下一步流程
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool NextStep(string strUserId, App_ApplyJobRecordModel model)
        {
            var now = ResultHelper.NowTime;
            var iPreStep = Utils.ObjToInt(model.Step, 0) - 1;
            bool createRecordFlag = true;
            var customer = customerRepository.GetById(model.PK_App_Customer_CustomerName);
            //首先更新申请主信息
            App_ApplyJob entity = m_Rep.GetById(model.PK_App_ApplyJob_Id);
            entity.ModificationTime = now;
            entity.ModificationUserName = strUserId;
            entity.CurrentStep = model.Step;
            entity.EnumApplyStatus = "0";
            if (model.Step == "3")
            {
                if (entity.EnumApplyJobSource == "0")
                {
                    if (entity.EnumApplyStatus == "6")
                    {
                        //此时说明雇主可能不方便点同意，那么管理员也可以下一步，然后状态正常走到进行中的0
                        //如果步骤到3了，就锁定成面试中，不可面试其他职位
                        customer.SwitchBtnInterview = "1";
                        entity.EnumApplyStatus = "0";
                    }
                    else
                    {
                        //如果是app端发起的申请，则此时不更新步骤数，也不添加记录数，只是改成已支付即可，然后把状态改成6（待雇主同意）
                        entity.CurrentStep = "2";
                        entity.EnumApplyStatus = "6";
                        createRecordFlag = false;
                    }
                }
                else
                {
                    //如果步骤到3了，就锁定成面试中，不可面试其他职位
                    customer.SwitchBtnInterview = "1";
                }
            }
            if (model.Step == "9")
            {
                //如果步骤到9了，就改成已完成
                entity.EnumApplyStatus = "1";
                //如果步骤到9了，就把用户面试锁定解除
                customer.SwitchBtnInterview = "0";
            }
            m_Rep.Edit(entity);
            customer.ModificationTime = now;
            customer.ModificationUserName = strUserId;
            customerRepository.Edit(customer);
            SysMessage sysMessage = new SysMessage()
            {
                Id = ResultHelper.NewId,
                CreateTime = now,
                CreateUserName = strUserId,
                ModificationTime = now,
                ModificationUserName = strUserId,
                BusinessTable = "ApplyJob",
                BusinessID = model.PK_App_ApplyJob_Id,
                PK_App_Customer_CustomerName = model.PK_App_Customer_CustomerName,
                SwitchBtnRead = "0",
                EnumMessageType = "1",
                Title = "应聘进度更新",
            };
            sysMessage.WorkerId = model.PK_App_Customer_CustomerName;
            //增加判断，如果这个邀请的是个工友，则需要推送消息给工人
            if (!string.IsNullOrEmpty(customer.ParentId))
            {
                sysMessage.PK_App_Customer_CustomerName = customer.ParentId;
            }
            if ("3".Equals(model.Step))
            {
                sysMessage.Content = "您的应聘进度有更新——待面试";
            }
            if ("4".Equals(model.Step))
            {
                sysMessage.Content = "您的应聘进度有更新——待考试";
            }
            if ("5".Equals(model.Step))
            {
                sysMessage.Content = "您的应聘进度有更新——待支付服务费";
            }
            if ("6".Equals(model.Step))
            {
                sysMessage.Content = "您的应聘进度有更新——待审核材料";
            }
            if ("7".Equals(model.Step))
            {
                sysMessage.Content = "您的应聘进度有更新——待签约";
            }
            if ("8".Equals(model.Step))
            {
                sysMessage.Content = "您的应聘进度有更新——待支付尾款";
            }
            if ("9".Equals(model.Step))
            {
                sysMessage.Content = "您的应聘完成啦~~祝一切顺利";
            }
            sysMessageRepository.Create(sysMessage);
            model.ModificationUserName = strUserId;
            model.ModificationTime = ResultHelper.NowTime;
            App_ApplyJobRecord applyJobRecord = new App_ApplyJobRecord();
            if (createRecordFlag)
            {
                LinqHelper.ModelTrans(model, applyJobRecord);
                applyJobRecordRepository.Create(applyJobRecord);
            }
            //把上一个步骤的状态改成已完成
            var PreStep = iPreStep.ToString();
            applyJobRecord = applyJobRecordRepository.Find(EF => EF.PK_App_ApplyJob_Id == model.PK_App_ApplyJob_Id && EF.Step == PreStep);
            applyJobRecord.Result = "已完成";
            applyJobRecord.ModificationTime = now;
            applyJobRecord.ModificationUserName = strUserId;
            if (string.IsNullOrEmpty(applyJobRecord.HappenDate))
            {
                applyJobRecord.HappenDate = now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            applyJobRecordRepository.Edit(applyJobRecord);
            return true;
        }
        #endregion

        #region 雇主同意面试
        /// <summary>
        /// 雇主同意面试
        /// </summary>
        /// <param name="applyJobPost"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public bool EmployerAgree(EmployerAgreePost employerAgreePost, ref string ErrorMsg)
        {
            sysLog.WriteServiceLog(employerAgreePost.UserId, employerAgreePost.ToString(), "开始", "EmployerAgree", "App_ApplyJobBLL");
            string ApplyJobId = employerAgreePost.ApplyJobId;
            var now = ResultHelper.NowTime;
            var applyJob = m_Rep.GetById(ApplyJobId);
            if (null == applyJob)
            {
                ErrorMsg = "修改的申请不存在";
                sysLog.WriteServiceLog(employerAgreePost.UserId, employerAgreePost.ToString() + ErrorMsg, "结束", "EmployerAgree", "App_ApplyJobBLL");
                return false;
            }
            applyJob.ModificationTime = now;
            applyJob.ModificationUserName = employerAgreePost.UserId;
            applyJob.EnumApplyStatus = "0";
            applyJob.CurrentStep = "3";
            try
            {
                m_Rep.Edit(applyJob);
                App_ApplyJobRecord applyJobRecord = new App_ApplyJobRecord();
                //添加应聘记录--增加面试进行中记录
                applyJobRecord.Id = ResultHelper.NewId;
                applyJobRecord.CreateTime = now;
                applyJobRecord.CreateUserName = employerAgreePost.UserId;
                applyJobRecord.ModificationTime = now;
                applyJobRecord.ModificationUserName = employerAgreePost.UserId;
                applyJobRecord.PK_App_ApplyJob_Id = applyJob.Id;
                applyJobRecord.PK_App_Customer_CustomerName = applyJob.PK_App_Customer_CustomerName;
                applyJobRecord.Step = "3";
                applyJobRecord.EnumApplyStatus = "3";
                applyJobRecord.Result = "进行中";
                applyJobRecord.Content = "面试";
                applyJobRecord.HappenDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                applyJobRecordRepository.Create(applyJobRecord);
                ErrorMsg = "申请更新成功";
                sysLog.WriteServiceLog(employerAgreePost.UserId, employerAgreePost.ToString() + ErrorMsg, "结束", "EmployerAgree", "App_ApplyJobBLL");
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = "申请更新出现异常";
                sysLog.WriteServiceLog(employerAgreePost.UserId, employerAgreePost.ToString() + ErrorMsg + ex.Message, "结束", "EmployerAgree", "App_ApplyJobBLL");
                return false;
            }
        }
        #endregion
    }
}

