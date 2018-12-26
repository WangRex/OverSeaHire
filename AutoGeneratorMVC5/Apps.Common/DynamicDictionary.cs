/**
* 命名空间: Apps.Common
*
* 功 能： 为了能修改object属性
* 类 名： DynamicDictionary
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017-11-28 13:25:31 王仁禧 初版
* 
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：大连安琪科技有限公司 　　　　　　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using System.Collections.Generic;
using System.Dynamic;

namespace Apps.Common
{
    /// <summary>
    /// 动态object
    /// <code>
    /// 用法为dynamic data = new DynamicDictionary();
    /// data.Url = "";
    /// 这里灵活的地方是不用管是否有Url这个属性，可以直接操作值
    /// </code>
    /// </summary>
    public class DynamicDictionary : DynamicObject
    {
        // The inner dictionary.
        Dictionary<string, object> dictionary
            = new Dictionary<string, object>();

        // This property returns the number of elements
        // in the inner dictionary.
        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }

        // If you try to get a value of a property 
        // not defined in the class, this method is called.
        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            string name = binder.Name.ToLower();

            // If the property name is found in a dictionary,
            // set the result parameter to the property value and return true.
            // Otherwise, return false.
            return dictionary.TryGetValue(name, out result);
        }

        // If you try to set a value of a property that is
        // not defined in the class, this method is called.
        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            dictionary[binder.Name.ToLower()] = value;

            // You can always add a value to a dictionary,
            // so this method always returns true.
            return true;
        }
    }
}
