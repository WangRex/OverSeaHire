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
	public class Virtual_App_CustomerWorkExpModel
	{
			public virtual string Id { get; set; }
			public virtual System.DateTime CreateTime { get; set; }
			public virtual System.DateTime ModificationTime { get; set; }
			public virtual string CreateUserName { get; set; }
			public virtual string ModificationUserName { get; set; }
			public virtual int SortCode { get; set; }
			public virtual string ParentId { get; set; }
			public virtual string PK_App_Customer_CustomerName { get; set; }
			public virtual string StartDate { get; set; }
			public virtual string EndDate { get; set; }
			public virtual string Company { get; set; }
			public virtual string Position { get; set; }
		}
}
