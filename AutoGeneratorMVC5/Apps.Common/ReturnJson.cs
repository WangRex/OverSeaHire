/**
* 命名空间: Apps.Common
*
* 功 能： N/A
* 类 名： ReturnJson
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017-3-18 9:23:17 王仁禧 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：大连安琪科技有限公司 　　　　　　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Common
{
    public class ReturnJson
    {
        public int code { get; set; }
        public string sessionId { get; set; }
        public string success { get; set; }
    }
    public class Response
    {

        /// <summary>
        /// 返回代码. 0-成功，1-失败，其他-具体见方法返回值说明
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public dynamic Data { get; set; }
    }

    #region 异常信息
    /// <summary>
    /// 异常信息
    /// </summary>
    public class ShowError
    {
        public ShowError(string key, string message)
        {
            Key = key;
            Message = message;
        }
        public string Key { get; set; }
        public string Message { get; set; }
    }
    #endregion
}
