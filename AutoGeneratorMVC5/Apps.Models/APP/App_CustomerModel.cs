using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
using Apps.Models.App;

namespace Apps.Models.App
{
    #region App_CustomerModel
    public partial class App_CustomerModel
    {
        [Display(Name = "主键")]
        public override string Id { get; set; }

        [Display(Name = "创建时间")]
        public override DateTime CreateTime { get; set; }

        [Display(Name = "更新时间")]
        public override DateTime ModificationTime { get; set; }

        [Display(Name = "创建人姓名")]
        public override string CreateUserName { get; set; }

        [Display(Name = "修改人姓名")]
        public override string ModificationUserName { get; set; }

        [Display(Name = "排序码")]
        public override int SortCode { get; set; }

        [Display(Name = "关联数据Id")]
        public override string ParentId { get; set; }

        [Display(Name = "拥有人姓名")]
        public string OwnerName { get; set; }

        [Display(Name = "积分")]
        public override int? Record { get; set; }

        [Display(Name = "个性签名")]
        public override string Info { get; set; }

        [Display(Name = "用户等级")]
        public override string EnumCustomerLevel { get; set; }

        [Display(Name = "真实姓名")]
        public override string CustomerName { get; set; }

        [Display(Name = "密码")]
        public override string Password { get; set; }

        [Display(Name = "性别")]
        public override string Sex { get; set; }

        [Display(Name = "电话")]
        public override string Phone { get; set; }

        [Display(Name = "昵称")]
        public override string NickName { get; set; }

        [Display(Name = "年龄")]
        public override int Age { get; set; }

        [Display(Name = "身高")]
        public override string Height { get; set; }

        [Display(Name = "体重")]
        public override string Weight { get; set; }

        [Display(Name = "民族")]
        public override string Nation { get; set; }

        [Display(Name = "个人简介")]
        public override string Introduction { get; set; }

        [Display(Name = "照片")]
        public override string CustomerPhoto { get; set; }

        [Display(Name = "OpenID")]
        public override string OpenID { get; set; }

        [Display(Name = "生日")]
        public override string BirthDay { get; set; }

        [Display(Name = "籍贯")]
        public override string BirthPlace { get; set; }

        [Display(Name = "现居地")]
        public override string CurrentPlace { get; set; }

        [Display(Name = "微信号")]
        public override string WeChatNumber { get; set; }

        [Display(Name = "文化水平")]
        public override string Cultural { get; set; }

        [Display(Name = "用户身份")]
        public override string EnumCustomerType { get; set; }

        [Display(Name = "外语水平")]
        public override string EnumForeignLangGrade { get; set; }

        [Display(Name = "护照")]
        public override string SwitchBtnPassport { get; set; }

        [Display(Name = "出国经历")]
        public override string AbroadExp { get; set; }

        [Display(Name = "车票")]
        public override string EnumDriverLicence { get; set; }

        [Display(Name = "是否推荐")]
        public override string SwitchBtnRecommend { get; set; }

        [Display(Name = "视频路径")]
        public override string VideoPath { get; set; }

        [Display(Name = "简历路径")]
        public override string WordPath { get; set; }

        [Display(Name = "简历名称")]
        public override string WordName { get; set; }

        [Display(Name = "后缀名")]
        public override string WordExt { get; set; }

        [Display(Name = "求职意向")]
        public override string JobIntension { get; set; }

        [Display(Name = "求职意向")]
        public string JobIntensionNames { get; set; }

        public override string ToString()
        {
            return "Record:" + Record + ",Info:" + Info + ",EnumCustomerLevel:" + EnumCustomerLevel
                 + ",CustomerName:" + CustomerName + ",Password:" + Password + ",Sex:" + Sex
                 + ",Phone:" + Phone + ",NickName:" + NickName + ",Age:" + Age
                 + ",Height:" + Height + ",Weight:" + Weight + ",Nation:" + Nation
                 + ",Introduction:" + Introduction + ",CustomerPhoto:" + CustomerPhoto + ",OpenID:" + OpenID
                 + ",BirthDay:" + BirthDay + ",BirthPlace:" + BirthPlace + ",CurrentPlace:" + CurrentPlace
                 + ",WeChatNumber:" + WeChatNumber + ",Cultural:" + Cultural + ",EnumCustomerType:" + EnumCustomerType
                 + ",EnumForeignLangGrade:" + EnumForeignLangGrade + ",SwitchBtnPassport:" + SwitchBtnPassport + ",AbroadExp:" + AbroadExp
                 + ",EnumDriverLicence:" + EnumDriverLicence + ",SwitchBtnRecommend:" + SwitchBtnRecommend + ",VideoPath:" + VideoPath
                 + ",WordPath:" + WordPath + ",WordName:" + WordName + ",WordExt:" + WordExt
                 + ",JobIntension:" + JobIntension;
        }
    }
    #endregion

