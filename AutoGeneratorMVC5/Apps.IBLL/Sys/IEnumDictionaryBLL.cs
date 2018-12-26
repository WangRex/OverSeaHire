
using System;
using Apps.Common;
using System.Collections.Generic;
using Apps.Models.Sys;
using Apps.Models;

namespace Apps.IBLL.Sys
{
	public partial interface IEnumDictionaryBLL:IBaseBLL<EnumDictionaryModel>
	{
        List<EnumDictionary> GetDropDownList(string TableName);
    }
}
