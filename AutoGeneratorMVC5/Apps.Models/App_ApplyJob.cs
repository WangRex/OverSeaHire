//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Apps.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class App_ApplyJob
    {
        public string Id { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime ModificationTime { get; set; }
        public string CreateUserName { get; set; }
        public string ModificationUserName { get; set; }
        public int SortCode { get; set; }
        public string ParentId { get; set; }
        public string PK_App_Requirement_Title { get; set; }
        public string PK_App_Customer_CustomerName { get; set; }
        public string CurrentStep { get; set; }
        public string EnumApplyStatus { get; set; }
        public decimal PromiseMoney { get; set; }
        public string EnumPromisePayWay { get; set; }
        public decimal ServiceMoney { get; set; }
        public string EnumServicePayWay { get; set; }
        public decimal TailMoney { get; set; }
        public string EnumTailPayWay { get; set; }
    }
}
