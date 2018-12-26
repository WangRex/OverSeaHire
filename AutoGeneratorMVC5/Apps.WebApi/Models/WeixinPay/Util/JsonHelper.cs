using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace tenpay
{
    public class JsonHelper
    {
        /// <summary>
        /// 获取Json字符串某节点的值
        /// </summary>
        public static string GetJsonValue(string jsonStr, string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                key = "\"" + key.Trim('"') + "\"";
                int index = jsonStr.IndexOf(key) + key.Length + 1;
                if (index > key.Length + 1)
                {
                    //先截逗号，若是最后一个，截"｝"号，取最小值
                    int end = jsonStr.IndexOf(',', index);
                    if (end == -1)
                    {
                        end = jsonStr.IndexOf('}', index);
                    }
                    result = jsonStr.Substring(index, end - index);
                    result = result.Trim(new[] { '"', ' ', '\"' }); //过滤引号或空格
                }
            }
            return result;
        }
    }

}
