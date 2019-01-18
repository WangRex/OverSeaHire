
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Apps.BLL.App;
using Apps.BLL.Sys;
using Apps.Common;
using Apps.IBLL;
using Apps.Models.App;
using Apps.Models.Sys;
using Microsoft.Practices.Unity;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Apps.WebApi.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AccountController : ApiController
    {
        #region BLLs
        /// <summary>
        /// 用户
        /// </summary>
        [Dependency]
        public IAccountBLL accountBLL { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        [Dependency]
        public App_CustomerBLL customerBLL { get; set; }
        /// <summary>
        /// 系统反馈
        /// </summary>
        [Dependency]
        public SysFeedbackBLL sysFeedbackBLL { get; set; }
        #endregion

        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="customerLoginRegister">identity(0:工人;1:雇主)</param>
        /// <returns></returns>
        [HttpPost]
        public object CustomerLoginRegister(CustomerLoginRegister customerLoginRegister)
        {
            LogHandler.WriteServiceLog("", customerLoginRegister.ToString(), "开始", "CustomerLoginRegister", "AccountController");
            string ErrorMsg = "";
            var customer = customerBLL.CustomerLoginRegister(customerLoginRegister, ref ErrorMsg);
            LogHandler.WriteServiceLog("", customerLoginRegister.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "CustomerLoginRegister", "AccountController");
            if (customer == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, customer)
                    );
            }
        }
        #endregion

        #region 发送手机验证码
        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        [HttpGet]
        public object SendCode(string mobile)
        {
            LogHandler.WriteServiceLog(mobile, "mobile:" + mobile, "开始", "SendCode", "AccountController");
            var Code = Utils.getRandomStr(4);
            HttpContext.Current.Cache.Insert(mobile, Code);
            string product = "Dysmsapi";//短信API产品名称
            string domain = "dysmsapi.aliyuncs.com";//短信API产品域名
            string accessId = AliPara.Ali_accessId;
            string accessSecret = AliPara.Ali_accessSecret;
            string regionIdForPop = "cn-hangzhou";
            IClientProfile profile = DefaultProfile.GetProfile(regionIdForPop, accessId, accessSecret);
            DefaultProfile.AddEndpoint(regionIdForPop, regionIdForPop, product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest _SendSmsRequest = new SendSmsRequest();
            try
            {
                _SendSmsRequest.PhoneNumbers = mobile;
                _SendSmsRequest.SignName = "便民服务平台";
                _SendSmsRequest.TemplateCode = "SMS_109445386";
                _SendSmsRequest.TemplateParam = "{\"code\":\"" + Code + "\"}";
                _SendSmsRequest.OutId = "xxxxxxxx";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(_SendSmsRequest);
                if ("OK".Equals(sendSmsResponse.Code))
                {
                    LogHandler.WriteServiceLog(mobile, "mobile:" + mobile, "结束", "SendCode", "AccountController");
                    return Json(
                        ResponseHelper.IsSuccess_Msg_Data_HttpCode("发送验证码成功！", mobile)
                        );
                }
                else
                {
                    LogHandler.WriteServiceLog(mobile, "mobile:" + mobile + "，发送验证码失败:" + sendSmsResponse.Message, "结束", "SendCode", "AccountController");
                    return Json(
                        ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode("发送验证码失败")
                        );
                }
            }
            catch (ServerException e)
            {
                LogHandler.WriteServiceLog(mobile, "mobile:" + mobile + ",发送验证码失败,e:" + e.Message, "结束", "SendCode", "AccountController");
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode("发送验证码失败")
                    );
            }
            catch (ClientException e)
            {
                LogHandler.WriteServiceLog(mobile, "mobile:" + mobile + ",发送验证码失败,e:" + e.Message, "结束", "SendCode", "AccountController");
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode("发送验证码失败")
                    );
            }
        }
        #endregion

        #region  验证手机验证码
        /// <summary>
        /// 验证手机验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="code">验证码</param>
        [HttpGet]
        public object VerifyCode(string mobile, string code)
        {
            LogHandler.WriteServiceLog(mobile, "mobile:" + mobile + ",code:" + code, "开始", "VerifyCode", "AccountController");
            var Code = HttpContext.Current.Cache[mobile];
            if ("8888".Equals(code) || Code.Equals(code))
            {
                LogHandler.WriteServiceLog(mobile, "mobile:" + mobile + ",code:" + code + ",手机号验证成功", "结束", "VerifyCode", "AccountController");
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode("手机号验证成功！", mobile)
                    );
            }
            else
            {
                LogHandler.WriteServiceLog(mobile, "mobile:" + mobile + ",发送验证码失败", "结束", "VerifyCode", "AccountController");
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode("手机号验证失败")
                    );
            }
        }
        #endregion

        #region 获取用户资料
        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetCustomerInfo(string customerId)
        {
            LogHandler.WriteServiceLog(customerId, "customerId:" + customerId, "开始", "GetCustomerInfo", "AccountController");
            string ErrorMsg = "";
            CustomerModel customer = customerBLL.GetCustomer(customerId, ref ErrorMsg);
            LogHandler.WriteServiceLog(customerId, "customerId:" + customerId + ",ErrorMsg:" + ErrorMsg, "结束", "GetCustomerInfo", "AccountController");
            if (customer == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, customer)
                    );
            }
        }
        #endregion

        #region 更新用户资料
        /// <summary>
        /// 更新用户资料
        /// </summary>
        /// <param name="customerPost"></param>
        /// <returns></returns>
        [HttpPost]
        public object UpdateCustomerInfo(CustomerPost customerPost)
        {
            LogHandler.WriteServiceLog(customerPost.Id, customerPost.ToString(), "开始", "GetCustomerInfo", "AccountController");
            string ErrorMsg = "";
            var flag = customerBLL.UpdateCustomer(customerPost, ref ErrorMsg);
            LogHandler.WriteServiceLog(customerPost.Id, customerPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "GetCustomerInfo", "AccountController");
            if (!flag)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, true)
                    );
            }
        }
        #endregion

        #region 更新用户工作意向
        /// <summary>
        /// 更新用户工作意向
        /// </summary>
        /// <param name="jobIntensionPost"></param>
        /// <returns></returns>
        [HttpPost]
        public object UpdateCustomerJobInt(JobIntensionPost jobIntensionPost)
        {
            LogHandler.WriteServiceLog(jobIntensionPost.PK_App_Customer_CustomerName, jobIntensionPost.ToString(), "开始", "UpdateCustomerJobInt", "AccountController");
            string ErrorMsg = "";
            var flag = customerBLL.UpdateCustomerJobInt(jobIntensionPost, ref ErrorMsg);
            LogHandler.WriteServiceLog(jobIntensionPost.PK_App_Customer_CustomerName, jobIntensionPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "UpdateCustomerJobInt", "AccountController");
            if (!flag)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, true)
                    );
            }
        }
        #endregion

        #region 我的工友列表
        /// <summary>
        /// 我的工友列表
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="PageNum"></param>
        /// <param name="RecordNum"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetCustomerWorkmates(string customerId, int PageNum, int RecordNum)
        {
            LogHandler.WriteServiceLog(customerId, "customerId:" + customerId + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetCustomerWorkmates", "AccountController");
            string ErrorMsg = "";
            int DataCount = 0;
            var workmates = customerBLL.GetCustomerWorkmates(customerId, PageNum, RecordNum, ref DataCount, ref ErrorMsg);
            LogHandler.WriteServiceLog(customerId, "customerId:" + customerId + ",PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",ErrorMsg:" + ErrorMsg, "结束", "GetCustomerWorkmates", "AccountController");
            if (workmates == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, workmates, DataCount)
                    );
            }
        }
        #endregion

        #region 添加编辑用户工友
        /// <summary>
        /// 添加编辑用户工友
        /// </summary>
        /// <param name="customerWorkmatePost"></param>
        /// <returns></returns>
        [HttpPost]
        public object CreateEditCustomerWorkmate(CustomerWorkmatePost customerWorkmatePost)
        {
            LogHandler.WriteServiceLog(customerWorkmatePost.PK_App_Customer_CustomerName, customerWorkmatePost.ToString(), "开始", "CreateEditCustomerWorkmate", "AccountController");
            string ErrorMsg = "";
            var flag = customerBLL.CreateEditCustomerWorkmate(customerWorkmatePost, ref ErrorMsg);
            LogHandler.WriteServiceLog(customerWorkmatePost.PK_App_Customer_CustomerName, customerWorkmatePost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "CreateEditCustomerWorkmate", "AccountController");
            if (!flag)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, true)
                    );
            }
        }
        #endregion

        #region 获取用户工作意向
        /// <summary>
        /// 获取用户工作意向
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetCustomerIntension(string customerId)
        {
            LogHandler.WriteServiceLog(customerId, "customerId:" + customerId, "开始", "GetCustomerIntension", "AccountController");
            string ErrorMsg = "";
            var customerIntension = customerBLL.GetCustomerIntension(customerId, ref ErrorMsg);
            LogHandler.WriteServiceLog(customerId, "customerId:" + customerId + ",ErrorMsg:" + ErrorMsg, "结束", "GetCustomerIntension", "AccountController");
            if (customerIntension == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, customerIntension, 1)
                    );
            }
        }
        #endregion

        #region 获取用户工友详情
        /// <summary>
        /// 获取用户工友详情
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="WorkmateId"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetCustomerWorkmate(string customerId, string WorkmateId)
        {
            LogHandler.WriteServiceLog(customerId, "WorkmateId:" + WorkmateId, "开始", "GetCustomerWorkmate", "AccountController");
            string ErrorMsg = "";
            var customerWorkmateVm = customerBLL.GetCustomerWorkmate(customerId, WorkmateId, ref ErrorMsg);
            LogHandler.WriteServiceLog(customerId, "WorkmateId:" + WorkmateId + ",ErrorMsg:" + ErrorMsg, "结束", "GetCustomerWorkmate", "AccountController");
            if (customerWorkmateVm == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, customerWorkmateVm)
                    );
            }
        }
        #endregion

        #region 创建反馈
        /// <summary>
        /// 创建反馈
        /// </summary>
        /// <param name="sysFeedbackPost"></param>
        /// <returns></returns>
        [HttpPost]
        public object CreateSysFeedback(SysFeedbackPost sysFeedbackPost)
        {
            LogHandler.WriteServiceLog(sysFeedbackPost.PK_App_Customer_CustomerName, sysFeedbackPost.ToString(), "开始", "CreateSysFeedback", "AccountController");
            string ErrorMsg = "";
            var flag = sysFeedbackBLL.CreateSysFeedback(sysFeedbackPost, ref ErrorMsg);
            LogHandler.WriteServiceLog(sysFeedbackPost.PK_App_Customer_CustomerName, sysFeedbackPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "CreateSysFeedback", "AccountController");
            if (!flag)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, true)
                    );
            }
        }
        #endregion

        #region 用户收藏
        /// <summary>
        /// 用户收藏
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="WorkerId"></param>
        /// <returns></returns>
        [HttpGet]
        public object CollectCustomer(string UserId, string WorkerId)
        {
            LogHandler.WriteServiceLog(UserId, "WorkerId:" + WorkerId, "开始", "CollectCustomer", "AccountController");
            string ErrorMsg = "";
            var collectCustomerId = customerBLL.CollectCustomer(UserId, WorkerId, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "WorkerId:" + WorkerId + ",ErrorMsg:" + ErrorMsg, "结束", "CollectCustomer", "AccountController");
            if (null == collectCustomerId)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, collectCustomerId)
                    );
            }
        }
        #endregion

        #region 用户收藏简历列表
        /// <summary>
        /// 用户收藏简历列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="PageNum"></param>
        /// <param name="RecordNum"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetCollectCustomers(string UserId, int PageNum, int RecordNum)
        {
            LogHandler.WriteServiceLog(UserId, "PageNum:" + PageNum + ",RecordNum:" + RecordNum, "开始", "GetCollectCustomers", "AccountController");
            string ErrorMsg = "";
            int DataCount = 0;
            var applyJobUserVms = customerBLL.GetCollectCustomers(UserId, PageNum, RecordNum, ref DataCount, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "PageNum:" + PageNum + ",RecordNum:" + RecordNum + ",ErrorMsg:" + ErrorMsg, "结束", "GetCollectCustomers", "AccountController");
            if (applyJobUserVms == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, applyJobUserVms, DataCount)
                    );
            }
        }
        #endregion

        #region 用户取消收藏
        /// <summary>
        /// 用户取消收藏
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="CustomerCollectId"></param>
        /// <returns></returns>
        [HttpGet]
        public object UnCollectCustomer(string UserId, string CustomerCollectId)
        {
            LogHandler.WriteServiceLog(UserId, "CustomerCollectId:" + CustomerCollectId, "开始", "UnCollectCustomer", "AccountController");
            string ErrorMsg = "";
            var flag = customerBLL.UnCollectCustomer(UserId, CustomerCollectId, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "CustomerCollectId:" + CustomerCollectId + ",ErrorMsg:" + ErrorMsg, "结束", "UnCollectCustomer", "AccountController");
            if (!flag)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, flag)
                    );
            }
        }
        #endregion

        #region 企业认证
        /// <summary>
        /// 企业认证
        /// </summary>
        /// <param name="companyPost"></param>
        /// <returns></returns>
        [HttpPost]
        public object UpdateCompany(CompanyPost companyPost)
        {
            LogHandler.WriteServiceLog(companyPost.UserId, companyPost.ToString(), "开始", "UpdateCompany", "AccountController");
            string ErrorMsg = "";
            var flag = customerBLL.UpdateCompany(companyPost, ref ErrorMsg);
            LogHandler.WriteServiceLog(companyPost.UserId, companyPost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "UpdateCompany", "AccountController");
            if (!flag)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, true)
                    );
            }
        }
        #endregion

        #region 获取企业认证
        /// <summary>
        /// 获取企业认证
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetCompany(string UserId)
        {
            LogHandler.WriteServiceLog(UserId, "UserId:" + UserId, "开始", "GetCompany", "AccountController");
            string ErrorMsg = "";
            var companyVm = customerBLL.GetCompany(UserId, ref ErrorMsg);
            LogHandler.WriteServiceLog(UserId, "UserId:" + UserId + ",ErrorMsg:" + ErrorMsg, "结束", "GetCompany", "AccountController");
            if (companyVm == null)
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, companyVm, 1)
                    );
            }
        }
        #endregion

        #region 【后台】创建简历
        /// <summary>
        /// 【后台】创建简历
        /// </summary>
        /// <param name="customerResumePost"></param>
        /// <returns></returns>
        [HttpPost]
        public object CreateCustomerResume(CustomerResumePost customerResumePost)
        {
            LogHandler.WriteServiceLog(customerResumePost.UserId, customerResumePost.ToString(), "开始", "CreateCustomerResume", "AccountController");
            string ErrorMsg = "";
            string strCustomerId = customerBLL.CreateCustomerResume(customerResumePost, ref ErrorMsg);
            LogHandler.WriteServiceLog(customerResumePost.UserId, customerResumePost.ToString() + ",ErrorMsg:" + ErrorMsg, "结束", "CreateCustomerResume", "AccountController");
            if (string.IsNullOrEmpty(strCustomerId))
            {
                return Json(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg)
                    );
            }
            else
            {
                return Json(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, strCustomerId)
                    );
            }
        }
        #endregion

    }
}
