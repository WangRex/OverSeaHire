//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using Apps.Models;
using System;
namespace Apps.Models.App
{
	public class Virtual_App_ApplyJobModel
	{
			public virtual string Id { get; set; }
			public virtual System.DateTime CreateTime { get; set; }
			public virtual System.DateTime ModificationTime { get; set; }
			public virtual string CreateUserName { get; set; }
			public virtual string ModificationUserName { get; set; }
			public virtual int SortCode { get; set; }
			public virtual string ParentId { get; set; }
			public virtual string PK_App_Requirement_Title { get; set; }
			public virtual string PK_App_Customer_CustomerName { get; set; }
			public virtual string CurrentStep { get; set; }
			public virtual string EnumApplyStatus { get; set; }
			public virtual decimal PromiseMoney { get; set; }
			public virtual string EnumPromisePayWay { get; set; }
			public virtual decimal ServiceMoney { get; set; }
			public virtual string EnumServicePayWay { get; set; }
			public virtual decimal TailMoney { get; set; }
			public virtual string EnumTailPayWay { get; set; }
		}
}