    #region 用户登录注册
    public class CustomerLoginRegister
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 身份类型
        //0:工人;1:雇主
        /// </summary>
        public string identity { get; set; }

        public override string ToString()
        {
            return "mobile:" + mobile + ",code:" + code + ",identity:" + identity;
        }
    }
    #endregion

    #region 用户登录返回
    public class LoginVm
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string Photo { get; set; }
        public string Phone { get; set; }
        public bool IntroFlag { get; set; }
        public bool IntensionFlag { get; set; }
    }
    #endregion

    #region 用户资料
    public class CustomerModel : CustomerPost
    {
        public string SwitchBtnVip { get; set; }
        public string EnumCustomerType { get; set; }
        public string CustomerType { get; set; }
        public bool IntroFlag { get; set; }
        public bool IntensionFlag { get; set; }
    }
    #endregion

    #region 更新用户资料
    public class CustomerPost
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 身高
        /// </summary>
        public string Height { get; set; }
        /// <summary>
        /// 体重
        /// </summary>
        public string Weight { get; set; }
        /// <summary>
        /// 籍贯
        /// </summary>
        public string BirthPlace { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        public string Nation { get; set; }
        /// <summary>
        /// 个人简介
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// Word简历
        /// </summary>
        public string WordPath { get; set; }
        /// <summary>
        /// Word简历名称
        /// </summary>
        public string WordName { get; set; }
        /// <summary>
        /// Word简历后缀
        /// </summary>
        public string WordExt { get; set; }
        /// <summary>
        /// 视频
        /// </summary>
        public string VideoPath { get; set; }
        /// <summary>
        /// 工作经历
        /// </summary>
        public List<WorkExpPost> workExpPosts { get; set; }
        /// <summary>
        /// 教育经历
        /// </summary>
        public List<EduExpPost> eduExpPosts { get; set; }
        /// <summary>
        /// 家庭关系
        /// </summary>
        public List<FamilyPost> familyPosts { get; set; }
        public override string ToString()
        {
            string str = "Id:" + Id + ",Photo:" + Photo
                + ",CustomerName:" + CustomerName
                + ",Phone:" + Phone + ",Sex:" + Sex
                + ",Age:" + Age
                + ",Height:" + Height
                + ",Weight:" + Weight
                + ",Nation:" + Nation
                + ",Introduction:" + Introduction;
            if (null != workExpPosts)
            {
                foreach (var item in workExpPosts)
                {
                    str += ",workExpPosts:" + item.ToString();
                }
            }
            if (null != eduExpPosts)
            {
                foreach (var item in eduExpPosts)
                {
                    str += ",eduExpPosts:" + item.ToString();
                }
            }
            if (null != familyPosts)
            {
                foreach (var item in familyPosts)
                {
                    str += ",familyPosts:" + item.ToString();
                }
            }
            return str;
        }
    }
    #endregion

    #region 步骤用用户信息
    public class ApplyJobUserVm
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 籍贯
        /// </summary>
        public string BirthPlace { get; set; }
        /// <summary>
        /// 出国经历
        /// </summary>
        public string AbroadExp { get; set; }
        /// <summary>
        /// 驾驶证
        /// </summary>
        public string EnumDriverLicence { get; set; }
        /// <summary>
        /// 驾驶证
        /// </summary>
        public string DriverLicence { get; set; }
        /// <summary>
        /// 工种
        /// </summary>
        public string JobIntension { get; set; }
        /// <summary>
        /// 工种
        /// </summary>
        public string JobIntensionName { get; set; }
        /// <summary>
        /// 系统推荐
        /// </summary>
        public string SwitchBtnRecommend { get; set; }
        /// <summary>
        /// 视频路径
        /// </summary>
        public string VideoPath { get; set; }
        /// <summary>
        /// 应聘申请主键
        /// </summary>
        public string ApplyJobId { get; set; }
        /// <summary>
        /// 用户收藏
        /// </summary>
        public string CustomerCollectId { get; set; }
        /// <summary>
        /// 用户来源
        /// </summary>
        public string EnumCustomerLevel { get; set; }
    }
    #endregion

    #region 雇主端首页数据
    public class EmployerHomePage
    {
        public EmployerHomePage()
        {
            applyJobUserVms = new List<ApplyJobUserVm>();
            positionTreeVms = new List<PositionTreeVm>();
        }
        public List<ApplyJobUserVm> applyJobUserVms { get; set; }
        public List<PositionTreeVm> positionTreeVms { get; set; }
    }
    #endregion

    #region 工友表单
    public class CustomerWorkmatePost
    {
        public string Id { get; set; }
        /// <summary>
        /// 用户(关联创建这个工友的用户)
        /// </summary>
        public string PK_App_Customer_CustomerName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 籍贯
        /// </summary>
        public string BirthPlace { get; set; }
        /// <summary>
        /// 现居地
        /// </summary>
        public string CurrentPlace { get; set; }
        /// <summary>
        /// 文化水平
        /// </summary>
        public string Cultural { get; set; }
        /// <summary>
        /// 外语水平
        /// </summary>
        public string EnumForeignLangGrade { get; set; }
        /// <summary>
        /// 护照
        /// </summary>
        public string SwitchBtnPassport { get; set; }
        /// <summary>
        /// 出国经历
        /// </summary>
        public string AbroadExp { get; set; }
        /// <summary>
        /// 个人简介
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 视频路径
        /// </summary>
        public string VideoPath { get; set; }
        /// <summary>
        /// Word路径
        /// </summary>
        public string WordPath { get; set; }
        /// <summary>
        /// Word名称
        /// </summary>
        public string WordName { get; set; }
        /// <summary>
        /// Word后缀
        /// </summary>
        public string WordExt { get; set; }
        /// <summary>
        /// 求职意向
        /// </summary>
        public string JobIntension { get; set; }
        /// <summary>
        /// 驾照
        /// </summary>
        public string EnumDriverLicence { get; set; }

        public override string ToString()
        {
            return "PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName + ",Photo:" + Photo + ",Name:" + Name
                + ",Phone:" + Phone + ",Sex:" + Sex + ",Age:" + Age
                + ",BirthPlace:" + BirthPlace + ",CurrentPlace:" + CurrentPlace + ",Cultural:" + Cultural
                + ",EnumForeignLangGrade:" + EnumForeignLangGrade + ",SwitchBtnPassport:" + SwitchBtnPassport + ",AbroadExp:" + AbroadExp
                + ",Introduction:" + Introduction + ",VideoPath:" + VideoPath + ",WordPath:" + WordPath
                + ",WordName:" + WordName + ",WordExt:" + WordExt + ",JobIntension:" + JobIntension
                + ",EnumDriverLicence:" + EnumDriverLicence;
        }
    }
    #endregion

    #region 工友返回
    public class CustomerWorkmateVm : CustomerWorkmatePost
    {
        public CustomerWorkmateVm()
        {
            customerJobIntension = new CustomerJobIntensionVm();
        }
        public string ForeignLangGrade { get; set; }
        public string JobIntensionNames { get; set; }
        public string DriverLicenceName { get; set; }
        public string AbroadExpName { get; set; }
        /// <summary>
        /// 用户收藏
        /// </summary>
        public string CustomerCollectId { get; set; }
        public CustomerJobIntensionVm customerJobIntension { get; set; }
    }
    #endregion

    #region 获取简历列表查询条件
    public class CustomerResumeQuery
    {
        public string CustomerName { get; set; }
        public string Sex { get; set; }
        public string CustomerId { get; set; }
        public string CustomerPhone { get; set; }
        public int WorkLimitAgeHigh { get; set; }
        public int WorkLimitAgeLow { get; set; }
        public bool AdminFlag { get; set; }
        /// <summary>
        /// 对应的需求的主键
        /// </summary>
        public string RequirementId { get; set; }
        public string JobIntension { get; set; }
        /// <summary>
        /// Applyed:已经应聘的,InterView:面试中的,Recommend:推荐的,Invite:雇主邀请的
        /// </summary>
        public string QueryFlag { get; set; }
        public override string ToString()
        {
            return "CustomerId:" + CustomerId + ",CustomerName:" + CustomerName + ",Sex:" + Sex
                 + ",CustomerPhone:" + CustomerPhone + ",WorkLimitAgeHigh:" + WorkLimitAgeHigh + ",WorkLimitAgeLow:" + WorkLimitAgeLow
                   + ",AdminFlag:" + AdminFlag + ",RequirementId:" + RequirementId + ",JobIntension:" + JobIntension
                    + ",QueryFlag:" + QueryFlag;
        }
    }
    #endregion

    #region ApplyJobResumeModel
    public partial class ApplyJobResumeModel : App_CustomerModel
    {
        public string ApplyJobId { get; set; }
    }
    #endregion

}

