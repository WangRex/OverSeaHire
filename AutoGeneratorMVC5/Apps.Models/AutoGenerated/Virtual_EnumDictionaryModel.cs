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
namespace Apps.Models.Sys
{
	public class Virtual_EnumDictionaryModel
	{
			public virtual int Id { get; set; }
			public virtual Nullable<int> parentId { get; set; }
			public virtual string TableName { get; set; }
			public virtual string ItemName { get; set; }
			public virtual string ItemValue { get; set; }
			public virtual System.DateTime CreateTime { get; set; }
			public virtual System.DateTime ModificationTime { get; set; }
			public virtual string CreateUserName { get; set; }
			public virtual string ModificationUserName { get; set; }
			public virtual Nullable<int> SortCode { get; set; }
			public virtual string ItemPhoto { get; set; }
		}
}