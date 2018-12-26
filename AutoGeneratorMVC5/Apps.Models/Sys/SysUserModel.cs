using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.Web.Mvc;

namespace Apps.Models.Sys
{
    #region 用户登录
    /// <summary>
    /// 用户登录
    /// </summary>
    public class SysUserLogingData
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Ticket { get; set; }

    }
    #endregion

    #region 用户模型
    /// <summary>
    /// 用户模型
    /// </summary>
    public partial class SysUserModel
    {
        [Display(Name = "ID")]

        public override string Id { get; set; }
        [Display(Name = "用户名")]
        public override string UserName { get; set; }
        [StringLength(50, MinimumLength = 5)]
        //[System.Web.Mvc.Compare("ComparePassword", ErrorMessage = "两次密码不一致")]
        [Display(Name = "密码")]
        public override string Password { get; set; }
        //[System.Web.Mvc.Compare("Password", ErrorMessage = "两次密码不一致")]
        [Display(Name = "确认密码")]
        public string ComparePassword { get; set; }
        [NotNullExpression]
        [Display(Name = "姓名")]

        public override string TrueName { get; set; }
        [Display(Name = "身份证")]
        public override string Card { get; set; }
        [Display(Name = "手机号码")]
        [NotNullExpression]
        [RegularExpression(@"^1[0-9]{10}$", ErrorMessage = "手机号格式不正确")]
        public override string MobileNumber { get; set; }
        [Display(Name = "座机号")]
        public override string PhoneNumber { get; set; }
        [Display(Name = "QQ")]
        public override string QQ { get; set; }
        [Display(Name = "Email")]
        public override string EmailAddress { get; set; }
        [Display(Name = "其他联系方式")]
        public override string OtherContact { get; set; }
        [Display(Name = "省份")]
        public override string Province { get; set; }
        [Display(Name = "城市")]
        public override string City { get; set; }
        [Display(Name = "地区")]
        public override string Village { get; set; }
        [Display(Name = "详细地址")]
        public override string Address { get; set; }
        [Display(Name = "状态")]
        public override bool State { get; set; }
        [DateExpression]//如果填写判断是否是日期
        [Display(Name = "创建时间")]
        public override DateTime? CreateTime { get; set; }
        [Display(Name = "创建人")]
        public override string CreatePerson { get; set; }
        [Display(Name = "性别")]
        [Required]
        public override string Sex { get; set; }
        [DateExpression]//如果填写判断是否是日期
        [Display(Name = "生日")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public override DateTime? Birthday { get; set; }
        [DateExpression]//如果填写判断是否是日期
        [Display(Name = "加入时间")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public override DateTime? JoinDate { get; set; }
        [Display(Name = "婚姻状况")]
        public override string Marital { get; set; }
        [Display(Name = "党派")]
        public override string Political { get; set; }
        [Display(Name = "籍贯")]
        public override string Nationality { get; set; }
        [Display(Name = "所在地")]
        public override string Native { get; set; }
        [Display(Name = "学校")]
        public override string School { get; set; }
        [Display(Name = "专业")]
        public override string Professional { get; set; }
        [Display(Name = "学历")]
        public override string Degree { get; set; }
        [NotNullExpression]
        [Display(Name = "部门")]
        public override string DepId { get; set; }
        public string DepName { get; set; }
        [Display(Name = "职位")]
        public override string PosId { get; set; }
        [Display(Name = "备注")]
        public override string Expertise { get; set; }
        [Display(Name = "是否在职")]
        public override bool JobState { get; set; }
        [Display(Name = "照片")]
        public override string Photo { get; set; }
        [Display(Name = "附件")]
        public override string Attach { get; set; }
        [Display(Name = "角色")]
        public string RoleName { get; set; }//拥有的用户

        public string Flag { get; set; }//用户分配角色
        public string PosName { get; set; }


        [MaxWordsExpression(50)]
        [Display(Name = "上级领导")]
        public override string Lead { get; set; }
        public override string LeadName { get; set; }
        [Display(Name = "是否可以自选领导")]
        public override bool IsSelLead { get; set; }

        [Display(Name = "日否启动日程汇报是否启用  启用后 他的上司领导将可以看到他的 工作日程汇报.")]
        public override bool IsReportCalendar { get; set; }

        [Display(Name = "开启 小秘书")]
        public override bool IsSecretary { get; set; }

        [Display(Name = "OpenID")]
        public override string OpenID { get; set; }

        [Display(Name = "修改时间")]
        public override DateTime ModificationTime { get; set; }

        [Display(Name = "修改人")]
        public override string ModificationUser { get; set; }

        [Display(Name = "是否领导")]
        public override string SwitchBtnLead { get; set; }

        [Display(Name = "关联用户信息")]
        public override string PK_App_Customer_CustomerName { get; set; }

        [Display(Name = "用户类型")]
        public override string EnumUserType { get; set; }

        public override string ToString()
        {
            return "UserName:" + UserName + ",Password:" + Password + ",TrueName:" + TrueName
                 + ",Card:" + Card + ",MobileNumber:" + MobileNumber + ",PhoneNumber:" + PhoneNumber
                 + ",QQ:" + QQ + ",EmailAddress:" + EmailAddress + ",OtherContact:" + OtherContact
                 + ",Province:" + Province + ",City:" + City + ",Village:" + Village
                 + ",Address:" + Address + ",State:" + State + ",CreatePerson:" + CreatePerson
                 + ",Sex:" + Sex + ",Birthday:" + Birthday + ",JoinDate:" + JoinDate
                 + ",Marital:" + Marital + ",Political:" + Political + ",Nationality:" + Nationality
                 + ",Native:" + Native + ",School:" + School + ",Professional:" + Professional
                 + ",Degree:" + Degree + ",DepId:" + DepId + ",PosId:" + PosId
                 + ",Expertise:" + Expertise + ",JobState:" + JobState + ",Photo:" + Photo
                 + ",Attach:" + Attach + ",Lead:" + Lead + ",LeadName:" + LeadName
                 + ",IsSelLead:" + IsSelLead + ",IsReportCalendar:" + IsReportCalendar + ",IsSecretary:" + IsSecretary
                 + ",OpenID:" + OpenID + ",ModificationUser:" + ModificationUser + ",SwitchBtnLead:" + SwitchBtnLead
                 + ",PK_App_Customer_CustomerName:" + PK_App_Customer_CustomerName + ",EnumUserType:" + EnumUserType;
        }
    }
    #endregion

    #region 编辑用户
    /// <summary>
    /// 编辑用户
    /// </summary>
    public class SysUserEditModel
    {
        [Display(Name = "ID")]
        public string Id { get; set; }
        [Display(Name = "用户名")]
        public string UserName { get; set; }
        [StringLength(50, MinimumLength = 5)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "确认密码")]
        public string ComparePassword { get; set; }
        [NotNullExpression]
        [Display(Name = "姓名")]
        public string TrueName { get; set; }
        [Display(Name = "身份证")]
        public string Card { get; set; }

        [Display(Name = "手机号码")]
        [NotNullExpression]
        [RegularExpression(@"^1[0-9]{10}$", ErrorMessage = "手机号格式不正确")]
        public string MobileNumber { get; set; }

        [Display(Name = "电话号码")]
        public string PhoneNumber { get; set; }
        [Display(Name = "QQ")]
        public string QQ { get; set; }
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }
        [Display(Name = "其他联系方式")]
        public string OtherContact { get; set; }
        [Display(Name = "省份")]
        public string Province { get; set; }

        [Display(Name = "城市")]
        public string City { get; set; }

        [Display(Name = "地区")]
        public string Village { get; set; }

        [Display(Name = "详细地址")]
        public string Address { get; set; }
        [Display(Name = "状态")]
        public bool State { get; set; }
        [DateExpression]//如果填写判断是否是日期
        [Display(Name = "创建时间")]
        public DateTime? CreateTime { get; set; }
        [Display(Name = "创建人")]
        public string CreatePerson { get; set; }
        [Display(Name = "性别")]
        [Required]
        public string Sex { get; set; }
        [DateExpression]//如果填写判断是否是日期
        [Display(Name = "生日")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public string Birthday { get; set; }
        [DateExpression]//如果填写判断是否是日期
        [Display(Name = "加入日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public string JoinDate { get; set; }
        [Display(Name = "婚姻状况")]
        public string Marital { get; set; }
        [Display(Name = "党派")]
        public string Political { get; set; }
        [Display(Name = "籍贯")]
        public string Nationality { get; set; }
        [Display(Name = "所在地")]
        public string Native { get; set; }
        [Display(Name = "学校")]
        public string School { get; set; }
        [Display(Name = "专业")]
        public string Professional { get; set; }
        [Display(Name = "学历")]
        public string Degree { get; set; }
        [NotNullExpression]
        [Display(Name = "部门")]
        public string DepId { get; set; }
        [Display(Name = "职位")]
        public string PosId { get; set; }
        [Display(Name = "备注")]
        public string Expertise { get; set; }
        [Display(Name = "是否在职")]
        public bool JobState { get; set; }
        [Display(Name = "照片")]
        public string Photo { get; set; }
        [Display(Name = "附件")]
        public string Attach { get; set; }
        [Display(Name = "角色")]
        public string RoleName { get; set; }//拥有的用户
        public string CityName { get; set; }
        public string ProvinceName { get; set; }
        public string VillageName { get; set; }
        public string DepName { get; set; }
        public string PosName { get; set; }


        [MaxWordsExpression(50)]
        [Display(Name = "上级领导")]
        public string Lead { get; set; }
        public string LeadName { get; set; }
        [Display(Name = "是否可以自选领导")]
        public bool IsSelLead { get; set; }

        [Display(Name = "日否启动日程汇报是否启用  启用后 他的上司领导将可以看到他的 工作日程汇报.")]
        public bool IsReportCalendar { get; set; }

        [Display(Name = "开启小秘书")]
        public bool IsSecretary { get; set; }
        public string OpenID { get; set; }

        [Display(Name = "修改时间")]
        public DateTime ModificationTime { get; set; }

        [Display(Name = "修改人")]
        public string ModificationUser { get; set; }

        [Display(Name = "是否领导")]
        public string SwitchBtnLead { get; set; }

        [Display(Name = "关联用户信息")]
        public string PK_App_Customer_CustomerName { get; set; }

        [Display(Name = "用户类型")]
        public string EnumUserType { get; set; }
    }
    #endregion

    #region 用户分组
    /// <summary>
    /// 用户分组
    /// </summary>
    public class UserGroupModel
    {
        public string ContextId { get; set; }
    }
    #endregion

    #region 在线用户
    /// <summary>
    /// 在线用户
    /// </summary>
    public class SysOnlineUserModel
    {
        //ContextId
        public string ContextId { get; set; }
        //Online Status
        public int Status { get; set; }
        //用户ID
        public string UserId { get; set; }
        //用户名字
        public string TrueName { get; set; }
        //用户头像
        public string Photo { get; set; }
        //电话号码
        public string PhoneNumber { get; set; }
        //邮件地址
        public string Email { get; set; }
        //组织架构ID
        public string StructId { get; set; }
        //组织架构名字
        public string StructName { get; set; }
        //组织架构排序
        public int? Sort { get; set; }
        //职位名字
        public string PosName { get; set; }
    }
    #endregion

    #region 用户资料
    /// <summary>
    /// 用户资料
    /// </summary>
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TrueName { get; set; }
        public string MobileNumber { get; set; }
        public string Photo { get; set; }
        public string Expertise { get; set; }
        public string DepId { get; set; }
        public string DepName { get; set; }
        public string EmailAddress { get; set; }
        public string Sex { get; set; }
        public string OpenID { get; set; }
        public bool SwitchBtnAllergy { get; set; }
        public string Hair { get; set; }
        public string Food { get; set; }
        public string Maintain { get; set; }
        public string RoleName { get; set; }
        /// <summary>
        /// 维修师负责分类
        /// </summary>
        public string DeviceCategory { get; set; }
    }
    #endregion

    #region 用户模块
    /// <summary>
    /// 用户模块
    /// </summary>
    public class UserAppModuleModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string TrueName { get; set; }
        public string MobileNumber { get; set; }
        public string Hair { get; set; }
        public string Food { get; set; }
        public string Maintain { get; set; }
    }
    #endregion

    #region 用户资料修改
    /// <summary>
    /// 用户资料修改
    /// </summary>
    public class UserProfileEditPostBody
    {
        public string Id { get; set; }
        public string EmailAddress { get; set; }
        public string Photo { get; set; }
        public override string ToString()
        {
            return "Id:" + Id + ",EmailAddress:" + EmailAddress + ",Photo:" + Photo;
        }
    }
    #endregion
}
